using CJJ.Blog.NetWork.WcfHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CJJ.Blog.Main.Tools;
using CJJ.Blog.Service.Model.View;
using CJJ.Blog.Service.Models.Data;
using CJJ.Blog.Main.Filters;
using CJJ.Blog.Main.Models;
using Newtonsoft.Json;
using FastDev.Common.Code;

namespace CJJ.Blog.Main.Controllers
{
    [CheckPermisstionFilter]
    public class MenuController : BaseController
    {
        // GET: Menu
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// 获取list数据
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult List()
        {
            var emp = EmployeeInfo.Model;
            var menus = BlogHelper.GetListByuserid_Sysmenu(emp.KID);
            //var userinfo = System.Web.HttpContext.Current?.Request?.Cookies.Get(WebUtil.Userinfokey)?.Value;
            //var menus = userinfo.DeserializeObject<SysLoginUser>()?.UserAuthorMenu?.UserMenuList;
            return MyJson(menus);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult AddOrUpdate(Dictionary<string, Object> dic)
        {
            //var data = JsonConvert.DeserializeObject<Dictionary<string, object>>(model.Data.ToString());
            var result = new Result() { IsSucceed=false};
            var emp = EmployeeInfo;
            string kid = string.Empty;
            if (dic == null || dic?.Keys.Count <= 0)
            {
                return MyJson(result);
            }
            if (dic.ContainsKey(nameof(Sysmenu.KID)))
            {
                kid = dic[nameof(Sysmenu.KID)].ToString();
            }
            if (!string.IsNullOrEmpty(kid))
            {
                dic.Remove(nameof(Sysmenu.KID));
                dic.Add(nameof(Sysmenu.UpdateTime), DateTime.Now);
                dic.Add(nameof(Sysmenu.UpdateUserId), emp?.Model.KID);
                dic.Add(nameof(Sysmenu.UpdateUserName), emp?.Model.UserName);
                result = BlogHelper.Update_Sysmenu(dic, kid.ToInt(), new Service.Models.View.OpertionUser());
            }
            else
            {
                dic.Remove(nameof(Sysmenu.KID));
                dic.Add(nameof(Sysmenu.CreateTime), DateTime.Now);
                dic.Add(nameof(Sysmenu.CreateUserId), emp?.Model.KID);
                dic.Add(nameof(Sysmenu.CreateUserName), emp?.Model.UserName);
                result = BlogHelper.Add_Sysmenu(dic, new Service.Models.View.OpertionUser());
            }
            return MyJson(result);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="menuid"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Delete(int menuid)
        {
            var result = new Result();
            result = BlogHelper.DeleteByWhere_Sysmenu(new Dictionary<string, object>() { {
                    nameof(Sysmenu.KID),menuid
                } }, new Service.Models.View.OpertionUser());
            return MyJson(result);
        }
    }
}