using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace Haooyou.Tool.WebApp.Controllers
{
    public class strController : Controller
    {
        private void Merge()
        {
            /*var s1 = File.ReadAllLines("1.txt");
            var s2 = File.ReadAllLines("2.txt");
            if (s1.Length == s2.Length)
            {
                var sb = new StringBuilder();
                for (int i = 0; i < s1.Length; i++)
                {
                    sb.AppendFormat("{0}----{1}", s2[i], s1[i]);
                    sb.AppendLine();
                }
            }*/
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult down(string data, int num)
        {
            if (data.IsNullOrEmpty())
            {
                return Content("数据为空");
            }
            string fileName = string.Format("{0}.txt", num);
            return File(Encoding.UTF8.GetBytes(data), MediaTypeNames.Application.Octet, fileName);
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
                    return "原账号格式输入错误，获取分割符失败。";

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
