using System;
using EasyHookLib.Model;

namespace EasyHookLib.Interfaces
{
    public interface INotifyEvents
    {
        event EventHandler<HookedEventArgs> IsInstalledEvent;
        event EventHandler<HookedEventArgs> PingEvent;
        event EventHandler<HookedEventArgs> ReportExceptionEvent;
        event EventHandler<HookedEventArgs> MethodHookedEvent;
    }
}