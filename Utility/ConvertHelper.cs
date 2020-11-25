using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace System
{
    public static class ConvertHelper
    {
        /// <summary>
        /// 字符串转十六进制。
        /// </summary>
        /// <param name="str">待转换的字符串</param>
        /// <param name="encoding">str参数的字符编码，默认：Encoding.Default</param>
        /// <returns></returns>
        public static string StrToHex(string str, Encoding encoding)
        {
            if (str == null)
                throw new ArgumentNullException("str");
            if (str.Length == 0)
                return string.Empty;

            if (encoding == null)
                encoding = Encoding.Default;

            return BitConverter.ToString(encoding.GetBytes(str)).Replace("-", "");
        }
        /// <summary>
        /// 十六进制转字符串。
        /// </summary>
        /// <param name="hex">十六进制字符串</param>
        /// <param name="encoding">返回字符编码，默认：Encoding.Default</param>
        /// <returns></returns>
        public static string HexToStr(string hex, Encoding encoding)
        {
            if (hex == null)
                throw new ArgumentNullException("hex");
            if (hex.Length == 0)
                return string.Empty;

            if (encoding == null)
                encoding = Encoding.Default;

            if ((hex.Length % 2) == 1)
                hex += '0';
            byte[] bytes = new byte[hex.Length / 2];
            for (int i = 0; i < hex.Length; i += 2)
            {
                bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
            }
            return encoding.GetString(bytes);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="str"></param>
        /// <param name="encoding">str参数的字符编码</param>
        /// <returns></returns>
        public static string StrToBytes(string str, Encoding encoding)
        {
            if (str == null)
                throw new ArgumentNullException("str");
            if (str.Length == 0)
                return string.Empty;

            var bArr = encoding.GetBytes(str);
            var sb = new StringBuilder();
            foreach (var item in bArr)
            {
                sb.AppendFormat("{0},", item);
            }
            sb.Remove(sb.Length - 1, 1);
            return sb.ToString();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="str"></param>
        /// <param name="encoding">返回字符的编码</param>
        /// <returns></returns>
        public static string BytesToStr(string str, Encoding encoding)
        {
            if (str == null)
                throw new ArgumentNullException("str");
            if (str.Length == 0)
                return string.Empty;

            var mc = Regex.Matches(str, @"(\d{1,3})[,]?");
            if (mc.Count == 0)
                throw new ArgumentException("您输入的不是有效的字节序列");

            var bArr = (from Match m in mc select byte.Parse(m.Groups[1].Value)).ToArray();
            return encoding.GetString(bArr);
        }
    }
}