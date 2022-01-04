using Microsoft.AspNetCore.Mvc;

namespace Haooyou.Tool.Web.Controllers
{
    public class OtherController : Controller
    {
        public ActionResult Index()
        {
            return Content("not found action");
        }

        public ActionResult GetJieRi()
        {
            return View();
        }

        public ActionResult GetJieRiApiEx(int year, int month, int day)
        {
            try
            {
                if (year <= 0 || year > DateTime.MaxValue.Year)
                    return Content("输入的年份有误");

                if (month <= 0 || month > 12)
                    return Content("输入的月份有误");

                var maxDay = new DateTime(year, month, 1).AddMonths(1).AddDays(-1).Day; //当前月的最后一天
                if (day <= 0 || day > maxDay)
                    return Content("输入的日数有误");

                var dt = new DateTime(2018, 1, 25);//天河街;

                if (new DateTime(year, month, day) < dt)
                    return Content("输入的年月日不能小于2018-01-25");

                while (true)
                {
                    if (dt.Year == year && dt.Month == month)
                    {
                        while (dt.Month == month)
                        {
                            if (dt.Day == day)
                            {
                                return Content("天河");
                            }
                            if (dt.Day - 1 == day || dt.Day + 2 == day)
                            {
                                return Content("怀群");
                            }
                            if (dt.Day - 2 == day || dt.Day + 1 == day)
                            {
                                return Content("华张");
                            }
                            dt = dt.AddDays(3);
                        }
                        return Content("计算出错！");
                    }
                    dt = dt.AddDays(3);
                }
            }
            catch (Exception ex)
            {
                return Content("计算出错：" + ex.Message);
            }
        }

        public ActionResult GetJieRiApi(string date)
        {
            if (date.IsNullOrEmpty())
                return Content("请输入要查询的日期");

            DateTime d;
            if (!DateTime.TryParse(date, out d))
                return Content("输入的日期格式有误");

            return GetJieRiApiEx(d.Year, d.Month, d.Day);
        }

    }
}
