using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CJJ.WeChart.API.Models
{
    public class SendTemplateModel
    {
        public string touser { get; set; }
        public string template_id { get; set; }
        public string url { get; set; }
    }
}
