using System;
using System.Runtime.InteropServices;
using EasyHookLib.Win32.Model;

namespace EasyHookLib.Delegates
{
    [UnmanagedFunctionPointer(CallingConvention.StdCall,
        CharSet = CharSet.Ansi,
        SetLastError = true)]
    public delegate bool CreateProcessDelegateA([MarshalAs(UnmanagedType.LPStr)]string lpApplicationName, [MarshalAs(UnmanagedType.LPStr)] string lpCommandLine,
        IntPtr lpProcessAttributes, IntPtr lpThreadAttributes,
        bool bInheritHandles, uint dwCreationFlags, IntPtr lpEnvironment,
        [MarshalAs(UnmanagedType.LPStr)]string lpCurrentDirectory, ref StartupInfoA lpStartupInfo, ref ProcessInformation pInfo);
}