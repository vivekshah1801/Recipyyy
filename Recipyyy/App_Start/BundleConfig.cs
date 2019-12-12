using System.Web;
using System.Web.Optimization;

namespace Recipyyy
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));
            // November Edit
            bundles.Add(new ScriptBundle("~/bundles/themeCustomScriptsBundle").Include(
                      "~/Scripts/themeCustomStyles/aos.js",
                      "~/Scripts/themeCustomStyles/bootstrap-datepicker.js",
                      "~/Scripts/themeCustomStyles/cl-switch.js",
                      "~/Scripts/themeCustomStyles/isotope.min.js",
                      "~/Scripts/themeCustomStyles/jquery.counterup.min.js",
                      "~/Scripts/themeCustomStyles/jQuery.style.switcher.js",
                      "~/Scripts/themeCustomStyles/owl.carousel.min.js",
                      "~/Scripts/themeCustomStyles/perfect-scrollbar.jquery.min.js",
                      "~/Scripts/themeCustomStyles/popper.min.js",
                      "~/Scripts/themeCustomStyles/select2.min.js",
                      "~/Scripts/themeCustomStyles/slick.js",
                      "~/Scripts/themeCustomStyles/summernote.js",
                      "~/Scripts/themeCustomStyles/custom.js"
                      ));

            
            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/site.css",
                      "~/Content/bootstrap.css"
                      ));

            // November Edit
            bundles.Add(new StyleBundle("~/Content/customCss").Include(
                      "~/Content/themeCustom/default.css", 
                      "~/Content/themeCustom/plugins.css", 
                      "~/Content/themeCustom/styles.css"
                      ));
        }
    }
}
