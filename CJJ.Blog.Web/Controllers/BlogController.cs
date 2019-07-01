using CJJ.Blog.NetWork.WcfHelper;
using CJJ.Blog.Service.Model.View;
using CJJ.Blog.Service.Models.Data;
using FastDev.Common.Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CJJ.Blog.Web.Controllers
{
    public class BlogController : BaseController
    {
        [HttpGet]
        public ActionResult Index(string categorytype)
        {
            var list = new List<Bloginfo>();
            var where = new Dictionary<string, object>();
            if (!string.IsNullOrEmpty(categorytype))
            {
                where.Add(nameof(Bloginfo.Type), categorytype);
            }
            var jresult = BlogHelper.GetJsonListPage_Bloginfo(1, 15,"", where);
            if (jresult!=null && jresult.code.Toint()==0)
            {
                list = jresult.data;
            }
            return View(list);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="categorytype"></param>
        /// <returns></returns>
        //[HttpPost]
        //public List<Bloginfo> Index(string categorytype)
        //{
        //    var list = new List<Bloginfo>();
        //    var where = new Dictionary<string, object>();
        //    if (!string.IsNullOrEmpty(categorytype))
        //    {
        //        where.Add(nameof(Bloginfo.Type), categorytype);
        //    }
        //    var jresult = BlogHelper.GetJsonListPage_Bloginfo(1, 15, "", where);
        //    if (jresult.code.Toint() == 0)
        //    {
        //        list = jresult.data;
        //    }
        //    return list;
        //    //return FastJson(list,"操作成功",0,list.Count);
        //}

        [HttpGet]
        public ActionResult Detail(int blogid)
        {
            var blog = BlogHelper.GetModelByWhere_Blogcontent(new Dictionary<string, object>() {
                { nameof(Blogcontent.BloginfoId),blogid}
            });
            return View(blog);
        }
        public ActionResult ceshi()
        {
            return View();
        }
    }
}