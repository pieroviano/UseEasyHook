using System;
using CreateProcessAHookLib.Win32.Model;
using CreateProcessHookALib.Win32;
using CreateProcessHookLib.Win32.Model;
using EasyHook;
using EasyHookLib.Hooking;

namespace CreateProcessAHookLib
{
    internal class CreateProcessAHookerImplementation<T> where T : HookerBase
    {
        private readonly T _t;

        public CreateProcessAHookerImplementation(T t)
        {
            _t = t;
        }


        public object CallMethod(object[] parameters, out Tuple<string, object>[] tuplesForNotification)
        {
            var lpStartupInfo = (StartupInfoA) parameters[8];
            var pInfo = (ProcessInformation) parameters[9];
            var processHook = Win32Interop.CreateProcessA((string) parameters[0], (string) parameters[1],
                (IntPtr) parameters[2], (IntPtr) parameters[3], (bool) parameters[4],
                (uint) parameters[5] | (uint) ProcessCreationFlags.CreateSuspended,
                (IntPtr) parameters[6], (string) parameters[7], ref lpStartupInfo, ref pInfo);
            parameters[8] = lpStartupInfo;
            parameters[9] = pInfo;
            tuplesForNotification = new[]
            {
                new Tuple<string, object>("DwProcessId", pInfo.DwProcessId),
                new Tuple<string, object>("HProcess", pInfo.HProcess)
            };
            return processHook;
        }

        public LocalHook CreateHook(T This, Delegate createProcessDelegateA)
        {
            return _t.HookMethod("kernel32.dll", "CreateProcessA", createProcessDelegateA, This);
        }

        public static bool CreateProcessHandlerStatic(string lpApplicationName, string lpCommandLine, IntPtr lpProcessAttributes,
            IntPtr lpThreadAttributes, bool bInheritHandles, uint dwCreationFlags, IntPtr lpEnvironment,
            string lpCurrentDirectory, StartupInfoA lpStartupInfo, ProcessInformation pInfo)
        {
            var createProcessAHookerImplementation = new CreateProcessAHookerImplementation<T>(null);
            return createProcessAHookerImplementation.CreateProcessHandler(lpApplicationName, lpCommandLine,
                lpProcessAttributes,
                lpThreadAttributes, bInheritHandles, dwCreationFlags, lpEnvironment,
                lpCurrentDirectory, lpStartupInfo, pInfo);
        }

        public bool CreateProcessHandler(string lpApplicationName, string lpCommandLine, IntPtr lpProcessAttributes,
                IntPtr lpThreadAttributes, bool bInheritHandles, uint dwCreationFlags, IntPtr lpEnvironment,
                string lpCurrentDirectory, StartupInfoA lpStartupInfo, ProcessInformation pInfo)
            {
            var processHook = false;
            var parameters = new object[]
            {
                lpApplicationName, lpCommandLine, lpProcessAttributes,
                lpThreadAttributes, bInheritHandles, dwCreationFlags, lpEnvironment, lpCurrentDirectory, lpStartupInfo,
                pInfo
            };
            if (_t == null)
            {
                processHook = (bool)RemoteHookerBase.CallMethodAndNotifyHookerStatic(parameters);
            }
            else
            {
                processHook = (bool)_t.CallMethodAndNotifyHooker(parameters);
            }
            var threadHandle = ((ProcessInformation) parameters[parameters.Length - 1]).HThread;
            CreateProcessHookLib.Win32.Win32Interop.ResumeThread(threadHandle);
            return processHook;
        }
    }
}