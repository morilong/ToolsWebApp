using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Haooyou.Tool.WebApp.Models;

namespace Haooyou.Tool.WebApp.Controllers
{
    public class buyhouseController : Controller
    {
        //
        // GET: /buyhouse/

        public ActionResult Index()
        {
            return View();
        }

        /*public ActionResult sum2()
        {
            return View();
        }*/

        /*public ActionResult Sum(BuyHouseSumParam param)
        {
            if (param.Area < 1)
                return Json(new PostResultpInfo(PostResultStatus.建筑面积为必填项));
            if (param.Price < 1)
                return Json(new PostResultpInfo(PostResultStatus.毛坯价格为必填项));
            if (param.DownPayRatio < 1)
                return Json(new PostResultpInfo(PostResultStatus.毛坯首付款百分比为必填项));
            if (param.LoansYear < 1)
                return Json(new PostResultpInfo(PostResultStatus.毛坯按揭年数为必填项));
            if (param.RateRise < 1)
                return Json(new PostResultpInfo(PostResultStatus.毛坯利率上浮百分比为必填项));
            if (param.ZwjPrice < 1)
                return Json(new PostResultpInfo(PostResultStatus.专项维修基金单价为必填项));
            if (param.PriceZx > 0 && (param.DownPayRatioZx < 1 || param.LoansYearZx < 1 || param.RateRiseZx < 1))
                return Json(new PostResultpInfo(PostResultStatus.装修部分的其他参数不能为空));

            var data = new BuyHouseSumData { SumPrice = param.Price + param.PriceZx };

            var res = new BuyHouseSumResult(PostResultStatus.成功) { data = data };
            return Json(res);
        }*/

        public ActionResult GetSumHtml(BuyHouseSumParam param)
        {
            if (param.Area < 1)
                return Json(new PostResultpInfo(PostResultStatus.建筑面积为必填项));
            if (param.Price < 1)
                return Json(new PostResultpInfo(PostResultStatus.毛坯价格为必填项));
            if (param.DownPayRatio < 1)
                return Json(new PostResultpInfo(PostResultStatus.毛坯首付款百分比为必填项));
            if (param.LoansYear < 1)
                return Json(new PostResultpInfo(PostResultStatus.毛坯按揭年数为必填项));
            /*if (param.RateRise < 1)
                return Json(new PostResultpInfo(PostResultStatus.毛坯利率上浮百分比为必填项));*/
            /*if (param.ZwjPrice < 1)
                return Json(new PostResultpInfo(PostResultStatus.专项维修基金单价为必填项));*/
            if (param.PriceZx > 0 && (param.DownPayRatioZx < 1 || param.LoansYearZx < 1))
                return Json(new PostResultpInfo(PostResultStatus.装修部分的其他参数不能为空));

            var items = GetBuyHouseSumItems(param);
            var html = CreateSumHtml(param, items);
            var res = new BuyHouseSumResult(PostResultStatus.成功) { Html = html };
            return Json(res);
        }

        private string CreateSumHtml(BuyHouseSumParam param, List<BuyHouseSumItem> items)
        {
            var zwjAmount = Math.Round(param.ZwjPrice * param.Area, 2);
            var mp = items.Find(x => x.Type == BuyHouseSumType.毛坯);
            var zx = items.Find(x => x.Type == BuyHouseSumType.装修) ?? new BuyHouseSumItem();
            var tot = items.Find(x => x.Type == BuyHouseSumType.总价);
            var price = param.Price + param.PriceZx;
            var wsPrice = Math.Round(param.WaiShou / param.Area + price, 2);
            var sb = new StringBuilder();
            sb.Append("<table class=\"table table-bordered\">");
            sb.AppendFormat("<tr><th>楼盘名称</th><td colspan=\"3\">{0}</td></tr>", param.Name);
            sb.AppendFormat("<tr><th>建筑面积</th><td colspan=\"3\">{0} ㎡</td></tr>", param.Area);
            sb.AppendFormat("<tr><th></th><th>毛坯部分</th>{0}</tr>", param.PriceZx > 0 ? "<th>装修部分</th><th>合计</th>" : "");
            sb.AppendFormat(GetCommonTr("建筑单价", param.Price, param.PriceZx, price));
            sb.AppendFormat(GetCommonTr("总价", mp.TotalPrice, zx.TotalPrice, tot.TotalPrice));
            sb.AppendFormat(GetCommonTr("贷款", mp.LoansAmount, zx.LoansAmount, tot.LoansAmount));
            sb.AppendFormat(GetCommonTrRatio("首付款", mp.DownPayment, zx.DownPayment, tot.DownPayment, param.DownPayRatio, param.DownPayRatioZx));
            sb.AppendFormat(GetCommonTr("月供", mp.MonthlyPayment, zx.MonthlyPayment, tot.MonthlyPayment));
            sb.AppendFormat(GetCommonTr("利息", mp.TotalInterest, zx.TotalInterest, tot.TotalInterest));
            sb.AppendFormat(GetCommonTr("还款总额", mp.TotalRepayment, zx.TotalRepayment, tot.TotalRepayment));
            sb.AppendFormat(GetCommonTr("总价+利息", mp.TotalPriceAndInterest, zx.TotalPriceAndInterest, tot.TotalPriceAndInterest));
            if (zwjAmount > 0)
                sb.AppendFormat("<tr><th>公维金</th><td colspan=\"3\">{0}</td></tr>", zwjAmount.ToStringEx());
            if (param.OtherFee > 0)
                sb.AppendFormat("<tr><th>其他费用</th><td colspan=\"3\">{0}</td></tr>", param.OtherFee.ToStringEx());
            if (param.WaiShou > 0)
                sb.AppendFormat("<tr><th>外收</th><td>{0}</td><td colspan=\"2\">{1}{2}</td></tr>", param.WaiShou, GetSmallFont("加外收后房款单价："), wsPrice);
            sb.AppendFormat("<tr><th>月供</th><td>{0}</td><td colspan=\"2\">{1}{2}</td></tr>", tot.MonthlyPayment, GetSmallFont("工资流水需大于："), tot.MonthlyPayment * 2);
            sb.AppendFormat("<tr><th>首付预算</th><td colspan=\"3\">{0}{1}{2}{3} = {4}</td></tr>", tot.DownPayment.ToStringEx(), param.WaiShou > 0 ? " + " + param.WaiShou.ToStringEx() : "", zwjAmount > 0 ? " + " + zwjAmount.ToStringEx() : "", param.OtherFee > 0 ? " + " + param.OtherFee.ToStringEx() : "", (tot.DownPayment + param.WaiShou + zwjAmount + param.OtherFee).ToStringEx());
            sb.Append("</table>");
            return sb.ToString();
        }

        private string GetSmallFont(string str)
        {
            return string.Format("<span style=\"font-size: 11px;\">{0}</span>", str);
        }

        private string GetCommonTr(string title, decimal maopi, decimal zhuangxiu, decimal total)
        {
            var tdZx = zhuangxiu > 0 ? string.Format("<td>{0}</td><td>{1}</td>", zhuangxiu.ToStringEx(), total.ToStringEx()) : "";
            return string.Format("<tr><th>{0}</th><td>{1}</td>{2}</tr>", title, maopi.ToStringEx(), tdZx);
        }

        private string GetCommonTrRatio(string title, decimal maopi, decimal zhuangxiu, decimal total, int ratio, int ratioZx)
        {
            string tdZx = "";
            if (zhuangxiu > 0)
            {
                string zx="";
                if (ratioZx > 0)
                    //zx = string.Format("{0}<span style=\"font-size: 11px;\">{1}%</span>", zhuangxiu.ToStringEx(), ratioZx);
                    zx = zhuangxiu.ToStringEx();
                else zx = "";

                tdZx = string.Format("<td>{0}</td><td>{1}</td>", zx, total.ToStringEx());
            }

            return string.Format("<tr><th>{0}</th><td>{1}</td>{2}</tr>", title, maopi.ToStringEx(), tdZx);
        }

        private List<BuyHouseSumItem> GetBuyHouseSumItems(BuyHouseSumParam param)
        {
            var list = new List<BuyHouseSumItem>();
            var item = GetBuyHouseSumItem(BuyHouseSumType.毛坯, param, param.Price, param.DownPayRatio, param.LoansYear, param.RateRise);
            list.Add(item);
            var flags = BindingFlags.Instance | BindingFlags.Public;
            BuyHouseSumItem itemZx = null;
            List<PropertyInfo> pInfoZx = null;
            if (param.PriceZx > 0)
            {
                itemZx = GetBuyHouseSumItem(BuyHouseSumType.装修, param, param.PriceZx, param.DownPayRatioZx, param.LoansYearZx, param.RateRiseZx);
                list.Add(itemZx);
                pInfoZx = itemZx.GetType().GetProperties(flags).ToList();
            }
            var itemTotal = new BuyHouseSumItem() { Type = BuyHouseSumType.总价 };
            var totalPinfos = itemTotal.GetType().GetProperties(flags).ToList();
            foreach (var pInfo in item.GetType().GetProperties(flags))
            {
                if (pInfo.PropertyType == typeof(decimal))
                {
                    decimal zxAmount = itemZx == null ? 0 : (decimal)pInfoZx.Find(x => x.Name == pInfo.Name).GetValue(itemZx, null);
                    var total = (decimal)pInfo.GetValue(item, null) + zxAmount;
                    totalPinfos.Find(x => x.Name == pInfo.Name).SetValue(itemTotal, total, null);
                }
            }
            list.Add(itemTotal);
            return list;
        }

        private BuyHouseSumItem GetBuyHouseSumItem(BuyHouseSumType type, BuyHouseSumParam param, decimal price, int downPayRatio, int loansYear, int rateRise)
        {
            decimal totalPrice = param.Area * price;
            decimal loansAmount = SumLoansAmount(totalPrice, downPayRatio, param.LoansRound);
            var debx = GetLoansDebx(loansAmount, loansYear, rateRise);
            decimal totalPriceAndInterest = totalPrice + debx.TotalInterest;
            var item = new BuyHouseSumItem()
            {
                Type = type,
                TotalPrice = totalPrice,
                LoansAmount = loansAmount,
                DownPayment = totalPrice - loansAmount,
                MonthlyPayment = debx.MonthlyPayment,
                TotalInterest = debx.TotalInterest,
                TotalRepayment = debx.TotalRepayment,
                TotalPriceAndInterest = totalPriceAndInterest
            };
            return item;
        }

        private decimal SumLoansAmount(decimal totalPrice, int downPayRatio, int loansRound)
        {
            decimal loans = totalPrice * ((100 - downPayRatio) / 100m);//贷款金额
            return loans - (loans % loansRound);//取整
        }

        private GetLoansDebxResult GetLoansDebx(decimal loansAmount, int years, int rateRise)
        {
            double r = 4.9;
            double lilv = Math.Round(rateRise / 100d * r + r, 3);
            double newlilv = lilv / 100 / 12;//月利率
            int dig = 8;
            var monthlypayments = Math.Round((loansAmount * (decimal)newlilv * (decimal)Math.Pow(1 + newlilv, years * 12)) / (decimal)(Math.Pow(1 + newlilv, years * 12) - 1), dig);
            var total = Math.Round(monthlypayments * years * 12, dig);
            var interest = Math.Round(total - loansAmount, dig);
            var res = new GetLoansDebxResult()
            {
                MonthlyPayment = Math.Round(monthlypayments, 2),
                TotalRepayment = Math.Round(total, 2),
                TotalInterest = Math.Round(interest, 2)
            };
            return res;
        }

    }
}
