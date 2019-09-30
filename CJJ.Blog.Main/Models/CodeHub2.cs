using FastDev.Configer;
using FastDev.Http;
using FastDev.Log;
using Microsoft.AspNet.SignalR;
using Newtonsoft.Json;
using System;

namespace CJJ.Blog.Main.Models
{
    /// <summary>
    /// 二维码登录集线器
    /// </summary>
    public class CodeHub2 : Hub
    {
        public void SendLoginRes(string key)
        {
            #region 查询api接口，登录结果
            LogHelper.WriteLog("in msg");
            try
            {
                string url = ConfigHelper.GetConfigToString("wxapiurl");
                url = url + "/api/wchart/CheckState";
                HttpItem httpItem = new HttpItem();
                httpItem.URL = url;
                httpItem.Method = "POST";
                httpItem.Postdata = JsonConvert.SerializeObject(key);
                HttpHelper helper = new HttpHelper();
                HttpResult httpResult = helper.GetHtml(httpItem);
                string reshtml = httpResult.Html;
                LogHelper.WriteLog("reshtml:" + reshtml);

                //model = JsonConvert.DeserializeObject<CodeLoginModel>(reshtml);
                Clients.All.receiveMessage(reshtml);
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog(ex, "SendLoginRes");

            }

            #endregion
        }

        public void getCodeState(string key)
        {
            LogHelper.WriteLog(key);
            SendLoginRes(key);
        }
    }
}