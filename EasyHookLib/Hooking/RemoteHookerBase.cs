using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using EasyHook;
using EasyHookLib.RemoteInjection;
using EasyHookLib.Utility;

namespace EasyHookLib.Hooking
{
    public abstract class RemoteHookerBase : HookerBase, IEntryPoint
    {

        protected readonly Stack<string> Queue;

        protected PostbackMessageHandler Interface;

        public RemoteHookerBase(
            RemoteHooking.IContext inContext,
            string inChannelName)
        {
            NotifyImmedialety = true;
            Queue = new Stack<string>();
            // connect to host...
            Interface = RemoteHooking.IpcConnectClient<PostbackMessageHandler>(inChannelName);

            Interface.Ping();
        }

        public bool NotifyImmedialety { get; set; }

        // this is where we are intercepting all file accesses!
        public static object CallMethodAndNotifyHookerStatic(params object[] parameters)
        {
            RemoteHookerBase This = null;
            var fileHooked = IntPtr.Zero;
            This = (RemoteHookerBase) HookRuntimeInfo.Callback;
            return This.CallMethodAndNotifyHooker(parameters);
        }

        public override void NotifyMethodHooked(params Tuple<string, object>[] args)
        {
            lock (Queue)
            {
                var message = "";
                foreach (var tuple in args)
                {
                    if (!string.IsNullOrEmpty(message))
                    {
                        message += ", ";
                    }
                    message += $"{tuple.Item1}={tuple.Item2}";
                }
                Queue.Push(
                    $"{GetType().FullName}-[{RemoteHooking.GetCurrentProcessId()}:{RemoteHooking.GetCurrentThreadId()}]: \"{message}\"");
            }
            if (NotifyImmedialety)
            {
                SignalMethodHooked();
            }
        }

        public void Run(
            RemoteHooking.IContext inContext,
            string inChannelName)
        {
            MessageBox.Show("PPP");
            // install hook...
            try
            {
                CreateHook();
            }
            catch (Exception extInfo)
            {
                Interface.ReportException(extInfo);

                return;
            }
            try
            {
                var thread = new Thread(NotifyInstalled);
                thread.Start();

                RemoteHooking.WakeUpProcess();
            }
            catch (Exception e)
            {
                Tracer.WriteLine(e);
            }

            // wait for host process termination...
            try
            {
                while (true)
                {
                    Thread.Sleep(500);

                    if (!NotifyImmedialety)
                    {
                        SignalMethodHooked();
                    }
                    else
                    {
                        // transmit newly monitored file accesses...
                        Interface.Ping();
                    }
                }
            }
            catch (Exception ex)
            {
                try
                {
                    Interface.ReportException(ex); //will raise an exception if host is unreachable
                }
                catch (Exception e)
                {
                    Tracer.WriteLine(e);
                }
            }
        }

        private void NotifyInstalled()
        {
            Interface.IsInstalled(RemoteHooking.GetCurrentProcessId());
        }


        protected void SignalMethodHooked()
        {
            string[] package;

            lock (Queue)
            {
                package = Queue.ToArray();

                Queue.Clear();
            }

            Interface.ProcessMessages(RemoteHooking.GetCurrentProcessId(), package);
        }
    }
}