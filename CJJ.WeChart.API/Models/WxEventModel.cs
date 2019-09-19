using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace CJJ.WeChart.API.Models
{
    [XmlRoot("xml")]
    public class WxEventModel
    {
        [XmlElementAttribute("ToUserName", IsNullable = false)]
        public string ToUserName { get; set; }
        [XmlElementAttribute("FromUserName", IsNullable = false)]
        public string FromUserName { get; set; }
        [XmlElementAttribute("CreateTime", IsNullable = false)]
        public string CreateTime { get; set; }
        [XmlElementAttribute("MsgType", IsNullable = false)]
        public string MsgType { get; set; }
        /// <summary>
        /// 事件类型
        /// </summary>
        [XmlElementAttribute("Event", IsNullable = false)]
        public string Event { get; set; }
        /// <summary>
        /// 事件KEY值，是一个32位无符号整数，即创建二维码时的二维码scene_id
        /// </summary>
        [XmlElementAttribute("EventKey", IsNullable = false)]
        public string EventKey { get; set; }
        /// <summary>
        /// 二维码的ticket，可用来换取二维码图片
        /// </summary>
        [XmlElementAttribute("Ticket", IsNullable = false)]
        public string Ticket { get; set; }
        /// <summary>
        /// 纬度
        /// </summary>
        [XmlElementAttribute("Latitude", IsNullable = false)]
        public string Latitude { get; set; }
        /// <summary>
        /// 经度
        /// </summary>
        [XmlElementAttribute("Longitude", IsNullable = false)]
        public string Longitude { get; set; }
        /// <summary>
        /// 地理位置精度
        /// </summary>
        [XmlElementAttribute("Precision", IsNullable = false)]
        public string Precision { get; set; }
    }
}
