using System;
using System.Runtime.InteropServices;
using CreateWindowHookLib.Win32.Model;

namespace CreateWindowHookLib.Delegates
{
    public delegate IntPtr CreateWindowExDelegate(
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
        IntPtr lpParam);
}