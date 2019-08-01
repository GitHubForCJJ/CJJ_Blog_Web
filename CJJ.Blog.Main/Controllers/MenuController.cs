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
        public JsonResult AddOrUpdate(FormCollection dic) 
        {
            //var data = JsonConvert.DeserializeObject<Dictionary<string, object>>(model.Data.ToString());

            return MyJson(dic.AllKeys);
        }
        private Dictionary<string,object>FormToDic(FormCollection form)
        {
            var dic = new Dictionary<string, object>();
            foreach(var item in form.Keys)
            {

            }
            return dic;
        }
    }
}