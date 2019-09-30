using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Owin;
using Microsoft.AspNet.SignalR;

[assembly: OwinStartup(typeof(CJJ.Blog.Main.Models.Startup))]
namespace CJJ.Blog.Main.Models
{

    public class Startup
    {
        /// <summary>
        /// 注册方法
        /// </summary>
        /// <param name="app">The application.</param>
        public void Configuration(IAppBuilder app)
        {
            //服务器的hub注册
            app.MapSignalR();
        }
    }
}