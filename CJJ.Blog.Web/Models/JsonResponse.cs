using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CJJ.Blog.Web
{
    public class JsonResponse
    {
        public int Code { get; set; }
        public string Msg { get; set; }
        public int Count { get; set; }
        public object Data { get; set; }
    }
}