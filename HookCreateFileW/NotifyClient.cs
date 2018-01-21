using System;
using System.Linq;
using EasyHookLib.Interfaces;
using EasyHookLib.RemoteInjection;
using EasyHookLib.Utility;

namespace FileMonitor
{
    public class NotifyClient : NotifyClientBase
    {

        public override void NotifyMethodHooked(params Tuple<string, object>[] args)
        {
            foreach (var tuple in args)
            {
                Console.WriteLine($"{tuple.Item1}:{tuple.Item2}");
            }
        }

    }
}