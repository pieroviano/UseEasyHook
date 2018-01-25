using System;
using CreateProcessHookLib.Win32.Model;
using CreateProcessWHookLib.Win32.Model;
using EasyHook;
using EasyHookLib.Hooking;

namespace CreateProcessWHookLib
{
    public class CreateProcessWRemoteHooker : RemoteHookerBase
    {
        private readonly CreateProcessWHookerImplementation<CreateProcessWRemoteHooker> _createProcessWHookerImplementation;

        public CreateProcessWRemoteHooker(
            RemoteHooking.IContext inContext,
            string inChannelName) : base(inContext, inChannelName)
        {
            _createProcessWHookerImplementation = new CreateProcessWHookerImplementation<CreateProcessWRemoteHooker>(this);
        }

        protected override object CallMethod(object[] parameters, out Tuple<string, object>[] tuplesForNotification)
        {
            return _createProcessWHookerImplementation.CallMethod(parameters, out tuplesForNotification);
        }

        public static bool CreateProcessHandler(string lpApplicationName, string lpCommandLine,
            IntPtr lpProcessAttributes, IntPtr lpThreadAttributes,
            bool bInheritHandles, uint dwCreationFlags, IntPtr lpEnvironment,
            string lpCurrentDirectory, ref StartupInfoW lpStartupInfo, ref ProcessInformation pInfo)
        {
            return CreateProcessWHookerImplementation<CreateProcessWRemoteHooker>.CreateProcessHandlerStatic(
                lpApplicationName, lpCommandLine,
                lpProcessAttributes, lpThreadAttributes, bInheritHandles, dwCreationFlags, lpEnvironment,
                lpCurrentDirectory, ref lpStartupInfo, ref pInfo);
        }

        public override LocalHook CreateHook()
        {
            return _createProcessWHookerImplementation.CreateHook(this, null);
        }
    }
}
