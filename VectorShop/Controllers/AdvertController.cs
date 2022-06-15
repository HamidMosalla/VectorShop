using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using PagedList;
using VectorShop.Helpers;
using VectorShop.Models;
using VectorShop.Models.ViewModels;

namespace VectorShop.Controllers
{
    [Authorize(Roles="admin")]
    public class AdvertController : Controller
    {
        private VectorShopDb _db = new VectorShopDb();

        [Route("Advert/ManageAdvert/{page?}")]
        public ActionResult ManageAdvert(int? page)
        {
            var model = _db.Adverts.OrderByDescending(c => c.AdvertId);
            var pageNumber = page ?? 1;
            var onePageOfAdverts = model.ToPagedList(pageNumber, 2);
            //TODO: Increase the paging size when project finished.

            return View(onePageOfAdverts);
        }

        [AllowAnonymous]
        public PartialViewResult AdvertPartial()
        {
            var model = _db.Adverts.OrderBy(a => a.AdvertPriority).ThenBy(a => a.AdvertTitle).Take(4);

            return PartialView("_AdvertPartial", model);
        }

        [AllowAnonymous]
        public PartialViewResult AdvertPartialInline()
        {
            var model = _db.Adverts.OrderBy(a => a.AdvertPriority).ThenBy(a => a.AdvertTitle).Take(5);

            return PartialView("_AdvertPartialInline", model);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(AdvertViewModel viewModel, HttpPostedFileBase advertPicture)
        {

            if (ModelState.IsValid)
            {
                var advert = new Advert
                {
                    AdvertTitle = viewModel.AdvertTitle,
                    AdvertPriority = viewModel.AdvertPriority,
                    AdvertUrl = viewModel.AdvertUrl,
                    AdvertDateAdded = DateTime.Now
                };

                if (advertPicture != null && advertPicture.ContentLength > 0)
                {
                    var allowedExtensions = new[]
                    {
                        ".jpg", ".jpeg", ".png", ".gif", ".bmp",
                        ".JPG", ".JPEG", ".PNG", ".GIF", ".BMP"
                    };

                    var extension = Path.GetExtension(advertPicture.FileName);

                    if (!allowedExtensions.Contains(extension))
                    {
                        ModelState.AddModelError("AdvertPicture",
                            "فرمت فایل انتخابی مناسب نیست، فرمت های قابل قبول (jpg, jpeg, png, gif, bmp) میباشند.");
                        return View(viewModel);
                    }

                    if (advertPicture.ContentLength > (2 * (1024 * 1024)))
                    {
                        ModelState.AddModelError("AdvertPicture",
                            "سایز فایل بیش از حد مجاز است، حداکثر سایز فایل آپلود شده 2 مگابایت می باشد.");
                        return View(viewModel);
                    }

                    if (!VectorShopUtility.IsImgDimAcceptable(advertPicture, 150, 100, 500, 500))
                    {
                        ModelState.AddModelError("AdvertPicture",
                            "اندازه تصویر باید با حداقل عرض 150 و طول 100 و حداکثر عرض 200 و طول 200 باشد.");
                        return View(viewModel);
                    }

                    var fileName = Path.GetFileName(advertPicture.FileName);

                    var path = Path.Combine(
                        Server.MapPath("~/Content/Images/Advert/")
                        , DateTime.Now.ToString("yyyy-dd-M--HH-mm-ss")
                        + "-"
                        + fileName);

                    advert.AdvertPicture = "/Content/Images/Advert/" + Path.GetFileName(path);

                    advertPicture.SaveAs(path);
                }

                _db.Adverts.Add(advert);
                _db.SaveChanges();

                return RedirectToAction("ManageAdvert");

            }


            return View(viewModel);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var model = _db.Adverts.Find(id);

            if (model == null)
            {
                return HttpNotFound();
            }

            var viewModel = Mapper.Map<Advert, AdvertViewModel>(model);

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(AdvertViewModel viewModel, HttpPostedFileBase advertPicture)
        {

            var advert = _db.Adverts.Find(viewModel.AdvertId);
            var advertViewModel = Mapper.Map<Advert, AdvertViewModel>(advert);


            var advertPictureValidation = ModelState["AdvertPicture"];
            advertPictureValidation.Errors.Clear();

            if (ModelState.IsValid)
            {


                advert.AdvertTitle = viewModel.AdvertTitle;
                advert.AdvertPriority = viewModel.AdvertPriority;
                advert.AdvertUrl = viewModel.AdvertUrl;

                string oldPicturePath = advert.AdvertPicture;

                if (viewModel.AdvertPicture != null)
                {



                    if (advertPicture != null && advertPicture.ContentLength > 0)
                    {
                        var allowedExtensions = new[]
                                {
                                    ".jpg", ".jpeg", ".png", ".gif", ".bmp",
                                    ".JPG", ".JPEG", ".PNG", ".GIF", ".BMP"
                                };

                        var extension = Path.GetExtension(advertPicture.FileName);

                        if (!allowedExtensions.Contains(extension))
                        {
                            ModelState.AddModelError("AdvertPicture",
                                "فرمت فایل انتخابی مناسب نیست، فرمت های قابل قبول (jpg, jpeg, png, gif, bmp) میباشند.");
                            return View(advertViewModel);
                        }

                        if (advertPicture.ContentLength > (2 * (1024 * 1024)))
                        {
                            ModelState.AddModelError("AdvertPicture",
                                "سایز فایل بیش از حد مجاز است، حداکثر سایز فایل آپلود شده 2 مگابایت می باشد.");
                            return View(advertViewModel);
                        }

                        if (!VectorShopUtility.IsImgDimAcceptable(advertPicture, 150, 100, 500, 500))
                        {
                            ModelState.AddModelError("AdvertPicture",
                                "اندازه تصویر باید با حداقل عرض 150 و طول 100 و حداکثر عرض 500 و طول 500 باشد.");
                            return View(advertViewModel);
                        }

                        var fileName = Path.GetFileName(advertPicture.FileName);

                        var path = Path.Combine(
                            Server.MapPath("~/Content/Images/Advert/")
                            , DateTime.Now.ToString("yyyy-dd-M--HH-mm-ss")
                            + "-"
                            + fileName);

                        advert.AdvertPicture = "/Content/Images/Advert/" + Path.GetFileName(path);

                        advertPicture.SaveAs(path);
                    }

                    string fullPath = Request.MapPath("~" + oldPicturePath);
                    if (System.IO.File.Exists(fullPath))
                    {
                        System.IO.File.Delete(fullPath);
                    }

                }

                _db.SaveChanges();

                return RedirectToAction("ManageAdvert");

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

            var model = _db.Adverts.Find(id);

            if (model == null)
            {
                return HttpNotFound();
            }

            try
            {
                string fullPath = Request.MapPath("~" + model.AdvertPicture);
                if (System.IO.File.Exists(fullPath))
                {
                    System.IO.File.Delete(fullPath);
                }

                _db.Adverts.Remove(model);
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
