using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CJJ.Blog.Main.Tools
{
    public static class JsonExtension
    {
        public static string SerializeObject(this object obj)
        {
            string res = string.Empty;
            try
            {
                res = JsonConvert.SerializeObject(obj);
            }
            catch
            {

            }
            return res;
        }
        public static T DeserializeObject<T>(this string obj)
        {

            try
            {
                IsoDateTimeConverter timeFormat = new IsoDateTimeConverter();
                timeFormat.DateTimeFormat = "yyyy-MM-dd HH:mm:ss";
                return JsonConvert.DeserializeObject<T>(obj, timeFormat);
            }
            catch
            {
                return default(T);
            }

        }
    }
}