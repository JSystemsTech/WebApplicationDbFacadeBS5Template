using System;
using System.Web;
using WebApplicationDbFacadeBS5Template.Services.Configuration;

namespace WebApplicationDbFacadeBS5Template.Extensions
{
    public static class AuthenticationCookieExtensions
    {
        public static bool TryGetAuthenticationCookie(this HttpRequestBase request, out string value)
        {            
            value = request.GetHttpCookieValue(ApplicationConfiguration.AuthenticationCookieName);
            return true;
        }
        public static void AddAuthenticationCookie(this HttpResponseBase response, string value)
        {
            response.AddHttpCookie(ApplicationConfiguration.AuthenticationCookieName, value, value.GetTokenExpirationDate());
        }
        
        public static void ExpireAuthenticationCookie(this HttpResponseBase response)
        {
            response.ExpireHttpCookie(ApplicationConfiguration.AuthenticationCookieName);
        }
    }
}