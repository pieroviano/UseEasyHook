using System;
using System.Collections.Generic;
using CreateProcessHookLib.Win32;
using EasyHookLib.Model;
using EasyHookLib.Utility;

namespace HookCreateProcessW
{
    public class NotifyClient : NotifyClientBase
    {
        public override void NotifyMethodHooked(params Tuple<string, object>[] args)
        {
            foreach (var tuple in args)
            {
                var tupleItem2 = tuple.Item2.ToString().Split(':')[2].Replace("\"","").Split(',');
                var dictionar = new Dictionary<string, string>();
                for (int i = 0; i < tupleItem2.Length; i++)
                {
                    var strings = tupleItem2[i].Split('=');
                    dictionar.Add(strings[0], strings[1]);
                    Console.WriteLine($"{tuple.Item1}:{strings[0]}={strings[1]}");
                }

            }
            base.NotifyMethodHooked(args);
        }

        public override void ReportException(Exception inInfo)
        {
            Console.WriteLine($"Exception: {inInfo}");
            base.ReportException(inInfo);
        }
    }
}