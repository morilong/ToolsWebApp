using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System
{
    public static class CollectionsHelper
    {
        #region string 转为 List<T>

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="str"></param>
        /// <param name="convertAction"></param>
        /// <param name="options"></param>
        /// <param name="separator">默认：","</param>
        /// <returns></returns>
        public static List<T> ToListEx<T>(this string str, Action<string, List<T>> convertAction, StringSplitOptions options = StringSplitOptions.RemoveEmptyEntries, params string[] separator)
        {
            var list = new List<T>();

            if (str.IsNullOrEmpty())
                return list;

            if (separator.Length == 0)
                separator = new[] { "," };

            foreach (var item in str.Split(separator, options))
            {
                convertAction(item, list);
            }
            return list;
        }
        /// <summary>
        /// StringSplitOptions.RemoveEmptyEntries
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="str"></param>
        /// <param name="convertAction"></param>
        /// <param name="separator">默认：","</param>
        /// <returns></returns>
        public static List<T> ToListEx<T>(this string str, Action<string, List<T>> convertAction, params string[] separator)
        {
            return ToListEx(str, convertAction, StringSplitOptions.RemoveEmptyEntries, separator);
        }
        /// <summary>
        /// 不会返回 null，已过滤掉 0
        /// </summary>
        /// <param name="str"></param>
        /// <param name="separator">默认：","</param>
        /// <returns></returns>
        public static List<int> ToListInt(this string str, params string[] separator)
        {
            return ToListEx<int>(str, (s, list) =>
            {
                var num = s.ToInt32();
                if (num > 0) list.Add(num);
            }, separator);
        }
        /// <summary>
        /// 不会返回 null，已过滤掉 0
        /// </summary>
        /// <param name="str"></param>
        /// <param name="separator">默认：","</param>
        /// <returns></returns>
        public static List<long> ToListLong(this string str, params string[] separator)
        {
            return ToListEx<long>(str, (s, list) =>
            {
                var num = s.ToInt64();
                if (num > 0) list.Add(num);
            }, separator);
        }
        /// <summary>
        /// 不会返回 null
        /// </summary>
        /// <param name="str"></param>
        /// <param name="separator">默认：","</param>
        /// <returns></returns>
        public static List<string> ToListString(this string str, params string[] separator)
        {
            return ToListEx<string>(str, (s, list) => { list.Add(s); }, separator);
        }

        #endregion

        #region string ToStringEx<T>(this IList<T> list, string separator = ",")
        /// <summary>
        /// 不会返回 null
        /// </summary>
        /// <param name="list"></param>
        /// <param name="separator"></param>
        /// <returns></returns>
        public static string ToStringEx<T>(this IList<T> list, string separator = ",")
        {
            return ToStringEx(list, null, separator);
        }

        /// <summary>
        /// 不会返回 null
        /// </summary>
        /// <param name="list"></param>
        /// <param name="func">用于修改返回值数据类型</param>
        /// <param name="separator"></param>
        /// <returns></returns>
        public static string ToStringEx<T>(this IList<T> list, Func<T, object> func, string separator = ",")
        {
            var sb = new StringBuilder();
            foreach (var item in list)
            {
                if (sb.Length > 0)
                    sb.Append(separator);

                sb.Append(func == null ? item : func(item));
            }
            return sb.ToString();
        }

        #endregion

        /// <summary>
        /// 从 list 筛选出一个出现次数最多的值。
        /// <para>1、将 list 分组，2、Count(T)，3、Count 降序排序，4、返回第一个 T 值。</para>
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public static T GroupCountDescFirstValue<T>(this IEnumerable<T> list)
        {
            return list.GroupBy(x => x).ToDictionary(k => k.Key, v => v.Count()).OrderByDescending(x => x.Value).First().Key;
        }


    }
}
