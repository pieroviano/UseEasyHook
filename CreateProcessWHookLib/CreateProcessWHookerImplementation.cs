using System;
using CreateProcessHookLib.Win32;
using CreateProcessHookLib.Win32.Model;
using CreateProcessWHookLib.Delegates;
using CreateProcessWHookLib.Win32.Model;
using EasyHook;
using EasyHookLib.Hooking;
using Win32WInterop = CreateProcessWHookLib.Win32.Win32WInterop;

namespace CreateProcessWHookLib
{
    internal class CreateProcessWHookerImplementation<T> where T : HookerBase
    {
        private readonly T _t;

        public CreateProcessWHookerImplementation(T t)
        {
            _t = t;
        }


        public object CallMethod(object[] parameters, out Tuple<string, object>[] tuplesForNotification)
        {
            var lpStartupInfo = (StartupInfoW) parameters[8];
            var pInfo = (ProcessInformation) parameters[9];
            var processHook = Win32WInterop.CreateProcessW((string) parameters[0], (string) parameters[1],
                (IntPtr) parameters[2], (IntPtr) parameters[3], (bool) parameters[4],
                (uint) parameters[5] | (uint) ProcessCreationFlags.CreateSuspended,
                (IntPtr) parameters[6], (string) parameters[7], ref lpStartupInfo, ref pInfo);
            parameters[8] = lpStartupInfo;
            parameters[9] = pInfo;
            tuplesForNotification = new[]
            {
                new Tuple<string, object>("DwProcessId", pInfo.DwProcessId),
                new Tuple<string, object>("HProcess", pInfo.HProcess),
                new Tuple<string, object>("HThread", pInfo.HThread)
            };
            return processHook;
        }

        public LocalHook CreateHook(T This, Delegate @delegate)
        {
            return _t.HookMethod("kernel32.dll", "CreateProcessW",
                @delegate == null
                    ? new CreateProcessWDelegate(CreateProcessWHookerImplementation<CreateProcessWRemoteHooker>
                        .CreateProcessHandlerStatic)
                    : @delegate, This);
        }


        public bool CreateProcessHandler(string lpApplicationName, string lpCommandLine, IntPtr lpProcessAttributes,
            IntPtr lpThreadAttributes, bool bInheritHandles, uint dwCreationFlags, IntPtr lpEnvironment,
            string lpCurrentDirectory, ref StartupInfoW lpStartupInfo, ref ProcessInformation pInfo)
        {
            var processHook = false;
            var parameters = new object[]
            {
                lpApplicationName, lpCommandLine, lpProcessAttributes,
                lpThreadAttributes, bInheritHandles, dwCreationFlags, lpEnvironment, lpCurrentDirectory, lpStartupInfo,
                pInfo
            };
            if (_t != null)
            {
                processHook = (bool) _t.CallMethodAndNotifyHooker(parameters);
            }
            else
            {
                processHook = (bool) RemoteHookerBase.CallMethodAndNotifyHookerStatic(parameters);
            }
            lpStartupInfo = (StartupInfoW) parameters[parameters.Length - 2];
            pInfo = (ProcessInformation) parameters[parameters.Length - 1];
            Win32Utility.ResumeThread(pInfo.HThread);
            return processHook;
        }

        public static bool CreateProcessHandlerStatic(string lpApplicationName, string lpCommandLine,
            IntPtr lpProcessAttributes,
            IntPtr lpThreadAttributes, bool bInheritHandles, uint dwCreationFlags, IntPtr lpEnvironment,
            string lpCurrentDirectory, ref StartupInfoW lpStartupInfo, ref ProcessInformation pInfo)
        {
            var createProcessAHookerImplementation = new CreateProcessWHookerImplementation<T>(null);
            return createProcessAHookerImplementation.CreateProcessHandler(lpApplicationName, lpCommandLine,
                lpProcessAttributes,
                lpThreadAttributes, bInheritHandles, dwCreationFlags, lpEnvironment,
                lpCurrentDirectory, ref lpStartupInfo, ref pInfo);
        }

        public bool CreateProcessHookStatic(string lpApplicationName, string lpCommandLine, IntPtr lpProcessAttributes,
            IntPtr lpThreadAttributes, bool bInheritHandles, uint dwCreationFlags, IntPtr lpEnvironment,
            string lpCurrentDirectory, ref StartupInfoW lpStartupInfo, ref ProcessInformation pInfo, T This)
        {
            var createProcessWHookerImplementation = new CreateProcessWHookerImplementation<T>(null);
            return createProcessWHookerImplementation.CreateProcessHandler(lpApplicationName, lpCommandLine,
                lpProcessAttributes,
                lpThreadAttributes, bInheritHandles, dwCreationFlags, lpEnvironment,
                lpCurrentDirectory, ref lpStartupInfo, ref pInfo);
        }
    }
}