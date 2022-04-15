using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace WebApplicationDbFacadeBS5Template.Extensions.Html.Bootstrap
{
    public static class InputExtensions
    {

        private static IDictionary<string, object> AppendFormControlClass(this IDictionary<string, object> htmlAttributes)
            => htmlAttributes.AppendClass("form-control");


        public static IHtmlString FormControlTextBox(this HtmlHelper htmlHelper, string name, object value, string format, IDictionary<string, object> htmlAttributes)
            => htmlHelper.TextBox(name, value, format, htmlAttributes.AppendFormControlClass());
        public static IHtmlString FormControlTextBox(this HtmlHelper htmlHelper, string name, object value, IDictionary<string, object> htmlAttributes)
            => htmlHelper.TextBox(name, value, htmlAttributes.AppendFormControlClass());
        public static IHtmlString FormControlTextBox(this HtmlHelper htmlHelper, string name, object value, string format, object htmlAttributes)
            => htmlHelper.FormControlTextBox(name, value, format, htmlAttributes.ToHtmlAttributesDictionary());
        public static IHtmlString FormControlTextBox(this HtmlHelper htmlHelper, string name, object value)
            => htmlHelper.FormControlTextBox(name, value, new { });
        public static IHtmlString FormControlTextBox(this HtmlHelper htmlHelper, string name, object value, string format)
            => htmlHelper.FormControlTextBox(name, value, format, new { });
        public static IHtmlString FormControlTextBox(this HtmlHelper htmlHelper, string name)
            => htmlHelper.FormControlTextBox(name, "");

        public static IHtmlString FormControlTextBox(this HtmlHelper htmlHelper, string name, object value, object htmlAttributes)
            => htmlHelper.FormControlTextBox(name, value, htmlAttributes.ToHtmlAttributesDictionary());
        public static IHtmlString FormControlTextBoxFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, string format, object htmlAttributes)
            => htmlHelper.FormControlTextBoxFor(expression, format, htmlAttributes.ToHtmlAttributesDictionary());
        public static IHtmlString FormControlTextBoxFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, object htmlAttributes)
            => htmlHelper.FormControlTextBoxFor(expression, htmlAttributes.ToHtmlAttributesDictionary());
        public static IHtmlString FormControlTextBoxFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, IDictionary<string, object> htmlAttributes)
            => htmlHelper.TextBoxFor(expression, htmlHelper.HasValidationError(expression) ? htmlAttributes.AppendFormControlClass().AppendClass("is-invalid") : htmlAttributes.AppendFormControlClass());
        public static IHtmlString FormControlTextBoxFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression)
            => htmlHelper.FormControlTextBoxFor(expression, new { });
        public static IHtmlString FormControlTextBoxFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, string format)
            => htmlHelper.FormControlTextBoxFor(expression, format, new { });
        public static IHtmlString FormControlTextBoxFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, string format, IDictionary<string, object> htmlAttributes)
            => htmlHelper.TextBoxFor(expression, format, htmlHelper.HasValidationError(expression) ? htmlAttributes.AppendFormControlClass().AppendClass("is-invalid") : htmlAttributes.AppendFormControlClass());


        public static IHtmlString FormControlTextAreaFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression)
            => htmlHelper.FormControlTextAreaFor(expression, new { });
        public static IHtmlString FormControlTextAreaFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, object htmlAttributes)
            => htmlHelper.FormControlTextAreaFor(expression, htmlAttributes.ToHtmlAttributesDictionary());
        public static IHtmlString FormControlTextAreaFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, IDictionary<string, object> htmlAttributes)
        => htmlHelper.TextAreaFor(expression, htmlHelper.HasValidationError(expression) ? htmlAttributes.AppendFormControlClass().AppendClass("is-invalid") : htmlAttributes.AppendFormControlClass());
        public static IHtmlString FormControlTextAreaFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, int rows, int columns)
            => htmlHelper.FormControlTextAreaFor(expression, rows, columns, new { });
        public static IHtmlString FormControlTextAreaFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, int rows, int columns, object htmlAttributes)
            => htmlHelper.FormControlTextAreaFor(expression, rows,columns, htmlAttributes.ToHtmlAttributesDictionary());
        public static IHtmlString FormControlTextAreaFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, int rows, int columns, IDictionary<string, object> htmlAttributes)
            => htmlHelper.TextAreaFor(expression, rows, columns, htmlHelper.HasValidationError(expression) ? htmlAttributes.AppendFormControlClass().AppendClass("is-invalid") : htmlAttributes.AppendFormControlClass());


        public static IHtmlString FormControlDateFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, string format, object htmlAttributes)
            => htmlHelper.FormControlDateFor(expression, format, htmlAttributes.ToHtmlAttributesDictionary());
        public static IHtmlString FormControlDateFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, object htmlAttributes)
            => htmlHelper.FormControlDateFor(expression, htmlAttributes.ToHtmlAttributesDictionary());
        public static IHtmlString FormControlDateFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, IDictionary<string, object> htmlAttributes)
            => htmlHelper.TextBoxFor(expression, "{0:yyyy-MM-dd}", htmlHelper.HasValidationError(expression) ? htmlAttributes.AppendFormControlClass().AppendClass("is-invalid").AppendHtmlAttribute("type", "date") : htmlAttributes.AppendFormControlClass().AppendHtmlAttribute("type", "date"));
        public static IHtmlString FormControlDateFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression)
            => htmlHelper.FormControlTextBoxFor(expression, new { });
        public static IHtmlString FormControlDateFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, string format)
            => htmlHelper.FormControlTextBoxFor(expression, format, new { });
        public static IHtmlString FormControlDateFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, string format, IDictionary<string, object> htmlAttributes)
            => htmlHelper.TextBoxFor(expression, format, htmlHelper.HasValidationError(expression) ? htmlAttributes.AppendFormControlClass().AppendClass("is-invalid").AppendHtmlAttribute("type","date") : htmlAttributes.AppendFormControlClass().AppendHtmlAttribute("type", "date"));




        public static IHtmlString FormControlValidationMessageFor<TModel, TResult>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TResult>> expression, string message, object htmlAttributes)
        => htmlHelper.ValidationMessageFor(expression, message, htmlAttributes.ToHtmlAttributesDictionary().AppendClass("invalid-feedback"));
        public static IHtmlString FormControlValidationMessageFor<TModel, TResult>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TResult>> expression, string message)
        => htmlHelper.FormControlValidationMessageFor(expression, message, new { });
        public static IHtmlString FormControlValidationMessageFor<TModel, TResult>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TResult>> expression)
        => htmlHelper.FormControlValidationMessageFor(expression, htmlHelper.ErrorMessageFor(expression));

        private static IHtmlString FormControlTextBoxForBase<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, IDictionary<string, object> htmlAttributes)
        {
            ModelMetadata metaData = htmlHelper.MetaDataFor(expression);

            return htmlHelper.TextBoxFor(expression, htmlAttributes.AppendFormControlClass());
        }
        private static IHtmlString FormControlTextBoxForBase<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, string format, IDictionary<string, object> htmlAttributes)
        {
            ModelMetadata metadata = htmlHelper.MetaDataFor(expression);

            return htmlHelper.TextBoxFor(expression, format, htmlAttributes.AppendFormControlClass());
        }



        public static IHtmlString FormControlLabelFor<TModel, TResult>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TResult>> expression)
        => htmlHelper.FormControlLabelFor(expression, new { });
        public static IHtmlString FormControlLabelFor<TModel, TResult>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TResult>> expression, object htmlAttributes)
        {
            ModelMetadata metadata = htmlHelper.MetaDataFor(expression);
            string extraClasses = metadata.IsRequired ? "form-label required" : "form-label";
            return htmlHelper.LabelFor(expression, htmlAttributes.ToHtmlAttributesDictionary().AppendClass(extraClasses));
        }


        public static IHtmlString FormControlPasswordFor(this HtmlHelper htmlHelper, string name, object value, object htmlAttributes)
            => htmlHelper.FormControlPasswordFor(name, value, htmlAttributes.ToHtmlAttributesDictionary());
        public static IHtmlString FormControlPasswordFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, string format, object htmlAttributes)
            => htmlHelper.FormControlPasswordFor(expression, format, htmlAttributes.ToHtmlAttributesDictionary());
        public static IHtmlString FormControlPasswordFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, object htmlAttributes)
            => htmlHelper.FormControlPasswordFor(expression, htmlAttributes.ToHtmlAttributesDictionary());
        public static IHtmlString FormControlPasswordFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, IDictionary<string, object> htmlAttributes)
        {
            htmlAttributes.Add("type", "password");
            return htmlHelper.FormControlTextBoxFor(expression, htmlAttributes);
        }
        public static IHtmlString FormControlPasswordFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression)
            => htmlHelper.FormControlPasswordFor(expression, new { });
        public static IHtmlString FormControlPasswordFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, string format)
            => htmlHelper.FormControlPasswordFor(expression, format, new { });
        public static IHtmlString FormControlPasswordFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, string format, IDictionary<string, object> htmlAttributes)
        {
            htmlAttributes.Add("type", "password");
            return htmlHelper.FormControlTextBoxFor(expression, format, htmlAttributes);
        }


         


        //public static IHtmlString FormControlApplicationPasswordFor(this HtmlHelper htmlHelper, string name, object value, object htmlAttributes)
        //    => htmlHelper.FormControlApplicationPasswordFor(name, value, htmlAttributes.ToHtmlAttributesDictionary());
        //public static IHtmlString FormControlApplicationPasswordFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, string format, object htmlAttributes)
        //    => htmlHelper.FormControlApplicationPasswordFor(expression, format, htmlAttributes.ToHtmlAttributesDictionary());
        //public static IHtmlString FormControlApplicationPasswordFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, object htmlAttributes)
        //    => htmlHelper.FormControlApplicationPasswordFor(expression, htmlAttributes.ToHtmlAttributesDictionary());
        //public static IHtmlString FormControlApplicationPasswordFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, IDictionary<string, object> htmlAttributes)
        //{
        //    IServiceProvider provider = htmlHelper.ViewContext.HttpContext.RequestServices;
        //    PasswordOptions passwordOptions = provider.GetService<IOptions<PasswordOptions>>().Value;
        //    htmlAttributes.Add("minlength", passwordOptions.MinLength);
        //    htmlAttributes.Add("maxlength", passwordOptions.MaxLength);
        //    return htmlHelper.FormControlPasswordFor(expression, htmlAttributes);
        //}
        //public static IHtmlString FormControlApplicationPasswordFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression)
        //    => htmlHelper.FormControlApplicationPasswordFor(expression, new { });
        //public static IHtmlString FormControlApplicationPasswordFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, string format)
        //    => htmlHelper.FormControlApplicationPasswordFor(expression, format, new { });
        //public static IHtmlString FormControlApplicationPasswordFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, string format, IDictionary<string, object> htmlAttributes)
        //{
        //    IServiceProvider provider = htmlHelper.ViewContext.HttpContext.RequestServices;
        //    PasswordOptions passwordOptions = provider.GetService<IOptions<PasswordOptions>>().Value;
        //    htmlAttributes.Add("minlength", passwordOptions.MinLength);
        //    htmlAttributes.Add("maxlength", passwordOptions.MaxLength);
        //    return htmlHelper.FormControlPasswordFor(expression, format, htmlAttributes);
        //}


        public static IHtmlString FormControlFileFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression)
            => htmlHelper.FormControlFileFor(expression, new { });
        public static IHtmlString FormControlFileFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, object htmlAttributes)
            => htmlHelper.FormControlFileFor(expression, htmlAttributes.ToHtmlAttributesDictionary());
        public static IHtmlString FormControlFileFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, IDictionary<string, object> htmlAttributes)
        {
            htmlAttributes.Add("type", "file");
            return htmlHelper.FormControlTextBoxFor(expression, htmlAttributes);
        }

        public static IHtmlString FormControlImageFileFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression)
            => htmlHelper.FormControlImageFileFor(expression, new { });
        public static IHtmlString FormControlImageFileFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, object htmlAttributes)
            => htmlHelper.FormControlImageFileFor(expression, htmlAttributes.ToHtmlAttributesDictionary());
        public static IHtmlString FormControlImageFileFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, IDictionary<string, object> htmlAttributes)
        {
            htmlAttributes.Add("type", "file");
            htmlAttributes.Add("accept", "image/*");
            return htmlHelper.FormControlTextBoxFor(expression, htmlAttributes);
        }

        public static IHtmlString FormControlVideoFileFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression)
            => htmlHelper.FormControlVideoFileFor(expression, new { });
        public static IHtmlString FormControlVideoFileFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, object htmlAttributes)
            => htmlHelper.FormControlVideoFileFor(expression, htmlAttributes.ToHtmlAttributesDictionary());
        public static IHtmlString FormControFormControlVideoFileForlImageFileFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, IDictionary<string, object> htmlAttributes)
        {
            htmlAttributes.Add("type", "file");
            htmlAttributes.Add("accept", "video/*");
            return htmlHelper.FormControlTextBoxFor(expression, htmlAttributes);
        }
        public static IHtmlString FormControlAudioFileFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression)
            => htmlHelper.FormControlAudioFileFor(expression, new { });
        public static IHtmlString FormControlAudioFileFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, object htmlAttributes)
            => htmlHelper.FormControlAudioFileFor(expression, htmlAttributes.ToHtmlAttributesDictionary());
        public static IHtmlString FormControlAudioFileFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, IDictionary<string, object> htmlAttributes)
        {
            htmlAttributes.Add("type", "file");
            htmlAttributes.Add("accept", "audio/*");
            return htmlHelper.FormControlTextBoxFor(expression, htmlAttributes);
        }

        private static string AcceptPDF = "application/pdf";
        
        public static IHtmlString FormControlPDFFileFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression)
            => htmlHelper.FormControlPDFFileFor(expression, new { });
        public static IHtmlString FormControlPDFFileFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, object htmlAttributes)
            => htmlHelper.FormControlPDFFileFor(expression, htmlAttributes.ToHtmlAttributesDictionary());
        public static IHtmlString FormControlPDFFileFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, IDictionary<string, object> htmlAttributes)
        {
            htmlAttributes.Add("type", "file");
            htmlAttributes.Add("accept", AcceptPDF);
            return htmlHelper.FormControlTextBoxFor(expression, htmlAttributes);
        }

        private static string AcceptPowerPoint = ".ppt,.pptx,application/vnd.ms-powerpoint,application/vnd.openxmlformats-officedocument.presentationml.slideshow,application/vnd.openxmlformats-officedocument.presentationml.presentation";
        public static IHtmlString FormControlPowerPointFileFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression)
            => htmlHelper.FormControlPowerPointFileFor(expression, new { });
        public static IHtmlString FormControlPowerPointFileFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, object htmlAttributes)
            => htmlHelper.FormControlPowerPointFileFor(expression, htmlAttributes.ToHtmlAttributesDictionary());
        public static IHtmlString FormControlPowerPointFileFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, IDictionary<string, object> htmlAttributes)
        {
            htmlAttributes.Add("type", "file");
            htmlAttributes.Add("accept", AcceptPowerPoint);
            return htmlHelper.FormControlTextBoxFor(expression, htmlAttributes);
        }
        private static string AcceptWord = ".doc,.docx,application/msword,application/vnd.openxmlformats-officedocument.wordprocessingml.document";
        public static IHtmlString FormControlWordFileFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression)
            => htmlHelper.FormControlWordFileFor(expression, new { });
        public static IHtmlString FormControlWordFileFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, object htmlAttributes)
            => htmlHelper.FormControlWordFileFor(expression, htmlAttributes.ToHtmlAttributesDictionary());
        public static IHtmlString FormControlWordFileFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, IDictionary<string, object> htmlAttributes)
        {
            htmlAttributes.Add("type", "file");
            htmlAttributes.Add("accept", AcceptWord);
            return htmlHelper.FormControlTextBoxFor(expression, htmlAttributes);
        }

        private static string AcceptExcel = ".csv,.xls,.xlsx,application/vnd.openxmlformats-officedocument.spreadsheetml.sheet,application/vnd.ms-excel";
        public static IHtmlString FormControlExcelFileFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression)
            => htmlHelper.FormControlExcelFileFor(expression, new { });
        public static IHtmlString FormControlExcelFileFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, object htmlAttributes)
            => htmlHelper.FormControlExcelFileFor(expression, htmlAttributes.ToHtmlAttributesDictionary());
        public static IHtmlString FormControlExcelFileFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, IDictionary<string, object> htmlAttributes)
        {
            htmlAttributes.Add("type", "file");
            htmlAttributes.Add("accept", AcceptExcel);
            return htmlHelper.FormControlTextBoxFor(expression, htmlAttributes);
        }

        public static IHtmlString FormControlDocumentFileFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression)
            => htmlHelper.FormControlDocumentFileFor(expression, new { });
        public static IHtmlString FormControlDocumentFileFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, object htmlAttributes)
            => htmlHelper.FormControlDocumentFileFor(expression, htmlAttributes.ToHtmlAttributesDictionary());
        public static IHtmlString FormControlDocumentFileFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, IDictionary<string, object> htmlAttributes)
        {
            htmlAttributes.Add("type", "file");
            htmlAttributes.Add("accept", $"{AcceptPDF},{AcceptPowerPoint},{AcceptWord},{AcceptExcel}");
            return htmlHelper.FormControlTextBoxFor(expression, htmlAttributes);
        }


        public static MvcForm BeginUploadForm(this HtmlHelper htmlHelper, string actionName, string controllerName)
            => htmlHelper.BeginUploadForm(actionName, controllerName, new { });
        public static MvcForm BeginUploadForm(this HtmlHelper htmlHelper, string actionName, string controllerName, object routeValues)
            => htmlHelper.BeginUploadForm(actionName, controllerName, routeValues, new { });
        public static MvcForm BeginUploadForm(this HtmlHelper htmlHelper, string actionName, string controllerName, object routeValues, object htmlAttributes)
            => htmlHelper.BeginUploadForm(actionName, controllerName, routeValues, htmlAttributes.ToHtmlAttributesDictionary());
        public static MvcForm BeginUploadForm(this HtmlHelper htmlHelper, string actionName, string controllerName, object routeValues, IDictionary<string, object> htmlAttributes)
        {
            htmlAttributes.Add("enctype", "multipart/form-data");
            return htmlHelper.BeginForm(actionName, controllerName, routeValues, FormMethod.Post, htmlAttributes);
        }


    }
}