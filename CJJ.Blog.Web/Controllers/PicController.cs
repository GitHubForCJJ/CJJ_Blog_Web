
using Qiniu.Http;
using Qiniu.Storage;
using Qiniu.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CJJ.Blog.Web.Controllers
{
    public class PicController : Controller
    {

        [HttpPost]
        public string GetQiniuCode()
        {
            string token = "";
            try
            {

                token = HttpRuntime.Cache["QiniuToken"]?.ToString() ?? "";

                if (string.IsNullOrEmpty(token))
                {

                    //key和密钥 以及空间名来自于七牛云控制台，配置在config文件中
                    Mac mac = new Mac("TYb8ZurNoN-xrUOhnNom_q3ZdPl1OLJqUsvoP0xB", "d7ajdjoitbUiJZQY7ANw5bSf3p6K3nQA8KkGxIDq");
                    PutPolicy putPolicy = new PutPolicy();
                    // 设置要上传的目标空间
                    putPolicy.Scope = "cjjpic";
                    // 上传策略的过期时间(单位:秒)
                    putPolicy.SetExpires(2 * 60 * 60);
                    // 生成上传token
                    token = Auth.CreateUploadToken(mac, putPolicy.ToJsonString());
                    HttpRuntime.Cache["QiniuToken"] = token;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return token;
        }
        [HttpPost]
        public string UploadImage(string filePath)
        {
            Mac mac = new Mac("TYb8ZurNoN-xrUOhnNom_q3ZdPl1OLJqUsvoP0xB", "d7ajdjoitbUiJZQY7ANw5bSf3p6K3nQA8KkGxIDq");
            // 上传文件名
            string key = "putty2.gif";
            // 本地文件路径
            //string filePath = @"E:\MY\FastDevStu\Mvc\Models\微信图片_20190508193805.gif";
            // 空间名
            string Bucket = "cjj81pic";
            // 设置上传策略，详见：https://developer.qiniu.com/kodo/manual/1206/put-policy
            PutPolicy putPolicy = new PutPolicy();
            putPolicy.Scope = Bucket + ":" + key;
            putPolicy.SetExpires(3600);
            string token = Auth.CreateUploadToken(mac, putPolicy.ToJsonString());
            Qiniu.Storage.Config config = new Qiniu.Storage.Config();
            // 设置上传区域
            config.Zone = Zone.ZONE_CN_South;
            // 设置 http 或者 https 上传
            config.UseHttps = true;
            config.UseCdnDomains = true;
            config.ChunkSize = ChunkUnit.U512K;
            ResumableUploader target = new ResumableUploader(config);
            PutExtra extra = new PutExtra();
            //设置断点续传进度记录文件
            extra.ResumeRecordFile = ResumeHelper.GetDefaultRecordKey(filePath, key);
            Console.WriteLine("record file:" + extra.ResumeRecordFile);
            extra.ResumeRecordFile = "test.progress";
            HttpResult result = target.UploadFile(filePath, key, token, extra);
            Console.WriteLine("resume upload: " + result.ToString());
            return result.ToString();
        }
        [HttpPost]
        public string Download(string key)
        {
            Mac mac = new Mac("TYb8ZurNoN-xrUOhnNom_q3ZdPl1OLJqUsvoP0xB", "d7ajdjoitbUiJZQY7ANw5bSf3p6K3nQA8KkGxIDq");
            string domain = "http://ptdzsd6xm.bkt.clouddn.com/";
            //string key = @"ba027e8e-4d68-4671-81fc-20bf9dcd3db7\1560388312.jpg";
            string privateUrl = DownloadManager.CreatePrivateUrl(mac, domain, key, 3600);
            Console.WriteLine(privateUrl);
            return privateUrl;
        }
    }
}