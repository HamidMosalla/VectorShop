using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using PagedList;
using VectorShop.Helpers;
using VectorShop.Models;
using VectorShop.Models.ViewModels;

namespace VectorShop.Controllers
{
    [Authorize(Roles = "admin")]
    public class NewDesignOrderController : Controller
    {
        private VectorShopDb _db = new VectorShopDb();

        [Route("NewDesignOrder/ManageNewDesignOrder/{page?}")]
        public ActionResult ManageNewDesignOrder(int? page)
        {
            var model = _db.NewDesignOrders.OrderByDescending(c => c.NewDesignOrderId);
            var pageNumber = page ?? 1;
            var onePageOfNewDesignOrders = model.ToPagedList(pageNumber, 2);
            //TODO: Increase the paging size when project finished.

            return View(onePageOfNewDesignOrders);
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var model = _db.NewDesignOrders.Find(id);

            if (model == null)
            {
                return HttpNotFound();
            }

            return View(model);
        }

        [AllowAnonymous]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public ActionResult Create(NewDesignOrderViewModel viewModel, HttpPostedFileBase newDesignOrderPicture, FormCollection formCollection)
        {

            var response = Request["g-recaptcha-response"];
            //secret that was generated in key value pair
            const string secret = "6Le2XggTAAAAAOKf6asx_ftBArje7oEf27Bm23-Y";

            var client = new WebClient();
            var reply =
                client.DownloadString(
                    string.Format("https://www.google.com/recaptcha/api/siteverify?secret={0}&response={1}", secret, response));

            var captchaResponse = JsonConvert.DeserializeObject<CaptchaResponse>(reply);

            if (captchaResponse.Success == "False")
            {
                ModelState.AddModelError(string.Empty, "لطفا مرحله ی من ربات نیستم مربوط به فرم را تکمیل نمایید.");
                return View(viewModel);
            }

            if (ModelState.IsValid && bool.Parse(captchaResponse.Success))
            {

                var newDesignOrder = new NewDesignOrder
                {
                    NewDesignOrderAddress = viewModel.NewDesignOrderAddress,
                    NewDesignOrderDate = DateTime.Now,
                    NewDesignOrderDescBody = viewModel.NewDesignOrderDescBody,
                    NewDesignOrderEmail = viewModel.NewDesignOrderEmail,
                    NewDesignOrderHowFindUs = viewModel.NewDesignOrderHowFindUs,
                    NewDesignOrderMaxAffordablePrice = viewModel.NewDesignOrderMaxAffordablePrice,
                    NewDesignOrderName = viewModel.NewDesignOrderName,
                    NewDesignOrderPhone = viewModel.NewDesignOrderPhone,
                    NewDesignOrderTitle = viewModel.NewDesignOrderTitle,
                    NewDesignOrderWebSite = viewModel.NewDesignOrderWebSite
                };

                if (newDesignOrderPicture != null && newDesignOrderPicture.ContentLength > 0)
                {
                    var allowedExtensions = new[]
                    {
                        ".jpg", ".jpeg", ".png", ".gif", ".bmp",
                        ".JPG", ".JPEG", ".PNG", ".GIF", ".BMP"
                    };

                    var extension = Path.GetExtension(newDesignOrderPicture.FileName);

                    if (!allowedExtensions.Contains(extension))
                    {
                        ModelState.AddModelError("NewDesignOrderPicture",
                            "فرمت فایل انتخابی مناسب نیست، فرمت های قابل قبول (jpg, jpeg, png, gif, bmp) میباشند.");
                        return View(viewModel);
                    }

                    if (newDesignOrderPicture.ContentLength > (4 * (1024 * 1024)))
                    {
                        ModelState.AddModelError("NewDesignOrderPicture",
                            "سایز فایل بیش از حد مجاز است، حداکثر سایز فایل آپلود شده 4 مگابایت می باشد.");
                        return View(viewModel);
                    }

                    var fileName = Path.GetFileName(newDesignOrderPicture.FileName);

                    var path = Path.Combine(
                        Server.MapPath("~/Content/Images/NewDesignOrder/")
                        , DateTime.Now.ToString("yyyy-dd-M--HH-mm-ss")
                        + "-"
                        + fileName);

                    newDesignOrder.NewDesignOrderPicture = "/Content/Images/NewDesignOrder/" + Path.GetFileName(path);

                    var resizedPicture = VectorShopUtility.ScaleImage(newDesignOrderPicture, 700, 500);
                    resizedPicture.Save(path);

                    //newDesignOrderPicture.SaveAs(path);
                }

                _db.NewDesignOrders.Add(newDesignOrder);
                _db.SaveChanges();

                return View("Success");

            }


            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var model = _db.NewDesignOrders.Find(id);

            if (model == null)
            {
                return HttpNotFound();
            }

            try
            {
                string fullPath = Request.MapPath("~" + model.NewDesignOrderPicture);
                if (System.IO.File.Exists(fullPath))
                {
                    System.IO.File.Delete(fullPath);
                }

                _db.NewDesignOrders.Remove(model);
                _db.SaveChanges();
                return Json(new { Status = "Deleted" });
            }

            catch (Exception eX)
            {
                return Json(new { Status = "Error", eXMessage = eX.Message });
            }
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
