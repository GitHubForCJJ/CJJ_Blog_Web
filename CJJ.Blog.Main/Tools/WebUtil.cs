using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CJJ.Blog.Main
{
    public class WebUtil
    {
        //授权码
        public static string Tokenkey = "Token";
        //菜单
        public static string Sysmenukey = "Sysmenu";
        //登录信息
        public static string Userinfokey = "Userinfokey";


        public static void WriteCookie(string name,string data,DateTime date, bool httponly=true)
        {
            var cookie = System.Web.HttpContext.Current.Response.Cookies[name];
            if (cookie != null)
            {
                System.Web.HttpContext.Current.Response.Cookies.Remove(name);
            }
            cookie = new HttpCookie(name);
            cookie.Value = data;
            cookie.Expires = date;
            cookie.HttpOnly = httponly;
            System.Web.HttpContext.Current.Response.Cookies.Add(cookie);
        }
        public static string ReadCookie(string name)
        {
            var cookie = System.Web.HttpContext.Current.Request.Cookies.Get(name);
            if (cookie == null)
            {
                return string.Empty;
            }
            return cookie?.Value;
        }

    }
}