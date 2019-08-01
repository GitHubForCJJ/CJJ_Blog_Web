using CJJ.Blog.Main.Filters;
using CJJ.Blog.Main.Models;
using CJJ.Blog.Main.Tools;
using CJJ.Blog.Service.Model.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace CJJ.Blog.Main.Controllers
{
    public class BaseController : Controller
    {
        public JsonResult MyJson(object data)
        {
            return Json(data, "application/json; charset=utf-8", Encoding.UTF8, JsonRequestBehavior.AllowGet);
        }
        protected override JsonResult Json(object data, string contentType, Encoding contentEncoding, JsonRequestBehavior behavior)
        {
            return new JsonResultExtension()
            { Data = data, ContentType = contentType, ContentEncoding = contentEncoding, JsonRequestBehavior = behavior };
        }

        public SysLoginUser EmployeeInfo
        {
            get
            {
                return UserData.GetSysLoginUser();
            }
        }

        /// <summary>
        /// 获取客户端IP地址
        /// </summary>
        /// <returns>System.String.</returns>
        public static string GetIP()
        {
            try
            {
                string result = System.Web.HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
                if (string.IsNullOrEmpty(result))
                {
                    result = System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
                }

                if (string.IsNullOrEmpty(result))
                {
                    result = System.Web.HttpContext.Current.Request.UserHostAddress;
                }

                if (string.IsNullOrEmpty(result))
                {
                    return "0.0.0.0";
                }

                return result;
            }
            catch (Exception)
            {
                return "0.0.0.0";
            }
        }
    }
}