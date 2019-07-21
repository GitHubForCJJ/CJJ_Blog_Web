using CJJ.Blog.NetWork.WcfHelper;
using CJJ.Blog.Service.Model.View;
using CJJ.Blog.Service.Models.Data;
using CJJ.Blog.Web.Models;
using FastDev.Common.Code;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Caching;
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
                ViewBag.categorytype = categorytype;
                where.Add(nameof(Bloginfo.Type), categorytype);
            }
            var jresult = BlogHelper.GetJsonListPage_Bloginfo(1, 15, "", where);
            if (jresult != null && jresult.code.Toint() == 0)
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
        /// <summary>
        /// 获取推荐数据，默认序号靠前五条
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetRecommendList()
        {
            var listdata = BlogHelper.GetJsonListPage_Bloginfo(1, 5, "SORC DESC", new Dictionary<string, object>()
            {
                {nameof(Bloginfo.IsDeleted),0 }
            });
            return Json(listdata);
        }

        /// <summary>
        /// 获取博客分类数据
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetCatList()
        {
            var layoutmodel = new LayoutModel();
            var catlistdata = new List<CategoryView>();
            var recomlistdata = new List<RecomBloginfoView>();

            try
            {
                var categorycachedata = HttpContext.Cache.Get(Constant.CategoryKey);
                var hotnewsdata = HttpContext.Cache.Get(Constant.HotKey);
                var recommenddata = HttpContext.Cache.Get(Constant.RecommendKey);
                if (categorycachedata != null)
                {
                    catlistdata = categorycachedata.DeserializeObject<CategoryView>();
                }
                else
                {
                    var list = BlogHelper.GetAllList_Category();
                    var selectdata = list?.Select(x => new { x.KID, x.Name })?.OrderBy(X => X.KID).ToList();
                    catlistdata = selectdata.SerializeObject().DeserializeObject<CategoryView>();
                    HttpContext.Cache.Insert(Constant.CategoryKey, selectdata.SerializeObject(), null, DateTime.Now.AddYears(1), TimeSpan.Zero, CacheItemPriority.High, null);
                }
                if (recommenddata != null)
                {
                    recomlistdata = recommenddata.DeserializeObject<RecomBloginfoView>();
                }
                else
                {
                    var list = BlogHelper.GetJsonListPage_Bloginfo(1, 5, "SORC DESC", new Dictionary<string, object>()
                     {
                        {nameof(Bloginfo.IsDeleted),0 }
                        });
                    var selectdata = list?.data?.Select(x => new { x.KID, x.Title }).ToList();
                    recomlistdata = selectdata.SerializeObject().DeserializeObject<RecomBloginfoView>();

                    var hotexpirtime = DateTime.Parse($"{DateTime.UtcNow.AddDays(1).ToString("yyyy-MM-dd 23:59:59")}");
                    HttpContext.Cache.Insert(Constant.RecommendKey, recomlistdata.SerializeObject(),null, hotexpirtime,Cache.NoSlidingExpiration);
                }
                layoutmodel.BloginfoViews = recomlistdata;
                layoutmodel.Categorys = catlistdata;
            }
            catch
            {

            }

            return Json(layoutmodel);
        }
    }
}