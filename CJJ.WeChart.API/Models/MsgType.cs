using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CJJ.WeChart.API.Models
{
    public enum MsgType
    {
        //文本
        text = 0,
        image = 1,
        voice = 2,
        music = 3,
        //图文
        news = 4,
        //菜单消息，有选项的消息
        msgmenu = 5,
        //小程序
        miniprogrampage = 6
    }
}
