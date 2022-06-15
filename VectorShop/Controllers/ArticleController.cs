using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using MvcSiteMapProvider.Web.Mvc.Filters;
using PagedList;
using VectorShop.Models;
using VectorShop.Models.ViewModels;

namespace VectorShop.Controllers
{
    [Authorize(Roles="admin")]
    public class ArticleController : Controller
    {
        private VectorShopDb _db = new VectorShopDb();

        [AllowAnonymous]
        [Route("Article/Index/{page?}")]
        public ActionResult Index(int? page)
        {
            var model = _db.Articles.OrderByDescending(c => c.ArticleId);
            var pageNumber = page ?? 1;
            var onePageOfArticles = model.ToPagedList(pageNumber, 2);
            //TODO: Increase the paging size when project finished.

            return View(onePageOfArticles);
        }

        [AllowAnonymous]
        public PartialViewResult ArticlePartial()
        {
            var model = _db.Articles.OrderByDescending(a=>a.ArticleId).Take(10);
            return PartialView("_ArticlePartial", model);
        }

        [Route("Article/ManageArticle/{page?}")]
        public ActionResult ManageArticle(int? page)
        {
            var model = _db.Articles.OrderByDescending(c => c.ArticleId);
            var pageNumber = page ?? 1;
            var onePageOfArticles = model.ToPagedList(pageNumber, 2);
            //TODO: Increase the paging size when project finished.

            return View(onePageOfArticles);
        }

        [AllowAnonymous]
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var model = await _db.Articles.FindAsync(id);

            if (model == null)
            {
                return HttpNotFound();
            }

            return View(model);
        }


        public ActionResult Create()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [SiteMapCacheRelease]
        [ValidateInput(false)]
        public ActionResult Create(ArticleViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var article = new Article { ArticleTitle = viewModel.ArticleTitle, ArticleDate = DateTime.Now, ArticleBody = viewModel.ArticleBody };
                _db.Articles.Add(article);
                _db.SaveChanges();
                return RedirectToAction("ManageArticle");
            }
            return View(viewModel);
        }


        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var model = _db.Articles.Find(id);

            if (model == null)
            {
                return HttpNotFound();
            }

            var viewModel = Mapper.Map<Article, ArticleViewModel>(model);

            return View(viewModel);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [SiteMapCacheRelease]
        [ValidateInput(false)]
        public ActionResult Edit(ArticleViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var article = _db.Articles.Find(viewModel.ArticleId);
                article.ArticleTitle = viewModel.ArticleTitle;
                article.ArticleBody = viewModel.ArticleBody;
                _db.SaveChanges();
                return RedirectToAction("ManageArticle");
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

            var model = _db.Articles.Find(id);

            if (model == null)
            {
                return HttpNotFound();
            }

            try
            {
                _db.Articles.Remove(model);
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
