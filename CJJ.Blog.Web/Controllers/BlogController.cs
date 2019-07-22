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
            where.Add(nameof(Bloginfo.IsDeleted), 0);
            where.Add(nameof(Bloginfo.States), 0);
            var jresult = BlogHelper.GetJsonListPage_Bloginfo(1, 15, "", where);
            if (jresult != null && jresult.code.Toint() == 0)
            {
                list = jresult.data;
            }
            return View(list);
        }
        /// <summary>
        /// 详情页面
        /// </summary>
        /// <param name="blogid"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Detail(int blogid)
        {
            var blog = BlogHelper.GetModelByKID_Bloginfo(blogid);
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
                {nameof(Bloginfo.IsDeleted),0 },
                 {nameof(Bloginfo.States),0 }
            });
            return Json(listdata);
        }

        /// <summary>
        /// 模板也的数据，先读取缓存
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetCatList()
        {
            var layoutmodel = new LayoutModel();
            var catlistdata = new List<CategoryView>();
            var recomlistdata = new List<RecomBloginfoView>();
            var hotlistdata = new List<HotNewView>();
            try
            {
                var categorycachedata = HttpContext.Cache.Get(Constant.CategoryKey);
                var hotnewsdata = HttpContext.Cache.Get(Constant.HotKey);
                var recommenddata = HttpContext.Cache.Get(Constant.RecommendKey);

                #region 导航博客分类NAV数据
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
                #endregion

                #region 热点数据
                if (hotnewsdata != null)
                {
                    hotlistdata = hotnewsdata.DeserializeObject<HotNewView>();
                }
                else
                {
                    var list = BlogHelper.GetJsonListPage_HotNew(1, 15, $" CreateTime desc ", new Dictionary<string, object>());
                    var selectdata = list?.data?.Select((x) => new { x.KID, x.Title, x.Url }).OrderByDescending(X => X.KID).ToList();
                    hotlistdata = selectdata.SerializeObject().DeserializeObject<HotNewView>();
                    var hotexpirtime = DateTime.Parse($"{DateTime.UtcNow.AddDays(1).ToString("yyyy-MM-dd 23:59:59")}");
                    HttpContext.Cache.Insert(Constant.HotKey, selectdata.SerializeObject(), null, hotexpirtime, Cache.NoSlidingExpiration, CacheItemPriority.High, null);
                }
                #endregion

                #region 推荐博客数据
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

                    var recomexpirtime = DateTime.Parse($"{DateTime.UtcNow.AddDays(1).ToString("yyyy-MM-dd 23:59:59")}");
                    HttpContext.Cache.Insert(Constant.RecommendKey, recomlistdata.SerializeObject(), null, recomexpirtime, Cache.NoSlidingExpiration, CacheItemPriority.High, null);
                }
                #endregion

                layoutmodel.BloginfoViews = recomlistdata;
                layoutmodel.Categorys = catlistdata;
                layoutmodel.HotNews = hotlistdata;
            }
            catch
            {

            }

            return Json(layoutmodel);
        }
    }
}