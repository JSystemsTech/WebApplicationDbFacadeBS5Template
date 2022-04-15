using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Filters;
using System.Web.Routing;
using WebApplicationDbFacadeBS5Template.Attributes;
using WebApplicationDbFacadeBS5Template.DomainLayer;
using WebApplicationDbFacadeBS5Template.DomainLayer.Models.Data;
using WebApplicationDbFacadeBS5Template.Extensions;
using WebApplicationDbFacadeBS5Template.Extensions.Aspose.Cells;
using WebApplicationDbFacadeBS5Template.Identity;
using WebApplicationDbFacadeBS5Template.Models;
using WebApplicationDbFacadeBS5Template.Models.DataTable;
using WebApplicationDbFacadeBS5Template.Models.Helpers;
using WebApplicationDbFacadeBS5Template.Models.Helpers.DataTable;
using WebApplicationDbFacadeBS5Template.Services;
using WebApplicationDbFacadeBS5Template.Services.Configuration;
using WebApplicationDbFacadeBS5Template.Services.Container;

namespace WebApplicationDbFacadeBS5Template.Controllers
{
    [HandleError(View = "Error")]
    public abstract class ControllerBase : Controller
    {
        /*hide base*/     
        public new JsonResult Json(object value, JsonRequestBehavior jsonRequestBehavior = JsonRequestBehavior.AllowGet)
            => new CamelJsonHelper().Json(value, ResolveJsonGlobalValues(), jsonRequestBehavior);
        public JsonResult Json(object value, Notification notification, JsonRequestBehavior jsonRequestBehavior = JsonRequestBehavior.AllowGet)
            => new CamelJsonHelper().Json(value, ResolveJsonGlobalValues(notification), jsonRequestBehavior);

        public JsonResult InvalidModelStateJson(string title, JsonRequestBehavior jsonRequestBehavior = JsonRequestBehavior.AllowGet)
            => Json(new {
                modelStateErrors = ModelState.Where(kv => kv.Value.Errors.Count() > 0).ToDictionary(kv => kv.Key, kv => string.Join(", ", kv.Value.Errors.Select(e => GetErrorMessage(e))))
            }, Notification.Warning(title, $"Model validation did not pass. Please check the form for more infomation"), jsonRequestBehavior);

        protected virtual object ResolveJsonGlobalValues(Notification notification = null) => new
        {
            notification
        };
        private string GetErrorMessage(ModelError e)
        => string.IsNullOrWhiteSpace(e.ErrorMessage) ? e.Exception.Message : e.ErrorMessage;
        protected string RenderViewToString(string viewName, object model)
        {
            ViewData.Model = model;
            using (var sw = new StringWriter())
            {
                var viewResult = ViewEngines.Engines.FindPartialView(ControllerContext,viewName);
                var viewContext = new ViewContext(ControllerContext, viewResult.View,ViewData, TempData, sw);
                viewResult.View.Render(viewContext, sw);
                viewResult.ViewEngine.ReleaseView(ControllerContext, viewResult.View);
                //return sw.GetStringBuilder().ToString().Replace("\r\n","");
                return Regex.Replace(sw.GetStringBuilder().ToString().Replace("\r\n", "").Trim(), " +", " ").Replace("> <","><");
            }
        }
        private ConcurrentDictionary<string, (string url, object options)> DataTableOptionsMap { get; set; }
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
            var url = filterContext.HttpContext.Request.Url.AbsoluteUri;
            var relativeUrl = VirtualPathUtility.ToAppRelative(new Uri(filterContext.HttpContext.Request.Url.PathAndQuery, UriKind.Relative).ToString());
            DataTableOptionsMap = new ConcurrentDictionary<string, (string url, object options)>();
            ResolveDataTableOptions();
            dynamic DataTableOptions = new ExpandoObject();
            var dtOpts = DataTableOptions as IDictionary<string, object>;
            foreach (var kv in DataTableOptionsMap)
            {
                dtOpts.Add(kv.Key, kv.Key);
            }
            ViewBag.DataTableOptions = DataTableOptions;
        }

        protected void AddError(Exception e)
        {
            ViewBag.Errors = ViewBag.Errors ?? new Exception[0];
            if(ViewBag.Errors is IEnumerable<Exception> errors)
            {
                ViewBag.Errors = errors.Append(e);
            }
        }
        protected string GetDataTableColumnTemplate<TModel>(string template, TModel model)
            => RenderViewToString($"~/Views/Datatable/ColumnTemplates/{template}.cshtml", model);
        protected string GetDataTableColumnActionButtons(params ActionButtonVM[] buttons)
            => GetDataTableColumnTemplate("ActionButtons", buttons);
        protected string GetDataTableColumnImage(string name, Guid? fileId, bool useDefault = false)
            => GetDataTableColumnTemplate("Image", (name,fileId,useDefault));

        [AllowAnonymous]
        public virtual ActionResult Unauthorized(string Message)
        {
            ViewBag.UnauthorizedMessage = Message;
            return View("Unauthorized");
        }
        protected virtual RedirectToRouteResult RedirectToUnauthorized(string Message)
        => RedirectToAction("Unauthorized", Message);
        protected virtual PartialViewResult UnauthorizedPartialView(string Message)
        {
            ViewBag.UnauthorizedMessage = Message;
            return PartialView("Unauthorized", Message); 
        }
        public virtual JsonResult GetDataTableOptions(string key) {
            if(DataTableOptionsMap.TryGetValue(key, out (string url, object options) config))
            {
                return Json(config.options);
            }
            return Json(new { });
        }
        public ActionResult ShowImage(Guid id)
        {
            var response = AppDomainFacade.GetFile(id, out AppFileRecord file);
            if (response.HasError) {
                return new EmptyResult();
            }
            return File(file.Data, file.MIMEType, file.FileName);
        }
        protected virtual void ResolveDataTableOptions() { }
        protected void RegisterDataTableOptions<T>(string key, DataTableOptions<T> options)
        where T : class
        {
            DataTableOptionsMap.TryAdd(key, (options.Ajax.Url, options));
        }
        protected DataTableOptions<T> GetDataTableOptions<T>(string key)
         where T : class
        {
            if (DataTableOptionsMap.TryGetValue(key, out (string url, object options) config) && config.options is DataTableOptions<T> dtOptions)
            {
                return dtOptions;
            }
            return default(DataTableOptions<T>);
        }
        
        protected DataTableOptions<T> GetDataTableOptions<T>()
         where T : class
        {
            string relativeUrl = VirtualPathUtility.ToAppRelative(new Uri(Request.Url.PathAndQuery, UriKind.Relative).ToString());
            if (DataTableOptionsMap.Values.FirstOrDefault(m => $"~{m.url}" == relativeUrl.ToString()).options is DataTableOptions<T> dtOptions)
            {
                return dtOptions;
            }
            return default(DataTableOptions<T>);
        }
        
        protected ActionResult DataTableResult<T>(
            IEnumerable<T> data,
            DataTableRequest request) 
            where T : class
        => DataTableResult(GetDataTableOptions<T>(), data, request);
        protected ActionResult DataTableResult<T>(
            string key,
            IEnumerable<T> data,
            DataTableRequest request)
            where T : class
        => DataTableResult(GetDataTableOptions<T>(key), data, request);

        protected ActionResult DataTableResult<T>(
            DataTableOptions<T> options,
            IEnumerable<T> data,
            DataTableRequest request)
            where T : class
        {
            var response = data.ProcessDataTable(request, options);
            if (request.Download)
            {
                return File(
                    WorkbookExtensions.ImportToWorkbook(response.ToSystemDataTable()).ExportToXlxs(),
                    MSOfficeMime.Xlxs,
                    string.IsNullOrWhiteSpace(request.FileName) ? "DataTableDownload" : request.FileName
                    );
            }
            return Json(response);
        }

        [AllowAnonymous]
        [LogoutFilter]
        public ActionResult Logout() => RedirectToAction("Index", "Auth");

        protected PartialViewResult ListManagmentView<T>(string url, IEnumerable<T> data, Func<T, string> getName, Func<T, string> getDescription, Func<T, object> getValue, Func<T, bool> isSelected)
        => PartialView("ListManagement", ListManagementVM.Create(url, data, getName, getDescription, getValue, isSelected));

        protected PartialViewResult ErrorPartialView(Exception ex)
        {
            string actionName = ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = ControllerContext.RouteData.Values["controller"].ToString();
            return PartialView("Error", new HandleErrorInfo(ex, controllerName, actionName));
        }
        protected PartialViewResult UnknownPartialView(string name)
        {
            string actionName = ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = ControllerContext.RouteData.Values["controller"].ToString();
            return PartialView("Error", new HandleErrorInfo(new Exception($"Unknown Partial View '{name}'"), controllerName, actionName));
        }
        protected PartialViewResult TryGetPartialView(Func<PartialViewResult> handler)
        {            
            try
            {
                return handler();
            }
            catch (Exception ex)
            {
                return ErrorPartialView(ex);
            }
        }

    }
    [FederatedAuthenticationFilter]
    public abstract class AuthenticatedControllerBase : ControllerBase {
        protected CommonPrincipal ApplicationUser => User as CommonPrincipal;
        protected override object ResolveJsonGlobalValues(Notification notification = null)
        => new
        {
            sessionExpireDate = ApplicationUser.ExpireDate.ToMomentSafeDateTimeString(),
            notification 
        };
        private object MapModelStateErrors()
        => ModelState.Where(kv => kv.Value.Errors.Count() > 0).ToDictionary(kv => kv.Key, kv => string.Join(", ", kv.Value.Errors.Select(e => GetErrorMessage(e))));
        private string GetErrorMessage(ModelError e)
        => string.IsNullOrWhiteSpace(e.ErrorMessage) ? e.Exception.Message: e.ErrorMessage;


        protected sealed override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);            
            if (ApplicationUser.Identity.IsAuthenticated)
            {
                ViewBag.SessionExpireDate = ApplicationUser.ExpireDate.ToMomentSafeDateTimeString();
                var response = AppDomainFacade.GetUserSettings(ApplicationUser.ActingUserGuid, out UserSettings userSettings);
                ViewBag.DarkMode = !response.HasError ? userSettings.DarkMode : false;
                ViewBag.ProfilePictureFileGuid = !response.HasError ? userSettings.ProfilePictureFileGuid : null;
            }
            OnActionExecutingCustom(filterContext);
        }
        protected virtual void OnActionExecutingCustom(ActionExecutingContext filterContext) { }

        protected override void OnAuthentication(AuthenticationContext filterContext)
        {
            base.OnAuthentication(filterContext);
            filterContext.OnAuthentication();
        }
        protected override void OnAuthenticationChallenge(AuthenticationChallengeContext filterContext)
        {
            base.OnAuthenticationChallenge(filterContext);
            filterContext.OnAuthenticationChallenge();
        }


        [FederatedAuthenticationFilter]
        public override JsonResult GetDataTableOptions(string key)
            => base.GetDataTableOptions(key);
    }
    public class ErrorController : ControllerBase
    {
        [AllowAnonymous]
        public ActionResult Index(string aspxerrorpath) => View("Error", aspxerrorpath);

    }

}