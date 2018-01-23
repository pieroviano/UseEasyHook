using System;
using EasyHookLib.Utility;

namespace HookCreateProcessW
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