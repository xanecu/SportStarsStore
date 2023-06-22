using System.Web;
using System.Web.Optimization;

namespace SportStars
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/slider").Include(
                        "~/ScriptsIII/jquery-3.5.1.min.js",
                        "~/ScriptsIII/lightslider.js"));

            bundles.Add(new ScriptBundle("~/bundles/valida").Include(
                             "~/ScriptsII/jquery-3.5.1.min.js",
                              "~/ScriptsII/additional-methods.js",
                             "~/ScriptsII/jquery.validate.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include( //nome do molho
                      "~/Content/bootstrap.css", //ficheiro do molho
                      "~/Content/site.css")); //ficheiro do molho onde defenir style, background por ex.
        }
    }
}
