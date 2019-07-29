using CJJ.Blog.NetWork.WcfHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CJJ.Blog.Main.Controllers
{
    public class MenuController : BaseController
    {
        // GET: Menu
        public ActionResult Index()
        {
            var emp = EmployeeInfo.Model;
            var menus = BlogHelper.GetListByuserid_Sysmenu(emp.KID);
            return View(menus);
        }
    }
}