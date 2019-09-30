using CJJ.Blog.NetWork.WcfHelper;
using CJJ.Blog.Service.Models.Data;
using CJJ.WeChart.API.Helper;
using CJJ.WeChart.API.Models;
using FastDev.Common.Encrypt;
using FastDev.Http;
using FastDev.Log;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using HttpHelper = CJJ.WeChart.API.Helper.HttpHelper;
using HttpItem = CJJ.WeChart.API.Helper.HttpItem;
using HttpResult = CJJ.WeChart.API.Helper.HttpResult;

namespace CJJ.WeChart.API.Controllers
{
    public class WchartController : BaseApiController
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public JsonResponse Test()
        {
            LogHelper.WriteLog("测试");
            return FastResponse("测试");
        }

        /// <summary>
        /// 测试号接口验证,接收接口事件推送
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public string Check()
        {
            string signature = HttpContext.Current.Request.QueryString["signature"]?.ToString() ?? "";
            string timestamp = HttpContext.Current.Request.QueryString["timestamp"]?.ToString() ?? "";
            string token = HttpContext.Current.Request.QueryString["token"]?.ToString() ?? "";
            string nonce = HttpContext.Current.Request.QueryString["nonce"]?.ToString() ?? "";
            string echostr = HttpContext.Current.Request.QueryString["echostr"]?.ToString() ?? "";

            LogHelper.WriteLog("signature:" + signature);
            LogHelper.WriteLog("timestamp:" + timestamp);
            LogHelper.WriteLog("token:" + token);
            LogHelper.WriteLog("nonce:" + nonce);
            LogHelper.WriteLog("echostr:" + echostr);

            #region 获取事件推送
            string body = "";
            using (var reader = new StreamReader(HttpContext.Current.Request.InputStream))
            {
                body = reader.ReadToEnd();
                LogHelper.WriteLog("wx事件内容" + body);
            }
            if (string.IsNullOrEmpty(body))
            {
                return echostr;
            }
            WxEventModel reinfo = WxXmlHelper.Deserialize(typeof(WxEventModel), body) as WxEventModel;
            CallBackEvent(reinfo);
            #endregion

            string[] ArrTmp = { token, timestamp, nonce };
            string tmpStr = string.Join("", ArrTmp);
            var sha1 = System.Security.Cryptography.SHA1.Create();
            var a = System.Text.Encoding.UTF8.GetBytes(tmpStr);
            var bytes = sha1.ComputeHash(a);
            var ostr = Convert.ToBase64String(bytes);
            LogHelper.WriteLog("ostr:" + ostr);
            ostr = ostr.ToLower();
            if (ostr == signature)
            {
                return echostr;
            }
            else
            {
                return echostr;
            }
        }

        #region JSAPI

        /// <summary>
        /// 获取jsapi accesstoken
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public JsonResponse GetJsapiToken()
        {
            string res = GetJStoken();
            return FastResponse(res);
        }
        private string GetJStoken()
        {
            string jstoken = "";
            try
            {
                jstoken = HttpContext.Current.Cache.Get(WchartApiConst.jsapiaccesstokenkey)?.ToString() ?? "";
                if (!string.IsNullOrEmpty(jstoken))
                {
                    return jstoken;
                }
                string url = @"https://api.weixin.qq.com/cgi-bin/ticket/getticket?access_token=" + GetAccesstoken() + "&type=jsapi";
                string res = BuildGet(url);
                LogHelper.WriteLog("jsapires:" + res);
                JSAcctokenModel model = JsonConvert.DeserializeObject<JSAcctokenModel>(res);
                if (model.errcode == 0)
                {
                    HttpContext.Current.Cache.Insert(WchartApiConst.jsapiaccesstokenkey, model.ticket, null, DateTime.Now.AddMinutes(119), System.Web.Caching.Cache.NoSlidingExpiration);
                }

                jstoken = model?.ticket ?? "";
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog(ex, "GetJStoken");
            }
            return jstoken;
        }
        /// <summary>
        /// 计算签名值
        /// </summary>
        /// <param name="noncestr"></param>
        /// <param name="timespan"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResponse BuildSign([FromBody]Signdata data)
        {
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            data.appId = WchartApiConst.appid;
            data.noncestr = Guid.NewGuid().ToString();
            data.timespan = Convert.ToInt32(ts.TotalSeconds).ToString();
            string jsapi_acctoken = GetJStoken();
            LogHelper.WriteLog("jsapi_acctoken:" + jsapi_acctoken);
            data.url = HttpUtility.UrlDecode(data.url, System.Text.Encoding.UTF8);
            LogHelper.WriteLog(data.url);
            string signdata = $"jsapi_ticket={jsapi_acctoken}&noncestr={data.noncestr}&timestamp={data.timespan}&url={data.url}";
            LogHelper.WriteLog(signdata);
            string res = Sha1Encrypt(signdata)?.ToLower();
            data.signature = res;
            return FastResponse(data);
        }

        private string Sha1Encrypt(string source)
        {
            try
            {
                var sha1 = new SHA1CryptoServiceProvider();
                byte[] byt1 = System.Text.Encoding.UTF8.GetBytes(source);
                byte[] byt2 = sha1.ComputeHash(byt1);
                sha1.Dispose();
                string result = BitConverter.ToString(byt2);
                result = result.Replace("-", "");
                return result;
            }
            catch
            {
                return "";
            }
        }

        #endregion


        #region 扫码登录

        /// <summary>
        /// 微信二维码登录
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public JsonResponse CodeLogin()
        {
            try
            {
                string state = HttpContext.Current.Request.QueryString["state"]?.ToString() ?? "";
                WxOauthModel info = GetOpenidByCode();
                string infomodel = System.Web.HttpContext.Current.Cache.Get(info.openid)?.ToString() ?? "";
                if (!string.IsNullOrEmpty(infomodel))
                {
                    WxUserBaseInfoModel baseinfo = JsonConvert.DeserializeObject<WxUserBaseInfoModel>(infomodel.ToString());
                    System.Web.HttpContext.Current.Cache.Insert(state, baseinfo.openid, null, DateTime.Now.AddMinutes(5), System.Web.Caching.Cache.NoSlidingExpiration);
                    //客服消息提示扫码成功
                    return FastResponse("扫码授权成功");
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog(ex, "codelogin");
            }

            return FastResponse("扫码授权失败");
        }
        /// <summary>
        /// 查询是否授权成功
        /// </summary>
        [HttpPost]
        public JsonResponse CheckState([FromBody]string key)
        {
            var openid = HttpContext.Current.Cache.Get(key)?.ToString() ?? "";
            if (string.IsNullOrEmpty(openid))
            {
                return FastResponse(new { auth = false });
            }
            return FastResponse(new { auth = true, openid = openid });
        }

        #endregion

        #region private
        /// <summary>
        /// 获取接口调用accesstoken，这里指普通的那个
        /// </summary>
        /// <returns></returns>
        private string GetAccesstoken()
        {
            string accesstoken = "";
            try
            {
                accesstoken = HttpContext.Current.Cache.Get(WchartApiConst.accesstokenkey)?.ToString() ?? "";
                if (!string.IsNullOrEmpty(accesstoken))
                {
                    return accesstoken;
                }

                string url = $"{WchartApiConst.access_token}?grant_type=client_credential&appid={WchartApiConst.appid}&secret={WchartApiConst.appsecret}";
                HttpItem httpItem = new HttpItem();
                httpItem.URL = url;
                httpItem.Method = "GET";
                HttpHelper helper = new HttpHelper();
                HttpResult httpResult = helper.GetHtml(httpItem);
                var reshtml = httpResult.Html;
                LogHelper.WriteLog("reshtml:" + reshtml);
                var model = JsonConvert.DeserializeObject<AccesstokenModel>(reshtml);
                if (model.errcode == 0)
                {
                    HttpContext.Current.Cache.Insert(WchartApiConst.accesstokenkey, model.access_token, null, DateTime.Now.AddMinutes(120), System.Web.Caching.Cache.NoSlidingExpiration);
                    return model.access_token;
                }
            }
            catch (Exception ex)
            {

            }
            return string.Empty;

        }
        #endregion


        #region 微信回调事件处理

        private void CallBackEvent(WxEventModel data)
        {
            if (data.MsgType.ToUpper() == "EVENT")
            {
                var eventtype = data.Event.ToUpper();
                switch (eventtype)
                {
                    //关注/取消关注
                    case "SUBSCRIBE":
                        SubscribeEvent(data, "WelCommon to my app");
                        break;
                    //扫描带参数二维码事件已关注的情况下
                    case "SCAN":
                        SubscribeEvent(data, "WelCommon to my app two");
                        break;
                    //位置
                    case "LOCATION":

                        break;

                    //点击自定义菜单
                    case "CLICK":
                        break;
                }

            }
            else if (data.MsgType.ToUpper() == "VIEW")
            {

            }
        }

        /// <summary>
        /// 关注公众号
        /// </summary>
        /// <param name="data"></param>
        private void SubscribeEvent(WxEventModel data, string content)
        {
            //WxUserBaseInfoModel model = GetBaseByOpenid(data.FromUserName);
            CsMsgModel msgModel = new CsMsgModel();
            msgModel.msgtype = MsgType.text;
            msgModel.touser = @"oQQbc1XNMQ9cEd5ROVZjgM5WToAA";
            msgModel.text = new
            {
                content = content
            };
            SendCsMsg(msgModel);
        }

        /// <summary>
        /// 点击自定义菜单
        /// </summary>
        /// <param name="data"></param>
        private void ClickEvent(WxEventModel data)
        {

        }

        /// <summary>
        /// 发送客服消息
        /// </summary>
        /// <param name="msg"></param>
        private BaseModel SendCsMsg(CsMsgModel model)
        {
            BaseModel res = null;
            try
            {
                string acctoken = GetAccesstoken();
                string url = @"https://api.weixin.qq.com/cgi-bin/message/custom/send?access_token=" + acctoken;
                BaseModel resmodel = BuildPost(model, url);

            }
            catch (Exception ex)
            {
                LogHelper.WriteLog(ex, "SendCsMsg");
                res = new BaseModel();
            }
            return res;
        }

        #endregion


        #region 公众号基本信息相关
        /// <summary>
        /// 创建自定义菜单
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResponse CreateMenu()
        {
            string accesstoken = GetAccesstoken();
            string url = @"https://api.weixin.qq.com/cgi-bin/menu/create?access_token=" + accesstoken;
            var buttonlist = new List<MenuModel>();
            buttonlist.Add(new MenuModel
            {
                name = "今朝有酒t7",
                sub_button = new List<MenuModel>()
                  {
                      new MenuModel
                      {
                          type="view",
                          name="获取info",
                          url=@"https://open.weixin.qq.com/connect/oauth2/authorize?appid="+WchartApiConst.appid+"&redirect_uri=http%3a%2f%2frodman.nat300.top%2fapi%2fwchart%2fGetUserBaseInfo&response_type=code&scope=snsapi_userinfo&state=aaa&connect_redirect=1#wechat_redirect"
                      }
                  }
            });
            buttonlist.Add(new MenuModel
            {
                name = "今朝有酒tt7",
                sub_button = new List<MenuModel>()
                  {
                      new MenuModel
                      {
                          type="view",
                          name="获取info2",
                          url=@"https://open.weixin.qq.com/connect/oauth2/authorize?appid="+WchartApiConst.appid+"&redirect_uri=http%3a%2f%2frodman.nat300.top%2fwsdk%2findex&response_type=code&scope=snsapi_userinfo&state=aaa&connect_redirect=1#wechat_redirect"
                      }
                  }
            });

            HttpItem httpItem = new HttpItem();
            httpItem.URL = url;
            httpItem.PostEncoding = System.Text.Encoding.UTF8;
            httpItem.Encoding = System.Text.Encoding.UTF8;
            httpItem.Method = "POST";
            httpItem.Postdata = JsonConvert.SerializeObject(new { button = buttonlist });
            HttpHelper helper = new HttpHelper();
            HttpResult httpResult = helper.GetHtml(httpItem);
            var reshtml = httpResult.Html;
            LogHelper.WriteLog("reshtml:" + reshtml);

            var model = JsonConvert.DeserializeObject<AccesstokenModel>(reshtml);
            if (model.errcode == 0)
            {
                return FastResponse("just ok");
            }
            else
            {
                return FastResponse(reshtml);
            }
        }

        [HttpGet]
        public JsonResponse GetMenu()
        {
            string accesstoken = GetAccesstoken();
            string url = @"https://api.weixin.qq.com/cgi-bin/menu/get?access_token=" + accesstoken;
            HttpItem httpItem = new HttpItem();
            httpItem.URL = url;
            httpItem.Method = "GET";
            HttpHelper helper = new HttpHelper();
            HttpResult httpResult = helper.GetHtml(httpItem);
            var reshtml = httpResult.Html;
            LogHelper.WriteLog("getmenu html:" + reshtml);
            var model = JsonConvert.DeserializeObject<AccesstokenModel>(reshtml);

            return FastResponse(reshtml);

        }

        /// <summary>
        /// 删除自定义菜单
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public JsonResponse DeleteMenu()
        {
            string accesstoken = GetAccesstoken();
            string url = $"https://api.weixin.qq.com/cgi-bin/menu/delete?access_token=" + accesstoken;
            HttpItem httpItem = new HttpItem();
            httpItem.URL = url;
            httpItem.Method = "GET";
            HttpHelper helper = new HttpHelper();
            HttpResult httpResult = helper.GetHtml(httpItem);
            var reshtml = httpResult.Html;
            LogHelper.WriteLog("delete html:" + reshtml);
            var model = JsonConvert.DeserializeObject<AccesstokenModel>(reshtml);
            if (model.errcode == 0)
            {
                return FastResponse("OK");
            }
            else
            {
                return FastResponse(reshtml);
            }
        }

        /// <summary>
        /// 模拟网页回调获取用户基本信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public JsonResponse GetUserBaseInfo()
        {
            var res = UserInfo();
            return FastResponse(res?.Openid ?? "no openid");

        }
        /// <summary>
        /// code换取openid和网页授权acctoken
        /// </summary>
        /// <returns></returns>
        private WxOauthModel GetOpenidByCode()
        {
            WxOauthModel res = new WxOauthModel();
            try
            {
                //第一步code换取accesstoken、openid
                string code = HttpContext.Current.Request.QueryString["code"]?.ToString();
                LogHelper.WriteLog("code:" + code);
                //获取网页授权accesstoken
                string url = $"https://api.weixin.qq.com/sns/oauth2/access_token?appid=" + WchartApiConst.appid + "&secret=" + WchartApiConst.appsecret + "&code=" + code + "&grant_type=authorization_code";
                HttpItem httpItem = new HttpItem();
                httpItem.URL = url;
                httpItem.Method = "GET";
                HttpHelper helper = new HttpHelper();
                HttpResult httpResult = helper.GetHtml(httpItem);
                var reshtml = httpResult.Html;
                LogHelper.WriteLog("html1:" + reshtml);
                res = JsonConvert.DeserializeObject<WxOauthModel>(reshtml);
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog(ex, "GetOpenid");
            }
            return res;
        }
        /// <summary>
        /// 获取用户基本信息
        /// </summary>
        /// <returns></returns>
        private WxUser UserInfo()
        {
            WxUser wxuser = null;
            try
            {
                var model = GetOpenidByCode();
                if (model != null && !string.IsNullOrEmpty(model.openid))
                {
                    wxuser = BlogHelper.GetModelByWhere_WxUser(new Dictionary<string, object>()
                    {
                        {nameof(WxUser.Openid),model.openid },
                        {nameof(WxUser.States),0 },
                        {nameof(WxUser.IsDeleted),0 }
                    });
                    if (wxuser != null && wxuser.KID > 0)
                    {
                        System.Web.HttpContext.Current.Cache.Insert(wxuser.Openid, JsonConvert.SerializeObject(wxuser), null, DateTime.Now.AddDays(1), System.Web.Caching.Cache.NoSlidingExpiration);
                    }
                    else
                    {

                        #region 第二步OPENID换取用户信息

                        WxUserBaseInfoModel model2 = GetBaseByOpenid(model.openid, model.access_token);
                        //异步写入数据库

                        WxUser user = new WxUser();
                        user.Openid = model2.openid;
                        user.NickName = model2.nickname;
                        user.Privilege = string.Join(",", model2.privilege);
                        user.Province = model2.province;
                        user.Sex = model2.sex;
                        user.City = model2.city;
                        user.Country = model2.country;
                        user.HeadImgUrl = model2.headimgurl;
                        user.CreateUserId = "1";
                        user.CreateUserName = "system";
                        user.CreateTime = DateTime.Now;
                        user.UpdateTime = DateTime.Now;
                        var res = BlogHelper.AddByEntity_WxUser(user, null);
                        LogHelper.WriteLog("addentity");
                        if (res.IsSucceed)
                        {
                            user.KID = Convert.ToInt32(res.Message);
                            System.Web.HttpContext.Current.Cache.Insert(user.Openid, JsonConvert.SerializeObject(user), null, DateTime.Now.AddDays(1), System.Web.Caching.Cache.NoSlidingExpiration);
                            wxuser = user;
                        }

                        #endregion
                    }

                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog(ex, "GetUserBaseInfo");
            }
            return wxuser;
        }
        /// <summary>
        /// openid换取基本信息
        /// </summary>
        /// <param name="openid"></param>
        /// <returns></returns>
        private WxUserBaseInfoModel GetBaseByOpenid(string openid, string acctoken)
        {
            WxUserBaseInfoModel res = new WxUserBaseInfoModel();
            if (string.IsNullOrEmpty(openid) || string.IsNullOrEmpty(acctoken))
            {
                return res;
            }
            try
            {
                string url2 = @"https://api.weixin.qq.com/sns/userinfo?access_token=" + acctoken + "&openid=" + openid + "&lang=zh_CN";
                HttpItem httpItem2 = new HttpItem();
                httpItem2.URL = url2;
                httpItem2.Method = "GET";
                HttpHelper helper2 = new HttpHelper();
                HttpResult httpResult2 = helper2.GetHtml(httpItem2);
                res = JsonConvert.DeserializeObject<WxUserBaseInfoModel>(httpResult2.Html);
                LogHelper.WriteLog("html2userinfo:" + httpResult2.Html);
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog(ex, "GetBaseByOpenid");
            }
            return res;
        }

        /// <summary>
        /// 添加客服
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResponse AddCustomerService()
        {
            string accesstoken = GetAccesstoken();
            string url = @"https://api.weixin.qq.com/customservice/kfaccount/add?access_token=" + accesstoken;

            HttpItem httpItem = new HttpItem();
            httpItem.URL = url;
            httpItem.Method = "POST";
            httpItem.Postdata = JsonConvert.SerializeObject(new
            {
                kf_account = "rodman",
                nickname = "Rodman客服",
                password = "123"
            });
            HttpHelper helper = new HttpHelper();
            HttpResult httpResult = helper.GetHtml(httpItem);
            var reshtml = httpResult.Html;
            LogHelper.WriteLog("reshtml:" + reshtml);

            var model = JsonConvert.DeserializeObject<AccesstokenModel>(reshtml);
            if (model.errcode == 0)
            {
                return FastResponse("JUST OK");
            }
            else
            {
                return FastResponse(reshtml);
            }
        }

        #endregion

        #region 消息管理相关
        /// <summary>
        /// 设置所属行业
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResponse SetIndustry()
        {
            string accesstoken = GetAccesstoken();
            string url = @"https://api.weixin.qq.com/cgi-bin/template/api_set_industry?access_token=" + accesstoken;

            HttpItem httpItem = new HttpItem();
            httpItem.URL = url;
            httpItem.Method = "POST";
            httpItem.Postdata = JsonConvert.SerializeObject(new
            {
                industry_id1 = "1",
                industry_id2 = "4",
                industry_id3 = "10",
                industry_id4 = "11",
                industry_id5 = "12"
            });
            HttpHelper helper = new HttpHelper();
            HttpResult httpResult = helper.GetHtml(httpItem);
            var reshtml = httpResult.Html;
            LogHelper.WriteLog("SetIndustry:" + reshtml);

            return FastResponse(reshtml);

        }
        /// <summary>
        /// 获取所属行业
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public JsonResponse GetIndustry()
        {
            string accesstoken = GetAccesstoken();
            string url = @"https://api.weixin.qq.com/cgi-bin/template/get_industry?access_token=" + accesstoken;

            HttpItem httpItem = new HttpItem();
            httpItem.URL = url;
            httpItem.Method = "GET";
            HttpHelper helper = new HttpHelper();
            HttpResult httpResult = helper.GetHtml(httpItem);
            var reshtml = httpResult.Html;
            LogHelper.WriteLog("GetIndustry:" + reshtml);

            return FastResponse(reshtml);
        }
        /// <summary>
        /// 获取模板消息列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public JsonResponse GetPrivateTemplate()
        {
            string accesstoken = GetAccesstoken();
            string url = @"https://api.weixin.qq.com/cgi-bin/template/get_all_private_template?access_token=" + accesstoken;

            HttpItem httpItem = new HttpItem();
            httpItem.URL = url;
            httpItem.Method = "GET";
            HttpHelper helper = new HttpHelper();
            HttpResult httpResult = helper.GetHtml(httpItem);
            var reshtml = httpResult.Html;
            LogHelper.WriteLog("GetPrivateTemplate:" + reshtml);

            return FastResponse(reshtml);

        }

        /// <summary>
        /// 发送模板消息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResponse SendTemplate()
        {
            string accesstoken = GetAccesstoken();
            string url = @"https://api.weixin.qq.com/cgi-bin/message/template/send?access_token=" + accesstoken;

            HttpItem httpItem = new HttpItem();
            httpItem.URL = url;
            httpItem.Method = "POST";
            httpItem.Postdata = JsonConvert.SerializeObject(new
            {
                touser = "oQQbc1XNMQ9cEd5ROVZjgM5WToAA",
                template_id = "pr3N9o1zbPh9V5uFkiPIeI4m0u2uG8sqS-YaaNlBlE0",
                url = "",
                miniprogram = new
                {
                    appid = "",
                    pagepath = ""
                },
                data = new
                {
                    first = new
                    {
                        value = "恭喜你购买成功！",
                        color = "#173177"
                    },
                    keyword1 = new
                    {
                        value = "Rodman",
                        color = "#173177"
                    },
                    keyword2 = new
                    {
                        value = "18888888888",
                        color = "#173177"
                    },
                    keyword3 = new
                    {
                        value = "102030405060708090",
                        color = "#173177"
                    },
                    keyword4 = new
                    {
                        value = "已发货",
                        color = "#173177"
                    },
                    keyword5 = new
                    {
                        value = "1000.0￥",
                        color = "#173177"
                    },
                    remark = new
                    {
                        value = "欢迎再次",
                        color = "#173177"
                    },
                }
            });
            HttpHelper helper = new HttpHelper();
            HttpResult httpResult = helper.GetHtml(httpItem);
            var reshtml = httpResult.Html;
            LogHelper.WriteLog("SendTemplate:" + reshtml);

            var model = JsonConvert.DeserializeObject<BaseReturnModel>(reshtml);
            if (model.errcode == 0)
            {
                return FastResponse("JUST OK");
            }
            else
            {
                return FastResponse(reshtml);
            }
        }

        #endregion

        #region 卡券相关



        #endregion

        #region basic方法

        private BaseModel BuildPost(object data, string url)
        {
            BaseModel model = null;
            try
            {
                //string accesstoken = GetAccesstoken();
                //string url = @"https://api.weixin.qq.com/customservice/kfaccount/add?access_token=" + accesstoken;

                HttpItem httpItem = new HttpItem();
                httpItem.URL = url;
                httpItem.Method = "POST";
                httpItem.Postdata = JsonConvert.SerializeObject(data);
                HttpHelper helper = new HttpHelper();
                HttpResult httpResult = helper.GetHtml(httpItem);
                string reshtml = httpResult.Html;
                LogHelper.WriteLog("reshtml:" + reshtml);

                model = JsonConvert.DeserializeObject<BaseModel>(reshtml);

            }
            catch (Exception ex)
            {
                LogHelper.WriteLog(ex, "BuildPost");

            }
            return model;
        }

        private string BuildGet(string url)
        {
            var reshtml = "";
            try
            {
                string accesstoken = GetAccesstoken();
                //string url = @"https://api.weixin.qq.com/cgi-bin/menu/get?access_token=" + accesstoken;
                HttpItem httpItem = new HttpItem();
                httpItem.URL = url;
                httpItem.Method = "GET";
                HttpHelper helper = new HttpHelper();
                HttpResult httpResult = helper.GetHtml(httpItem);
                reshtml = httpResult.Html;
                LogHelper.WriteLog("getmenu html:" + reshtml);
                //var model = JsonConvert.DeserializeObject<AccesstokenModel>(reshtml);
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog(ex, "BuildGet");
            }
            return reshtml;
        }

        #endregion


    }
}
