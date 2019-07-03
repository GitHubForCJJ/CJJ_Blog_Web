using CJJ.Blog.Main.Filters;
using CJJ.Blog.NetWork.WcfHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Util;

namespace CJJ.Blog.Main.Controllers
{
    [UserAuthorize]
    public class BlogController : BaseController
    {
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetListData(FormCollection form)
        {
            var where = FormToDic(form);
            var list = BlogHelper.GetJsonListPage_Bloginfo(1, 10, "", where);
            return MyJson(list);
        }
        private Dictionary<string, object> FormToDic(FormCollection form)
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
        [HttpGet]
        public ActionResult AddOrEdit()
        {
            return View();
        }
        [HttpPost]
        public void AddOrEdit(string content)
        {

        }
    }
}