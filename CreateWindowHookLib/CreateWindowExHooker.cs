using System;
using System.Runtime.InteropServices;
using CreateWindowHookLib.Delegates;
using CreateWindowHookLib.Win32.Model;
using EasyHook;
using EasyHookLib.Hooking;

namespace CreateWindowHookLib
{
    public class CreateWindowExHooker : HookerBase
    {
        private readonly CreateWindowWxHookerImplementation<CreateWindowExHooker> _createWindowExHookerImplementation;

        public CreateWindowExHooker()
        {
            _createWindowExHookerImplementation = new CreateWindowWxHookerImplementation<CreateWindowExHooker>(this);
        }

        protected override object CallMethod(object[] parameters, out Tuple<string, object>[] tuplesForNotification)
        {
            return _createWindowExHookerImplementation.CallMethod(parameters, out tuplesForNotification);
        }

        public override LocalHook CreateHook()
        {
            return _createWindowExHookerImplementation.CreateHook(null,
                new CreateWindowExDelegate(CreateWindowExHandler));
        }

        public IntPtr CreateWindowExHandler(
            WindowStylesEx dwExStyle,
            [MarshalAs(UnmanagedType.LPStr)] string lpClassName,
            [MarshalAs(UnmanagedType.LPStr)] string lpWindowName,
            WindowStyles dwStyle,
            int x,
            int y,
            int nWidth,
            int nHeight,
            IntPtr hWndParent,
            IntPtr hMenu,
            IntPtr hInstance,
            IntPtr lpParam)

        {
            return CreateWindowWxHookerImplementation<CreateWindowExRemoteHooker>.CreateWindowExHandlerStatic(dwExStyle,
                lpClassName,
                lpWindowName,
                dwStyle,
                x,
                y,
                nWidth,
                nHeight,
                hWndParent,
                hMenu,
                hInstance,
                lpParam);
        }
    }
}