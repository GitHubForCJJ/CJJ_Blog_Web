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
        public static SysLoginUser  GetSysLoginUser(string token)
        {
            if (string.IsNullOrEmpty(token))
            {
                return null;
            }
            var sys = BlogHelper.GetSysLoginUserByToken(token);
            return sys;
        }
    }
}