using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using Google.Apis.Analytics.v3;
using Google.Apis.Analytics.v3.Data;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using MvcSiteMapProvider.Web.Mvc.Filters;
using PagedList;
using VectorShop.Models;
using VectorShop.Models.ViewModels;
using VectorShop.Helpers;

namespace VectorShop.Controllers
{
    [Authorize(Roles = "admin")]
    public class ProductController : Controller
    {
        private VectorShopDb _db = new VectorShopDb();

        [AllowAnonymous]
        public ActionResult Index()
        {
            //TODO don't forget to return the newest product, currently it's the old products first.
            var model = _db.Products.OrderByDescending(p => p.ProductId).Take(4);
            return View(model);

        }

        [AllowAnonymous]
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var model = await _db.Products.FindAsync(id);

            if (model == null)
            {
                return HttpNotFound();
            }

            var priCat = await _db.PriCats.Where(c => c.PriCatId == model.PriCatIDfk).Select(c => new { Id = c.PriCatId, Title = c.PriCatTitle }).SingleOrDefaultAsync();

            var secCat = await _db.SecCats.Where(c => c.SecCatId == model.SecCatIDfk).Select(c => new { Id = c.SecCatId, Title = c.SecCatTitle }).SingleOrDefaultAsync();

            var similarProduct = _db.Products.Where(p => p.PriCatIDfk == model.PriCatIDfk && p.SecCatIDfk == model.SecCatIDfk).AsEnumerable().SkipWhile(p => p.ProductId == id).Take(3);

            if (priCat != null)
            {
                ViewBag.PriCatId = priCat.Id;
                ViewBag.PriCatTitle = priCat.Title;
            }
            else
            {
                ViewBag.PriCatId = null;
                ViewBag.PriCatTitle = null;
            }

            if (secCat != null)
            {
                ViewBag.SecCatId = secCat.Id;
                ViewBag.SecCatTitle = secCat.Title;
            }
            else
            {
                ViewBag.SecCatId = null;
                ViewBag.SecCatTitle = null;
            }

            ViewBag.SimilarProduct = similarProduct.Any() ? similarProduct : null;


            return View(model);
        }

        [AllowAnonymous]
        public ActionResult ProductsPriCat(int? id, int? page)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var model = _db.Products.OrderByDescending(p => p.ProductId).Where(p => p.PriCatIDfk == id);

            var pageNumber = page ?? 1;
            var onePageOfProductsPriCat = model.ToPagedList(pageNumber, 2);
            //TODO: Increase the paging size when project finished.

            if (!model.Any())
            {
                return HttpNotFound();
            }

            return View(onePageOfProductsPriCat);
        }

        [AllowAnonymous]
        public ActionResult ProductsCatPartial()
        {
            var model = _db.PriCats.OrderByDescending(p => p.PriCatId);

            return PartialView("_ProductsCatPartial", model);
        }

        [AllowAnonymous]
        public ActionResult ProductsSecCat(int? id, int? page)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var model = _db.Products.OrderByDescending(p => p.ProductId).Where(p => p.SecCatIDfk == id);

            var pageNumber = page ?? 1;
            var onePageOfProductsSecCat = model.ToPagedList(pageNumber, 2);
            //TODO: Increase the paging size when project finished.

            if (!model.Any())
            {
                return HttpNotFound();
            }

            return View(onePageOfProductsSecCat);
        }

        [AllowAnonymous]
        public PartialViewResult SelectedProductPartial()
        {
            var model = _db.Products.Where(p => p.IsProductIsInIndex);

            return PartialView("_SelectedProductPartial", model);
        }

        [AllowAnonymous]
        public PartialViewResult FreeProductPartial()
        {
            var model = _db.Products.Where(p => p.IsProductFree).OrderByDescending(p => p.ProductId).Take(10);

            return PartialView("_FreeProductPartial", model);
        }

        [AllowAnonymous]
        public PartialViewResult NewestProductPartial()
        {
            var model = _db.Products.OrderByDescending(p => p.ProductId).Take(10);

            return PartialView("_NewestProductPartial", model);
        }

        [OutputCache(Duration = 86400, VaryByParam = "none")]
        [AllowAnonymous]
        public PartialViewResult MostViewedProductPartial()
        {
            var path = Server.MapPath("~/Content/privatekey.p12");
            if (!System.IO.File.Exists(path))
            {
                throw new FileNotFoundException("The file containing the private key does not exist.");
            }

            try
            {
                //UPDATE this to match the path to your p12 certificate
                var certificate = new X509Certificate2(path, "notasecret", X509KeyStorageFlags.MachineKeySet | X509KeyStorageFlags.PersistKeySet | X509KeyStorageFlags.Exportable);

                //OAuth Service account Email address
                var credential =
                    new ServiceAccountCredential(new ServiceAccountCredential.Initializer(
                        "738851578499-l72ijr35mud9tfa9jhk1pobn1nftjefj@developer.gserviceaccount.com")
                    {
                        Scopes = new[] { AnalyticsService.Scope.Analytics }
                    }.FromCertificate(certificate));

                var analyticsService = new AnalyticsService(new BaseClientService.Initializer()
                {
                    HttpClientInitializer = credential,
                    ApplicationName = "VectorShop"
                });



                DataResource.GaResource.GetRequest analyticQuery =
                    analyticsService.Data.Ga.Get(
                        "ga:104731600", "2014-07-01", DateTime.Now.ToString("yyyy-MM-dd"),
                        "ga:pageviews, ga:uniquePageviews");


                analyticQuery.Dimensions = "ga:pagePath";
                analyticQuery.MaxResults = 10;
                analyticQuery.Sort = "-ga:pageviews";
                analyticQuery.Filters = "ga:pagePath=~/Product/Details";

                GaData returnedData = analyticQuery.Execute();

                var mostViewedPro = new List<int>();

                foreach (var item in returnedData.Rows)
                {
                    int index = Convert.ToInt32(item[0].Split('/')[3]);
                    mostViewedPro.Add(index);
                }

                //var model = _db.Products.Where(p => mostViewedPro.Contains(p.ProductId)); complexity of O(N square)
                var model = _db.Products.Join(mostViewedPro, p => p.ProductId, m => m, (p, m) => p);
                return PartialView("_MostViewedProductPartial", model);
            }

            catch //(Exception e)
            {
                var model = _db.Products.OrderByDescending(p => p.ProductName).Where(p => p.IsProductIsInIndex).Take(10);
                return PartialView("_MostViewedProductPartial", model);
            }

        }

        public JsonResult GetSecCatByPriCatId(int priCatId)
        {
            List<SecCat> objSecCat = new List<SecCat>();
            objSecCat = _db.SecCats.Where(s => s.PriCatIDfk == priCatId).ToList();
            SelectList objSecCatList = new SelectList(objSecCat, "SecCatId", "SecCatTitle", 0);
            return Json(objSecCatList);
        }

        [HttpPost]
        [AllowAnonymous]
        public JsonResult LoadMoreProduct(int size)
        {

            var model = _db.Products.OrderByDescending(p => p.ProductId).Skip(size).Take(4);
            int modelCount = _db.Products.Count();
            if (model.Any())
            {
                string modelString = RenderRazorViewToString("_LoadMoreProduct", model);

                return Json(new { ModelString = modelString, ModelCount = modelCount });
            }
            return Json(model);
        }
        public string RenderRazorViewToString(string viewName, object model)
        {
            ViewData.Model = model;
            using (var sw = new StringWriter())
            {
                var viewResult = ViewEngines.Engines.FindPartialView(ControllerContext,
                                                                         viewName);
                var viewContext = new ViewContext(ControllerContext, viewResult.View,
                                             ViewData, TempData, sw);
                viewResult.View.Render(viewContext, sw);
                viewResult.ViewEngine.ReleaseView(ControllerContext, viewResult.View);
                return sw.GetStringBuilder().ToString();
            }
        }

        [HttpGet]
        public ActionResult Create()
        {
            var priCatlistItems = new List<SelectListItem>();

            foreach (var item in _db.PriCats)
            {
                priCatlistItems.Add(new SelectListItem
                {
                    Text = item.PriCatTitle,
                    Value = item.PriCatId.ToString()
                });
            }

            ViewBag.priCatList = priCatlistItems;


            return View();

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [SiteMapCacheRelease]
        [ValidateInput(false)]
        public ActionResult Create(ProductViewModel viewModel, HttpPostedFileBase productPicture, HttpPostedFileBase productDownloadLink)
        {
            //you can also use IEnumerable<HttpPostedFileBase> files for multiple files but you should name the uploads the same
            var priCatlistItems = new List<SelectListItem>();

            foreach (var item in _db.PriCats)
            {
                priCatlistItems.Add(new SelectListItem
                {
                    Text = item.PriCatTitle,
                    Value = item.PriCatId.ToString()
                });
            }

            ViewBag.priCatList = priCatlistItems;

            if (ModelState.IsValid)
            {
                var product = new Product
                {
                    IsProductFree = viewModel.IsProductFree,
                    IsProductIsInIndex = viewModel.IsProductIsInIndex,
                    PriCatIDfk = viewModel.PriCatIDfk,
                    ProductDate = DateTime.Now,
                    ProductDescription = viewModel.ProductDescription,
                    ProductName = viewModel.ProductName,
                    ProductPrice = viewModel.ProductPrice,
                    SecCatIDfk = viewModel.SecCatIDfk == 0 ? null : viewModel.SecCatIDfk
                };


                #region getting the posted files the old and ugly way
                //if (Request.Files.Count > 0)
                //{
                //    var productPicture = Request.Files[0];
                //    var productDownloadLink = Request.Files[1];

                //    if (productPicture != null && productPicture.ContentLength > 0)
                //    {
                //        var fileName = Path.GetFileName(productPicture.FileName);
                //        var path = Path.Combine(Server.MapPath("~/Content/Images/Product/"), fileName);
                //        productPicture.SaveAs(path);
                //    }

                //    if (productDownloadLink != null && productDownloadLink.ContentLength > 0)
                //    {
                //        var fileName = Path.GetFileName(productDownloadLink.FileName);
                //        var path = Path.Combine(Server.MapPath("~/ProductFiles/"), fileName);
                //        productDownloadLink.SaveAs(path);
                //    }

                //}
                #endregion



                if (productPicture != null && productPicture.ContentLength > 0)
                {
                    var allowedExtensions = new[]
                    {
                        ".jpg", ".jpeg", ".png", ".gif", ".bmp",
                        ".JPG", ".JPEG", ".PNG", ".GIF", ".BMP"
                    };

                    var extension = Path.GetExtension(productPicture.FileName);

                    if (!allowedExtensions.Contains(extension))
                    {
                        ModelState.AddModelError("ProductPicture",
                            "فرمت فایل انتخابی مناسب نیست، فرمت های قابل قبول (jpg, jpeg, png, gif, bmp) میباشند.");
                        return View(viewModel);
                    }

                    var fileName = Path.GetFileName(productPicture.FileName);

                    var path = Path.Combine(
                        Server.MapPath("~/Content/Images/Product/")
                        , DateTime.Now.ToString("yyyy-dd-M--HH-mm-ss")
                        + "-"
                        + fileName);

                    product.ProductPicture = "/Content/Images/Product/" + Path.GetFileName(path);

                    productPicture.SaveAs(path);
                }

                if (productDownloadLink != null && productDownloadLink.ContentLength > 0)
                {
                    var fileName = Path.GetFileName(productDownloadLink.FileName);

                    var path = Path.Combine(
                        Server.MapPath("~/ProductFiles/")
                        , DateTime.Now.ToString("yyyy-dd-M--HH-mm-ss")
                        + "-"
                        + fileName);

                    product.ProductDownloadLink = "/ProductFiles/" + Path.GetFileName(path);

                    productDownloadLink.SaveAs(path);
                }

                _db.Products.Add(product);
                _db.SaveChanges();

                return RedirectToAction("Index");
            }




            return View(viewModel);
        }

        [Route("Product/ManageProduct/Page/{page?}")]
        [Route("Product/ManageProduct/{page?}")]
        public ActionResult ManageProduct(int? page)
        {
            const int pageSize = 5;

            var allProduct = _db.Products.OrderByDescending(p => p.ProductId);

            var productPages = new PaginatedList<Product>(allProduct, page ?? 0, pageSize);
            //TODO: Increase the paging size when project finished.

            ViewBag.SelectedPage = page;

            return View(productPages);
        }
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var model = _db.Products.Find(id);
            var productViewModel = Mapper.Map<Product, ProductViewModel>(model);

            #region doing the mapping the hard and nasty way.
            //var viewModelNasty = _db.Products.Where(p => p.ProductId == id).Select(vmp => new ProductViewModel()
            //{
            //    IsProductFree=vmp.IsProductFree,
            //    PriCatIDfk=vmp.PriCatIDfk,
            //     ProductDescription=vmp.ProductDescription,
            //      ProductDownloadLink=vmp.ProductDownloadLink,
            //       ProductId=vmp.ProductId,
            //        ProductName=vmp.ProductName,
            //         ProductPicture=vmp.ProductPicture,
            //          ProductPrice=vmp.ProductPrice,
            //          SecCatIDfk=vmp.SecCatIDfk
            //}).Single();
            #endregion

            if (model == null)
            {
                return HttpNotFound();
            }

            var priCatlistItems = new List<SelectListItem>();
            var secCatlistItems = new List<SelectListItem>();

            var secItem = _db.SecCats.SingleOrDefault(s => s.SecCatId == model.SecCatIDfk);

            if (secItem != null)
                secCatlistItems.Add(new SelectListItem { Text = secItem.SecCatTitle, Value = secItem.SecCatId.ToString() });

            foreach (var item in _db.PriCats)
            {
                priCatlistItems.Add(new SelectListItem
                {
                    Text = item.PriCatTitle,
                    Value = item.PriCatId.ToString()
                });
            }

            ViewBag.priCatList = priCatlistItems;
            ViewBag.secCatList = secCatlistItems;


            return View(productViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [SiteMapCacheRelease]
        [ValidateInput(false)]
        public ActionResult Edit(ProductViewModel viewModel, HttpPostedFileBase productPicture, HttpPostedFileBase productDownloadLink)
        {
            var product = _db.Products.Find(viewModel.ProductId);
            var productViewModel = Mapper.Map<Product, ProductViewModel>(product);

            var priCatlistItems = new List<SelectListItem>();
            var secCatlistItems = new List<SelectListItem>();

            var secItem = _db.SecCats.SingleOrDefault(s => s.SecCatId == product.SecCatIDfk);

            if (secItem != null)
                secCatlistItems.Add(new SelectListItem { Text = secItem.SecCatTitle, Value = secItem.SecCatId.ToString() });

            foreach (var item in _db.PriCats)
            {
                priCatlistItems.Add(new SelectListItem
                {
                    Text = item.PriCatTitle,
                    Value = item.PriCatId.ToString()
                });
            }

            ViewBag.priCatList = priCatlistItems;
            ViewBag.secCatList = secCatlistItems;

            var pictureValidation = ModelState["ProductPicture"];
            pictureValidation.Errors.Clear();
            var downloadLinkValidation = ModelState["ProductDownloadLink"];
            downloadLinkValidation.Errors.Clear();

            if (ModelState.IsValid)
            {
                product.IsProductFree = viewModel.IsProductFree;
                product.IsProductIsInIndex = viewModel.IsProductIsInIndex;
                product.PriCatIDfk = viewModel.PriCatIDfk;
                product.SecCatIDfk = viewModel.SecCatIDfk == 0 ? null : viewModel.SecCatIDfk;

                product.ProductName = viewModel.ProductName;
                product.ProductPrice = viewModel.ProductPrice;
                product.ProductDescription = viewModel.ProductDescription;
                string oldPicturePath = product.ProductPicture;
                string oldDownloadLinkPath = product.ProductDownloadLink;


                if (viewModel.ProductPicture != null)
                {


                    if (productPicture != null && productPicture.ContentLength > 0)
                    {
                        var allowedExtensions = new[]
                                {
                                    ".jpg", ".jpeg", ".png", ".gif", ".bmp",
                                    ".JPG", ".JPEG", ".PNG", ".GIF", ".BMP"
                                };

                        var extension = Path.GetExtension(productPicture.FileName);

                        if (!allowedExtensions.Contains(extension))
                        {
                            ModelState.AddModelError("ProductPicture",
                                "فرمت فایل انتخابی مناسب نیست، فرمت های قابل قبول (jpg, jpeg, png, gif, bmp) میباشند.");
                            return View(productViewModel);
                        }

                        var fileName = Path.GetFileName(productPicture.FileName);

                        var path = Path.Combine(
                            Server.MapPath("~/Content/Images/Product/")
                            , DateTime.Now.ToString("yyyy-dd-M--HH-mm-ss")
                            + "-"
                            + fileName);

                        product.ProductPicture = "/Content/Images/Product/" + Path.GetFileName(path);

                        productPicture.SaveAs(path);
                    }

                    string fullPath = Request.MapPath("~" + oldPicturePath);
                    if (System.IO.File.Exists(fullPath))
                    {
                        System.IO.File.Delete(fullPath);
                    }

                }

                if (viewModel.ProductDownloadLink != null)
                {

                    if (productDownloadLink != null && productDownloadLink.ContentLength > 0)
                    {
                        var fileName = Path.GetFileName(productDownloadLink.FileName);

                        var path = Path.Combine(
                            Server.MapPath("~/ProductFiles/")
                            , DateTime.Now.ToString("yyyy-dd-M--HH-mm-ss")
                            + "-"
                            + fileName);

                        product.ProductDownloadLink = "/ProductFiles/" + Path.GetFileName(path);

                        productDownloadLink.SaveAs(path);
                    }

                    string fullPath = Request.MapPath("~" + oldDownloadLinkPath);
                    if (System.IO.File.Exists(fullPath))
                    {
                        System.IO.File.Delete(fullPath);
                    }
                }

                _db.SaveChanges();

                return RedirectToAction("ManageProduct");
            }

            return View(productViewModel);

        }

        [HttpGet]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var model = _db.Products.Find(id);

            if (model == null)
            {
                return HttpNotFound();
            }

            return View(model);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var model = _db.Products.Find(id);

            string fullPicturePath = Request.MapPath("~" + model.ProductPicture);
            if (System.IO.File.Exists(fullPicturePath))
            {
                System.IO.File.Delete(fullPicturePath);
            }

            string fullDownloadLinkPath = Request.MapPath("~" + model.ProductDownloadLink);
            if (System.IO.File.Exists(fullDownloadLinkPath))
            {
                System.IO.File.Delete(fullDownloadLinkPath);
            }

            try
            {
                _db.Products.Remove(model);
                _db.SaveChanges();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty,
                    "خطایی در انجام فرمان مورد نظر به وجود آمده، لطفا دوباره تلاش کنید یا با مدیر سایت تماس بگیرید. "
                     + ex.Message);
                return View(model);
            }


            return RedirectToAction("ManageProduct");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _db.Dispose();
            }
            base.Dispose(disposing);
        }

    }
}
