using System.Web;
using System.Web.Optimization;

namespace EbaySalesTracker
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/thirdParty").Include(
                        "~/Scripts/Libs/jquery-{version}.js",
                        "~/Scripts/Libs/modernizr-{version}.js",
                        "~/Scripts/Libs/bootstrap.js",
                        "~/Scripts/Libs/respond.js",                        
                        "~/Scripts/Libs/jquery-ui-{version}.js",
                        "~/Scripts/Libs/jquery.easing.1.3.js",
                        "~/Scripts/ui.helpers.js",
                        "~/Scripts/bootstrap3-editable/js/bootstrap-editable.js"
                       ));

            bundles.Add(new ScriptBundle("~/bundles/eptJS").Include(
                    "~/Scripts/scene.startup.js",
                    "~/Scripts/scene.statemanager.js",
                    "~/Scripts/scene.layoutservice.js",
                    "~/Scripts/scene.dataservice.js",
                    "~/Scripts/scene.tile.binder.js",
                    "~/Scripts/scene.tile.renderer.js",
                    "~/Scripts/ebaysalestracker_validation.js"
                ));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
            "~/Scripts/jquery.validate*"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/css/font-awesome.min.css",
                      "~/Content/bootstrap3-editable/css/bootstrap-editable.css",
                      "~/Content/site.css"));
        }
    }
}
