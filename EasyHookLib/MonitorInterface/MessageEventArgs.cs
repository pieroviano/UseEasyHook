using System;

namespace EasyHookLib.MonitorInterface
{
    public class MessageEventArgs : EventArgs
    {
        /// <summary>
        /// The informational message sent from the client (FileMonitorInterceptor).
        /// </summary>
        public string Message { get; set; }

        public MessageEventArgs() { }

        public MessageEventArgs(string message)
        {
            this.Message = message;
        }
    }
}
