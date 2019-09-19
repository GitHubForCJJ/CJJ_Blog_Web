using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CJJ.WeChart.API.Models
{
    public class CsMsgModel
    {
        public string touser { get; set; }
        public MsgType msgtype { get; set; }
        public object text { get; set; }
        public object image { get; set; }
        public object voice { get; set; }
        /// <summary>
        /// 发送菜单消息
        /// </summary>
        public object msgmenu { get; set; }
    }
}
