using System;
using EasyHookLib.Utility;

namespace HookCreateFileW
{
    public class NotifyClient : NotifyClientBase
    {
        public override void IsInstalled(int inClientPid)
        {
        }

        public override void NotifyMethodHooked(params Tuple<string, object>[] args)
        {
            foreach (var tuple in args)
            {
                Console.WriteLine($"{tuple.Item1}:{tuple.Item2}");
            }
        }

        public override void Ping()
        {
        }

        public override void ReportException(Exception inInfo)
        {
        }
    }
}