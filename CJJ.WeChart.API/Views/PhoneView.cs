using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CJJ.WeChart.API.Views
{
    public class PhoneView
    {
        public string SessionId { get; set; }
        public string EncryptedData { get; set; }
        public string Iv { get; set; }
        public string OpenId { get; set; }
    }
}