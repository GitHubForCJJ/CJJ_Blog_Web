using CJJ.Blog.NetWork.WcfHelper;
using CJJ.Blog.Service.Model.View;
using CJJ.Blog.Service.Models.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CJJ.Blog.Main.Models
{
    public class UserData
    {
        public static SysLoginUser GetSysLoginUser()
        {
            string token = "";
            token = HttpContext.Current.Request.Params?[WebUtil.Tokenkey]?.ToString();
            if (string.IsNullOrEmpty(token))
            {
                token = HttpContext.Current.Request.Cookies?[WebUtil.Tokenkey].Value?.ToString();
            }
            if (string.IsNullOrEmpty(token))
            {
                return new SysLoginUser { IsSucceed=false};
            }
            var sys = BlogHelper.GetSysLoginUserByToken(token);
            return sys;
        }
        public static bool IsLogin
        {
            get
            {
                var sys = GetSysLoginUser();
                if (sys.IsSucceed && DateTime.Parse(sys.TokenExpiration) > DateTime.Now)
                {
                    return true;
                }
                return false;
            }
        }
    }
}