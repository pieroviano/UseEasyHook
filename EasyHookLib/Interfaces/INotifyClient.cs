using System;

namespace EasyHookLib.Interfaces
{
    public interface INotifyClient
    {
        void NotifyMethodHooked(params Tuple<string, object>[] args);
    }
}