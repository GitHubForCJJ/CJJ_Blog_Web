using CJJ.Blog.Main.Models;
using CJJ.Blog.NetWork.WcfHelper;
using CJJ.Blog.Service.Models.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using LoginView = CJJ.Blog.Main.Models.LoginView;

namespace CJJ.Blog.Main.Controllers
{
    public class CommonController : Controller
    {
        // GET: Common
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult SysLogin()
        {
            return View();
        }
        /// <summary>
        /// 管理员登录
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResponse Login(string UserAccount,string Password)
        {
            //if (model == null||string.IsNullOrWhiteSpace(model.UserAccount)||string.IsNullOrWhiteSpace(model.Password))
            //{
            //    return new JsonResponse("", 1,"数据不合法");
            //}
            //var srcip = System.Web.HttpContext.Current?.Request?.UserHostAddress;
            //var agent = System.Web.HttpContext.Current?.Request?.UserAgent;
            //var dns = System.Web.HttpContext.Current?.Request?.UserHostName;
            //var res = BlogHelper.EmployeePasswordLogin(model.UserAccount, model.Password, srcip, agent, dns);
            //if (res.IsSucceed)
            //{
            //    var cookie = new HttpCookie("Token", res.Token);
            //    cookie.Domain = ".cjj81.cn";
            //    cookie.HttpOnly = true;
                
            //    System.Web.HttpContext.Current?.Response.Cookies.Add(cookie);
            //    return new JsonResponse(res, 0, "登录成功");
            //}
            return new JsonResponse("", 1, "登录失败");

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