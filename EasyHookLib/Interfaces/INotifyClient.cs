using System;

namespace EasyHookLib.Interfaces
{
    public interface INotifyClient : INotifyEvents
    {
        void IsInstalled(int inClientPid);
        void NotifyMethodHooked(params Tuple<string, object>[] args);

        void Ping();

        void ReportException(Exception inInfo);
    }
}