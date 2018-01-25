using System;
using CreateProcessHookLib.Win32.Model;
using CreateProcessWHookLib.Delegates;
using CreateProcessWHookLib.Win32.Model;
using EasyHook;
using EasyHookLib.Hooking;

namespace CreateProcessWHookLib
{
    public class CreateProcessWHooker : HookerBase
    {
        private readonly CreateProcessWHookerImplementation<CreateProcessWHooker> _createProcessWHookerImplementation;

        public CreateProcessWHooker()
        {
            _createProcessWHookerImplementation = new CreateProcessWHookerImplementation<CreateProcessWHooker>(this);
        }

        protected override object CallMethod(object[] parameters, out Tuple<string, object>[] tuplesForNotification)
        {
            return _createProcessWHookerImplementation.CallMethod(parameters, out tuplesForNotification);
        }

        public override LocalHook CreateHook()
        {
            return _createProcessWHookerImplementation.CreateHook(null, new CreateProcessWDelegate(CreateProcessHandler));
        }

        public bool CreateProcessHandler(string lpApplicationName, string lpCommandLine,
            IntPtr lpProcessAttributes, IntPtr lpThreadAttributes,
            bool bInheritHandles, uint dwCreationFlags, IntPtr lpEnvironment,
            string lpCurrentDirectory, ref StartupInfoW lpStartupInfo, ref ProcessInformation pInfo)
        {
            return _createProcessWHookerImplementation.CreateProcessHandler(lpApplicationName, lpCommandLine,
                lpProcessAttributes, lpThreadAttributes, bInheritHandles, dwCreationFlags, lpEnvironment,
                lpCurrentDirectory, ref lpStartupInfo, ref pInfo);
        }
    }
}