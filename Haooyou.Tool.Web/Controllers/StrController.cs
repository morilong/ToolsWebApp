using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.RegularExpressions;

namespace Haooyou.Tool.Web.Controllers
{
    public class StrController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult zh()
        {
            return View();
        }

        public ActionResult hb()
        {
            return View();
        }

        public ActionResult qcf()
        {
            return View();
        }

        public ActionResult daluan()
        {
            return View();
        }

    }

    class StringFormatConvert
    {
        /// <summary>
        /// 传入格式字符串，返回全部分割符。
        /// </summary>
        /// <param name="format"></param>
        /// <returns></returns>
        private static string[] GetAllFgf(string format)
        {
            var mc = Regex.Matches(format, @"}(.*?){");
            var list = new List<string>();
            foreach (Match item in mc)
            {
                list.Add(item.Groups[1].Value);
            }
            return list.ToArray();
        }

        /// <summary>
        /// 传入格式字符串，返回全部索引。
        /// </summary>
        /// <param name="format"></param>
        /// <returns></returns>
        private static int[] GetAllIndex(string format)
        {
            var mc = Regex.Matches(format, @"{(\d+)}");
            var list = new List<int>();
            foreach (Match item in mc)
            {
                list.Add(item.Groups[1].Value.ToInt32());
            }
            return list.ToArray();
        }

        public static string Convert(string data, string format1, string format2)
        {
            try
            {
                var fgf1 = GetAllFgf(format1);
                if (fgf1.Length == 0)
                    return "原文本格式输入错误，获取分割符失败。";

                var fgf2 = GetAllFgf(format2);
                if (fgf2.Length == 0)
                    return "转换后格式输入错误，获取分割符失败。";

                var index2 = GetAllIndex(format2);
                if (index2.Length == 0)
                    return "转换后格式输入错误，获取索引失败。";

                var lines = data.Split(new[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
                var sb = new StringBuilder();
                foreach (var line in lines)
                {
                    var columns = line.Split(fgf1, StringSplitOptions.None);
                    for (int j = 0; j < index2.Length; j++)
                    {
                        sb.Append(columns[index2[j] - 1]);
                        if (j != index2.Length - 1)
                        {
                            sb.Append(fgf2[j]);
                        }
                    }
                    sb.AppendLine();
                }
                return sb.ToString();
            }
            catch (Exception ex)
            {
                return "转换出错！错误信息：" + ex.Message;
            }
        }
    }

}
