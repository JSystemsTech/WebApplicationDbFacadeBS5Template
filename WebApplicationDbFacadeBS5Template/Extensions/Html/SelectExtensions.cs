using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;

namespace WebApplicationDbFacadeBS5Template.Extensions.Html
{
    public class SelectListStandardItem : SelectListItem
    {
        public static readonly string StandardValueField = "Value";
        public static readonly string StandardTextField = "Text";
        public static readonly string StandardGroupField = "Group.Name";
        public object HtmlAttributes { get; set; }
        public SelectListStandardItem() { }
        public static SelectListStandardItem Create<T>(T data, Func<T, object> dataValueSelector, Func<T, object> dataTextSelector)
            => new SelectListStandardItem() { Text = dataTextSelector(data).ToString(), Value = dataValueSelector(data).ToString() };

        public static SelectListStandardItem Create<T>(T data, Func<T, object> dataValueSelector, Func<T, object> dataTextSelector, Func<T, object> dataGroupSelector)
            => new SelectListStandardItem() { Text = dataTextSelector(data).ToString(), Value = dataValueSelector(data).ToString(), Group = new SelectListGroup() { Name = dataGroupSelector(data).ToString() } };

    }
    public static class MultiSelectListExtensions
    {

        public static MultiSelectList<T> ToMultiSelectList<T>(
            this IEnumerable<T> data,
            Func<T, object> htmlAttributeSelector,
            Func<T, object> dataValueSelector,
            IEnumerable<object> selectedValues = null)
            where T : class
        => new MultiSelectList<T>(data, htmlAttributeSelector, dataValueSelector, selectedValues);
        public static MultiSelectList<T> ToMultiSelectList<T>(
            this IEnumerable<T> data,
            Func<T, object> htmlAttributeSelector,
            Func<T, object> dataValueSelector,
            IEnumerable<object> selectedValues,
            IEnumerable<object> disabledValues)
            where T : class
        => new MultiSelectList<T>(data, htmlAttributeSelector, dataValueSelector, selectedValues, disabledValues);

        public static MultiSelectList<T> ToMultiSelectList<T>(
            this IEnumerable<T> data,
            Func<T, object> htmlAttributeSelector,
            Func<T, object> dataValueSelector,
            Func<T, object> dataTextSelector,
            IEnumerable<object> selectedValues = null,
            Func<T, object> dataGroupSelector = null)
        => new MultiSelectList<T>(data, htmlAttributeSelector, dataValueSelector, dataTextSelector, selectedValues, dataGroupSelector);
        public static MultiSelectList<T> ToMultiSelectList<T>(
            this IEnumerable<T> data,
            Func<T, object> htmlAttributeSelector,
            Func<T, object> dataValueSelector,
            Func<T, object> dataTextSelector,
            IEnumerable<object> selectedValues,
            IEnumerable<object> disabledValues,
            Func<T, object> dataGroupSelector = null)
            => new MultiSelectList<T>(data, htmlAttributeSelector, dataValueSelector, dataTextSelector, selectedValues, disabledValues, dataGroupSelector);
        internal static IEnumerable<SelectListStandardItem> ToSelectListStandardItems<T>(
            this IEnumerable<T> data,
            Func<T, object> htmlAttributeSelector,
            Func<T, object> dataValueSelector,
            Func<T, object> dataTextSelector,
            IEnumerable<object> selectedValues,
            IEnumerable<object> disabledValues,
            Func<T, object> dataGroupSelector = null)
       => data.Select(item => {
           SelectListStandardItem selectListItem =
               dataGroupSelector != null ? SelectListStandardItem.Create(item, dataValueSelector, dataTextSelector, dataGroupSelector) :
               SelectListStandardItem.Create(item, dataValueSelector, dataTextSelector);
           selectListItem.HtmlAttributes = htmlAttributeSelector != null ? htmlAttributeSelector(item) : new { };
           selectListItem.Selected = selectedValues != null && selectedValues.Any(sv => sv.ToString() == selectListItem.Value);
           selectListItem.Disabled = disabledValues != null && disabledValues.Any(sv => sv.ToString() == selectListItem.Value);
           return selectListItem;
       });
        internal static IEnumerable<SelectListStandardItem> ToSelectListStandardItems<T>(
            this IEnumerable<T> data,
            Func<T, object> htmlAttributeSelector,
            Func<T, object> dataValueSelector,
            Func<T, object> dataTextSelector,
            object selectedValue,
            IEnumerable<object> disabledValues,
            Func<T, object> dataGroupSelector = null)
       => data.Select(item => {
           SelectListStandardItem selectListItem =
               dataGroupSelector != null ? SelectListStandardItem.Create(item, dataValueSelector, dataTextSelector, dataGroupSelector) :
               SelectListStandardItem.Create(item, dataValueSelector, dataTextSelector);
           selectListItem.HtmlAttributes = htmlAttributeSelector != null ? htmlAttributeSelector(item) : new { };
           selectListItem.Selected = selectedValue != null && selectedValue.ToString() == selectListItem.Value;
           selectListItem.Disabled = disabledValues != null && disabledValues.Any(sv => sv.ToString() == selectListItem.Value);
           return selectListItem;
       });
    }
    public class MultiSelectList<T> : MultiSelectList, IEnumerable<SelectListStandardItem>
    {
        private IEnumerable<SelectListStandardItem> ExtendedItems { get; set; }
        IEnumerator<SelectListStandardItem> IEnumerable<SelectListStandardItem>.GetEnumerator()
        => ExtendedItems.GetEnumerator();
        public MultiSelectList(
            IEnumerable<T> data,
            Func<T, object> htmlAttributeSelector,
            Func<T, object> dataValueSelector,
            IEnumerable<object> selectedValues = null)
            : this(data, htmlAttributeSelector, dataValueSelector, dataValueSelector, selectedValues) { }
        public MultiSelectList(
            IEnumerable<T> data,
            Func<T, object> htmlAttributeSelector,
            Func<T, object> dataValueSelector,
            IEnumerable<object> selectedValues,
            IEnumerable<object> disabledValues)
            : this(data, htmlAttributeSelector, dataValueSelector, dataValueSelector, selectedValues, disabledValues) { }

        public MultiSelectList(
            IEnumerable<T> data,
            Func<T, object> htmlAttributeSelector,
            Func<T, object> dataValueSelector,
            Func<T, object> dataTextSelector,
            IEnumerable<object> selectedValues = null,
            Func<T, object> dataGroupSelector = null)
        : this(data, htmlAttributeSelector, dataValueSelector, dataTextSelector, selectedValues, null, dataGroupSelector) { }


        public MultiSelectList(
                    IEnumerable<T> data,
                    Func<T, object> htmlAttributeSelector,
                    Func<T, object> dataValueSelector,
                    Func<T, object> dataTextSelector,
                    IEnumerable<object> selectedValues,
                    IEnumerable<object> disabledValues,
                    Func<T, object> dataGroupSelector = null) :
                    base(data.ToSelectListStandardItems(htmlAttributeSelector, dataValueSelector, dataTextSelector, selectedValues, disabledValues, dataGroupSelector), SelectListStandardItem.StandardValueField, SelectListStandardItem.StandardTextField, SelectListStandardItem.StandardGroupField, selectedValues, disabledValues)
        {
            ExtendedItems = data.ToSelectListStandardItems(htmlAttributeSelector, dataValueSelector, dataTextSelector, selectedValues, disabledValues, dataGroupSelector);
        }

        
    }

    public static class SelectListExtensions
    {


        public static SelectList<T> ToSelectList<T>(
            this IEnumerable<T> data,
            Func<T, object> htmlAttributeSelector,
            Func<T, object> dataValueSelector,
            object selectedValue = null)
        => new SelectList<T>(data, htmlAttributeSelector, dataValueSelector, selectedValue);
        public static SelectList<T> ToSelectList<T>(
            IEnumerable<T> data,
            Func<T, object> htmlAttributeSelector,
            Func<T, object> dataValueSelector,
            object selectedValue,
            IEnumerable<object> disabledValues)
        => new SelectList<T>(data, htmlAttributeSelector, dataValueSelector, selectedValue, disabledValues);

        public static SelectList<T> ToSelectList<T>(
            this IEnumerable<T> data,
            Func<T, object> htmlAttributeSelector,
            Func<T, object> dataValueSelector,
            Func<T, object> dataTextSelector,
            object selectedValue = null,
            Func<T, object> dataGroupSelector = null)
        => new SelectList<T>(data, htmlAttributeSelector, dataValueSelector, dataTextSelector, selectedValue, dataGroupSelector);
        public static SelectList<T> ToSelectList<T>(
            this IEnumerable<T> data,
            Func<T, object> htmlAttributeSelector,
            Func<T, object> dataValueSelector,
            Func<T, object> dataTextSelector,
            object selectedValue,
            IEnumerable<object> disabledValues,
            Func<T, object> dataGroupSelector = null)
            => new SelectList<T>(data, htmlAttributeSelector, dataValueSelector, dataTextSelector, selectedValue, disabledValues, dataGroupSelector);
    }
    public class SelectList<T> : SelectList, IEnumerable<SelectListStandardItem>
    {
        private IEnumerable<SelectListStandardItem> ExtendedItems { get; set; }
        IEnumerator<SelectListStandardItem> IEnumerable<SelectListStandardItem>.GetEnumerator()
        => ExtendedItems.GetEnumerator();
        public SelectList(
                IEnumerable<T> data,
                Func<T, object> htmlAttributeSelector,
                Func<T, object> dataValueSelector,
                object selectedValue = null)
                : this(data, htmlAttributeSelector, dataValueSelector, selectedValue, null) { }
        public SelectList(
            IEnumerable<T> data,
            Func<T, object> htmlAttributeSelector,
            Func<T, object> dataValueSelector,
            object selectedValue,
            IEnumerable<object> disabledValues = null)
                : this(data, htmlAttributeSelector, dataValueSelector, dataValueSelector, selectedValue, disabledValues) { }
        public SelectList(
            IEnumerable<T> data,
            Func<T, object> htmlAttributeSelector,
            Func<T, object> dataValueSelector,
            Func<T, object> dataTextSelector,
            object selectedValue,
            Func<T, object> dataGroupSelector = null)
            : this(data, htmlAttributeSelector, dataValueSelector, dataTextSelector, selectedValue, null, dataGroupSelector) { }
        public SelectList(
            IEnumerable<T> data,
            Func<T, object> htmlAttributeSelector,
            Func<T, object> dataValueSelector,
            Func<T, object> dataTextSelector,
            object selectedValue,
            IEnumerable<object> disabledValues,
            Func<T, object> dataGroupSelector = null)
            : base(data.ToSelectListStandardItems(htmlAttributeSelector, dataValueSelector, dataTextSelector, selectedValue, disabledValues, dataGroupSelector), SelectListStandardItem.StandardValueField, SelectListStandardItem.StandardTextField, SelectListStandardItem.StandardGroupField, selectedValue, disabledValues)
        {
            ExtendedItems = data.ToSelectListStandardItems(htmlAttributeSelector, dataValueSelector, dataTextSelector, selectedValue, disabledValues, dataGroupSelector);
        }
    }
    

    public static class ExtendedSelectExtensions
    {
        internal static object GetModelStateValue(this HtmlHelper htmlHelper, string key, Type destinationType)
        {
            ModelState modelState;
            if (htmlHelper.ViewData.ModelState.TryGetValue(key, out modelState))
            {
                if (modelState.Value != null)
                {
                    return modelState.Value.ConvertTo(destinationType, null /* culture */);
                }
            }
            return null;
        }


        public static MvcHtmlString ExtendedDropDownListFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper,
            Expression<Func<TModel, TProperty>> expression, IEnumerable<SelectListStandardItem> selectList, IDictionary<string, object> htmlAttributes)
        {
            if (expression == null)
                throw new ArgumentNullException("expression");
            ModelMetadata metadata = ModelMetadata.FromLambdaExpression<TModel, TProperty>(expression, htmlHelper.ViewData);
            return SelectInternal(htmlHelper, metadata, ExpressionHelper.GetExpressionText(expression), selectList,
                false /* allowMultiple */, htmlAttributes);
        }
        public static MvcHtmlString ExtendedListBoxFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper,
            Expression<Func<TModel, TProperty>> expression, IEnumerable<SelectListStandardItem> selectList, IDictionary<string, object> htmlAttributes)
        {
            if (expression == null)
                throw new ArgumentNullException("expression");
            ModelMetadata metadata = ModelMetadata.FromLambdaExpression<TModel, TProperty>(expression, htmlHelper.ViewData);
            return SelectInternal(htmlHelper, metadata, ExpressionHelper.GetExpressionText(expression), selectList,
                true /* allowMultiple */, htmlAttributes);
        }
        private static MvcHtmlString SelectInternal(this HtmlHelper htmlHelper, ModelMetadata metadata, string name,
            IEnumerable<SelectListStandardItem> selectList, bool allowMultiple,
            IDictionary<string, object> htmlAttributes)
        {
            string fullName = htmlHelper.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(name);
            if (String.IsNullOrEmpty(fullName))
                throw new ArgumentException("No name");

            if (selectList == null)
                throw new ArgumentException("No selectlist");

            object defaultValue = (allowMultiple)
                ? htmlHelper.GetModelStateValue(fullName, typeof(string[]))
                : htmlHelper.GetModelStateValue(fullName, typeof(string));

            // If we haven't already used ViewData to get the entire list of items then we need to
            // use the ViewData-supplied value before using the parameter-supplied value.
            if (defaultValue == null)
                defaultValue = htmlHelper.ViewData.Eval(fullName);

            if (defaultValue != null)
            {
                IEnumerable defaultValues = (allowMultiple) ? defaultValue as IEnumerable : new[] { defaultValue };
                IEnumerable<string> values = from object value in defaultValues
                                             select Convert.ToString(value, CultureInfo.CurrentCulture);
                HashSet<string> selectedValues = new HashSet<string>(values, StringComparer.OrdinalIgnoreCase);
                List<SelectListStandardItem> newSelectList = new List<SelectListStandardItem>();

                foreach (SelectListStandardItem item in selectList)
                {
                    item.Selected = (item.Value != null)
                        ? selectedValues.Contains(item.Value)
                        : selectedValues.Contains(item.Text);
                    newSelectList.Add(item);
                }
                selectList = newSelectList;
            }

            // Convert each ListItem to an <option> tag
            string optionsString = "";

            if(selectList.All(m=> m.Group != null))
            {
                string optgroups = string.Join("", selectList.GroupBy(m => m.Group.Name).Select(g => {
                    TagBuilder optgroupBuilder = new TagBuilder("optgroup")
                    {
                        InnerHtml = string.Join("",g.Select(m=> ListItemToOption(m)))
                    };
                    optgroupBuilder.MergeAttribute("label", g.Key, true);
                    return optgroupBuilder.ToString(TagRenderMode.Normal);
                }));
                optionsString = $"{optionsString}{optgroups}";
            }
            else
            {
                string options = string.Join("", selectList.Select(m => ListItemToOption(m)));
                optionsString = $"{optionsString}{options}";
            }
            
            

            TagBuilder tagBuilder = new TagBuilder("select")
            {
                InnerHtml = optionsString
            };
            tagBuilder.MergeAttributes(htmlAttributes);
            tagBuilder.MergeAttribute("name", fullName, true /* replaceExisting */);
            tagBuilder.GenerateId(fullName);
            if (allowMultiple)
                tagBuilder.MergeAttribute("multiple", "multiple");

            // If there are any errors for a named field, we add the css attribute.
            System.Web.Mvc.ModelState modelState;
            if (htmlHelper.ViewData.ModelState.TryGetValue(fullName, out modelState))
            {
                if (modelState.Errors.Count > 0)
                {
                    tagBuilder.AddCssClass(HtmlHelper.ValidationInputCssClassName);
                }
            }

            tagBuilder.MergeAttributes(htmlHelper.GetUnobtrusiveValidationAttributes(fullName, metadata));

            return MvcHtmlString.Create(tagBuilder.ToString(TagRenderMode.Normal));
        }

        internal static string ListItemToOption(SelectListStandardItem item)
        {
            TagBuilder builder = new TagBuilder("option")
            {
                InnerHtml = HttpUtility.HtmlEncode(item.Text)
            };
            if (item.Value != null)
            {
                builder.Attributes["value"] = item.Value;
            }
            if (item.Selected)
            {
                builder.Attributes["selected"] = "selected";
            }
            builder.MergeAttributes(HtmlHelper.AnonymousObjectToHtmlAttributes(item.HtmlAttributes));
            return builder.ToString(TagRenderMode.Normal);
        }

    }
}