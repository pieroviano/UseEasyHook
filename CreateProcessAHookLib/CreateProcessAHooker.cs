using System;
using CreateProcessAHookLib.Delegates;
using CreateProcessHookLib.Win32.Model;
using CreateProcessHookLib.Win32.ModelA;
using EasyHook;
using EasyHookLib.Hooking;

namespace CreateProcessAHookLib
{
    public class CreateProcessAHooker : HookerBase
    {
        private readonly CreateProcessAHookerImplementation<CreateProcessAHooker> _createProcessAHookerImplementation;

        public CreateProcessAHooker()
        {
            _createProcessAHookerImplementation = new CreateProcessAHookerImplementation<CreateProcessAHooker>(this);
        }

        protected override object CallMethod(object[] parameters, out Tuple<string, object>[]tuplesForNotification)
        {
            return _createProcessAHookerImplementation.CallMethod(parameters, out tuplesForNotification);
        }

        public override LocalHook CreateHook()
        {
            return _createProcessAHookerImplementation.CreateHook(null, new CreateProcessADelegate(CreateProcessHandler));
        }

        public bool CreateProcessHandler(string lpApplicationName, string lpCommandLine,
            IntPtr lpProcessAttributes, IntPtr lpThreadAttributes,
            bool bInheritHandles, uint dwCreationFlags, IntPtr lpEnvironment,
            string lpCurrentDirectory, ref StartupInfoA lpStartupInfo, ref ProcessInformation pInfo)
        {
            return _createProcessAHookerImplementation.CreateProcessHandler(lpApplicationName, lpCommandLine,
                lpProcessAttributes, lpThreadAttributes, bInheritHandles, dwCreationFlags, lpEnvironment,
                lpCurrentDirectory, ref lpStartupInfo, ref pInfo);
        }
    }
}