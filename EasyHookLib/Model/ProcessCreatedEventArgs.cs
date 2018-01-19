using System;

namespace EasyHookLib
{
    public class ProcessCreatedEventArgs: EventArgs
    {
        public ProcessCreatedEventArgs(uint processId, IntPtr hProcess)
        {
            ProcessId = processId;
            HProcess = hProcess;
        }

        public uint ProcessId { get; }
        public IntPtr HProcess { get; }
    }
}