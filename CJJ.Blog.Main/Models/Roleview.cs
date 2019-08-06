using CJJ.Blog.Service.Models.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CJJ.Blog.Main.Models
{
    public class Roleview:Employee
    {
        /// <summary>
        /// 是否选中
        /// </summary>
        public bool Checked { get; set; }
    }
}