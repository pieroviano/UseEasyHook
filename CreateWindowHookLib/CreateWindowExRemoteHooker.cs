using System;
using CreateWindowHookLib.Delegates;
using CreateWindowHookLib.Win32.Model;
using EasyHook;
using EasyHookLib.Hooking;

namespace CreateWindowHookLib
{
    public class CreateWindowExRemoteHooker : RemoteHookerBase
    {
        private readonly CreateWindowWxHookerImplementation<CreateWindowExRemoteHooker>
            _createWindowExHookerImplementation;

        public CreateWindowExRemoteHooker(
            RemoteHooking.IContext inContext,
            string inChannelName) : base(inContext, inChannelName)
        {
            _createWindowExHookerImplementation =
                new CreateWindowWxHookerImplementation<CreateWindowExRemoteHooker>(this);
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

        public static IntPtr CreateWindowExHandler(WindowStylesEx dwExStyle,
            string lpClassName,
            string lpWindowName,
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