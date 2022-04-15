using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Hosting;
using System.Web.Optimization;
using WebApplicationDbFacadeBS5Template.Services.Configuration;

namespace WebApplicationDbFacadeBS5Template
{
    public class BundleConfig
    {
        private static IDictionary<string, IEnumerable<string>> ControllerViewJSBundleMap = new Dictionary<string, IEnumerable<string>>() {
            { "Auth", new string[]{ "Index"} },
            { "Home", new string[]{ "Index","Sandbox"} },
            { "Admin", new string[]{ "Index"} },
            { "Profile", new string[]{ "Index"} }
        };
        private static string[] jsBasePaths = new string[] {
                "~/Scripts/jquery-{version}.js",
                "~/Scripts/jquery.validate*",
                "~/Scripts/bootstrap.bundle.js",
                "~/Scripts/fontawesome/all.js",
                "~/Scripts/lodash.js",
                "~/Scripts/jquery.dataTables.js",
                "~/Scripts/dataTables.bootstrap5.js",
                "~/Scripts/jszip.js",
                "~/Scripts/pdfmake/pdfmake.js",
                "~/Scripts/jszip.js",
                "~/Scripts/dataTables.buttons.js",
                "~/Scripts/buttons.colVis.js",
                "~/Scripts/buttons.html5.js",
                "~/Scripts/buttons.print.js",
                "~/Scripts/buttons.bootstrap5.js",
                "~/Scripts/dataTables.select.js",
                "~/Scripts/Chart.js/chart.js",
                "~/Scripts/moment.js",
                //load custom scripts last
                "~/Scripts/custom/pollyfill.js",
                "~/Scripts/custom/jquery.bootstrap5.js",
                "~/Scripts/custom/datatable.unobtrusive.js",

                "~/Scripts/custom/jquery.notification.js",
                "~/Scripts/custom/jquery.mailtip.bootstrap5.js",
                "~/Scripts/custom/jquery.select.bootstrap5.js",
                "~/Scripts/custom/jquery.serverside.modal.js",
                "~/Scripts/custom/jquery.validate.bootstrap5.js",
                "~/Scripts/custom/chartjs.unobtrusive.js",
                "~/Scripts/custom/checklist.unobtrusive.js",
                "~/Scripts/custom/jquery.counter.js",
                "~/Scripts/custom/jquery.ajax.setup.js",
                "~/Scripts/custom/jquery.ajax.form.js"
            };
        private static string DefaultJsBundle = $"~/bundles/js";
        private static string GetJsBundleName(string controller, string view) => $"~/bundles/{controller}-{view}-js";
        private static string GetThemeBundleName(string theme) => $"~/Content/Themes/{theme}";
        public static void RegisterBundles(BundleCollection bundles)
        {
            //BundleTable.EnableOptimizations = true;
            bundles.Add(new Bundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new Bundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new Bundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new Bundle(DefaultJsBundle).Include(jsBasePaths));
            foreach (var kv in ControllerViewJSBundleMap)
            {
                string controller = kv.Key;
                foreach (var view in kv.Value)
                {
                    bundles.Add(new Bundle(GetJsBundleName(controller, view))
                        .Include(jsBasePaths)
                        .IncludeExisting($"~/Scripts/Controllers/{controller}/{view}.js"));
                }
            }
            foreach (string theme in ApplicationConfiguration.Themes)
            {
                bundles.Add(new Bundle(GetThemeBundleName(theme)).Include(
                    $"~/Content/themes/{theme}.css",
                    "~/Content/dataTables.bootstrap5.css",
                    "~/Content/dataTables.bootstrap5.overrides.css",
                    "~/Content/buttons.bootstrap5.css"
                    ));
            }
        }
        public static string GetThemeBundle(string theme, bool darkMode = false)
        {

            string bundle = GetThemeBundleName(theme);
            string darkBundle = GetThemeBundleName($"{theme}_dark");
            return darkMode && BundleTable.Bundles.GetBundleFor(darkBundle) is Bundle ? darkBundle :
                BundleTable.Bundles.GetBundleFor(bundle) is Bundle ? bundle : GetThemeBundleName(ApplicationConfiguration.DefaultTheme);

        }
        public static string GetJsBundle(string controller, string view)
        {

            string bundle = GetJsBundleName(controller, view);
            return BundleTable.Bundles.GetBundleFor(bundle) is Bundle ? bundle : DefaultJsBundle;

        }
    }
    public static class BundleHelper
    {
        private static bool CheckExistence(string virtualPath)
        {
            int i = virtualPath.LastIndexOf('/');
            string path = HostingEnvironment.MapPath(virtualPath.Substring(0, i));
            string fileName = virtualPath.Substring(i + 1);

            bool found = Directory.Exists(path);

            if (found)
            {
                if (fileName.Contains("{version}"))
                {
                    var re = new Regex(fileName.Replace(".", @"\.").Replace("{version}", @"(\d+(?:\.\d+){1,3})"));
                    fileName = fileName.Replace("{version}", "*");
                    found = Directory.EnumerateFiles(path, fileName).Where(file => re.IsMatch(file)).FirstOrDefault() != null;
                }
                else // fileName may contain '*'
                    found = Directory.EnumerateFiles(path, fileName).FirstOrDefault() != null;
            }

            return found;
        }

        public static Bundle IncludeExisting(this Bundle bundle, params string[] virtualPaths)
        => bundle.Include(virtualPaths.Where(vp => CheckExistence(vp)).ToArray());

        public static Bundle IncludeExisting(this Bundle bundle, string virtualPath, params IItemTransform[] transforms)
        => CheckExistence(virtualPath) ? bundle.Include(virtualPath, transforms) : bundle;
    }
}