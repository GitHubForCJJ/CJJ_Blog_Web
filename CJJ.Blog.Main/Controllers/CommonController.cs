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
using FastDev.Common.Code;
using QRCoder;
using System.Drawing;
using System.IO;
using System.Drawing.Imaging;

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
            WebUtil.WriteCookie(WebUtil.Tokenkey, "", DateTime.Now.AddDays(-5));
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
                return MyJson(new JsonResponse("", 1, "数据不合法"));
            }
            var srcip = System.Web.HttpContext.Current?.Request?.UserHostAddress;
            var agent = System.Web.HttpContext.Current?.Request?.UserAgent;
            var dns = System.Web.HttpContext.Current?.Request?.UserHostName;
            var res = BlogHelper.EmployeePasswordLogin(model.UserAccount, model.Password, srcip, agent, dns);
            if (res.IsSucceed)
            {

                //WebUtil.WriteCookie(WebUtil.Userinfokey, res.SerializeObject(), DateTime.Parse(res.TokenExpiration));
                WebUtil.WriteCookie(WebUtil.Tokenkey, res.Token, DateTime.Parse(res.TokenExpiration));
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
        [HttpGet]
        public ActionResult ChangePsw()
        {
            return View();
        }
        /// <summary>
        /// 改密码
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ChangePsw(string psw)
        {
            var res = new Result() { IsSucceed = false };
            if (string.IsNullOrEmpty(psw))
            {
                return MyJson(res);
            }
            var emp = EmployeeInfo?.Model;
            res = BlogHelper.UpdateByWhere_Employee(new Dictionary<string, object>()
            {
                {nameof(Employee.UserPassword),psw }
            }, new Dictionary<string, object>() {
                {nameof(Employee.KID), emp?.KID}
            }, new Service.Models.View.OpertionUser());
            return MyJson(res);
        }

        /// <summary>
        /// 创建二维码
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult CreateCode(string key)
        {
            // 生成二维码的内容
            string strCode = string.Format("{0}{1}{2}", @"https://open.weixin.qq.com/connect/oauth2/authorize?appid=wx8e37ff182337cd1f&redirect_uri=http%3a%2f%2frodman.nat300.top%2fapi%2fwchart%2fCodeLogin&response_type=code&scope=snsapi_userinfo&state=", key, @"&connect_redirect=1#wechat_redirect");
            QRCodeGenerator qrGenerator = new QRCoder.QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(strCode, QRCodeGenerator.ECCLevel.Q);
            QRCode qrcode = new QRCode(qrCodeData);

            // qrcode.GetGraphic 方法可参考最下发“补充说明”
            Bitmap qrCodeImage = qrcode.GetGraphic(5, Color.Black, Color.White, null, 15, 6, false);
            MemoryStream ms = new MemoryStream();
            qrCodeImage.Save(ms, ImageFormat.Jpeg);

            // 如果想保存图片 可使用  qrCodeImage.Save(filePath);

            //// 响应类型
            //context.Response.ContentType = "image /Jpeg";
            ////输出字符流
            //context.Response.BinaryWrite(ms.ToArray());
            return File(ms.ToArray(), "image/Jpeg");
        }


    }
}