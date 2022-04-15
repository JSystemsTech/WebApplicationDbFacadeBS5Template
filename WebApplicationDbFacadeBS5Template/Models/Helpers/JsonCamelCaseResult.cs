
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace WebApplicationDbFacadeBS5Template.Models.Helpers
{
    public class JsonCamelCaseResult : JsonResult
    {
        //public Encoding ContentEncoding { get; set; }
        //public string ContentType { get; set; }
        //public object Data { get; set; }

        private const string HttpMethod = "GET";
        private const string DefaultContentType = "application/json";
        public object Data2 { get; set; }
        public override void ExecuteResult(ControllerContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }
            if (JsonRequestBehavior == JsonRequestBehavior.DenyGet &&
                 String.Equals(context.HttpContext.Request.HttpMethod,
                               HttpMethod, StringComparison.OrdinalIgnoreCase))
            {
                throw new InvalidOperationException("HttpMethod 'GET' is not allowed");
            }

            HttpResponseBase response = context.HttpContext.Response;
            response.ContentType = string.IsNullOrEmpty(ContentType) ? DefaultContentType : ContentType;
            if (ContentEncoding != null)
            {
                response.ContentEncoding = ContentEncoding;
            }
            var jsonSerializerSettings = new JsonSerializerSettings
            {
                ContractResolver = new DefaultContractResolver(),
                Formatting = Formatting.Indented,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                PreserveReferencesHandling = PreserveReferencesHandling.None    /*prevent $id/$ref 
                                                                              at client end*/
            };
            object data = (Data is System.Collections.IEnumerable) && !(Data is string) ? new { Data } : Data;
            string serializedData = JsonConvert.SerializeObject(data, jsonSerializerSettings);
            if(Data2 == null)
            {
                response.Write(serializedData);
            }
            else
            {
                    object data2 = (Data2 is System.Collections.IEnumerable) && !(Data2 is string) ? new { Data2 } : Data2;
                    string serializedData2 = JsonConvert.SerializeObject(data2, jsonSerializerSettings);
                    var source1 = JObject.Parse($@"{serializedData}");
                    var source2 = JObject.Parse($@"{serializedData2}");
                    var result = new JObject();
                    result.Merge(source1);
                    result.Merge(source2);

                    response.Write(result);
                
            }
            
        }
    }
    public class CamelJsonHelper
    {
        public JsonCamelCaseResult Json(object data, object data2, JsonRequestBehavior jsonRequestBehavior = JsonRequestBehavior.AllowGet)
        {
            return new JsonCamelCaseResult
            {
                Data = data,
                Data2 = data2,
                JsonRequestBehavior = jsonRequestBehavior
            };
        }
    }
}