﻿using CJJ.Blog.Main.Models;
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
                var cookie = new HttpCookie(ConfigUtil.Userinfokey, res.SerializeObject());
                //cookie.Domain = ".cjj81.cn";
                cookie.HttpOnly = true;
                cookie.Expires = DateTime.Parse(res.TokenExpiration);
                System.Web.HttpContext.Current?.Response.Cookies.Add(cookie);
                var cookietoken = new HttpCookie(ConfigUtil.Tokenkey, res.Token);
                //cookie.Domain = ".cjj81.cn";
                cookietoken.HttpOnly = true;
                cookietoken.Expires = DateTime.Parse(res.TokenExpiration);
                System.Web.HttpContext.Current?.Response.Cookies.Add(cookietoken);
                return MyJson(new JsonResponse(res, 0, "登录成功"));
            }
            return MyJson(new JsonResponse("", 1, "登录失败"));

        }


    }
}