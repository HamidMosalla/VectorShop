using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using VectorShop.Models;
using VectorShop.Models.ViewModels;
using System.Security.Cryptography.X509Certificates;
using Google.Apis.Analytics.v3;
using Google.Apis.Analytics.v3.Data;
using Google.Apis.Services;
using Google.Apis.Auth.OAuth2;

namespace VectorShop.Controllers
{
    [Authorize(Roles = "admin")]
    public class ControlPanelController : Controller
    {
        private VectorShopDb _db = new VectorShopDb();
        public ActionResult Index()
        {
            var productModel = _db.Products.OrderByDescending(p => p.ProductId).Take(5);
            var articleModel = _db.Articles.OrderByDescending(a => a.ArticleId).Take(5);
            ViewBag.Products = productModel;
            ViewBag.Article = articleModel;

            return View();
        }

        [HttpGet]
        public ActionResult SlideShowNumber()
        {
            var model = _db.ControlPanels.OrderByDescending(c => c.ControlPanelId).Single();
            var viewModel = Mapper.Map<ControlPanel, ControlPanelViewModel>(model);
            return View(viewModel);
        }

        [HttpPost]
        public ActionResult SlideShowNumber(ControlPanelViewModel viewModel)
        {
            //don't forget to clear the error if you add any new column in the future
            var aboutValidation = ModelState["About"];
            aboutValidation.Errors.Clear();
            if (ModelState.IsValid)
            {
                var model = _db.ControlPanels.Find(viewModel.ControlPanelId);
                model.SlideShowNumber = viewModel.SlideShowNumber;
                _db.SaveChanges();
            }
            return View();
        }


        public ActionResult GoogleAnalytics(string filter, int? maxResult, int days = 0)
        {
            var path = Server.MapPath("~/Content/privatekey.p12");
            if (!System.IO.File.Exists(path))
            {
                throw new FileNotFoundException("The file containing the private key does not exist.");
            }

            //UPDATE this to match the path to your p12 certificate
            var certificate = new X509Certificate2(path, "notasecret", X509KeyStorageFlags.MachineKeySet | X509KeyStorageFlags.PersistKeySet | X509KeyStorageFlags.Exportable);

            //OAuth Service account Email address
            var credential = new ServiceAccountCredential(new ServiceAccountCredential.Initializer("738851578499-l72ijr35mud9tfa9jhk1pobn1nftjefj@developer.gserviceaccount.com")
            {
                Scopes = new[] { AnalyticsService.Scope.Analytics }
            }.FromCertificate(certificate));

            var analyticsService = new AnalyticsService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = "VectorShop"
            });

            DataResource.GaResource.GetRequest analyticQuery = null;

            if (days == 0)
            {
                analyticQuery =
                    analyticsService.Data.Ga.Get(
                    "ga:104731600", "2014-07-01", DateTime.Now.ToString("yyyy-MM-dd"), "ga:pageviews, ga:uniquePageviews");
            }

            if (days != 0)
            {
                //UPDATE the 104731600 string to match your profile Id from Google Analytics
                analyticQuery =
                    analyticsService.Data.Ga.Get(
                    "ga:104731600", DateTime.Now.AddDays(-days).ToString("yyyy-MM-dd"), DateTime.Now.ToString("yyyy-MM-dd"), "ga:pageviews, ga:uniquePageviews");
            }

            analyticQuery.Dimensions = "ga:pagePath";
            analyticQuery.MaxResults = 10;
            if (maxResult.HasValue) { analyticQuery.MaxResults = maxResult; }

            analyticQuery.Sort = "-ga:pageviews";

            if (!string.IsNullOrEmpty(filter))
            {
                analyticQuery.Filters = "ga:pagePath=~/" + filter;
            }

            GaData returnedData = analyticQuery.Execute();

            var page = new List<string>();
            var pageviews = new List<string>();
            var uniquePageviews = new List<string>();

            foreach (var item in returnedData.Rows)
            {
                page.Add(item[0]);
                pageviews.Add(item[1]);
                uniquePageviews.Add(item[2]);
            }

            ViewBag.Page = page;
            ViewBag.Pageviews = pageviews;
            ViewBag.UniquePageviews = uniquePageviews;

            return View();
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
