using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace VectorShop
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapMvcAttributeRoutes();

            // elFinder's connector route
            routes.MapRoute(null, "connector", new { controller = "File", action = "Index" });

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}/{title}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional, title=UrlParameter.Optional }
            );
        }
    }
}
