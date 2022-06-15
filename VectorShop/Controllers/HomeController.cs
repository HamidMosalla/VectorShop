using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using PagedList;
using VectorShop.Models;
using VectorShop.Models.ViewModels;

namespace VectorShop.Controllers
{
    public class HomeController : Controller
    {
        private VectorShopDb _db = new VectorShopDb();
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult NotFound()
        {
            return View();
        }
        public ActionResult Search(string q, int? page, string proSort, string catSort)
        {
            IQueryable<Product> model;
            var cat = _db.PriCats;
            int catInt;
            int.TryParse(catSort, out catInt);
                
            model = proSort == "Newest" ?
                _db.Products.OrderByDescending(p => p.ProductId).Where(p => p.ProductName.Contains(q)) :
                        proSort == "Oldest" ?
                        _db.Products.OrderBy(p => p.ProductId).Where(p => p.ProductName.Contains(q)):
                        proSort == "Price" ?
                        _db.Products.OrderByDescending(p => p.ProductPrice).Where(p => p.ProductName.Contains(q))
                : _db.Products.OrderByDescending(p => p.ProductId).Where(p => p.ProductName.Contains(q));

            if (catInt != 0)
            {
                model = proSort == "Newest" ?
                _db.Products.OrderByDescending(p => p.ProductId).Where(p => p.ProductName.Contains(q) && p.PriCatIDfk == catInt) :
                        proSort == "Oldest" ?
                        _db.Products.OrderBy(p => p.ProductId).Where(p => p.ProductName.Contains(q) && p.PriCatIDfk == catInt) :
                        proSort == "Price" ?
                        _db.Products.OrderByDescending(p => p.ProductPrice).Where(p => p.ProductName.Contains(q) && p.PriCatIDfk == catInt)
                : _db.Products.OrderByDescending(p => p.ProductId).Where(p => p.ProductName.Contains(q) && p.PriCatIDfk == catInt);
            }


            ViewBag.ProductCount = model.Count();
            ViewBag.Category = cat;
            var pageNumber = page ?? 1;
            var onePageOfProduct = model.ToPagedList(pageNumber, 2);
            //TODO: Increase the paging size when project finished.

            return Request.IsAjaxRequest()
                ? (ActionResult)PartialView("_PagingPartial", onePageOfProduct)
                : View(onePageOfProduct);
        }

        public JsonResult AutoCompleteSearch(string term)
        {
            var result = (from r in _db.Products
                          where r.ProductName.ToLower().Contains(term.ToLower())
                          select new { r.ProductName }).Distinct();

            //var result = _db.Products.Where(
            //    p => p.ProductName.ToLower().Contains(term.ToLower())).Distinct().Select(p => new {p.ProductName});
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        
        public ActionResult About()
        {
            var modle = _db.ControlPanels.OrderByDescending(c => c.About).Single();

            return View(modle);
        }

        [HttpGet]
        [Authorize(Roles = "admin")]
        public ActionResult AboutEdit()
        {
            var model = _db.ControlPanels.OrderByDescending(c => c.ControlPanelId).Single();
            var viewModel = Mapper.Map<ControlPanel, ControlPanelViewModel>(model);

            return View(viewModel);
        }

        [HttpPost]
        [ValidateInput(false)]
        [Authorize(Roles = "admin")]
        public ActionResult AboutEdit(ControlPanelViewModel viewModel)
        {
            var model = _db.ControlPanels.Find(viewModel.ControlPanelId);
            var slideShowNumber = ModelState["SlideShowNumber"];
            slideShowNumber.Errors.Clear();

            if (ModelState.IsValid)
            {
                model.About = viewModel.About;
                _db.SaveChanges();
                return RedirectToAction("About");
            }

            return View(viewModel);
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