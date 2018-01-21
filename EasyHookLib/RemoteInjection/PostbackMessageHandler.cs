using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using EasyHookLib.Interfaces;
using EasyHookLib.Utility;

namespace EasyHookLib.RemoteInjection
{
    public class PostbackMessageHandler : MarshalByRefObject
    {

        public static List<Tuple<INotifyClient, int>> RemoteHookerBasesToNotify { get; } = new List<Tuple<INotifyClient, int>>();

        public void IsInstalled(int inClientPid)
        {
            Tracer.WriteLine($"Hooking has been installed in target {inClientPid}.\r\n");
        }

        public void Ping()
        {
            Tracer.WriteLine("I'm alive");
        }

        public void ProcessMessages(int inClientPid, string[] messsages)
        {
            for (var i = 0; i < messsages.Length; i++)
            {
                var message = messsages[i];
                Tracer.WriteLine(message);
                for (var j = 0; j < RemoteHookerBasesToNotify.Count; j++)
                {
                    if (RemoteHookerBasesToNotify[j].Item2 == inClientPid)
                    {
                        RemoteHookerBasesToNotify[j].Item1.NotifyMethodHooked(new Tuple<string, object>("Message",
                            message));
                    }
                }
            }
        }

        public void ReportException(Exception inInfo)
        {
            Tracer.WriteLine("The target process has reported an error:\r\n" + inInfo);
        }
    }
}