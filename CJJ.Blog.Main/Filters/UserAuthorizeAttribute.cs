using CJJ.Blog.Main.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CJJ.Blog.Main.Filters
{
    public class UserAuthorizeAttribute : AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            try
            {
                if (!UserData.IsLogin)
                {
                    ProcessNoPermisstion(filterContext);
                }
            }
            catch
            {
                ProcessNoPermisstion(filterContext);
            }
        }
        /// <summary>
        /// 处理未登录的，跳转到登录
        /// </summary>
        /// <param name="filterContext"></param>
        public void ProcessNoPermisstion(AuthorizationContext filterContext)
        {
            var url = "/Common/SysLogin";

            filterContext.Result = new RedirectResult(url);
        }
    }
}