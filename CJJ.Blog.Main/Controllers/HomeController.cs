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
        public ActionResult Index(int a=123)
        {
            return View();
        }
    }
}