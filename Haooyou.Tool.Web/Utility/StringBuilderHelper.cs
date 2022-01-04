using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Haooyou.TopApi.WebApp.Utility
{
    public static class StringBuilderHelper
    {
        /// <summary>
        /// if (sb.Length > 0) sb.AppendLine();
        /// </summary>
        /// <param name="sb"></param>
        public static void AppendLineEx(this StringBuilder sb)
        {
            if (sb.Length > 0)
                sb.AppendLine();
        }
    }
}