using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

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

        public static Dictionary<string, object> FormToDic(FormCollection form)
        {
            var dic = new Dictionary<string, object>();
            if (form.AllKeys.Length > 0)
            {
                foreach (var itemkey in form.AllKeys)
                {
                    if (!dic.ContainsKey(itemkey))
                    {
                        dic.Add(itemkey, form[itemkey]);
                    }
                }
            }
            return dic;
        }
    }
}