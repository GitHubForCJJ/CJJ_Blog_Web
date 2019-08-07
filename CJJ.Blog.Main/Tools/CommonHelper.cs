using CJJ.Blog.Main.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CJJ.Blog.Main
{
    public static class CommonHelper
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
        /// <summary>
        /// layui的表格渲染条件
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        public static FormItem TableWhere(this FormCollection form)
        {
            var item = new FormItem();
            try
            {
                if (form.AllKeys.Length > 0)
                {
                    item.DicWhere = new Dictionary<string, object>();
                    foreach (var itemkey in form.AllKeys)
                    {
                        if (itemkey == "page")
                        {
                            item.Page = Convert.ToInt32(form[itemkey]);
                        }
                        else if (itemkey == "limit")
                        {
                            item.Limit = Convert.ToInt32(form[itemkey]);
                        }
                        else
                        {
                            item.DicWhere.Add(itemkey, form[itemkey]);
                        }
                    }
                }
            }
            catch(Exception ex)
            {

            }

            return item;
        }
    }
}