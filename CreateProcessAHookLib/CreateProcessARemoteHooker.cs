using System;
using CreateProcessAHookLib.Win32.Model;
using CreateProcessHookLib.Win32.Model;
using EasyHook;
using EasyHookLib.Hooking;

namespace CreateProcessAHookLib
{
    public class CreateProcessARemoteHooker : RemoteHookerBase
    {
        private readonly CreateProcessAHookerImplementation<CreateProcessARemoteHooker>
            _createProcessAHookerImplementation;

        public CreateProcessARemoteHooker(
            RemoteHooking.IContext inContext,
            string inChannelName) : base(inContext, inChannelName)
        {
            _createProcessAHookerImplementation =
                new CreateProcessAHookerImplementation<CreateProcessARemoteHooker>(this);
        }

        protected override object CallMethod(object[] parameters, out Tuple<string, object>[] tuplesForNotification)
        {
            return _createProcessAHookerImplementation.CallMethod(parameters, out tuplesForNotification);
        }

        public override LocalHook CreateHook()
        {
            return _createProcessAHookerImplementation.CreateHook(this, null);
        }

        public bool CreateProcessHandler(string lpApplicationName, string lpCommandLine,
            IntPtr lpProcessAttributes, IntPtr lpThreadAttributes,
            bool bInheritHandles, uint dwCreationFlags, IntPtr lpEnvironment,
            string lpCurrentDirectory, ref StartupInfoA lpStartupInfo, ref ProcessInformation pInfo)
        {
            return CreateProcessAHookerImplementation<CreateProcessARemoteHooker>.CreateProcessHandlerStatic(
                lpApplicationName, lpCommandLine,
                lpProcessAttributes, lpThreadAttributes, bInheritHandles, dwCreationFlags, lpEnvironment,
                lpCurrentDirectory, lpStartupInfo, pInfo);
        }
    }
}