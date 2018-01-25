using System;
using CreateProcessAHookLib.Delegates;
using CreateProcessHookLib.Win32;
using CreateProcessHookLib.Win32.Model;
using CreateProcessHookLib.Win32.ModelA;
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
                new Tuple<string, object>("HProcess", pInfo.HProcess),
                new Tuple<string, object>("HThread", pInfo.HThread)
            };
            return processHook;
        }

        public LocalHook CreateHook(T This, Delegate @delegate)
        {
            return _t.HookMethod("kernel32.dll", "CreateProcessA",
                @delegate == null
                    ? new CreateProcessADelegate(CreateProcessAHookerImplementation<CreateProcessARemoteHooker>
                        .CreateProcessHandlerStatic)
                    : @delegate, This);
        }

        public bool CreateProcessHandler(string lpApplicationName, string lpCommandLine, IntPtr lpProcessAttributes,
            IntPtr lpThreadAttributes, bool bInheritHandles, uint dwCreationFlags, IntPtr lpEnvironment,
            string lpCurrentDirectory, ref StartupInfoA lpStartupInfo, ref ProcessInformation pInfo)
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
                processHook = (bool) RemoteHookerBase.CallMethodAndNotifyHookerStatic(parameters);
            }
            else
            {
                processHook = (bool) _t.CallMethodAndNotifyHooker(parameters);
            }
            lpStartupInfo = (StartupInfoA) parameters[parameters.Length - 2];
            pInfo = (ProcessInformation) parameters[parameters.Length - 1];
            return processHook;
        }

        public static bool CreateProcessHandlerStatic(string lpApplicationName, string lpCommandLine,
            IntPtr lpProcessAttributes,
            IntPtr lpThreadAttributes, bool bInheritHandles, uint dwCreationFlags, IntPtr lpEnvironment,
            string lpCurrentDirectory, ref StartupInfoA lpStartupInfo, ref ProcessInformation pInfo)
        {
            var createProcessAHookerImplementation = new CreateProcessAHookerImplementation<T>(null);
            return createProcessAHookerImplementation.CreateProcessHandler(lpApplicationName, lpCommandLine,
                lpProcessAttributes,
                lpThreadAttributes, bInheritHandles, dwCreationFlags, lpEnvironment,
                lpCurrentDirectory, ref lpStartupInfo, ref pInfo);
        }
    }
}