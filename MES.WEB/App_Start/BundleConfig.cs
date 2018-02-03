using System.Web.Optimization;

namespace MES.WEB
{
    public class BundleConfig
    {
        // Дополнительные сведения об объединении см. на странице https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                "~/Scripts/jquery-{version}.js",
                "~/Scripts/dist/js/adminlte.js",
                "~/Scripts/jquery.validate*",
                "~/Scripts/jquery.unobtrusive-ajax.js"
               ));

            bundles.Add(new ScriptBundle("~/bundles/DataTables").Include(
                "~/Scripts/DataTables/jquery.dataTables.js",
                "~/Scripts/DataTables/dataTables.bootstrap.js"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap-datepicker").Include(
                "~/Scripts/bootstrap-datepicker/bootstrap-datepicker.js",
                "~/Scripts/bootstrap-datepicker/locales/bootstrap-datepicker.ru.min.js"));


            // Используйте версию Modernizr для разработчиков, чтобы учиться работать. Когда вы будете готовы перейти к работе,
            // готово к выпуску, используйте средство сборки по адресу https://modernizr.com, чтобы выбрать только необходимые тесты.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                "~/Scripts/bootstrap.js",
                "~/Scripts/respond.js"));

            bundles.Add(new ScriptBundle("~/bundles/mes").Include(
                "~/Scripts/mes.js"));


            bundles.Add(new StyleBundle("~/Content/css").Include(
                "~/Content/bootstrap.css",
                "~/Content/font-awesome/css/font-awesome.css",
                "~/Content/dist/css/AdminLTE.css",
                "~/Content/dist/css/skins/_all-skins.css",
                "~/Content/DataTables/css/dataTables.bootstrap.css",
                "~/Content/bootstrap-datepicker/bootstrap-datepicker.css"));
        }
    }
}
