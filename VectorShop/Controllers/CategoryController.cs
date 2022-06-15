using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using VectorShop.Models;
using VectorShop.Models.ViewModels;

namespace VectorShop.Controllers
{
    [Authorize(Roles = "admin")]
    public class CategoryController : Controller
    {
        private VectorShopDb _db = new VectorShopDb();

        [ChildActionOnly]
        public PartialViewResult ShowPriCatSecCat()
        {
            var priCat = _db.PriCats.ToList();
            var secCat = _db.SecCats.ToList();
            var model = new PriCatSecCatViewModel { PriCat = priCat, SecCat = secCat };

            return PartialView("_ShowPriCatSecCat", model);
        }

        [HttpGet]
        public ActionResult Create()
        {

            var listItems = new List<SelectListItem>();

            foreach (var item in _db.PriCats)
            {
                listItems.Add(new SelectListItem
                {
                    Text = item.PriCatTitle,
                    Value = item.PriCatId.ToString()
                });
            }

            ViewBag.priCatList = listItems;

            //var priCatModel = new PriCatViewModel(); return View(priCatModel); we don't need to pass a new model in create
            //for when it is Get, it is redundant and stupid.

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(PriCatViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                if (viewModel.PriCatId == null)
                {
                    var priCat = new PriCat();
                    priCat.PriCatTitle = viewModel.PriCatTitle;
                    priCat.PriCatDesc = viewModel.PriCatDesc;
                    _db.PriCats.Add(priCat);
                    _db.SaveChanges();
                    return RedirectToAction("Create");
                }
                else
                {
                    var secCat = new SecCat();
                    secCat.SecCatTitle = viewModel.PriCatTitle;
                    secCat.SecCatDesc = viewModel.PriCatDesc;
                    secCat.PriCatIDfk = int.Parse(viewModel.PriCatId);
                    _db.SecCats.Add(secCat);
                    _db.SaveChanges();
                    return RedirectToAction("Create");
                }
            }
            return View(viewModel);

        }

        [HttpGet]
        public ActionResult Edit(int? priCatId, int? secCatId)
        {
            if (priCatId == null && secCatId == null)
            {
                return HttpNotFound();
            }


            var model = new PriCatSecCatViewModel();

            if (priCatId.HasValue)
            {
                PriCat priCat = _db.PriCats.Find(priCatId);
                model.SPriCat = priCat;
            }
            else
            {
                SecCat secCat = _db.SecCats.Find(secCatId);
                model.SSecCat = secCat;
            }
            return View(model);




            //priCat.PriCatTitle = viewModel.PriCatTitle;
            //priCat.PriCatDesc = viewModel.PriCatDesc;
            //_db.PriCats.Add(priCat);
            //_db.SaveChanges();


            //var secCat = new SecCat();
            //secCat.SecCatTitle = viewModel.PriCatTitle;
            //secCat.SecCatDesc = viewModel.PriCatDesc;
            //secCat.PriCatIDfk = int.Parse(viewModel.PriCatId);
            //_db.SecCats.Add(secCat);
            //_db.SaveChanges();
            //return RedirectToAction("Index");





        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(PriCatSecCatViewModel viewModel)
        {
            if (ModelState.IsValid)
            {

                if (viewModel.SPriCat != null)
                {
                    PriCat priCat = _db.PriCats.Find(viewModel.SPriCat.PriCatId);
                    priCat.PriCatTitle = viewModel.SPriCat.PriCatTitle;
                    priCat.PriCatDesc = viewModel.SPriCat.PriCatDesc;
                    _db.SaveChanges();
                    return RedirectToAction("Create");
                }
                else
                {
                    SecCat secCat = _db.SecCats.Find(viewModel.SSecCat.SecCatId);
                    secCat.SecCatTitle = viewModel.SSecCat.SecCatTitle;
                    secCat.SecCatDesc = viewModel.SSecCat.SecCatDesc;
                    _db.SaveChanges();
                    return RedirectToAction("Create");
                }

            }
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int? priCatId, int? secCatId)
        {
            if (priCatId.HasValue)
            {
                try
                {
                    var secItems = _db.SecCats
                        .Include(p => p.Products)
                        .Include(p => p.Orders)
                        .Include(p=>p.NewsLetters)
                        .Where(s => s.PriCatIDfk == priCatId);

                    foreach (var item in secItems)
                    {
                        _db.SecCats.Remove(item);
                    }

                    PriCat priCat =
                        _db.PriCats
                        .Include(p => p.Products)
                        .Include(p => p.Orders)
                        .Include(p=>p.NewsLetters)
                        .Single(p => p.PriCatId == priCatId);

                    //I should load the children into context otherwise EF doesn't inset null for me

                    _db.PriCats.Remove(priCat);

                    _db.SaveChanges();
                    //pass CatType and CatId as json to javascript and delete the seccat that have the id of its parent
                    //we defined it in _ShowPriCatSecCat that every parent and child should have its corresponding class
                    //based on CatId
                    return Json(new { Status = "Deleted", CatType = "PriCat", CatId = priCatId });
                }
                catch (Exception eX)
                {
                    return Json(new { Status = "Error", eXMessage = eX.Message });
                }
            }
            else
            {
                try
                {
                    SecCat secCat = _db.SecCats
                        .Include(s => s.Products)
                        .Include(s => s.Orders)
                        .Include(s=>s.NewsLetters)
                        .Single(s => s.SecCatId == secCatId);

                    //I should load the children into context otherwise EF doesn't inset null for me

                    _db.SecCats.Remove(secCat);

                    _db.SaveChanges();

                    return Json(new { Status = "Deleted" });
                }
                catch (Exception eX)
                {
                    return Json(new { Status = "Error", eXMessage = eX.Message });
                }

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
