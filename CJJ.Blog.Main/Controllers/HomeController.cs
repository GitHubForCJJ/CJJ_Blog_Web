using CJJ.Blog.Main.Models;
using CJJ.Blog.Service.Model.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CJJ.Blog.Main.Controllers
{
    public class HomeController : BaseController
    {
        // GET: Home
        public ActionResult Index()
        {
            SysLoginUser emp = UserData.GetSysLoginUser();
            return View();
        }
    }
}