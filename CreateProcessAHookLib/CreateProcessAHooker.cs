using System;
using CreateProcessHookALib.Delegates;
using CreateProcessHookLib.Win32;
using CreateProcessHookLib.Win32.Model;
using EasyHook;
using EasyHookLib.Hooking;

namespace CreateProcessHookALib
{
    public class CreateProcessAHooker : HookerBase
    {

        protected override object CallMethod(object[] parameters, out Tuple<string, object>[]tuplesForNotification)
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

        public override LocalHook CreateHook()
        {
            var hook = LocalHook.Create(
                LocalHook.GetProcAddress("kernel32.dll", "CreateProcessA"),
                new CreateProcessDelegateA(CreateProcessHook),
                null);

            hook.ThreadACL.SetExclusiveACL(new Int32[] { 0 });
            return hook;
        }

        /// <summary>
        ///     Our MessageBeep hook handler
        /// </summary>
        public bool CreateProcessHook(string lpApplicationName, string lpCommandLine,
            IntPtr lpProcessAttributes, IntPtr lpThreadAttributes,
            bool bInheritHandles, uint dwCreationFlags, IntPtr lpEnvironment,
            string lpCurrentDirectory, ref StartupInfoA lpStartupInfo, ref ProcessInformation pInfo)
        {
            var processHook = false;
            var parameters = new object[]{lpApplicationName, lpCommandLine, lpProcessAttributes,
                lpThreadAttributes, bInheritHandles, dwCreationFlags, lpEnvironment, lpCurrentDirectory, lpStartupInfo,
                pInfo};
            processHook = (bool)CallMethodAndNotifyHooker(parameters);
            var threadHandle = ((ProcessInformation)parameters[parameters.Length - 1]).HThread;
            Win32Interop.ResumeThread(threadHandle);
            return processHook;
        }
    }
}