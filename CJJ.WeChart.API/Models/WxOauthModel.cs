using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CJJ.WeChart.API.Models
{
    /// <summary>
    /// 网页授权model
    /// </summary>
    public class WxOauthModel
    {
        public int errcode { get; set; }
        public string errmsg { get; set; }
        /// <summary>
        /// 网页授权特有的accesstoken
        /// </summary>
        public string access_token { get; set; }
        public string expires_in { get; set; }
        public string refresh_token { get; set; }
        public string openid { get; set; }
        public string scope { get; set; }

    }
}
