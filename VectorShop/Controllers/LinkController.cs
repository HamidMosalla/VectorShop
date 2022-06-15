using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using PagedList;
using VectorShop.Models;
using VectorShop.Models.ViewModels;

namespace VectorShop.Controllers
{
    [Authorize(Roles = "admin")]
    public class LinkController : Controller
    {
        private VectorShopDb _db = new VectorShopDb();

        [Route("Link/ManageLink/{page?}")]
        public ActionResult ManageLink(int? page)
        {
            var model = _db.Links.OrderByDescending(c => c.LinkId);
            var pageNumber = page ?? 1;
            var onePageOfLinks = model.ToPagedList(pageNumber, 5);
            //TODO: Increase the paging size when project finished.

            return View(onePageOfLinks);
        }

        [AllowAnonymous]
        public PartialViewResult LinkPartial()
        {
            var model = _db.Links.OrderBy(l=>l.LinkPriority).ThenBy(l=>l.LinkTitle).Take(10);
            return PartialView("_LinkPartial", model);
        }


        public ActionResult Create()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(LinkViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var link = new Link
                {
                    LinkTitle = viewModel.LinkTitle,
                    LinkPriority = viewModel.LinkPriority,
                    LinkDesc = viewModel.LinkDesc,
                    LinkUrl = viewModel.LinkUrl
                };
                _db.Links.Add(link);
                _db.SaveChanges();
                return RedirectToAction("ManageLink");
            }

            return View(viewModel);
        }


        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var model = _db.Links.Find(id);
            var viewModel = Mapper.Map<Link, LinkViewModel>(model);

            if (model == null)
            {
                return HttpNotFound();
            }

            return View(viewModel);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(LinkViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var model = _db.Links.Find(viewModel.LinkId);
                model.LinkDesc = viewModel.LinkDesc;
                model.LinkPriority = viewModel.LinkPriority;
                model.LinkTitle = viewModel.LinkTitle;
                model.LinkUrl = viewModel.LinkUrl;
                _db.SaveChanges();
                return RedirectToAction("ManageLink");
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

            var model = _db.Links.Find(id);

            if (model == null)
            {
                return HttpNotFound();
            }

            try
            {
                _db.Links.Remove(model);
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
