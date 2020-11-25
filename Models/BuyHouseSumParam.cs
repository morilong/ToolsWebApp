using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Haooyou.Tool.WebApp.Models
{
    public class BuyHouseSumParam
    {
        /// <summary>
        /// 楼盘名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 建筑面积
        /// </summary>
        public decimal Area { get; set; }
        /// <summary>
        /// 建筑单价（毛坯）
        /// </summary>
        public decimal Price { get; set; }
        /// <summary>
        /// 首付款百分比（毛坯）
        /// </summary>
        public int DownPayRatio { get; set; }
        /// <summary>
        /// 按揭年数（毛坯）
        /// </summary>
        public int LoansYear { get; set; }
        /// <summary>
        /// 贷款利率上浮百分比（毛坯）
        /// </summary>
        public int RateRise { get; set; }
        /// <summary>
        /// 建筑单价（装修）
        /// </summary>
        public decimal PriceZx { get; set; }
        /// <summary>
        /// 首付款百分比（装修）
        /// </summary>
        public int DownPayRatioZx { get; set; }
        /// <summary>
        /// 按揭年数（装修）
        /// </summary>
        public int LoansYearZx { get; set; }
        /// <summary>
        /// 贷款利率上浮百分比（装修）
        /// </summary>
        public int RateRiseZx { get; set; }
        /// <summary>
        /// 贷款金额取整
        /// </summary>
        public int LoansRound { get; set; }
        /// <summary>
        /// 专项维修基金 ?元/㎡
        /// </summary>
        public decimal ZwjPrice { get; set; }
        /// <summary>
        /// 外收金额
        /// </summary>
        public decimal WaiShou { get; set; }
        /// <summary>
        /// 其他费用
        /// </summary>
        public decimal OtherFee { get; set; }
    }
}