using System.Web;
using System.Web.Optimization;

namespace CFC
{
    public class BundleConfig
    {
        // 如需統合的詳細資訊，請瀏覽 https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // 使用開發版本的 Modernizr 進行開發並學習。然後，當您
            // 準備好可進行生產時，請使用 https://modernizr.com 的建置工具，只挑選您需要的測試。
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            
            bundles.Add(new StyleBundle("~/Scripts/gis/b3/style/css").Include( //路徑須符合b3/css/bootstrap.css可載入字型檔b3/fonts
                      "~/Scripts/gis/b3/css/bootstrap.css"
                      ));
            bundles.Add(new StyleBundle("~/Content/css/site").Include(
                      "~/Content/prj/site.css"
                      ));
            //Dou
            bundles.Add(new Bundle("~/bundles/bootstrap_dou").Include(
                      "~/ddou_Scripts/bootstrap.bundle.min.js"));

            bundles.Add(new StyleBundle("~/Content/css_dou").Include(
                      "~/ddou_Content/bootstrap.css"
                      ));

            bundles.Add(new ScriptBundle("~/dou/js").Include(
                      "~/Scripts/gis/bootstraptable/bootstrap-table.js",
                      "~/Scripts/gis/bootstraptable/extensions/mobile/bootstrap-table-mobile.js",
                      "~/ddou_Scripts/gis/select/bselect/bootstrap-select.min.js",
                      "~/Scripts/Dou/datetimepicker/js/moment.js",
                      //"~/ddou_Scripts/Scripts/Dou/datetimepicker/js/locales.min.js",
                      "~/Scripts/Dou/datetimepicker/js/tempusdominus-bootstrap-4.min.js",
                      "~/Scripts/gis/helper.js",
                      "~/Scripts/gis/Main.js",
                       "~/Scripts/Dou/Dou.js"
                        ));


            bundles.Add(new StyleBundle("~/Scripts/gis/b3/dou/css").Include( //~/Scripts/gis/b3/dou/css為了b3/css/bootstrap.css可載入字型檔
                      "~/Scripts/gis/bootstraptable/bootstrap-table.css",
                      "~/Scripts/gis/select/bselect/bootstrap-select.min.css",
                      "~/Scripts/gis/b3/css/bootstrap.css",
                      "~/Scripts/gis/Main.css",
                       "~/Scripts/Dou/Dou.css",
                       "~/Scripts/Dou/datetimepicker/css/tempusdominus-bootstrap-4.min.css"));


            //Main
            bundles.Add(new Bundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.bundle.min.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css"
                      ));

            bundles.Add(new ScriptBundle("~/main/js").Include(
                      "~/Scripts/gis/bootstraptable/bootstrap-table.js",
                      "~/Scripts/gis/bootstraptable/extensions/mobile/bootstrap-table-mobile.js",
                      //"~/ddou_Scripts/Scripts/gis/select/bselect/bootstrap-select.min.js",
                      "~/Scripts/Dou/datetimepicker/js/moment.js",
                      //"~/Scripts/Dou/datetimepicker/js/locales.min.js",
                      "~/Scripts/Dou/datetimepicker/js/tempusdominus-bootstrap-4.min.js",
                      "~/Scripts/gis/helper.js",
                      "~/Scripts/gis/Main.js",
                       "~/Scripts/Dou/Dou.js"
                        ));
            

            bundles.Add(new StyleBundle("~/Scripts/gis/b3/main/css").Include( //~/Scripts/gis/b3/dou/css為了b3/css/bootstrap.css可載入字型檔
                      "~/Scripts/gis/bootstraptable/bootstrap-table.css",
                      "~/Scripts/gis/select/bselect/bootstrap-select.min.css",
                      "~/Scripts/gis/b3/css/bootstrap.css",
                      "~/Scripts/gis/Main.css",
                       "~/Scripts/Dou/Dou.css",
                       "~/Scripts/Dou/datetimepicker/css/tempusdominus-bootstrap-4.min.css"));

            //Dou Other
            bundles.Add(new ScriptBundle("~/dou/js/other").Include(
                      "~/Scripts/gis/helper.js"
                        ));
            bundles.Add(new StyleBundle("~/dou/css/other").Include(
                      "~/Scripts/gis/Main.css"
                        ));

            //CFC
            bundles.Add(new ScriptBundle("~/Scripts/prj/cfc").Include(
                      "~/Scripts/prj/cfc_n.js"
                        ));
            bundles.Add(new StyleBundle("~/Content/prj/cfc").Include(
                      "~/Content/prj/cfc.css"
                        ));
            //Optimizations壓縮會先讀*.min.js的檔案
            BundleTable.EnableOptimizations = false; //可由web.config設定<compilation debug="false" />
        }
    }
}
