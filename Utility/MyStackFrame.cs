using System;
using System.Reflection;

namespace System.Diagnostics
{
    public static class MyStackFrame
    {
        /// <summary>
        /// 获取当前方法的名称。
        /// </summary>
        /// <returns></returns>
        public static string GetMethodName()
        {
            return new StackFrame().GetMethod().Name;
        }

        /// <summary>
        /// 获取当前方法的完整名称。（命名空间.类名.方法名）
        /// </summary>
        /// <returns></returns>
        public static string GetMethodFullName()
        {
            var m = new StackFrame().GetMethod();
            return m.ReflectedType.FullName + "." + m.Name;
        }

        /// <summary>
        /// 获取当前方法的类名和方法名。（类名.方法名）
        /// </summary>
        /// <returns></returns>
        public static string GetMethodClassName()
        {
            var m = new StackFrame().GetMethod();
            return m.ReflectedType.Name + "." + m.Name;
        }

        /// <summary>
        /// 获取被调用方法的名称。
        /// </summary>
        /// <returns></returns>
        public static string GetCallMethodName()
        {
            var stackTrace = new StackTrace();
            var frame = stackTrace.GetFrames()[1];
            var m = frame.GetMethod();
            return m.Name;
        }

        /// <summary>
        /// 获取被调用方法的完整名称。（命名空间.类名.方法名）
        /// </summary>
        /// <returns></returns>
        public static string GetCallMethodFullName()
        {
            var stackTrace = new StackTrace();
            var frame = stackTrace.GetFrames()[1];
            var m = frame.GetMethod();
            return m.ReflectedType.FullName + "." + m.Name;
        }

        /// <summary>
        /// 获取被调用方法的类名和方法名。（类名.方法名）
        /// </summary>
        /// <returns></returns>
        public static string GetCallMethodClassName()
        {
            var stackTrace = new StackTrace();
            var frame = stackTrace.GetFrames()[1];
            var m = frame.GetMethod();
            return m.ReflectedType.Name + "." + m.Name;
        }

    }
}
