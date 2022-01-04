// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StringExtension.cs" company="Wedn.Net">
//   Copyright © 2014 Wedn.Net. All Rights Reserved.
// </copyright>
// <summary>
//   字符串拓展方法
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace System
{
    /// <summary>
    ///  字符串拓展方法
    /// </summary>
    public static class StringExtension
    {
        #region 判断输入的字符串是否全是英文（不区分大小写） + bool IsEnglish(string str)

        /// <summary>
        /// 判断输入的字符串是否全是英文（不区分大小写）
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsEnglish(string str)
        {
            return str != null && Regex.IsMatch(str, @"^[a-zA-Z]+$");
        }

        #endregion

        #region 判断所输入的字符串是否为中文 + bool IsChinese(string str)

        /// <summary>
        /// 判断所输入的字符串是否为中文
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsChinese(string str)
        {
            return str != null && Regex.IsMatch(str, @"^[\u4e00-\u9fa5]+$");
        }

        #endregion

        #region 判断指定字符串是否为合法IP地址 +static bool IsIpAddress(this string input)

        /// <summary>
        /// 判断指定字符串是否为合法IP地址
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="str">指定字符串</param>
        /// <returns>真或假</returns>
        public static bool IsIpAddress(this string str)
        {
            return str != null && Regex.IsMatch(str, @"^((2[0-4]\d|25[0-5]|[01]?\d\d?)\.){3}(2[0-4]\d|25[0-5]|[01]?\d\d?)$");
        }

        #endregion

        #region 判断指定字符串是否合法的日期格式 +static bool IsDateTime(this string input)

        /// <summary>
        /// 判断指定字符串是否合法的日期格式
        /// </summary>
        /// <remarks>
        /// 2013-11-18 18:53 Created By iceStone
        /// </remarks>
        /// <param name="input">指定字符串</param>
        /// <returns>真或假</returns>
        public static bool IsDateTime(this string input)
        {
            DateTime dt;
            return DateTime.TryParse(input, out dt);
        }

        #endregion

        #region 检查一个字符串是否为小数（包含负数，大小不限） + bool IsDouble(this string str)
        /// <summary>
        /// 检查一个字符串是否为小数（包含负数，大小不限）
        /// </summary>
        /// <param name="str">需验证的字符串。。</param>
        /// <returns>是否合法的bool值。</returns>
        public static bool IsDouble(this string str)
        {
            return str != null && Regex.IsMatch(str, "^[-+]?([0-9]{1,}[.][0-9]*)$");
        }
        #endregion

        #region 检查一个字符串是否为整数（包含负数，大小不限）+ bool IsInt(this string str)
        /// <summary>
        /// 检查一个字符串是否为整数（包含负数，大小不限）
        /// </summary>
        /// <param name="str">需验证的字符串。</param>
        /// <returns>是否合法的bool值。</returns>
        public static bool IsInt(this string str)
        {
            return str != null && Regex.IsMatch(str, @"^[-+]?[0-9]+$");
        }
        #endregion

        #region 检查一个字符串是否为整数或小数（包含大小不限的正负数、正负小数） + bool IsIntOrDouble(this string str)
        /// <summary>
        /// 检查一个字符串是否为整数或小数（包含大小不限的正负数、正负小数）
        /// </summary>
        /// <param name="str">需验证的字符串。</param>
        /// <returns>是否合法的bool值。</returns>
        public static bool IsIntOrDouble(this string str)
        {
            return str != null && Regex.IsMatch(str, @"^[-+]?[0-9]+(\.[0-9]+)?$");
        }
        #endregion

        #region 判断指定的字符串是否为Url地址 +static bool IsUrl(this string str)

        public const string UrlPattern = "(http[s]{0,1}|ftp)://[a-zA-Z0-9\\.\\-]+\\.([a-zA-Z]{2,4})(:\\d+)?(/[a-zA-Z0-9\\.\\-~!@#$%^&*+?:_/=<>]*)?";

        /// <summary>
        /// 判断指定的字符串是否为Url地址
        /// </summary>
        /// <remarks>
        /// 2013-11-18 18:53 Created By iceStone
        /// </remarks>
        /// <param name="str">要确认的字符串</param>
        /// <returns>真或假</returns>
        public static bool IsUrl(this string str)
        {
            return str != null && Regex.IsMatch(str, UrlPattern);
        }

        #endregion

        #region 判断指定的字符串是否为合法Email +static bool IsEmail(this string str)

        /// <summary>
        /// 判断指定的字符串是否为合法Email
        /// </summary>
        /// <remarks>
        /// 2013-11-18 18:53 Created By iceStone
        /// </remarks>
        /// <param name="str">指定的字符串</param>
        /// <returns>真或假</returns>
        public static bool IsEmail(this string str)
        {
            //@"\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"
            return str != null && Regex.IsMatch(str, @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$");
        }

        #endregion

        #region bool IsUsernameLength/IsPasswordLength(this string userName, int minLength, int maxLength)

        public static bool IsUsernameLength(this string userName, int minLength = 3, int maxLength = 25)
        {
            return userName != null && userName.Length >= minLength && userName.Length <= maxLength;
        }

        public static bool IsPasswordLength(this string password, int minLength = 6, int maxLength = 20)
        {
            return password != null && password.Length >= minLength && password.Length <= maxLength;
        }

        #endregion

        #region bool IsMobile(this string mobile)

        public static bool IsMobile(this string mobile)
        {
            return mobile != null && Regex.IsMatch(mobile, @"^[1]\d{10}$");
        }

        #endregion

        #region 判断字符串是否为Null或者为空 +static bool IsNullOrEmpty(this string input)

        /// <summary>
        /// 判断字符串是否为Null或者为空
        /// </summary>
        /// <param name="input">要判断的字符串</param>
        /// <returns>是否为Null或者为空</returns>
        public static bool IsNullOrEmpty(this string input)
        {
            return string.IsNullOrEmpty(input);
        }

        #endregion

        #region 对 URL 字符串进行编码, 返回 URL 字符串的编码结果 +static string UrlEncode(this string str, Encoding e = null)

        /// <summary>
        /// 对 URL 字符串进行编码, 返回 URL 字符串的编码结果
        /// </summary>
        /// <remarks>
        /// 2013-11-18 18:53 Created By iceStone
        /// </remarks>
        /// <param name="str">要编码的文本</param>
        /// <param name="e">编码，默认：Encoding.UTF8</param>
        /// <returns>一个已编码的字符串</returns>
        public static string UrlEncode(this string str, Encoding e = null)
        {
            e = e ?? Encoding.UTF8;
            return HttpUtility.UrlEncode(str, e);
        }

        #endregion

        #region 对 URL 字符串进行解码, 返回 URL 字符串的解码结果 +static string UrlDecode(this string str, Encoding e = null)

        /// <summary>
        /// 对 URL 字符串进行解码, 返回 URL 字符串的解码结果
        /// </summary>
        /// <remarks>
        /// 2013-11-18 18:53 Created By iceStone
        /// </remarks>
        /// <param name="str">要解码的文本</param>
        /// <param name="e">编码，默认：Encoding.UTF8</param>
        /// <returns>解码结果</returns>
        public static string UrlDecode(this string str, Encoding e = null)
        {
            e = e ?? Encoding.UTF8;
            return HttpUtility.UrlDecode(str, e);
        }

        #endregion

        #region 返回字符串真实长度, 1个汉字长度为2

        /// <summary>
        /// 返回字符串真实长度，1个汉字长度为2
        /// </summary>
        /// <remarks>
        /// 2013-11-18 18:53 Created By iceStone
        /// </remarks>
        /// <param name="str">The string.</param>
        /// <returns>System.Int32.</returns>
        public static int Length(this string str)
        {
            return string.IsNullOrEmpty(str) ? 0 : Encoding.Default.GetBytes(str).Length;
        }

        #endregion

        #region 根据左右字符串截取中间字符串 + string GetBetween(*)
        /// <summary>
        /// 根据左右字符串截取中间字符串，截取失败返回空字符串。
        /// </summary>
        /// <param name="str"></param>
        /// <param name="strLeft"></param>
        /// <param name="strRight"></param>
        /// <param name="startIndex"></param>
        /// <param name="comparisonType"></param>
        /// <returns>截取失败返回空字符串</returns>
        public static string GetBetween(this string str, string strLeft, string strRight, int startIndex = 0, StringComparison comparisonType = StringComparison.CurrentCulture)
        {
            if (str == null || strLeft == null || strRight == null)
                throw new ArgumentNullException();

            int left = str.IndexOf(strLeft, startIndex, comparisonType);
            if (left == -1)
                return string.Empty;

            left = left + strLeft.Length;

            int rigth = str.IndexOf(strRight, left, comparisonType);
            if (rigth == -1)
                return string.Empty;

            return str.Substring(left, rigth - left);
        }
        #endregion

        #region 根据左右字符串截取中间字符串（从最后一个字符开始往左边查找）+ string GetStrBetweenRev(*)
        /// <summary>
        /// 根据左右字符串截取中间字符串（从最后一个字符开始往左边查找）
        /// </summary>
        /// <param name="str">待截取的字符串</param>
        /// <param name="strLeft">左边字符串</param>
        /// <param name="strRight">右边字符串</param>
        /// <param name="startIndex">起始搜索位置</param>
        /// <param name="stringComparison">System.StringComparison 值之一。</param>
        /// <returns></returns>
        public static string GetBetweenRev(this string str, string strLeft, string strRight, int? startIndex = null, StringComparison stringComparison = StringComparison.CurrentCulture)
        {
            if (str == null || strLeft == null || strRight == null)
                throw new ArgumentNullException();

            int left = startIndex == null
                ? str.LastIndexOf(strLeft, stringComparison)
                : str.LastIndexOf(strLeft, startIndex.Value, stringComparison);

            if (left == -1)
                return string.Empty;

            left = left + strLeft.Length;

            int rigth = str.IndexOf(strRight, left, stringComparison);
            if (rigth == -1)
                return string.Empty;

            return str.Substring(left, rigth - left);
        }
        #endregion

        #region 取字符串左边 + static string GetLeft(*)
        /// <summary>
        /// 取字符串左边。（从字符串的左边按 IndexOf 开始寻找）
        /// </summary>
        /// <param name="str">待截取的字符串</param>
        /// <param name="value">要查找的字符串。</param>
        /// <param name="startIndex">起始搜索位置</param>
        /// <param name="stringComparison">System.StringComparison 值之一。</param>
        /// <returns></returns>
        public static string GetLeft(this string str, string value, int? startIndex = null, StringComparison? stringComparison = null)
        {
            if (value == null)
                throw new ArgumentNullException("value");

            int pos;
            if (startIndex != null && stringComparison != null)
                pos = str.IndexOf(value, startIndex.Value, stringComparison.Value);
            else if (startIndex != null)
                pos = str.IndexOf(value, startIndex.Value);
            else if (stringComparison != null)
                pos = str.IndexOf(value, stringComparison.Value);
            else
                pos = str.IndexOf(value);

            if (pos == -1)
                return string.Empty;

            return str.Substring(0, pos);
        }

        /// <summary>
        /// 返回字符串左边的指定长度（小于length返回str.Length），
        /// </summary>
        /// <param name="str"></param>
        /// <param name="length"></param>
        /// <param name="isRemoveNewlineTrim"></param>
        /// <returns></returns>
        public static string GetLeft(this string str, int length, bool isRemoveNewlineTrim = true)
        {
            if (str.IsNullOrEmpty())
                return string.Empty;

            if (str.Length < length)
                length = str.Length;

            str = str.Substring(0, length);
            return isRemoveNewlineTrim ? RemoveNewlineTrim(str) : str;
        }

        #endregion

        #region 取字符串右边 + string GetRight(*)
        /// <summary>
        /// 取字符串右边。（从字符串的右边按 LastIndexOf 开始寻找）
        /// </summary>
        /// <param name="str">待截取的字符串</param>
        /// <param name="value">要查找的字符串。</param>
        /// <param name="startIndex">起始搜索位置，从左边开始</param>
        /// <param name="stringComparison">System.StringComparison 值之一。</param>
        /// <returns></returns>
        public static string GetRight(this string str, string value, int? startIndex = null, StringComparison? stringComparison = null)
        {
            if (value == null)
                throw new ArgumentNullException("value");

            int pos;
            if (startIndex != null && stringComparison != null)
                pos = str.LastIndexOf(value, startIndex.Value, stringComparison.Value);
            else if (startIndex != null)
                pos = str.LastIndexOf(value, startIndex.Value);
            else if (stringComparison != null)
                pos = str.LastIndexOf(value, stringComparison.Value);
            else
                pos = str.LastIndexOf(value);

            if (pos == -1)
                return string.Empty;

            return str.Substring(pos + value.Length);
        }
        #endregion

        #region string RemoveNewlineTrim(this string str)
        /// <summary>
        /// str.Replace("\r\n", "").Replace("\n", "").Replace("\r", "").Trim()
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string RemoveNewlineTrim(this string str)
        {
            if (str == null)
                return null;

            return str.Replace("\r\n", "").Replace("\n", "").Replace("\r", "").Trim();
        }

        #endregion

        #region bool ExistAllKeys(this string data, params string[] keys)
        /// <summary>
        /// data 包含 keys 所有元素返回 true
        /// </summary>
        /// <param name="data">待查找的全部文本数据</param>
        /// <param name="keys">关键字</param>
        /// <returns></returns>
        public static bool ExistAllKeys(this string data, params string[] keys)
        {
            return keys.All(x => data.Contains(x));
        }

        #endregion

        #region 把字符串中间用星号*代替，返回字符串两边的几个字符。+ string GetSafeString(this string str, int leftCount, int rightCount)
        /// <summary>
        /// 把字符串中间用星号*代替，返回字符串两边的几个字符。
        /// </summary>
        /// <param name="str"></param>
        /// <param name="leftCount">左边保留几位，默认：四分之一</param>
        /// <param name="rightCount">右边保留几位，默认：四分之一</param>
        /// <returns></returns>
        public static string GetSafeString(this string str, int leftCount = 0, int rightCount = 0)
        {
            if (string.IsNullOrEmpty(str))
                throw new ArgumentNullException("str");
            if (leftCount > str.Length / 2)
                throw new ArgumentOutOfRangeException("leftCount");
            if (rightCount > str.Length / 2)
                throw new ArgumentOutOfRangeException("rightCount");

            if (leftCount == 0 || rightCount == 0)
            {
                var len = str.Length / (str.Length > 3 ? 4 : 2);
                leftCount = len;
                rightCount = len;
            }

            var left = str.Substring(0, leftCount);
            var right = str.Substring(str.Length - rightCount, rightCount);

            var xh = new string('*', str.Length - leftCount - rightCount);
            return left + xh + right;
        }

        #endregion

        /// <summary>
        /// 把 \r\n、\n 替换为 ＜br/＞
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string ReplaceBr(this string str)
        {
            return str.Replace("\r\n", "<br/>").Replace("\n", "<br/>");
        }

        /// <summary>
        /// 从 str 的左边截取 count 个数的字符串。（一个中文字符算两个字符长度）
        /// </summary>
        /// <param name="str"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public static string SubLeft(this string str, int count)
        {
            var chars = new List<char>();
            int n = 0;
            foreach (var item in str)
            {
                if (new ASCIIEncoding().GetBytes(new[] { item })[0] == 63)
                    n += 2;
                else
                    n++;

                if (n > count)
                    break;

                chars.Add(item);
            }
            return new string(chars.ToArray());
        }

    }
}