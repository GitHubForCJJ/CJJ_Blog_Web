using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace CJJ.Blog.Main.Tools
{
    /// <summary>
    /// jsonresult拓展处理时间格式
    /// </summary>
    public class JsonResultExtension : JsonResult
    {
        public override void ExecuteResult(ControllerContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }
            HttpResponseBase httpResponse = context.HttpContext.Response;
            if (!string.IsNullOrEmpty(this.ContentType))
            {
                httpResponse.ContentType = this.ContentType;
            }
            else
            {
                httpResponse.ContentType = "application/json";
            }
            if (ContentEncoding != null)
            {
                httpResponse.ContentEncoding = ContentEncoding;
            }
            if (this.Data != null)
            {
                httpResponse.Write(ToJsonString(this.Data));
            }
        }
        /// <summary>
        /// JSON序列化
        /// </summary>
        /// <param name="obj">对象</param>
        /// <returns>JSON字符串</returns>
        public  string ToJsonString(object obj)
        {
            try
            {
                var jsonSerializerSettings = new JsonSerializerSettings { DateFormatString = "yyyy-MM-dd HH:mm:ss" };
                return JsonConvert.SerializeObject(obj, jsonSerializerSettings);
            }
            catch
            {
                return string.Empty;
            }
        }

    }
}