
using FastDev.Common.Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CJJ.Blog.Web.Controllers
{
    public class BaseController : Controller
    {
        public JsonResponse FastJson(object data,string msg="",int code = 0, int retcut=0,string token="")
        {
            return new JsonResponse() { Code = code, Msg = msg, Count = retcut, Data = data };
        }
    }
}