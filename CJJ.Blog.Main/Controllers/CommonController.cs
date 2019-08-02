using CJJ.Blog.Main.Models;
using CJJ.Blog.NetWork.WcfHelper;
using CJJ.Blog.Service.Models.Data;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using LoginView = CJJ.Blog.Main.Models.LoginView;
using CJJ.Blog.Main.Tools;

namespace CJJ.Blog.Main.Controllers
{
    public class CommonController : BaseController
    {
        // GET: Common
        public ActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// 登录页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult SysLogin()
        {
            //WebUtil.WriteCookie(WebUtil.Userinfokey,"",DateTime.Now.AddDays(-5));
            WebUtil.WriteCookie(WebUtil.Tokenkey,"", DateTime.Now.AddDays(-5));
            return View();
        }
        /// <summary>
        /// 管理员登录
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Login(LoginView model)
        {
            if (model == null || string.IsNullOrWhiteSpace(model.UserAccount) || string.IsNullOrWhiteSpace(model.Password))
            {
                return MyJson(new JsonResponse ("", 1, "数据不合法" ));
            }
            var srcip = System.Web.HttpContext.Current?.Request?.UserHostAddress;
            var agent = System.Web.HttpContext.Current?.Request?.UserAgent;
            var dns = System.Web.HttpContext.Current?.Request?.UserHostName;
            var res = BlogHelper.EmployeePasswordLogin(model.UserAccount, model.Password, srcip, agent, dns);
            if (res.IsSucceed)
            {

                //WebUtil.WriteCookie(WebUtil.Userinfokey, res.SerializeObject(), DateTime.Parse(res.TokenExpiration));
                WebUtil.WriteCookie(WebUtil.Tokenkey,res.Token, DateTime.Parse(res.TokenExpiration));              
                return MyJson(new JsonResponse(res, 0, "登录成功"));
            }
            return MyJson(new JsonResponse("", 1, "登录失败"));

        }

        /// <summary>
        /// loginout
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult LoginOut()
        {
            //WebUtil.WriteCookie(WebUtil.Userinfokey, "", DateTime.Now.AddDays(-5));
            WebUtil.WriteCookie(WebUtil.Tokenkey, "", DateTime.Now.AddDays(-5));
            return RedirectToAction("SysLogin");
        }


    }
}