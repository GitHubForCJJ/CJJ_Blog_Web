using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CJJ.WeChart.API.Controllers
{
    public class WsdkController : Controller
    {
        // GET: Wsdk
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult Share()
        {
            return View();
        }
    }
}