using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CJJ.WeChart.API.Models
{
    public class MenuModel
    {
        public string type { get; set; }
        public string name { get; set; }
        public string key { get; set; }
        public string url { get; set; }
        public string appid { get; set; }
        public string pagepath { get; set; }
        public List<MenuModel> sub_button { get; set; }
    }
}
