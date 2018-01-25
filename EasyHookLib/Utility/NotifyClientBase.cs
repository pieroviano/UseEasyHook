using System;
using System.Linq;
using EasyHookLib.Interfaces;
using EasyHookLib.RemoteInjection;

namespace EasyHookLib.Utility
{
    public abstract class NotifyClientBase : NotifyEventsBase, INotifyClient, IDisposable
    {
        private bool _disposed;

        public void Dispose()
        {
            var enumerable = PostbackMessageHandler.RemoteHookerBasesToNotify.Where(e => e.Item1 == this).ToArray();
            foreach (var element in enumerable)
            {
                PostbackMessageHandler.RemoteHookerBasesToNotify.Remove(element);
            }
            _disposed = true;
        }

        ~NotifyClientBase()
        {
            if (!_disposed)
            {
                Dispose();
            }
        }
    }
}