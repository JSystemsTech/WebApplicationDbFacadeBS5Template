using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplicationDbFacadeBS5Template.Services.Configuration;

namespace WebApplicationDbFacadeBS5Template.Extensions.Html
{
    public static class ApplicationVersionExtensions
    {
        public static string ApplicationVersion(this HtmlHelper htmlHelper)
        => htmlHelper.ViewContext.Controller.GetType().Assembly.GetName().Version.ToString();
        public static string ApplicationName(this HtmlHelper htmlHelper)
        => ApplicationConfiguration.ApplicationName;
        public static string ApplicationEnvironment(this HtmlHelper htmlHelper)
        => ApplicationConfiguration.ShowEnvironmentName ? ApplicationConfiguration.EnvironmentName : "";

        public static string ApplicationNameEnvironment(this HtmlHelper htmlHelper)
        {
            string env = ApplicationConfiguration.ShowEnvironmentName ? $" - {ApplicationConfiguration.EnvironmentName}" : "";
            return $"{htmlHelper.ApplicationName()}{env}";
        }
        public static string ApplicationNameVersionEnvironment(this HtmlHelper htmlHelper)
        {
            string env = ApplicationConfiguration.ShowEnvironmentName ? $" - {ApplicationConfiguration.EnvironmentName}" : "";
            return $"{htmlHelper.ApplicationName()}{env} v{htmlHelper.ApplicationVersion()}";
        }

    }
}