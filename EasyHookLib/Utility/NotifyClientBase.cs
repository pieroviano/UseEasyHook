using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EasyHookLib.Interfaces;
using EasyHookLib.RemoteInjection;

namespace EasyHookLib.Utility
{
        public abstract class NotifyClientBase : INotifyClient, IDisposable
        {
            private bool _disposed;

            public abstract void NotifyMethodHooked(params Tuple<string, object>[] args);

            ~NotifyClientBase()
            {
                if (!_disposed)
                    Dispose();
            }

            public void Dispose()
            {
                var enumerable = PostbackMessageHandler.RemoteHookerBasesToNotify.Where(e => e.Item1 == this).ToArray();
                foreach (var element in enumerable)
                {
                    PostbackMessageHandler.RemoteHookerBasesToNotify.Remove(element);
                }
                _disposed = true;
            }
        }
}
