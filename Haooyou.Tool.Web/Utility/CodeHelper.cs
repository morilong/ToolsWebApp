using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Haooyou.Tool.Web.Models;

namespace Haooyou.Tool.Web.Utility
{
    public class CodeHelper
    {
        public static string GetContentTitle(EndeTypes type, string action, string strCharset)
        {
            if (type == EndeTypes.urlencode)
            {
                return action == "en" ? EndeConsts.UrlEn + strCharset : EndeConsts.UrlDe + strCharset;
            }
            else if (type == EndeTypes.base64)
            {
                return action == "en" ? EndeConsts.Base64En + strCharset : EndeConsts.Base64De + strCharset;
            }
            else if (type == EndeTypes.escape)
            {
                return action == "en" ? EndeConsts.EscapeEn : EndeConsts.EscapeDe;
            }
            else if (type == EndeTypes.gb2312ToUtf8)
            {
                return EndeConsts.GB2312ToUTF8;
            }
            else if (type == EndeTypes.utf8ToGb2312)
            {
                return EndeConsts.UTF8ToGB2312;
            }
            else if (type == EndeTypes.asciiToUnicode)
            {
                return EndeConsts.AsciiToUnicode;
            }
            else if (type == EndeTypes.unicodeToAscii)
            {
                return EndeConsts.UnicodeToAscii;
            }
            else if (type == EndeTypes.strToHex)
            {
                return EndeConsts.StrToHex + strCharset;
            }
            else if (type == EndeTypes.hexToStr)
            {
                return EndeConsts.HexToStr + strCharset;
            }
            else if (type == EndeTypes.strTobytes)
            {
                return EndeConsts.StrToBytes + strCharset;
            }
            else if (type == EndeTypes.bytesToStr)
            {
                return EndeConsts.BytesToStr + strCharset;
            }
            else if (type == EndeTypes.md5)
            {
                return EndeConsts.GetMd5 + strCharset;
            }
            else if (type == EndeTypes.sha1)
            {
                return EndeConsts.GetSHA1 + strCharset;
            }
            else if (type == EndeTypes.crc32)
            {
                return EndeConsts.GetCRC32 + strCharset;
            }
            else
            {
                return null;
            }
        }

        public static string GetDefaultCharset(EndeTypes type, string cookie)
        {
            if (!cookie.IsNullOrEmpty())
            {
                return cookie;
            }
            else if (type == EndeTypes.urlencode)
            {
                return EndeCharset.UTF8;
            }
            else if (type == EndeTypes.base64)
            {
                return EndeCharset.GB2312;
            }
            else if (type == EndeTypes.strToHex || type == EndeTypes.hexToStr)
            {
                return EndeCharset.GB2312;
            }
            else if (type == EndeTypes.strTobytes || type == EndeTypes.bytesToStr)
            {
                return EndeCharset.GB2312;
            }
            else if (type == EndeTypes.md5 || type == EndeTypes.sha1 || type == EndeTypes.crc32)
            {
                return EndeCharset.GB2312;
            }
            else
            {
                return EndeCharset.UTF8;
            }
        }

        /// <summary>
        /// 根据传入的 type 判断是否需要 charset，需要返回true。
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool NeedCharset(EndeTypes type)
        {
            bool need = type != EndeTypes.None && type != EndeTypes.escape &&
                type != EndeTypes.gb2312ToUtf8 && type != EndeTypes.utf8ToGb2312 &&
                type != EndeTypes.asciiToUnicode && type != EndeTypes.unicodeToAscii;
            return need;
        }

        public static string GetUrl(EndeGroups group, EndeTypes type, string action = "", string charset = "")
        {
            string charsetStr = NeedCharset(type) ? "&charset=" + charset : "";
            string actionStr = action.IsNullOrEmpty() ? "" : "&action=" + action;
            return string.Format("/code?group={0}&type={1}{2}{3}", group, type, actionStr, charsetStr);
        }
    }
}