using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace CJJ.Blog.Web.Models
{
    public static class JsonExpand
    {
        public static object SerializeObject( this object data)
        {
            string res = string.Empty;
            try
            {
                res = JsonConvert.SerializeObject(data);
            }
            catch
            {

            }
            return res;
        }
        public static List<T> DeserializeObject<T>(this object data)
        {
            var res = new List<T>();
            try
            {
                res= JsonConvert.DeserializeObject<List<T>>(data.ToString());
            }
            catch
            {

            }
            return res;
        }
    }
}