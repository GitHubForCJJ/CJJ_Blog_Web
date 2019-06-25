using CJJ.Blog.Main.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CJJ.Blog.Main.Controllers
{
    [CheckPermisstionFilter]
    public class BaseController : Controller
    {
    }
}