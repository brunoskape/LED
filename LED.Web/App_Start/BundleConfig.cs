using System.Web;
using System.Web.Optimization;

namespace LED.Web
{
    public class BundleConfig
    {
        // Para obter mais informações sobre o agrupamento, visite https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use a versão em desenvolvimento do Modernizr para desenvolver e aprender. Em seguida, quando estiver
            // pronto para a produção, utilize a ferramenta de build em https://modernizr.com para escolher somente os testes que precisa.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js"));

            bundles.Add(new ScriptBundle("~/bundles/inputmask").Include(
                        "~/Scripts/inputmask/jquery.inputmask.js"));

            bundles.Add(new ScriptBundle("~/bundles/custom").Include(
                      "~/Scripts/thema/aplication.js",
                      "~/Scripts/jquery.dataTables.js",
                      "~/Scripts/thema/jquery.slimscroll.min.js",
                      "~/Scripts/thema/app.js",
                      "~/Scripts/thema/styleswitcher.js",
                      "~/Scripts/thema/font-size.js",
                      "~/Scripts/LED.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.min.css",
                      "~/Content/jquery.dataTables.min.css",
                      "~/Content/thema/aplication.css",
                      "~/Content/thema/slide-menu.css",
                      "~/Content/thema/timeline.css",
                      "~/Content/font-awesome.min.css"));
        }
    }
}
