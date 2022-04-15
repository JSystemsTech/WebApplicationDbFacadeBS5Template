using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;

namespace WebApplicationDbFacadeBS5Template.Extensions
{
    public static class ThemeStyles
    {
        public static IHtmlString Render(string theme, bool darkMode = false)
        => Styles.Render(BundleConfig.GetThemeBundle(theme, darkMode));
    }
    public static class ViewScripts
    {
        public static IHtmlString Render(string controller, string view)
        => Scripts.Render(BundleConfig.GetJsBundle(controller, view));
    }
}