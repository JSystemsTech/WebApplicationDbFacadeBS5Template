using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text.Encodings.Web;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using WebApplicationDbFacadeBS5Template.Identity;
using WebApplicationDbFacadeBS5Template.Services.Configuration;

namespace WebApplicationDbFacadeBS5Template.Extensions.Html
{
    public static class HtmlHelperExtensions
    {
        private static string GetExpressionText<TModel, TResult>(
        this HtmlHelper<TModel> htmlHelper,
        Expression<Func<TModel, TResult>> expression)
        => ((MemberExpression)expression.Body).Member.Name;

        public static string MetaDataValueFor<TModel, TValue>(this HtmlHelper<TModel> html,
                                                            Expression<Func<TModel, TValue>> expression,
                                                            Func<ModelMetadata, string> property)
       => property(html.MetaDataFor(expression));
        public static string MetaDataValueFor<TModel, TValue>(this HtmlHelper<TModel> html,
                                                            ModelMetadata modelMetadata,
                                                            Func<ModelMetadata, string> property)
       => property(modelMetadata);
        public static ModelMetadata MetaDataFor<TModel, TValue>(
            this HtmlHelper<TModel> htmlHelper,
            Expression<Func<TModel, TValue>> expression)
        => ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);

        public static string ErrorMessageFor<TModel, TValue>(
            this HtmlHelper<TModel> html, 
            Expression<Func<TModel, TValue>> expression)
        {
            if (html.ViewData.ModelState.IsValid || html.ViewData.ModelState.Count <= 0)
            {
                return null;
            }
            IEnumerable<string> modelStateErrors = html.ViewData.ModelState.Keys.Where(k => k == html.GetExpressionText(expression))
                .Select(k => html.ViewData.ModelState[k].Errors).First().Select(e => e.ErrorMessage);

            return modelStateErrors.Count() > 0 ? string.Join(". ", modelStateErrors) : null;

        }
        public static bool HasValidationError<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression)
        {
            if (html.ViewData.ModelState.IsValid || html.ViewData.ModelState.Count <= 0)
            {
                return false;
            }
            IEnumerable<string> modelStateErrors = html.ViewData.ModelState.Keys.Where(k => k == html.GetExpressionText(expression))
                .Select(k => html.ViewData.ModelState[k].Errors).First().Select(e => e.ErrorMessage);

            return modelStateErrors.Count() > 0;
        }
        public static string UniqueId(this HtmlHelper htmlHelper)
        => UniqueId();
        public static string UniqueId()
        => $"uid{Guid.NewGuid().ToString().Replace("-", "")}";

        public static IHtmlString ProxyRequestToken(this HtmlHelper htmlHelper, Guid proxyUserGuid) {
            var user = htmlHelper.ViewContext.HttpContext.User as CommonPrincipal;
            if (user.Identity.IsAuthenticated && user.HasProxyAccessToUser(proxyUserGuid))
            {
                (Guid SessionGuid, Guid ProxyUserGuid) data = (user.SessionGuid, proxyUserGuid);
                string json = JsonConvert.SerializeObject(data);
                return new HtmlString($"<input type=\"hidden\" name=\"{ApplicationConfiguration.ProxyRequestParam}\" value=\"{json.Encrypt()}\"></input>");
            }
            return new HtmlString("");
        }
        public static IHtmlString Linkify(this HtmlHelper htmlHelper, string text)
        {
            string result = Regex.Replace(text, @"((http|https|ftp)\://[a-zA-Z0-9\-\.]+\.[a-zA-Z]{2,3}(:[a-zA-Z0-9]*)?/?([a-zA-Z0-9\-\._\?\,\'/\\\+&amp;%\$#\=~])*)", @"<a href='$1'>$1</a>");

            return new HtmlString(result);
        }
        public static IHtmlString ToHtmlAttributesString(this HtmlHelper htmlHelper, object htmlAttributes)
        {
            return new HtmlString(htmlAttributes.ToHtmlAttributesDictionary().ToHtmlAttributeString());
        }
        public static IHtmlString RenderJsInitData(this HtmlHelper htmlHelper, object htmlAttributes)
        {
            return new HtmlString($"<div {htmlAttributes.ToHtmlAttributesDictionary().AddUpdateHtmlAttribute("js-init-data", "true").ToHtmlAttributeString()}></div>");
        }
    }
    internal class ImageRequestToken
    {
        public Guid Guid { get; set; }
        public Guid ImageGuid { get; set; }
    }
    public static class ImageLoadHelpers
    {        
        public static IHtmlString ImageById(this HtmlHelper htmlHelper, Guid imageGuid)
            => htmlHelper.ImageById(imageGuid, new { });
        public static IHtmlString ImageById(this HtmlHelper htmlHelper, Guid imageGuid, object htmlAttributes)
        {
            var htmlAttrs = htmlAttributes.ToHtmlAttributesDictionary();
            UrlHelper urlHelper = new UrlHelper(htmlHelper.ViewContext.RequestContext);
            htmlAttrs.AppendHtmlAttribute("src", urlHelper.Action("ShowImage", new { id = imageGuid }));
            return new HtmlString($"<img {htmlAttrs.ToHtmlAttributeString()}/>");
        }

    }
    public static class ExpressionHelpers
    {
        public static TAttribute GetAttribute<TModel, TProperty, TAttribute>(this Expression<Func<TModel, TProperty>> expression)
            where TAttribute : Attribute
        {

            Type type = typeof(TModel);

            string propertyName = null;
            string[] properties = null;
            IEnumerable<string> propertyList;
            //unless it's a root property the expression NodeType will always be Convert
            switch (expression.Body.NodeType)
            {
                case ExpressionType.Convert:
                case ExpressionType.ConvertChecked:
                    var ue = expression.Body as UnaryExpression;
                    propertyList = (ue != null ? ue.Operand : null).ToString().Split(".".ToCharArray()).Skip(1); //don't use the root property
                    break;
                default:
                    propertyList = expression.Body.ToString().Split(".".ToCharArray()).Skip(1);
                    break;
            }

            //the propert name is what we're after
            propertyName = propertyList.Last();
            //list of properties - the last property name
            properties = propertyList.Take(propertyList.Count() - 1).ToArray(); //grab all the parent properties

            Expression expr = null;
            foreach (string property in properties)
            {
                PropertyInfo propertyInfo = type.GetProperty(property);
                expr = Expression.Property(expr, type.GetProperty(property));
                type = propertyInfo.PropertyType;
            }

            TAttribute attr;
            attr = (TAttribute)type.GetProperty(propertyName).GetCustomAttributes(typeof(TAttribute), true).SingleOrDefault();

            // Look for [MetadataType] attribute in type hierarchy
            // http://stackoverflow.com/questions/1910532/attribute-isdefined-doesnt-see-attributes-applied-with-metadatatype-class
            if (attr == null)
            {
                MetadataTypeAttribute metadataType = (MetadataTypeAttribute)type.GetCustomAttributes(typeof(MetadataTypeAttribute), true).FirstOrDefault();
                if (metadataType != null)
                {
                    var property = metadataType.MetadataClassType.GetProperty(propertyName);
                    if (property != null)
                    {
                        attr = (TAttribute)property.GetCustomAttributes(typeof(TAttribute), true).SingleOrDefault();
                    }
                }
            }
            return attr;
        }
    }
}