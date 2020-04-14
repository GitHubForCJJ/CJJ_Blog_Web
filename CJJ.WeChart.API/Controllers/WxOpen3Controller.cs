using CJJ.WeChart.API.Views;
using FastDev.Http;
using Senparc.Weixin;
using Senparc.Weixin.WxOpen.AdvancedAPIs.Sns;
using Senparc.Weixin.WxOpen.Containers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Caching;
using System.Web.Http;

namespace CJJ.WeChart.API.Controllers
{
    public class WxOpen3Controller : BaseApiController
    {
        /// <summary>
        /// wx.login登陆成功之后发送的请求
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResponse OnLogin([FromBody]string code)
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



                    return FastResponse(new { IsSuccess = true, Message = "", sessionId = sessionBag.Key, openId = sessionBag.OpenId, unionId = sessionBag.UnionId });
                }
                else
                {
                    return FastResponse(new { IsSuccess = false, Message = jsonResult.errmsg });
                }
            }
            catch (Exception ex)
            {
                return FastResponse(new { IsSuccess = false, Message = ex.Message });
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
        [HttpPost]
        public JsonResponse DecryptPhoneNumber([FromBody]PhoneView phoneView)
        {
            var sessionBag = SessionContainer.GetSession(phoneView.SessionId);
            try
            {
                var ret = Senparc.Weixin.WxOpen.Helpers.EncryptHelper.DecryptPhoneNumber(phoneView.SessionId, phoneView.EncryptedData, phoneView.Iv);
                HttpRuntime.Cache.Insert($"{phoneView.OpenId}_phone", ret.phoneNumber, null, DateTime.Now.AddMinutes(15), Cache.NoSlidingExpiration, CacheItemPriority.High, null);
                HttpRuntime.Cache.Insert($"{phoneView.OpenId}_openid", phoneView.OpenId, null, DateTime.Now.AddMinutes(15), Cache.NoSlidingExpiration, CacheItemPriority.High, null);
                return FastResponse(new { IsSuccess = true, Message = ret });
            }
            catch (Exception ex)
            {
                return FastResponse(new { IsSuccess = false, Message = ex.Message });
            }
        }

        /// <summary>
        /// 存储formid
        /// </summary>
        /// <param name="openId">The open identifier.</param>
        /// <param name="formId">The form identifier.</param>
        /// <returns>ActionResult.</returns>
        public JsonResponse StorageFormId(string openId, string formId)
        {
            if (!string.IsNullOrWhiteSpace(openId) && !string.IsNullOrWhiteSpace(formId) && formId != "the formId is a mock one")
            {
                //Result ret = UserHelper.Add_Wxopenformid(openId, formId, DateTime.Now.AddDays(6));
                return FastResponse(new { IsSuccess = true });
            }
            else
            {
                return FastResponse(new { IsSuccess = false, Message = "不能为空" });
            }
        }
    }
}
