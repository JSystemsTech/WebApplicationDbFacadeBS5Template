using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplicationDbFacadeBS5Template.Extensions
{
    public static class HttpCookieExtensions
    {
        public static HttpCookie EmptyCookie = default(HttpCookie);

        public static string GetHttpCookieValue(this HttpRequestBase request, string name)
        {
            if(request.Cookies.Get(name) is HttpCookie cookie && !string.IsNullOrWhiteSpace(cookie.Value))
            {
                return cookie.Value;
            }
            else if(request.Headers.Get("Cookie") is string cookiesStr && !string.IsNullOrWhiteSpace(cookiesStr))
            {
                var cookies = cookiesStr.Split(';')
                    .Select(c => c.Split(new[] { '=' }, 2))
                    .Select(c=>new KeyValuePair<string,string>(c[0].Trim(), c[1].Trim()));

                if (cookies.FirstOrDefault(kv => kv.Key == name) is KeyValuePair<string, string> headerCookie && !string.IsNullOrWhiteSpace(headerCookie.Value)) {
                    return headerCookie.Value;
                }
            }
            return null;
        }
        public static void AddHttpCookie(this HttpResponseBase response, string name, string value, DateTime? expirationDate= null, bool httpOnly = true)
        => response.Cookies.Add(
            expirationDate is DateTime expires ? 
            new HttpCookie(name)
            {
                Value = value,
                HttpOnly = httpOnly,
                Secure = true,
                SameSite = SameSiteMode.Lax,
                Expires = expires
            }: 
            new HttpCookie(name)
            {
                Value = value,
                HttpOnly = httpOnly,
                Secure = true,
                SameSite = SameSiteMode.Lax
            }
            );
        public static void ExpireHttpCookie(this HttpResponseBase response, string name)
            => response.AddHttpCookie(name, string.Empty, DateTime.Now.AddDays(-1));
    }
}