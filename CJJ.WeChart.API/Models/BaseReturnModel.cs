using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CJJ.WeChart.API.Models
{
    public class BaseReturnModel
    {
        public int errcode { get; set; }
        public string errmsg { get; set; }
        public string msgid { get; set; }
    }
}
