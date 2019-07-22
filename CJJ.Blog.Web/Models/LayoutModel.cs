
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CJJ.Blog.Web.Models
{
    /// <summary>
    /// 布局页面的 nav、博客推荐、今日热点数据
    /// </summary>
    public class LayoutModel
    {
        public List<CategoryView> Categorys { get; set; }
        public List<RecomBloginfoView> BloginfoViews { get; set; }
        public List<HotNewView> HotNews { get; set; }
    }
}