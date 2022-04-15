using Newtonsoft.Json;
using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using WebApplicationDbFacadeBS5Template.Models.Helpers.DataTable;

namespace WebApplicationDbFacadeBS5Template.Extensions.Html
{
    public static class PageExtensions
    {
        //public static Task<IHtmlContent> PageMenu(this IHtmlHelper htmlHelper, IEnumerable<Page> model)
        //=> htmlHelper.PartialAsync("PageMenu", model.GroupBy(m => string.IsNullOrWhiteSpace(m.Group) ? m.Guid.ToString() : m.Group));
        //public static Task<IHtmlContent> PageMenu(this IHtmlHelper htmlHelper, Guid pageGuid)
        //{
        //    IServiceProvider provider = htmlHelper.ViewContext.HttpContext.RequestServices;
        //    IAdventureWorksDomainFacade adventureWorksDomainFacade = provider.GetService<IAdventureWorksDomainFacade>();
        //    return htmlHelper.PageMenu(adventureWorksDomainFacade.GetPageMenu(pageGuid));
        //}
        //public static Task<IHtmlContent> PageBreadcrumbMenu(this IHtmlHelper htmlHelper, Guid pageGuid)
        //{
        //    IServiceProvider provider = htmlHelper.ViewContext.HttpContext.RequestServices;
        //    IAdventureWorksDomainFacade adventureWorksDomainFacade = provider.GetService<IAdventureWorksDomainFacade>();
        //    List<Page> pages = new List<Page>();

        //    var current = adventureWorksDomainFacade.GetPage(pageGuid);
        //    pages.Add(current);
        //    Guid? parentGuid = current.ParentGuid;
        //    while (parentGuid is Guid parentPageGuid)
        //    {
        //        var next = adventureWorksDomainFacade.GetPage(parentPageGuid);
        //        pages.Add(next);
        //        parentGuid = next.ParentGuid;
        //    }
        //    pages.Reverse();
        //    return htmlHelper.PartialAsync("PageBreadcrumbMenu", pages);
        //}
        public static IHtmlString DataTable(this HtmlHelper htmlHelper, Action<DataTableViewVM> init)
        {
            DataTableViewVM vm = new DataTableViewVM();
            init(vm);
            return htmlHelper.DataTable(vm);
        }
        public static IHtmlString DataTable(this HtmlHelper htmlHelper, DataTableViewVM vm)
        => htmlHelper.Partial("Datatable", vm);
        public static IHtmlString JSONSafeString(this HtmlHelper htmlHelper, string json)
        {
            return new HtmlString(HttpUtility.JavaScriptStringEncode(json));
        }
        public static IHtmlString JSONSafeString<T>(this HtmlHelper htmlHelper, T model)
        {
            return new HtmlString(HttpUtility.JavaScriptStringEncode(JsonConvert.SerializeObject(model)));
        }
    }
}