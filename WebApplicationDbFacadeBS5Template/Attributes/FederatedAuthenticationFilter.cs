using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Filters;
using System.Web.Routing;
using WebApplicationDbFacadeBS5Template.DomainLayer;
using WebApplicationDbFacadeBS5Template.Extensions;
using WebApplicationDbFacadeBS5Template.Identity;
using WebApplicationDbFacadeBS5Template.Services;
using WebApplicationDbFacadeBS5Template.Services.Configuration;

namespace WebApplicationDbFacadeBS5Template.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class FederatedAuthenticationFilter : ActionFilterAttribute, IAuthenticationFilter
    {
        public void OnAuthentication(AuthenticationContext filterContext)
        => filterContext.OnAuthentication();
        public void OnAuthenticationChallenge(AuthenticationChallengeContext filterContext) 
            => filterContext.OnAuthenticationChallenge();
    }
    
    [AttributeUsage(AttributeTargets.Method)]
    public class LogoutFilter : ActionFilterAttribute, IAuthenticationFilter
    {
        protected AuthenticationContext AuthenticationContext { get; set; }
        protected HttpContextBase HttpContext { get => AuthenticationContext.HttpContext; }
        protected HttpRequestBase Request { get => HttpContext.Request; }
        protected HttpResponseBase Response { get => HttpContext.Response; }
        public void OnAuthentication(AuthenticationContext filterContext)
        {
            AuthenticationContext = filterContext;
            HttpContext.Logout();
        }
        /*used for after principal is set*/
        public void OnAuthenticationChallenge(AuthenticationChallengeContext filterContext) { }
    }


    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public abstract class FederatedAuthorizationFilter : AuthorizeAttribute
    {        private bool IsAjaxRequest(HttpRequestBase request)
        {
            if (request == null)
                throw new ArgumentNullException("request");
            if (request.Headers != null)
                return request.Headers["X-Requested-With"] == "XMLHttpRequest";
            return false;
        }
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext) {
            if (IsAjaxRequest(filterContext.HttpContext.Request))
            {
                filterContext.HttpContext.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                filterContext.Result = new JsonResult
                {
                    Data = new { Success = false, Data = "Forbidden", Message = GetMessage() },
                    ContentEncoding = System.Text.Encoding.UTF8,
                    ContentType = "application/json",
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }
            else
            {
                var url = new UrlHelper(filterContext.HttpContext.Request.RequestContext);
                filterContext.Result = new RedirectResult(url.Action("Unauthorized", new { Message = GetMessage() }));
            }
        }
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (!(httpContext.User is CommonPrincipal)) {
                httpContext.TrySignInUser();//incase User gets reset to instance of WindowsPrincipal
            }
            return IsAuthorized(httpContext);  //httpContext.User.Identity.IsAuthenticated && IsAuthorized(httpContext);
        }
        protected virtual bool IsAuthorized(HttpContextBase httpContext) => true;
        protected virtual string GetMessage() => "You Are Unauthorized to view this Content";
    }
    [AttributeUsage(AttributeTargets.Method)]
    public class ValidateProxyRequestToken : FederatedAuthorizationFilter
    {
        public ValidateProxyRequestToken() { }
        protected override bool IsAuthorized(HttpContextBase httpContext) => httpContext.TryAuthorizeProxy(httpContext.Request.Form[ApplicationConfiguration.ProxyRequestParam]);
        protected override string GetMessage() => "Proxy access to user denied"; 
    }

    public class IsInGroup : FederatedAuthorizationFilter
    {
        private IEnumerable<string> Group { get; set; }
        private string Message { get; set; }
        public IsInGroup(string group, string message = "You do not have group access to this content")
        {
            Group = group.Split(',');
            Message = message;
        }
        protected override bool IsAuthorized(HttpContextBase httpContext)
        => httpContext.User is CommonPrincipal applicationUser && (applicationUser.IsAdmin || Group.Any(g => applicationUser.IsInGroup(g)));
        
        protected override string GetMessage() => Message;
    }
    public class IsAdmin : FederatedAuthorizationFilter
    {
        private string Message { get; set; }
        public IsAdmin(string message = "You do not have Admin Access")
        {
            Message = message;
        }
        protected override bool IsAuthorized(HttpContextBase httpContext)
        {
            if (httpContext.User is CommonPrincipal applicationUser && applicationUser.IsAdmin)
            {
                return true;
            }
            return false;
        }
        protected override string GetMessage() => Message;
    }

    public class AjaxOnlyAttribute : ActionMethodSelectorAttribute
    {
        private bool IsAjaxRequest(ControllerContext controllerContext)
        {
            var request = controllerContext.HttpContext.Request;
            if (request == null)
                throw new ArgumentNullException("request");
            if (request.Headers != null)
                return request.Headers["X-Requested-With"] == "XMLHttpRequest";
            return false;
        }
        public override bool IsValidForRequest(ControllerContext controllerContext, MethodInfo methodInfo)
        => IsAjaxRequest(controllerContext);
    }
}