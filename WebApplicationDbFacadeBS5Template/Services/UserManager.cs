using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Filters;
using WebApplicationDbFacadeBS5Template.DomainLayer;
using WebApplicationDbFacadeBS5Template.DomainLayer.Models.Data;
using WebApplicationDbFacadeBS5Template.DomainLayer.Models.Parameters;
using WebApplicationDbFacadeBS5Template.Extensions;
using WebApplicationDbFacadeBS5Template.Identity;
using WebApplicationDbFacadeBS5Template.Services.Configuration;

namespace WebApplicationDbFacadeBS5Template.Services
{
    public static class UserManager
    {
        private static IDictionary<string, object> ToTokenData(this IEnumerable<TokenClaim> claims) => claims.ToDictionary(c => c.Name, c => (object)c.GetValue());
        public static bool TrySignInWithExternalToken(this HttpContextBase httpContext)
        {
            string token = httpContext.Request.Form.AllKeys.Contains(ApplicationConfiguration.ExternalLoginTokenParam) ?
                httpContext.Request.Form[ApplicationConfiguration.ExternalLoginTokenParam] :
                httpContext.Request.Params.Get(ApplicationConfiguration.ExternalLoginTokenParam);
            
            if (!string.IsNullOrWhiteSpace(token))
            {
                // var userProfile = find user by external token
                
                if (!AppDomainFacade.ResolveUserIdentity(
                    new ResolveUserIdentityParameters
                    {
                        SSN = "000000001",
                        EDIDI = "0000000001",
                        FirstName = "Sample",
                        MiddleInitial = "J",
                        LastName = "User",
                        Email = "sample.user@sampleemail.com"
                    }, out Guid userGuid).HasError && 
                    !AppDomainFacade.CreateAppSession(userGuid, out AppSession session).HasError
                    )
                {
        var user = CommonPrincipal.CreateAuthenticated(
            session.UserGuid,
            session.ProxyUserGuid,
            session.Guid,
            session.ExpireDate, 
            session.IsAdmin,
            session.IsProxyMode,
            session.ProxyName,
            session.Name,
            session.LastLogin,
            session.IsProxyMode ? session.ProxyLastLogin : session.LastLogin,
            session.Roles,
            session.Groups,
            session.IsProxyMode ? session.ProxyName : session.Name
            );
                    httpContext.User = user;
                    httpContext.Response.AddAuthenticationCookie(Saml2TokenExtensions.CreateToken(session.ToClaims())); // create token
                    httpContext.ResolveDarkModeChange(user.ActingUserGuid);
                    return true;
                }                
            }
            return false;
        }
        private static IEnumerable<TokenClaim> ToClaims(this AppSession session)
        => new TokenClaim[] {
            new TokenClaim(){ Name = TokenClaimConfiguration.SessionGuidClaim, Value = session.Guid.ToString() },
            new TokenClaim(){ Name = TokenClaimConfiguration.ExpireDateClaim, Value = session.ExpireDate.ToString() }
            };
        private static void ResolveDarkModeChange(this HttpContextBase httpContext, Guid userGuid)
        {
            string darkModeStr = httpContext.Request.QueryString.Get(ApplicationConfiguration.DarkModeChangeParam);
            if(!string.IsNullOrWhiteSpace(darkModeStr) && bool.TryParse(darkModeStr, out bool darkMode))
            {
                AppDomainFacade.UpdateUserSettings(userGuid, darkMode, out UserSettings userSettings);
            }
        }
        public static bool TrySignInUser(this HttpContextBase httpContext)
        {
            if (httpContext.Request.TryGetAuthenticationCookie(out string authToken) && authToken.TryGetValidTokenClaims(out IEnumerable<TokenClaim> claims))
            {
                var tokenData = claims.ToTokenData();
                Guid sessionGuid = tokenData.GetSetting<Guid>(TokenClaimConfiguration.SessionGuidClaim);
                var sessionResponse = AppDomainFacade.GetAppSession(sessionGuid);
                if (sessionResponse.HasError || sessionResponse.Count() == 0)
                {
                    return false;
                }
                var session = sessionResponse.FirstOrDefault();
                if (!session.IsActive)
                {
                    return false;
                }

                bool endProxy = session.IsProxyMode &&
                    bool.TryParse(httpContext.Request.QueryString.Get(ApplicationConfiguration.ProxyEndParam), out bool end) && end;
                var renewedSessionResponse = AppDomainFacade.UpdateAppSession(session.Guid, endProxy ? session.UserGuid : session.ProxyUserGuid);
                if (renewedSessionResponse.HasError || renewedSessionResponse.Count() == 0)
                {
                    return false;
                }
                var renewedSession = renewedSessionResponse.FirstOrDefault();
                var user = CommonPrincipal.CreateAuthenticated(
                    renewedSession.UserGuid,
                    renewedSession.ProxyUserGuid,
                    renewedSession.Guid,
                    renewedSession.ExpireDate,
                    renewedSession.IsAdmin,
                    renewedSession.IsProxyMode,
                    renewedSession.ProxyName,
                    renewedSession.Name,
                    renewedSession.LastLogin,
                    renewedSession.IsProxyMode ? renewedSession.ProxyLastLogin : renewedSession.LastLogin,
                    renewedSession.Roles,
                    renewedSession.Groups,
                    renewedSession.IsProxyMode ? renewedSession.ProxyName : renewedSession.Name
                    );
                httpContext.User = user;
                httpContext.Response.AddAuthenticationCookie(Saml2TokenExtensions.CreateToken(renewedSession.ToClaims())); // create token
                httpContext.ResolveDarkModeChange(user.ActingUserGuid);
                return true;
            }
            return false;
        }
        private static bool TryGetProxyTokenData(string proxyAccessToken, out (Guid SessionGuid, Guid ProxyUserGuid) data)
        {
            if (proxyAccessToken != null)
            {
                var jsonStr = proxyAccessToken.Decrypt();
                if (!string.IsNullOrWhiteSpace(jsonStr))
                {
                    data = JsonConvert.DeserializeObject<(Guid SessionGuid, Guid ProxyUserGuid)>(proxyAccessToken.Decrypt());
                    return true;
                }
            }
            data = (default(Guid), default(Guid));
            return false;
        }
        internal static bool CheckProxyAccess(this CommonPrincipal user, Guid proxyUserGuid)
        {
            return false;
        }
        public static bool TryAuthorizeProxy(this HttpContextBase httpContext, string proxyAccessToken)
        {
            var user = httpContext.User as CommonPrincipal;
            if (TryGetProxyTokenData(proxyAccessToken, out (Guid SessionGuid, Guid ProxyUserGuid) data) && 
                user.SessionGuid == data.SessionGuid &&
                user.HasProxyAccessToUser(data.ProxyUserGuid))
            {
                var renewedSessionResponse = AppDomainFacade.UpdateAppSession(user.SessionGuid, data.ProxyUserGuid);
                if (renewedSessionResponse.HasError || renewedSessionResponse.Count() == 0)
                {
                    return false;
                }
                return true;
            }
            return false;
        }
        public static void Logout(this HttpContextBase httpContext)
        {
            if (httpContext.Request.TryGetAuthenticationCookie(out string authToken) && authToken.TryGetValidTokenClaims(out IEnumerable<TokenClaim> claims))
            {
                var tokenData = claims.ToTokenData();
                var response = AppDomainFacade.EndAppSession(tokenData.GetSetting<Guid>(TokenClaimConfiguration.SessionGuidClaim));
            }
            httpContext.Response.ExpireAuthenticationCookie();
            httpContext.User = CommonPrincipal.Public;
        }
        public static ActionResult RedirectToLogout(this HttpContextBase httpContext)
        {
            httpContext.Logout();
            if (httpContext.Request.IsAjaxRequest())
            {
                httpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                return new JsonResult
                {
                    Data = new { Success = false, Data = "Unauthorized" },
                    ContentEncoding = System.Text.Encoding.UTF8,
                    ContentType = "application/json",
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }
            else
            {
                return new RedirectResult(ApplicationConfiguration.LogoutUrl);
            }
        }
        public static void OnAuthentication(this AuthenticationContext filterContext)
        {
            if (!filterContext.HttpContext.TrySignInUser() && !filterContext.HttpContext.TrySignInWithExternalToken())
            {
                filterContext.Result = new HttpUnauthorizedResult();
            }
        }
        public static void OnAuthenticationChallenge(this AuthenticationChallengeContext filterContext)
        {
            if (filterContext.Result == null || filterContext.Result is HttpUnauthorizedResult)
            {
                filterContext.Result = filterContext.HttpContext.RedirectToLogout();
            }
        }
    }
}