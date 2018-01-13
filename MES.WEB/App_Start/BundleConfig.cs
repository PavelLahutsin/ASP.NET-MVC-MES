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
                "~/Scripts/dist/js/adminlte.js"));

            

            bundles.Add(new ScriptBundle("~/bundles/jquery-val").Include(
                "~/Scripts/jquery.validate*",
                "~/Scripts/jquery.unobtrusive-ajax.min.js"
            ));

            bundles.Add(new ScriptBundle("~/bundles/DataTables").Include(
                "~/Scripts/DataTables/jquery.dataTables.min.js",
                "~/Scripts/DataTables/dataTables.bootstrap.min.js"));

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

            bundles.Add(new StyleBundle("~/Content/css").Include(
                "~/Content/bootstrap.css",
                "~/Content/font-awesome/css/font-awesome.css",
                "~/Content/dist/css/AdminLTE.css",
                "~/Content/dist/css/skins/_all-skins.css"));

            bundles.Add(new StyleBundle("~/Data&Date/css").Include(
                "~/Content/DataTables/css/dataTables.bootstrap.min.css",
                "~/Content/bootstrap-datepicker/bootstrap-datepicker.min.css"));

        }
    }
}