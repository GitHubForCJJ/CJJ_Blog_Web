using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CJJ.Blog.Main.Controllers
{
    public class BlogController : BaseController
    {
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult AddOrEdit()
        {
            return View();
        }
        [HttpPost]
        public void AddOrEdit(string content)
        {

        }
    }
}