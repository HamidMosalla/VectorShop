using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using VectorShop.Helpers;
using VectorShop.Models;
using VectorShop.Models.ViewModels;

namespace VectorShop.Controllers
{
    [Authorize(Roles = "admin")]
    public class SlideShowController : Controller
    {
        private VectorShopDb _db = new VectorShopDb();

        [Route("SlideShow/ManageSlideShow/Page/{page?}")]
        [Route("SlideShow/ManageSlideShow/{page?}")]
        public ActionResult ManageSlideShow(int? page)
        {
            const int pageSize = 2;

            var allSlides = _db.SlideShows.OrderByDescending(p => p.SlideShowId);

            var productPages = new PaginatedList<SlideShow>(allSlides, page ?? 0, pageSize);
            //TODO: Increase the paging size when project finished.

            ViewBag.SelectedPage = page;

            return View(productPages);
        }

        [AllowAnonymous]
        public PartialViewResult SlideShowPartial()
        {
            var controlPanel = _db.ControlPanels.OrderByDescending(c => c.ControlPanelId).Single();
            var model = _db.SlideShows.OrderByDescending(p => p.SlideShowId).Take(controlPanel.SlideShowNumber);
            ViewBag.SlideShowNumber = controlPanel.SlideShowNumber;
            return PartialView("_SlideShowPartial", model);
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(SlideShowViewModel viewModel, HttpPostedFileBase SlideShowPictrure)
        {
            if (ModelState.IsValid)
            {

                var slideShow = new SlideShow
                {
                    SlideShowTitle = viewModel.SlideShowTitle,
                    SlideShowDescription = viewModel.SlideShowDescription,
                    SlideShowLink = viewModel.SlideShowLink
                };

                if (SlideShowPictrure != null && SlideShowPictrure.ContentLength > 0)
                {
                    var allowedExtensions = new[]
                    {
                        ".jpg", ".jpeg", ".png", ".gif", ".bmp",
                        ".JPG", ".JPEG", ".PNG", ".GIF", ".BMP"
                    };

                    var extension = Path.GetExtension(SlideShowPictrure.FileName);

                    if (!allowedExtensions.Contains(extension))
                    {
                        ModelState.AddModelError("SlideShowPictrure",
                            "فرمت فایل انتخابی مناسب نیست، فرمت های قابل قبول (jpg, jpeg, png, gif, bmp) میباشند.");
                        return View(viewModel);
                    }

                    if (SlideShowPictrure.ContentLength > (4 * (1024 * 1024)))
                    {
                        ModelState.AddModelError("SlideShowPictrure",
                            "سایز فایل بیش از حد مجاز است، حداکثر سایز فایل آپلود شده 4 مگابایت می باشد.");
                        return View(viewModel);
                    }

                    if (!VectorShopUtility.IsImgDimAcceptable(SlideShowPictrure, 300, 700, 700, 1500))
                    {
                        ModelState.AddModelError("SlideShowPictrure",
                            "اندازه تصویر باید با حداقل عرض 300 و طول 700 و حداکثر عرض 700 و طول 1500 باشد.");
                        return View(viewModel);
                    }

                    var fileName = Path.GetFileName(SlideShowPictrure.FileName);

                    var path = Path.Combine(
                        Server.MapPath("~/Content/Images/SlideShow/")
                        , DateTime.Now.ToString("yyyy-dd-M--HH-mm-ss")
                        + "-"
                        + fileName);

                    slideShow.SlideShowPictrure = "/Content/Images/SlideShow/" + Path.GetFileName(path);

                    SlideShowPictrure.SaveAs(path);
                }

                _db.SlideShows.Add(slideShow);
                _db.SaveChanges();

                return RedirectToAction("ManageSlideShow");

            }
            return View(viewModel);
        }

        [HttpGet]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var model = _db.SlideShows.Find(id);

            if (model == null)
            {
                return HttpNotFound();
            }


            var viewModel = Mapper.Map<SlideShow, SlideShowViewModel>(model);


            return View(viewModel);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(SlideShowViewModel viewModel, HttpPostedFileBase SlideShowPictrure)
        {

            var slideShow = _db.SlideShows.Find(viewModel.SlideShowId);
            var slideShowViewModel = Mapper.Map<SlideShow, SlideShowViewModel>(slideShow);


            var slidePictureValidation = ModelState["SlideShowPictrure"];
            slidePictureValidation.Errors.Clear();

            if (ModelState.IsValid)
            {


                slideShow.SlideShowTitle = viewModel.SlideShowTitle;
                slideShow.SlideShowDescription = viewModel.SlideShowDescription;
                slideShow.SlideShowLink = viewModel.SlideShowLink;
                string oldPicturePath = slideShow.SlideShowPictrure;

                if (viewModel.SlideShowPictrure != null)
                {



                    if (SlideShowPictrure != null && SlideShowPictrure.ContentLength > 0)
                    {
                        var allowedExtensions = new[]
                                {
                                    ".jpg", ".jpeg", ".png", ".gif", ".bmp",
                                    ".JPG", ".JPEG", ".PNG", ".GIF", ".BMP"
                                };

                        var extension = Path.GetExtension(SlideShowPictrure.FileName);

                        if (!allowedExtensions.Contains(extension))
                        {
                            ModelState.AddModelError("SlideShowPictrure",
                                "فرمت فایل انتخابی مناسب نیست، فرمت های قابل قبول (jpg, jpeg, png, gif, bmp) میباشند.");
                            return View(slideShowViewModel);
                        }

                        if (SlideShowPictrure.ContentLength > (4 * (1024 * 1024)))
                        {
                            ModelState.AddModelError("SlideShowPictrure",
                                "سایز فایل بیش از حد مجاز است، حداکثر سایز فایل آپلود شده 4 مگابایت می باشد.");
                            return View(slideShowViewModel);
                        }

                        if (!VectorShopUtility.IsImgDimAcceptable(SlideShowPictrure, 300, 700, 700, 1500))
                        {
                            ModelState.AddModelError("SlideShowPictrure",
                                "اندازه تصویر باید با حداقل عرض 300 و طول 700 و حداکثر عرض 700 و طول 1500 باشد.");
                            return View(slideShowViewModel);
                        }

                        var fileName = Path.GetFileName(SlideShowPictrure.FileName);

                        var path = Path.Combine(
                            Server.MapPath("~/Content/Images/SlideShow/")
                            , DateTime.Now.ToString("yyyy-dd-M--HH-mm-ss")
                            + "-"
                            + fileName);

                        slideShow.SlideShowPictrure = "/Content/Images/SlideShow/" + Path.GetFileName(path);

                        SlideShowPictrure.SaveAs(path);
                    }

                    string fullPath = Request.MapPath("~" + oldPicturePath);
                    if (System.IO.File.Exists(fullPath))
                    {
                        System.IO.File.Delete(fullPath);
                    }

                }

                _db.SaveChanges();

                return RedirectToAction("ManageSlideShow");

            }


            return View(slideShowViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var model = _db.SlideShows.Find(id);

            if (model == null)
            {
                return HttpNotFound();
            }

            try
            {
                string fullPath = Request.MapPath("~" + model.SlideShowPictrure);
                if (System.IO.File.Exists(fullPath))
                {
                    System.IO.File.Delete(fullPath);
                }

                _db.SlideShows.Remove(model);
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
