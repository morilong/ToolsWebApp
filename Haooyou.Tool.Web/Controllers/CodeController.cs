using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using Haooyou.Tool.Web.Models;

namespace Haooyou.Tool.Web.Controllers
{
    public class CodeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

#if DEBUG
        public ActionResult test()
        {
            return View();
        }
#endif

        [HttpPost]
        public ActionResult UrlEncode(string str, bool isDecode)
        {
            if (str.IsNullOrEmpty())
                return Content("");

            var res = isDecode ? HttpUtility.UrlDecode(str, Encoding.Default) : HttpUtilityEx.UrlEncode(str, Encoding.Default);
            return Content(res);
        }

        [HttpPost]
        public ActionResult Base64(string str, string charset, bool isDecode)
        {
            if (str.IsNullOrEmpty())
                return Content("");

            if (isDecode && !Regex.IsMatch(str, @"^[A-Za-z0-9+/=]+$"))
                return Content("输入的不是有效的 Base64 字符串");
            try
            {
                charset = charset.IsNullOrEmpty() ? EndeCharset.UTF8 : charset;
                string res = "";
                if (charset == EndeCharset.UTF8)
                {
                    res = isDecode ? Encoding.UTF8.GetString(Convert.FromBase64String(str)) : Convert.ToBase64String(Encoding.UTF8.GetBytes(str));
                }
                else if (charset == EndeCharset.GB2312)
                {
                    res = isDecode ? Encoding.Default.GetString(Convert.FromBase64String(str)) : Convert.ToBase64String(Encoding.Default.GetBytes(str));
                }
                return Content(res);
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }

        [HttpPost]
        public ActionResult Gb2312ToUtf8(string str)
        {
            if (str.IsNullOrEmpty())
                return Content("");

            var bytes = Encoding.UTF8.GetBytes(str);
            return Content(Encoding.Default.GetString(bytes));
        }
        [HttpPost]
        public ActionResult Utf8ToGb2312(string str)
        {
            if (str.IsNullOrEmpty())
                return Content("");

            var bytes = Encoding.Default.GetBytes(str);
            return Content(Encoding.UTF8.GetString(bytes));
        }

        private string StrConvert(string str, string charset, StrConvertType convertType)
        {
            if (str.IsNullOrEmpty())
                return "";

            Encoding encoding;
            if (charset.IsNullOrEmpty() || charset == EndeCharset.GB2312)
                encoding = Encoding.Default;
            else if (charset == EndeCharset.UTF8)
                encoding = Encoding.UTF8;
            else if (charset == EndeCharset.Unicode)
                encoding = Encoding.Unicode;
            else
                encoding = Encoding.Default;

            try
            {
                switch (convertType)
                {
                    case StrConvertType.StrToHex:
                        return ConvertHelper.StrToHex(str, encoding);
                    case StrConvertType.HexToStr:
                        return ConvertHelper.HexToStr(str, encoding);
                    case StrConvertType.StrToBytes:
                        return ConvertHelper.StrToBytes(str, encoding);
                    case StrConvertType.BytesToStr:
                        return ConvertHelper.BytesToStr(str, encoding);
                    default:
                        return string.Empty;
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        [HttpPost]
        public ActionResult StrToHex(string str, string charset)
        {
            return Content(StrConvert(str, charset, StrConvertType.StrToHex));
        }
        [HttpPost]
        public ActionResult HexToStr(string str, string charset)
        {
            if (str.IsNullOrEmpty())
                return Content("");

            if (!Regex.IsMatch(str, "^[0-9a-fA-F]+$"))
                return Content("输入的不是有效的十六进制字符串");

            return Content(StrConvert(str, charset, StrConvertType.HexToStr));
        }
        [HttpPost]

        public ActionResult StrToBytes(string str, string charset)
        {
            return Content(StrConvert(str, charset, StrConvertType.StrToBytes));
        }
        [HttpPost]
        public ActionResult BytesToStr(string str, string charset)
        {
            if (str.IsNullOrEmpty())
                return Content("");

            if (!Regex.IsMatch(str, @"(\d{1,3})[,]?"))
                return Content("输入的不是有效的字节序列");

            return Content(StrConvert(str, charset, StrConvertType.BytesToStr));
        }


        [HttpPost]
        public ActionResult MD5(string str)
        {
            if (str.IsNullOrEmpty())
                return Content("");

            return Content(EncryptHelper.MD5(str));
        }
        [HttpPost]
        public ActionResult SHA1(string str)
        {
            if (str.IsNullOrEmpty())
                return Content("");

            return Content(EncryptHelper.SHA1(str));
        }
        [HttpPost]
        public ActionResult CRC32(string str)
        {
            if (str.IsNullOrEmpty())
                return Content("");

            return Content(EncryptHelper.CRC32(str));
        }
    }

    enum StrConvertType
    {
        StrToHex,
        HexToStr,
        StrToBytes,
        BytesToStr
    }
}
