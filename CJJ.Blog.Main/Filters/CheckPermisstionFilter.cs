using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CJJ.Blog.Main.Filters
{
    public class CheckPermisstionFilter : ActionFilterAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
           
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {

        }
    }
}