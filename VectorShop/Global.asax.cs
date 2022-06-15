using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Elmah;
using Elmah.SqlServer.EFInitializer;
using VectorShop.Helpers;
using VectorShop.Models;

namespace VectorShop
{
    public class MvcApplication : System.Web.HttpApplication
    {
        private bool _sendEmail;
        void ErrorLog_Filtering(object sender, ExceptionFilterEventArgs e)
        {
            using (var context = new ElmahContext())
            {
                //if the exception had the same message and was from last 10 min it means it's the same we dismiss it
                _sendEmail = true;

                var lastErr = context.ELMAH_Errors.OrderByDescending(m => m.TimeUtc).Take(1).SingleOrDefault();

                if (lastErr != null &&
                    (e.Exception.Message == lastErr.Message && lastErr.TimeUtc > DateTime.UtcNow.AddMinutes(-10)))
                {
                    e.Dismiss();
                    _sendEmail = false;
                }
            }

            if (e.Exception.GetBaseException() is HttpRequestValidationException) e.Dismiss();
        }

        void ErrorMail_Filtering(object sender, ExceptionFilterEventArgs e)
        {
            if (_sendEmail == false)
            {
                e.Dismiss();
            }

            if (e.Exception.GetBaseException() is FileNotFoundException) e.Dismiss();
        }

        void ErrorLog_Logged(object sender, ErrorLoggedEventArgs args)
        {
            //keep the log form the last 90 days and delete the rest
            using (var context = new ElmahContext())
            {
                var baseLineDate = DateTime.UtcNow.AddDays(-90);
                var model = context.ELMAH_Errors.Where(p => p.TimeUtc < baseLineDate);
                foreach (var item in model)
                {
                    context.ELMAH_Errors.Remove(item);
                }
                context.SaveChanges();
            }
        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            AutoMapperConfiguration.Configure();

            //it removes the X-AspNetMvc-Version from the response header
            MvcHandler.DisableMvcResponseHeader = true;

            //EncryptDecryptWebConfig.EncryptConnString();
            //EncryptDecryptWebConfig.EncryptMailSettings();

            //EncryptDecryptWebConfig.DecryptConnString();
            //EncryptDecryptWebConfig.DecryptMailSettings();

            //Database.SetInitializer(
            //    new MigrateDatabaseToLatestVersion<VectorShopDb, Migrations.VectorShopDb.Configuration>());
            //Database.SetInitializer(
            //    new MigrateDatabaseToLatestVersion<ApplicationDbContext, Migrations.IdentityDbContext.Configuration>());
        }

        protected void Application_PreSendRequestHeaders()
        {
            Response.Headers.Remove("Server");
            Response.Headers.Remove("X-AspNet-Version");
            Response.Headers.Remove("X-AspNetMvc-Version");
            Response.Headers.Add("X-Frame-Options", "SAMEORIGIN");
            Response.AppendHeader("X-Development-By", "Hamid Mosalla => WebFor.ir");
        }
    }
}
