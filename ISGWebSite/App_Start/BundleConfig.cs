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
                        "~/Scripts/jquery-{version}.js",
                        "~/Scripts/jquery-ui-{version}.js"));

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
                      "~/Content/font-awesome.css").IncludeDirectory("~/CSS", "*.css", true));

            bundles.Add(new ScriptBundle("~/bundles/Scripts/pdfmake").Include(
              "~/Scripts/pdfmake/pdfmake.min.js",
              "~/Scripts/pdfmake/vfs_fonts.js"
              ));

            bundles.Add(new ScriptBundle("~/bundles/JS/Excel").Include(
              "~/JS/Excel/shim.min.js",
              "~/JS/Excel/xlsx.full.min.js"
              ));



            bundles.Add(new ScriptBundle("~/bundles/JS/Genel").Include(
                      "~/JS/Genel/Genel.js",
                      "~/JS/ArgemTool/Globalization/tr-TR.js",
                      "~/JS/ArgemTool/ArgemJSUtil.js",
                      "~/JS/ArgemTool/ArgemPopUp.js",
                      "~/JS/ArgemTool/ArgemDDLText.js"));





            bundles.Add(new ScriptBundle("~/JSNG/Yetki/Login").Include(
              "~/JSNG/Yetki/Login.js"));

            bundles.Add(new ScriptBundle("~/JSNG/Yetki/AnaSayfa").Include(
             "~/JSNG/Yetki/AnaSayfa.js"));


            //bundles.Add(new ScriptBundle("~/ClientSide/AngularVeriGetir").Include(
            //    "~/ClientSide/AngularVeriGetirIFrmApp.js",
            //    "~/ClientSide/AngularVeriGetirIFrm.js"
            //    ));
            bundles.Add(new ScriptBundle("~/JSNG/Yetki/Kullanici").Include(
             "~/JSNG/Yetki/Kullanici.js"));

            bundles.Add(new ScriptBundle("~/JSNG/Yetki/YetkiGrup").Include(
                "~/JSNG/Yetki/YetkiGrup.js"));

            bundles.Add(new ScriptBundle("~/JSNG/Yetki/YetkiVer").Include(
             "~/JSNG/Yetki/YetkiVer.js"));



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
