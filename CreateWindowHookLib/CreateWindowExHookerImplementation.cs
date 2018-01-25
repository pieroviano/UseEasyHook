using System;
using CreateWindowHookLib.Delegates;
using CreateWindowHookLib.Win32;
using CreateWindowHookLib.Win32.Model;
using EasyHook;
using EasyHookLib.Hooking;

namespace CreateWindowHookLib
{
    internal class CreateWindowWxHookerImplementation<T> where T : HookerBase
    {
        private readonly T _t;

        public CreateWindowWxHookerImplementation(T t)
        {
            _t = t;
        }

        public object CallMethod(object[] parameters, out Tuple<string, object>[] tuplesForNotification)
        {
            var intPtr = Win32Interop.CreateWindowEx(
                (WindowStylesEx) parameters[0],
                (string) parameters[1],
                (string) parameters[2],
                (WindowStyles) parameters[3],
                (int) parameters[4],
                (int) parameters[5],
                (int) parameters[6],
                (int) parameters[7],
                (IntPtr) parameters[8],
                (IntPtr) parameters[9],
                (IntPtr) parameters[10],
                (IntPtr) parameters[11]);
            tuplesForNotification = new[]
            {
                new Tuple<string, object>("HWindow", intPtr)
            };
            return intPtr;
        }

        public LocalHook CreateHook(T This, Delegate @delegate)
        {
            return _t.HookMethod("kernel32.dll", "CreateFileW",
                @delegate == null
                    ? new CreateWindowExDelegate(CreateWindowWxHookerImplementation<CreateWindowExRemoteHooker>
                        .CreateWindowExHandlerStatic)
                    : @delegate, This);
        }

        public IntPtr CreateWindowExHandler(WindowStylesEx dwExStyle,
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
            var parameters = new object[]
            {
                dwExStyle,
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
                lpParam
            };
            IntPtr processHook;
            if (_t == null)
            {
                processHook = (IntPtr) RemoteHookerBase.CallMethodAndNotifyHookerStatic(parameters);
            }
            else
            {
                processHook = (IntPtr) _t.CallMethodAndNotifyHooker(parameters);
            }
            return processHook;
        }

        public static IntPtr CreateWindowExHandlerStatic(
            WindowStylesEx dwExStyle,
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
            var createWindowExHookerImplementation = new CreateWindowWxHookerImplementation<T>(null);
            return createWindowExHookerImplementation.CreateWindowExHandler(dwExStyle,
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