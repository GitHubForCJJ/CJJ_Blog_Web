using CJJ.Blog.Main.Filters;
using CJJ.Blog.Main.Models;
using CJJ.Blog.NetWork.WcfHelper;
using CJJ.Blog.Service.Model.View;
using CJJ.Blog.Service.Models.Data;
using FastDev.Common.Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
        public ActionResult AddOrEdit(int blogid=0)
        {
            var res = new BloginfoView();
            if (blogid >0)
            {
                var blogres = BlogHelper.GetModelByKID_Bloginfo(blogid);
                res = blogres;
            }
            return View(res);
        }
        [HttpPost]
        public JsonResult AddOrEditPost(Blogview model)
        {
            if (model == null)
            {
                return MyJson(new JsonResponse("", 1, "参数不合法"));
            }
            var modelwhere = CommonHelper.ModelToDic<Blogview>(model);
            if (string.IsNullOrEmpty(model.KID))
            {
                modelwhere.Add(nameof(Bloginfo.Type), 1);
                modelwhere.Add(nameof(Bloginfo.CreateUserId), EmployeeInfo.Model?.KID);
                modelwhere.Add(nameof(Bloginfo.CreateUserName), EmployeeInfo.Model?.UserName);
                modelwhere.Add(nameof(Bloginfo.CreateTime), DateTime.Now);

                Result blog = BlogHelper.Add_Bloginfo(modelwhere, new Service.Models.View.OpertionUser());
                return MyJson(new JsonResponse("", blog.IsSucceed ? 0 : 1, blog.IsSucceed ? "添加成功" : "失败"));
            }
            else
            {
                if (modelwhere.ContainsKey(nameof(Bloginfo.KID)))
                {
                    modelwhere.Remove(nameof(Bloginfo.KID));
                }
                modelwhere.Add(nameof(Bloginfo.UpdateUserId), EmployeeInfo.Model?.KID);
                modelwhere.Add(nameof(Bloginfo.UpdateUserName), EmployeeInfo.Model?.UserName);
                modelwhere.Add(nameof(Bloginfo.UpdateTime), DateTime.Now);

                Result blog = BlogHelper.Update_Bloginfo(modelwhere, model.KID.Toint(), new Service.Models.View.OpertionUser());
                return MyJson(new JsonResponse("", blog.IsSucceed ? 0 : 1, blog.IsSucceed ? "修改成功" : "修改失败"));
            }

        }

        [HttpPost]
        public JsonResult Delete(int kid)
        {
            var res = BlogHelper.Delete_Bloginfo(kid.ToString(), new Service.Models.View.OpertionUser());
            return MyJson(new JsonResponse("", res.IsSucceed ? 0 : 1, res.IsSucceed ? "删除成功" : "删除失败"));
        }

        public Dictionary<string, object> ModelToDic<T>(T model)
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
                foreach (PropertyInfo item in props)
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