using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using EasyHook;
using EasyHookLib.RemoteInjection;

namespace FileMonInject
{
    [UnmanagedFunctionPointer(CallingConvention.StdCall,
        CharSet = CharSet.Unicode,
        SetLastError = true)]
    internal delegate IntPtr DCreateFile(
        string InFileName,
        uint InDesiredAccess,
        uint InShareMode,
        IntPtr InSecurityAttributes,
        uint InCreationDisposition,
        uint InFlagsAndAttributes,
        IntPtr InTemplateFile);

    public class Main : IEntryPoint
    {
        private readonly PostbackMessageHandler _interface;
        private readonly Stack<string> _queue = new Stack<string>();
        private bool _callBeforeNotify;
        private LocalHook _createFileHook;
        private readonly bool _notifyInnedialety = true;

        public Main(
            RemoteHooking.IContext inContext,
            string inChannelName)
        {
            // connect to host...
            _interface = RemoteHooking.IpcConnectClient<PostbackMessageHandler>(inChannelName);

            _interface.Ping();
        }

        // just use a P-Invoke implementation to get native API access from C# (this step is not necessary for C++.NET)
        [DllImport("kernel32.dll",
            CharSet = CharSet.Unicode,
            SetLastError = true,
            CallingConvention = CallingConvention.StdCall)]
        private static extern IntPtr CreateFile(
            string InFileName,
            uint InDesiredAccess,
            uint InShareMode,
            IntPtr InSecurityAttributes,
            uint InCreationDisposition,
            uint InFlagsAndAttributes,
            IntPtr InTemplateFile);

        // this is where we are intercepting all file accesses!
        private static IntPtr CreateFile_Hooked(
            string InFileName,
            uint InDesiredAccess,
            uint InShareMode,
            IntPtr InSecurityAttributes,
            uint InCreationDisposition,
            uint InFlagsAndAttributes,
            IntPtr InTemplateFile)
        {
            Main This = null;
            var fileHooked = IntPtr.Zero;
            try
            {
                This = (Main)HookRuntimeInfo.Callback;
                if (This._callBeforeNotify)
                {
                    fileHooked = CreateFile(
                        InFileName,
                        InDesiredAccess,
                        InShareMode,
                        InSecurityAttributes,
                        InCreationDisposition,
                        InFlagsAndAttributes,
                        InTemplateFile);
                }
                NotifyHooker(This, InFileName);
            }
            catch (Exception ex)
            {
                Debug.Write(ex);
            }

            // call original API...
            if (!This?._callBeforeNotify ?? true)
            {
                fileHooked = CreateFile(
                    InFileName,
                    InDesiredAccess,
                    InShareMode,
                    InSecurityAttributes,
                    InCreationDisposition,
                    InFlagsAndAttributes,
                    InTemplateFile);
            }
            return fileHooked;
        }

        private static void NotifyHooker(Main This, string message)
        {
            lock (This._queue)
            {
                This._queue.Push("[" + RemoteHooking.GetCurrentProcessId() + ":" +
                                 RemoteHooking.GetCurrentThreadId() + "]: \"" + message + "\"");
            }
            if (This._notifyInnedialety)
            {
                This.SignalMethdHooked();
            }
        }

        public void Run(
            RemoteHooking.IContext inContext,
            string inChannelName)
        {
            // install hook...
            try
            {
                var inTargetProc = LocalHook.GetProcAddress("kernel32.dll", "CreateFileW");
                Delegate dCreateFile = new DCreateFile(CreateFile_Hooked);
                _createFileHook = LocalHook.Create(
                    inTargetProc,
                    dCreateFile,
                    this);

                _createFileHook.ThreadACL.SetExclusiveACL(new[] { 0 });
            }
            catch (Exception extInfo)
            {
                _interface.ReportException(extInfo);

                return;
            }

            _interface.IsInstalled(RemoteHooking.GetCurrentProcessId());

            RemoteHooking.WakeUpProcess();

            // wait for host process termination...
            try
            {
                while (true)
                {
                    Thread.Sleep(500);

                    if (!_notifyInnedialety)
                    {
                        SignalMethdHooked();
                    }
                    else
                    {
                        // transmit newly monitored file accesses...
                        _interface.Ping();
                    }
                }
            }
            catch (Exception ex)
            {
                try
                {
                    _interface.ReportException(ex); //will raise an exception if host is unreachable
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e);
                }
            }
        }

        private void SignalMethdHooked()
        {
            string[] package;

            lock (_queue)
            {
                package = _queue.ToArray();

                _queue.Clear();
            }

            _interface.ProcessMessages(RemoteHooking.GetCurrentProcessId(), package);
        }
    }
}