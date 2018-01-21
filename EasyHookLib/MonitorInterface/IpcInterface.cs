using System;
using System.Collections.Generic;

namespace EasyHookLib.MonitorInterface
{
    /// <summary>
    ///     Allows communication between the IPC server and client.
    /// </summary>
    /// <remarks>
    ///     Both the client (FileMonitorInterceptor) and the server (FileMonitorController) access the methods and properties
    ///     of this same interface.
    /// </remarks>
    public class IpcInterface : MarshalByRefObject
    {
        /// <summary>
        ///     A map of intercepted file entries to each specific process.
        /// </summary>
        /// <remarks>
        ///     This same dictionary is filled by the client (FileMonitorInterceptor) and queried by the server
        ///     (FileMonitorController).
        /// </remarks>
        public readonly Dictionary<int, List<MessageEntry>> Entries;

        /// <summary>
        ///     Added to when FileMonitorController requests a process to terminate its hook.
        /// </summary>
        public readonly List<int> TerminatingProcesses;

        public IpcInterface()
        {
            Entries = new Dictionary<int, List<MessageEntry>>();
            TerminatingProcesses = new List<int>();
        }

        public void AddFileEntry(int processId, MessageEntry entry)
        {
            if (!Entries.ContainsKey(processId))
            {
                Entries.Add(processId, new List<MessageEntry>());
            }

            Entries[processId].Add(entry);
        }

        public override object InitializeLifetimeService()
        {
            return null;
        }

        /// <summary>
        ///     Occurs when the client posts an informational message.
        /// </summary>
        public event EventHandler<MessageEventArgs> OnMessagePosted;

        /// <summary>
        ///     Does nothing, but will throw an exception from either the client or server if the other is unreachable.
        /// </summary>
        public void Ping()
        {
        }

        public void PostException(Exception ex)
        {
            PostMessage(ex.ToString());
        }

        /// <summary>
        ///     Posts a message from the client (FileMonitorInterceptor) to the server (FileMonitorController).
        /// </summary>
        /// <remarks>
        ///     The message isn't really "sent". Rather, an event handler is fired, and the server has hopefully subscribed to this
        ///     event handler.
        /// </remarks>
        /// <param name="message">The string message to post.</param>
        public void PostMessage(string message)
        {
            if (OnMessagePosted != null)
            {
                OnMessagePosted(this, new MessageEventArgs(message));
            }
        }
    }
}