using System;
using System.Diagnostics;
using EasyHook;
using EasyHookLib.Delegates;
using EasyHookLib.Win32;
using EasyHookLib.Win32.Model;

namespace EasyHookLib
{
    public class CreateProcessAHooker : HookerBase
    {
        public ProcessCreatedEventArgs CreatedEventArgs { get; private set; }

        public event EventHandler<ProcessCreatedEventArgs> MethodHooked;

        /// <summary>
        /// Our MessageBeep hook handler
        /// </summary>
        private bool CreateProcessHook(string lpApplicationName, string lpCommandLine,
            IntPtr lpProcessAttributes, IntPtr lpThreadAttributes,
            bool bInheritHandles, uint dwCreationFlags, IntPtr lpEnvironment,
            string lpCurrentDirectory, ref StartupInfoA lpStartupInfo, ref ProcessInformation pInfo)
        {
            // We aren't going to call the original at all
            // but we could using: return MessageBeep(uType);
            Console.Write("...intercepted...");
            var processHook = Win32Interop.CreateProcessA(lpApplicationName, lpCommandLine,
                lpProcessAttributes, lpThreadAttributes,
                bInheritHandles, dwCreationFlags | (uint)ProcessCreationFlags.CreateSuspended, lpEnvironment,
                lpCurrentDirectory, ref lpStartupInfo, ref pInfo);
            CreatedEventArgs = new ProcessCreatedEventArgs(pInfo.DwProcessId, pInfo.HProcess);
            MethodHooked?.Invoke(this, CreatedEventArgs);
            IntPtr threadHandle = pInfo.HThread;
            Win32Interop.ResumeThread(threadHandle);
            return processHook;
        }

        public LocalHook HookCreateProcess()
        {
            IntPtr inTargetProc = LocalHook.GetProcAddress("kernel32.dll", "CreateProcessA");
            Delegate delegateToCall = new CreateProcessDelegateA(CreateProcessHook);
            return HookMethod(inTargetProc, delegateToCall);
        }

    }
}
