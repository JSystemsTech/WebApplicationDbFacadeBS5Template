using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace WebApplicationDbFacadeBS5Template.Extensions
{
    public static class ObjectHelpers
    {
        public static IDictionary<string, object> ToHtmlAttributesDictionary(this object htmlAttributes)
            => HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes);
        public static IDictionary<string, object> AppendHtmlAttribute(this IDictionary<string, object> htmlAttributes, string key, string value)
        {
            if (htmlAttributes.TryGetValue(key, out object currentValue))
            {
                if (currentValue is string currentValueStr)
                {
                    htmlAttributes[key] = $"{currentValueStr} {value}";
                }
            }
            else
            {
                htmlAttributes.Add(key, value);
            }
            return htmlAttributes;
        }
        public static IDictionary<string, object> AddUpdateHtmlAttribute(this IDictionary<string, object> htmlAttributes, string key, string value)
        {
            if (htmlAttributes.ContainsKey(key))
            {
                htmlAttributes[key] = value;
            }
            else
            {
                htmlAttributes.Add(key, value);
            }
            return htmlAttributes;
        }
        public static bool HasClass(this IDictionary<string, object> htmlAttributes, string value)
            => htmlAttributes.TryGetValue("class", out object currentValue) && currentValue is string currentValueStr && currentValueStr.HasClass(value);
        public static bool HasClass(this string classes, string value)
            => classes.Split(' ').Contains(value);
        public static IDictionary<string, object> AppendClass(this IDictionary<string, object> htmlAttributes, string value)
        {
            foreach (string cls in value.Split(' '))
            {
                htmlAttributes = !htmlAttributes.HasClass(cls) ? htmlAttributes.AppendHtmlAttribute("class", cls) : htmlAttributes;
            }
            return htmlAttributes;
        }

        public static RouteValueDictionary ToRouteValueDictionary(this object routeValues)
            => new RouteValueDictionary(routeValues);
        public static string ToHtmlAttributeString(this IDictionary<string, object> htmlAttributes)
            => string.Join(" ", htmlAttributes.Select(attr => $"{attr.Key}=\"{attr.Value}\""));

        public static byte[] ToArray(this HttpPostedFileBase file)
        {
            byte[] data;
            using (MemoryStream ms = new MemoryStream())
            {
                file.InputStream.CopyTo(ms);
                data = ms.ToArray();
            }
            return data;
        }
    }
}