using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CJJ.Blog.Main
{
    public class CommonHelper
    {

        public static Dictionary<string, object> ModelToDic<T>(T model)
        {

            var dic = new Dictionary<string, object>();
            try
            {
                if (model == null)
                {
                    return dic;
                }
                var type = model.GetType();
                var props = type.GetProperties();
                foreach (System.Reflection.PropertyInfo item in props)
                {
                    var val = item.GetValue(model);
                    if (val != null)
                    {
                        dic.Add(item.Name, val);
                    }
                }
                return dic;
            }
            catch
            {
                return dic;
            }


        }
    }
}