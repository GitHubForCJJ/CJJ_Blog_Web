﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CJJ.WeChart.API.Models
{
    public class JSAcctokenModel
    {
        public int errcode { get; set; }
        public string errmsg { get; set; }
        public string ticket { get; set; }
        public string expires_in { get; set; }
    }
}