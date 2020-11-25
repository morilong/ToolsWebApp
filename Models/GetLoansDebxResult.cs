using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Haooyou.Tool.WebApp.Models
{
    public class GetLoansDebxResult
    {
        /// <summary>
        /// 月供
        /// </summary>
        public decimal MonthlyPayment { get; set; }
        /// <summary>
        /// 还款总额
        /// </summary>
        public decimal TotalRepayment { get; set; }
        /// <summary>
        /// 总利息
        /// </summary>
        public decimal TotalInterest { get; set; }
    }
}