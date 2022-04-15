using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApplicationDbFacadeBS5Template.Extensions.Html
{
    public static class TableExtensions
    {
        public static IHtmlString Table(this HtmlHelper htmlHelper, object htmlAttributes)
        {
            var attrs = htmlAttributes.ToHtmlAttributesDictionary().AppendClass("table");
            return new HtmlString($"<table {attrs.ToHtmlAttributeString()}></table>");
        }
    }
}