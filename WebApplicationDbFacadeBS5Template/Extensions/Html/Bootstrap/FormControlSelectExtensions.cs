using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace WebApplicationDbFacadeBS5Template.Extensions.Html.Bootstrap
{
    public static class FormControlSelectExtensions
    {
        private static IDictionary<string, object> AppendFormControlClass(this IDictionary<string, object> htmlAttributes)
               => htmlAttributes.AppendClass("form-select");
        public static IHtmlString FormControlDropDownListFor<TModel, TProperty>(
            this HtmlHelper<TModel> htmlHelper,
            Expression<Func<TModel, TProperty>> expression,
            IEnumerable<SelectListStandardItem> selectList)
        => htmlHelper.FormControlDropDownListFor(expression, selectList, new { });
        public static IHtmlString FormControlDropDownListFor<TModel, TProperty>(
            this HtmlHelper<TModel> htmlHelper,
            Expression<Func<TModel, TProperty>> expression,
            IEnumerable<SelectListStandardItem> selectList,
            object htmlAttributes)
        {
            var htmlAttrs = htmlAttributes.ToHtmlAttributesDictionary().AppendFormControlClass();
            return htmlHelper.ExtendedDropDownListFor(expression, selectList, htmlAttrs);
        }

        public static IHtmlString FormControlListBoxFor<TModel, TProperty>(
            this HtmlHelper<TModel> htmlHelper,
            Expression<Func<TModel, TProperty>> expression,
            IEnumerable<SelectListStandardItem> selectList)
        => htmlHelper.FormControlListBoxFor(expression, selectList, new { });
        public static IHtmlString FormControlListBoxFor<TModel, TProperty>(
            this HtmlHelper<TModel> htmlHelper,
            Expression<Func<TModel, TProperty>> expression,
            IEnumerable<SelectListStandardItem> selectList,
            object htmlAttributes)
        {
            var htmlAttrs = htmlAttributes.ToHtmlAttributesDictionary().AppendFormControlClass();
            return htmlHelper.ExtendedListBoxFor(expression, selectList, htmlAttrs);
        }
    }
}