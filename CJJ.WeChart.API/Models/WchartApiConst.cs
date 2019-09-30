using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CJJ.WeChart.API.Models
{
    public class WchartApiConst
    {
        public static readonly string appid = "wx8e37ff182337cd1f";
        public static readonly string appsecret = "da5f6a1ff03b308d16dd650c7ea4e6be";
        public static readonly string accesstokenkey = "accesstoken";
        public static readonly string jsapiaccesstokenkey = "jsapiaccesstoken";

        #region API接口地址
        public static readonly string access_token = @"https://api.weixin.qq.com/cgi-bin/token";

        #endregion
    }
}
