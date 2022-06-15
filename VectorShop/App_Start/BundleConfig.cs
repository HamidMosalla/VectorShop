using System.Web;
using System.Web.Optimization;

namespace VectorShop
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js",
                        "~/Scripts/megamenu.js",
                        "~/Scripts/Site.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));



            #region elFinder and jquery-ui bundles

            bundles.Add(new ScriptBundle("~/bundles/jquery-ui").Include(
                        "~/Scripts/jquery-ui.js"));

            bundles.Add(new StyleBundle("~/Content/jquery-ui").Include(
                      "~/Content/jquery-ui/jquery-ui.css",
                      "~/Content/jquery-ui/jquery-ui.theme.css"
                      ));

            bundles.Add(new ScriptBundle("~/Scripts/elfinder").Include(
                             "~/Content/elfinder/js/elfinder.full.js"
                              //,"~/Content/elfinder/js/i18n/elfinder.fa.js"
                             ));

            bundles.Add(new StyleBundle("~/Content/elfinder").Include(
                            "~/Content/elfinder/css/elfinder.full.css",
                            "~/Content/elfinder/css/theme.css"));

            #endregion

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js",
                      "~/Scripts/pnotify.custom.min.js"
                      ));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/bootstrap-rtl.css",
                      "~/Content/site.css",
                      "~/Content/megamenu.css",
                      "~/Content/bootstrapCustomize.css",
                      "~/Content/pnotify.custom.min.css"
                      ));

            // Set EnableOptimizations to false for debugging. For more information,
            // visit http://go.microsoft.com/fwlink/?LinkId=301862
            BundleTable.EnableOptimizations = false;
            //TODO don't forget to set it to true for deployment.
        }
    }
}
