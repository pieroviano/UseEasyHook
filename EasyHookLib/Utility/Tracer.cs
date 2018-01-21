using System;
using System.Reflection;
using System.Runtime.InteropServices;
using log4net;

namespace EasyHookLib.Utility
{
    public class Tracer
    {
        private static readonly bool _isConsoleLog = AppConfig.Instance["Trace"] == "Console";
        private static readonly bool _isLog4NetLog = AppConfig.Instance["Trace"] == "Log4Net";

        private static readonly ILog log = LogManager.GetLogger
            (MethodBase.GetCurrentMethod().DeclaringType);

        static Tracer()
        {
            if (_isConsoleLog)
            {
                AllocConsole();
            }
        }

        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool AllocConsole();

        public static void WriteLine(object obj)
        {
            if (_isConsoleLog)
            {
                Console.WriteLine(obj);
            }
            if (_isLog4NetLog)
            {
                log.Info(obj);
            }
            else
            {
                WriteLine(obj);
            }
        }
    }
}