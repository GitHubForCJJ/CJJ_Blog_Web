using CJJ.Blog.Main.Filters;
using CJJ.Blog.Main.Models;
using CJJ.Blog.Main.Tools;
using CJJ.Blog.Service.Model.View;
using CJJ.Blog.Service.Models.Data;
using FastDev.Common.Code;
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

        //public void AddBaseInfo<T>(Dictionary<string,object>data,out Dictionary<string, object> outdata)
        //{
            
        //}

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

        /// <summary>
        /// 添加时处理模型的CreateTime、CreateUserId、CreateUserName。更新时处理UpdateTime...
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="model">待处理的model</param>
        /// <param name="emp">操作者员工</param>
        /// <param name="dic">返回的字典</param>
        /// <param name="opt">返回的操作者对象</param>
        public void AddBaseInfo<T>(T model, Employee emp, out Dictionary<string, object> dic, out Service.Models.View.OpertionUser opt)
        {
            dic = new Dictionary<string, object>();
            opt = new Service.Models.View.OpertionUser();
            var propertys = typeof(T).GetProperties();
            var idpro = propertys.FirstOrDefault(x => x.Name == "KID");
            int kid = 0;
            if (idpro != null)
            {
                kid = idpro.GetValue(model).Toint();
            }
            if (kid > 0)
            {
                if (propertys.Count(p => p.Name == "UpdateTime") > 0)
                {
                    dic.Add("UpdateTime", DateTime.Now);
                }
                if (propertys.Count(p => p.Name == "UpdateUserId") > 0)
                {
                    dic.Add("UpdateUserId", emp?.KID);
                }
                if (propertys.Count(p => p.Name == "UpdateUserName") > 0)
                {
                    dic.Add("UpdateUserName", emp?.UserName);
                }
            }
            else
            {
                if (propertys.Count(p => p.Name == "CreateTime") > 0)
                {
                    dic.Add("CreateTime", DateTime.Now);
                }
                if (propertys.Count(p => p.Name == "CreateUserId") > 0)
                {
                    dic.Add("CreateUserId", emp?.KID);
                }
                if (propertys.Count(p => p.Name == "CreateUserName") > 0)
                {
                    dic.Add("CreateUserName", emp?.UserName);
                }
            }
            opt.UserId = emp?.KID.ToString();
            opt.UserName = emp?.UserName;
            opt.UserClientIp = GetIP();

        }

    }
}