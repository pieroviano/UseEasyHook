using System;
using EasyHookLib.Interfaces;
using EasyHookLib.Model;

namespace EasyHookLib.Utility
{
    public class NotifyEventsBase : INotifyEvents
    {
        public event EventHandler<HookedEventArgs> IsInstalledEvent;
        public event EventHandler<HookedEventArgs> PingEvent;
        public event EventHandler<HookedEventArgs> ReportExceptionEvent;
        public event EventHandler<HookedEventArgs> MethodHookedEvent;

        public virtual void IsInstalled(int inClientPid)
        {
            ReportExceptionEvent?.Invoke(this,
                new HookedEventArgs(new Tuple<string, object>("ClientPid", inClientPid)));
        }

        public virtual void NotifyMethodHooked(params Tuple<string, object>[] args)
        {
            var e = new HookedEventArgs(args);
            MethodHookedEvent?.Invoke(this, e);
        }

        public virtual void Ping()
        {
            PingEvent?.Invoke(this, new HookedEventArgs());
        }

        public virtual void ReportException(Exception inInfo)
        {
            ReportExceptionEvent?.Invoke(this, new HookedEventArgs(new Tuple<string, object>("Exception", inInfo)));
        }
    }
}