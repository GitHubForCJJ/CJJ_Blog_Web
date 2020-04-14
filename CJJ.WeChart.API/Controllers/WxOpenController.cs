using CJJ.WeChart.API.Views;
using FastDev.Http;
using Senparc.Weixin;
using Senparc.Weixin.WxOpen.AdvancedAPIs.Sns;
using Senparc.Weixin.WxOpen.Containers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

using System.Threading;
using System.Web;
using System.Web.Caching;
using System.Web.Http;
using System.Web.Mvc;
using HttpPostAttribute = System.Web.Mvc.HttpPostAttribute;

namespace CJJ.WeChart.API.Controllers
{
    public class WxOpenController : Controller
    {
        /// <summary>
        /// wx.login登陆成功之后发送的请求
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult OnLogin(string code)
        {
            try
            {
                var jsonResult = SnsApi.JsCode2Json("wxf40f763aa2171652", "06c9233a3d979d45f665dbb507a01115", code);
                if (jsonResult.errcode == ReturnCode.请求成功)
                {
                    var unionId = "";
                    var sessionBag = SessionContainer.UpdateSession(null, jsonResult.openid, jsonResult.session_key, unionId);
                    HttpRuntime.Cache.Insert($"{sessionBag.OpenId}_openid", sessionBag.OpenId, null, DateTime.Now.AddMinutes(15), Cache.NoSlidingExpiration, CacheItemPriority.High, null);
                    HttpRuntime.Cache.Insert($"{sessionBag.OpenId}_unionid", sessionBag.UnionId, null, DateTime.Now.AddMinutes(15), Cache.NoSlidingExpiration, CacheItemPriority.High, null);



                    return Json(new { IsSuccess = true, Message = "", sessionId = sessionBag.Key, openId = sessionBag.OpenId, unionId = sessionBag.UnionId });
                }
                else
                {
                    return Json(new { IsSuccess = false, Message = jsonResult.errmsg });
                }
            }
            catch (Exception ex)
            {
                return Json(new { IsSuccess = false, Message = ex.Message });
            }
        }
        /// <summary>
        /// 解密电话号码
        /// </summary>
        /// <param name="sessionId">The session identifier.</param>
        /// <param name="encryptedData">The encrypted data.</param>
        /// <param name="iv">The iv.</param>
        /// <param name="openId">The open identifier.</param>
        /// <returns>ActionResult.</returns>
        public ActionResult DecryptPhoneNumber(string sessionId, string encryptedData, string iv, string openId)
        {
            var sessionBag = SessionContainer.GetSession(sessionId);
            try
            {
                var ret = Senparc.Weixin.WxOpen.Helpers.EncryptHelper.DecryptPhoneNumber(sessionId, encryptedData, iv);
                HttpRuntime.Cache.Insert($"{openId}_phone", ret.phoneNumber, null, DateTime.Now.AddMinutes(15), Cache.NoSlidingExpiration, CacheItemPriority.High, null);
                HttpRuntime.Cache.Insert($"{openId}_openid", openId, null, DateTime.Now.AddMinutes(15), Cache.NoSlidingExpiration, CacheItemPriority.High, null);
                return Json(new { IsSuccess = true, Message = ret });
            }
            catch (Exception ex)
            {
                return Json(new { IsSuccess = false, Message = ex.Message });
            }
        }

        /// <summary>
        /// 存储formid
        /// </summary>
        /// <param name="openId">The open identifier.</param>
        /// <param name="formId">The form identifier.</param>
        /// <returns>ActionResult.</returns>
        public ActionResult StorageFormId(string openId, string formId)
        {
            if (!string.IsNullOrWhiteSpace(openId) && !string.IsNullOrWhiteSpace(formId) && formId != "the formId is a mock one")
            {
                //Result ret = UserHelper.Add_Wxopenformid(openId, formId, DateTime.Now.AddDays(6));
                return Json(new { IsSuccess = true});
            }
            else
            {
                return Json(new { IsSuccess = false, Message = "不能为空" });
            }
        }
    }
}
