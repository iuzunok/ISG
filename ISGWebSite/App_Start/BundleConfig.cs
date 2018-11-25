using System.Web;
using System.Web.Optimization;

namespace ISGWebSite
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

            bundles.Add(new ScriptBundle("~/bundles/angular").Include(
                      "~/Scripts/angular.js",
                      "~/Scripts/angular-bootstrap.js",
                      "~/Scripts/angular-ui/ui-bootstrap-tpls.js"/*,

                      "~/Scripts/angular-touch.js",
                      "~/Scripts/angular-animate.js",
                      "~/Scripts/angular-aria.js",
                      "~/Scripts/ui-grid.js" */                     
                      ));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/font-awesome.css",
                      //"~/Content/site.css"
                      "~/CSS/Main.css"));



            bundles.Add(new ScriptBundle("~/bundles/JS/Excel").Include(
              "~/JS/Excel/shim.min.js",
              "~/JS/Excel/xlsx.full.min.js",
              "~/JS/PDF/jspdf.min.js"
              ));



            bundles.Add(new ScriptBundle("~/bundles/JS/Genel").Include(
                      "~/JS/Genel/Genel.js"));

            bundles.Add(new ScriptBundle("~/ClientSide/AngularVeriGetir").Include(
                // "~/ClientSide/DirPagination.js",
                "~/ClientSide/AngularVeriGetirIFrmApp.js",
                "~/ClientSide/AngularVeriGetirIFrm.js"
                ));

            bundles.Add(new ScriptBundle("~/ClientSide/AngularLogin").Include(
                "~/ClientSide/AngularLogin.js"));



            /*bundles.Add(new ScriptBundle("~/bundles/bootstrap-datepicker").Include(
                     "~/Scripts/bootstrap-datepicker*",
                     "~/Scripts/locales/bootstrap-datepicker.tr.min.js",
                     "~/Scripts/jquery.globalize/cultures/globalize.culture.tr*"
                     ));

            bundles.Add(new StyleBundle("~/Contents/css").Include(
                      "~/Contents/bootstrap-datepicker*"
                      ));*/
        }
    }
}
