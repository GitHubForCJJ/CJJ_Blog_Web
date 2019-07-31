using CJJ.Blog.NetWork.WcfHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CJJ.Blog.Main.Tools;
using CJJ.Blog.Service.Model.View;

namespace CJJ.Blog.Main.Controllers
{
    public class MenuController : BaseController
    {
        // GET: Menu
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
        public JsonResult List()
        {
            //var emp = EmployeeInfo.Model;
            //var menus = BlogHelper.GetListByuserid_Sysmenu(emp.KID);
            var userinfo = System.Web.HttpContext.Current?.Request?.Cookies.Get(WebUtil.Userinfokey)?.Value;
            var menus = userinfo.DeserializeObject<SysLoginUser>()?.UserAuthorMenu?.UserMenuList;
            return MyJson(menus);
        }
    }
}