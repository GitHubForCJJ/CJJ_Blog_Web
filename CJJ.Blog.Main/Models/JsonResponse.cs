using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CJJ.Blog.Main.Models
{
    public class JsonResponse
    {
        public int Code { get; set; }
        public string Msg { get; set; }
        public int Count { get; set; }
        public object Data { get; set; }
        public JsonResponse(object data, int code = 0,string msg="",int count=0)
        {
            Data = data;
            Code = code;
            Msg = msg;
            Count = count;
        }
        public JsonResponse()
        {

        }
    }
}