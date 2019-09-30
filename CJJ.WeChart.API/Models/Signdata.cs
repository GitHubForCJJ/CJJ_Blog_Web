using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CJJ.WeChart.API.Models
{
    public class Signdata
    {
        public string appId { get; set; }
        public string noncestr { get; set; }
        public string timespan { get; set; }
        public string url { get; set; }
        public string signature { get; set; }
    }
}