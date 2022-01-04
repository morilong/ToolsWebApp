namespace System
{
    /// <summary>
    /// DateTime 转换扩展类
    /// </summary>
    public static class DateTimeConvert
    {
        #region 返回格式：yyyy-MM-dd + string ToStringDate(this DateTime dateTime)
        /// <summary>
        /// 返回格式：yyyy-MM-dd
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static string ToStringDate(this DateTime dateTime)
        {
            return dateTime.ToString("yyyy-MM-dd");
        }
        #endregion

        #region 返回格式：HH:mm:ss + string ToStringTime(this DateTime dateTime)
        /// <summary>
        /// 返回格式：HH:mm:ss
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static string ToStringTime(this DateTime dateTime)
        {
            return dateTime.ToString("HH:mm:ss");
        }
        #endregion

        #region 将时间转换为格式：yyyy-MM-dd HH:mm:ss 的字符串。+ string ToStringEx(this DateTime dateTime)

        /// <summary>
        /// yyyy-MM-dd HH:mm:ss
        /// </summary>
        public const string DateTimeFormat = "yyyy-MM-dd HH:mm:ss";
        /// <summary>
        /// yyyy-MM-dd
        /// </summary>
        public const string DateFormat = "yyyy-MM-dd";

        /// <summary>
        /// 将时间转换为格式：yyyy-MM-dd HH:mm:ss 的字符串。
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static string ToStringEx(this DateTime dateTime)
        {
            return dateTime.ToString(DateTimeFormat);
        }

        #endregion

        #region 将DateTime时间格式转换为13位long的Unix时间戳格式 + long ToLong(this DateTime time)

        /// <summary>
        ///将DateTime时间格式转换为13位long的Unix时间戳格式
        /// </summary>
        /// <param name="dateTime">时间</param>
        /// <returns>long</returns>
        public static long ToLong(this DateTime dateTime)
        {
            return (dateTime.ToUniversalTime().Ticks - 621355968000000000) / 10000;
        }

        #endregion

        #region 将DateTime时间格式转换为10位int的Unix时间戳格式 + int ToInt(this DateTime dateTime)

        /// <summary>
        ///将DateTime时间格式转换为10位int的Unix时间戳格式
        /// </summary>
        /// <param name="dateTime">时间</param>
        /// <returns></returns>
        public static int ToInt(this DateTime dateTime)
        {
            return (int)((dateTime.ToUniversalTime().Ticks - 621355968000000000) / 10000000);
        }

        #endregion

        #region DateTime转成GMT时间 + string ToGMTString(this DateTime dateTime)

        /// <summary>
        ///DateTime转成GMT时间 + string ToGMTString(this DateTime dateTime)
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static string ToGMTString(this DateTime dateTime)
        {
            return dateTime.ToUniversalTime().ToString("r");
        }

        #endregion

        #region 多行POST用到，生成结果如：8d2d1b63a1bfc1c。+ string NowTicksToHex(this DateTime dateTime)
        /// <summary>
        /// 多行POST用到，生成结果如：8d2d1b63a1bfc1c。
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static string TicksToHex(this DateTime dateTime)
        {
            return dateTime.Ticks.ToString("x");
        }
        #endregion

        #region 将13位Unix时间戳转换为DateTime类型时间
        /// <summary>
        /// 将13位Unix时间戳转换为DateTime类型时间
        /// </summary>
        /// <param name="timeStamp">13位数字时间戳</param>
        /// <returns>DateTime</returns>
        public static DateTime ToDateTime(this long timeStamp)
        {
            DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            return startTime.AddMilliseconds(timeStamp);
        }
        #endregion

        #region 将10位Unix时间戳格式转换为c# DateTime时间格式
        /// <summary>
        /// 将10位Unix时间戳格式转换为c# DateTime时间格式
        /// </summary>
        /// <param name="timeStamp">10位数字时间戳</param>
        /// <returns></returns>
        public static DateTime ToDateTime(this int timeStamp)
        {
            DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            long lTime = long.Parse(timeStamp + "0000000");
            TimeSpan toNow = new TimeSpan(lTime);
            return dtStart.Add(toNow);
        }
        #endregion

        /// <summary>
        /// ts.Days > 0 ? "d'.'hh':'mm" : "hh':'mm"
        /// </summary>
        /// <param name="ts"></param>
        /// <returns></returns>
        public static string ToStringEx(this TimeSpan ts)
        {
            string format = ts.Days > 0 ? "d'.'hh':'mm" : "hh':'mm";
            return ts.ToString(format);
        }
    }
}