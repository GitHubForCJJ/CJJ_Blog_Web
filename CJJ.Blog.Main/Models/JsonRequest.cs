﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CJJ.Blog.Main.Models
{
    public class JsonRequest
    {
        public string Token { get; set; }
        public object Data { get; set; }
    }
}