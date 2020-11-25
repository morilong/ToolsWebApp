using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Haooyou.Tool.WebApp.Models
{
    public class BuyHouseSumItem
    {
        public BuyHouseSumType Type { get; set; }
        /// <summary>
        /// 总价
        /// </summary>
        public decimal TotalPrice { get; set; }
        /// <summary>
        /// 贷款
        /// </summary>
        public decimal LoansAmount { get; set; }
        /// <summary>
        /// 首付款
        /// </summary>
        public decimal DownPayment { get; set; }
        /// <summary>
        /// 月供
        /// </summary>
        public decimal MonthlyPayment { get; set; }
        /// <summary>
        /// 总利息
        /// </summary>
        public decimal TotalInterest { get; set; }
        /// <summary>
        /// 还款总额
        /// </summary>
        public decimal TotalRepayment { get; set; }
        /// <summary>
        /// 总价+利息
        /// </summary>
        public decimal TotalPriceAndInterest { get; set; }
    }
}