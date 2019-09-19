using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace CJJ.WeChart.API.Models
{
    public class RefundNoticeInfo
    {
        /// <summary>
        /// 微信订单号
        /// </summary>
        /// <value>
        /// The transaction identifier.
        /// </value>
        [XmlElementAttribute("transaction_id", IsNullable = false)]
        public string TransactionId { get; set; }

        /// <summary>
        ///商户订单号
        /// </summary>
        /// <value>
        /// The out trade no.
        /// </value>
        [XmlElementAttribute("out_trade_no", IsNullable = false)]
        public string OutTradeNo { get; set; }

        /// <summary>
        /// 微信退款单号
        /// </summary>
        /// <value>
        /// The refund identifier.
        /// </value>
        [XmlElementAttribute("refund_id", IsNullable = false)]
        public string RefundId { get; set; }

        /// <summary>
        /// 商户退款单号
        /// </summary>
        /// <value>
        /// The out refund no.
        /// </value>
        [XmlElementAttribute("out_refund_no", IsNullable = false)]
        public string OutRefundNo { get; set; }

        /// <summary>
        /// 订单总金额，单位为分，只能为整数，详见支付金额
        /// </summary>
        /// <value>
        /// The total fee.
        /// </value>
        [XmlElementAttribute("total_fee", IsNullable = false)]
        public int TotalFee { get; set; }

        /// <summary>
        /// 应结订单金额,单位为分
        /// </summary>
        /// <value>
        /// The settlement total fee.
        /// </value>
        [XmlElementAttribute("settlement_total_fee", IsNullable = false)]
        public int SettlementTotalFee { get; set; }

        /// <summary>
        /// 退款总金额,单位为分
        /// </summary>
        /// <value>
        /// The refund fee.
        /// </value>
        [XmlElementAttribute("refund_fee", IsNullable = false)]
        public int RefundFee { get; set; }

        /// <summary>
        /// 退款金额=申请退款金额-非充值代金券退款金额，退款金额< 等于申请退款金额
        /// </summary>
        /// <value>
        /// The settlement refund fee.
        /// </value>
        [XmlElementAttribute("settlement_refund_fee", IsNullable = false)]
        public int SettlementRefundFee { get; set; }

        /// <summary>
        /// 退款状态 SUCCESS-退款成功 CHANGE-退款异常 REFUNDCLOSE—退款关闭
        /// </summary>
        /// <value>
        /// The refund status.
        /// </value>
        [XmlElementAttribute("refund_status", IsNullable = false)]
        public string RefundStatus { get; set; }

        /// <summary>
        /// 退款成功时间 2017-12-15 09:46:01
        /// </summary>
        /// <value>
        /// The success time.
        /// </value>
        [XmlElementAttribute("success_time", IsNullable = false)]
        public string SuccessTime { get; set; }

        /// <summary>
        ///退款入账账户
        ///1）退回银行卡：{银行名称}{卡类型}{卡尾号} 2）退回支付用户零钱: 支付用户零钱 3）退还商户: 商户基本账户 商户结算银行账户  4）退回支付用户零钱通:支付用户零钱通
        /// </summary>
        /// <value>
        /// The refund recv accout.
        /// </value>
        [XmlElementAttribute("refund_recv_accout", IsNullable = false)]
        public string RefundRecvAccout { get; set; }

        /// <summary>
        ///退款资金来源 
        /// REFUND_SOURCE_RECHARGE_FUNDS 可用余额退款/基本账户   
        /// REFUND_SOURCE_UNSETTLED_FUNDS 未结算资金退款
        /// </summary>  
        /// <value>
        /// The refund account.
        /// </value>
        [XmlElementAttribute("refund_account", IsNullable = false)]
        public string RefundAccount { get; set; }

        /// <summary>
        /// 退款发起来源
        /// API接口
        /// VENDOR_PLATFORM商户平台
        /// </summary>
        /// <value>
        /// The refund request source.
        /// </value>
        [XmlElementAttribute("refund_request_source", IsNullable = false)]
        public string RefundRequestSource { get; set; }

    }
}
