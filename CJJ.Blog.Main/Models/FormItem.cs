using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CJJ.Blog.Main.Models
{
    public class FormItem
    {
        public int Page { get; set; }
        public int Limit { get; set; }
        public Dictionary<string, object> DicWhere { get; set; }
    }
}