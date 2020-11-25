using System;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace System.Web.Mvc
{
    public class NetLog
    {
        private static readonly object LockWrite = new object();

        private static void WriteLogFile(string dir, string msg)
        {
            var time = DateTime.Now;
            string fileFullPath = string.Format("{0}\\{1}.log", dir, time.ToString("yyyy-MM-dd"));
            var sb = new StringBuilder();
            sb.AppendLine(time.ToStringEx());
            sb.AppendLine(msg);
            sb.AppendLine("-----------------------------------------------------------");
            lock (LockWrite)
            {
                using (var sw = new StreamWriter(fileFullPath, true, Encoding.UTF8))
                {
                    sw.Write(sb.ToString());
                }
            }
#if DEBUG
            if (msg.Length < 200)
                Debug.WriteLine(time.ToStringEx() + " " + msg);
#endif
        }

        public static void Error(string msg)
        {
            string dir = AppDomain.CurrentDomain.BaseDirectory + @"Log\Error";
            WriteLogFile(dir, msg);
        }

        public static void Error(Exception ex)
        {
            string dir = AppDomain.CurrentDomain.BaseDirectory + @"Log\Error";
            WriteLogFile(dir, ex.ToString());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="appendMsg">附加错误信息</param>
        public static void Error(Exception ex, string appendMsg)
        {
            string dir = AppDomain.CurrentDomain.BaseDirectory + @"Log\Error";
            WriteLogFile(dir, ex.ToString() + "\r\n" + appendMsg);
        }

        /*  /// <summary>
        /// msg前加 MyStackFrame.GetCallMethodClassName()
        /// </summary>
        /// <param name="ex"></param>
        public static void ErrorEx(Exception ex)
        {
            Error(string.Format("{0} {1}", MyStackFrame.GetCallMethodClassName(), ex.ToString()));
        }

        /// <summary>
        /// msg前加 MyStackFrame.GetCallMethodClassName()
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="appendMsg"></param>
        public static void ErrorEx(Exception ex, string appendMsg)
        {
            Error(string.Format("{0} {1}\r\n{2}", MyStackFrame.GetCallMethodClassName(), ex.ToString(), appendMsg));
        }*/

        /*public static void Warn(string format, params object[] args)
        {
            string dir = AppDomain.CurrentDomain.BaseDirectory + @"Log\Warn";
            WriteLogFile(dir, string.Format(format, args));
        }*/

        public static void Warn(string msg)
        {
            string dir = AppDomain.CurrentDomain.BaseDirectory + @"Log\Warn";
            WriteLogFile(dir, msg);
        }

        public static void Info(string msg)
        {
            string dir = AppDomain.CurrentDomain.BaseDirectory + @"Log\Info";
            WriteLogFile(dir, msg);
        }

        /// <summary>
        /// msg前加 MyStackFrame.GetCallMethodClassName()
        /// </summary>
        /// <param name="msg"></param>
        public static void WarnEx(string msg)
        {
            Warn(string.Format("{0} {1}", MyStackFrame.GetCallMethodClassName(), msg));
        }

        /// <summary>
        /// msg前加 MyStackFrame.GetCallMethodClassName()
        /// </summary>
        /// <param name="msg"></param>
        public static void InfoEx(string msg)
        {
            Info(string.Format("{0} {1}", MyStackFrame.GetCallMethodClassName(), msg));
        }

        /// <summary>
        /// msg前加 MyStackFrame.GetCallMethodClassName()
        /// </summary>
        /// <param name="msg"></param>
        public static void ErrorEx(string msg)
        {
            Error(string.Format("{0} {1}", MyStackFrame.GetCallMethodClassName(), msg));
        }

    }
}