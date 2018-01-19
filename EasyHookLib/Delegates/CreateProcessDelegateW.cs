using System;
using System.Runtime.InteropServices;
using EasyHookLib.Win32.Model;

namespace EasyHookLib.Delegates
{
    [UnmanagedFunctionPointer(CallingConvention.StdCall,
        CharSet = CharSet.Unicode,
        SetLastError = true)]
    public delegate bool CreateProcessDelegateW([MarshalAs(UnmanagedType.LPWStr)]string lpApplicationName, [MarshalAs(UnmanagedType.LPWStr)] string lpCommandLine,
        IntPtr lpProcessAttributes, IntPtr lpThreadAttributes,
        bool bInheritHandles, uint dwCreationFlags, IntPtr lpEnvironment,
        [MarshalAs(UnmanagedType.LPWStr)]string lpCurrentDirectory, ref StartupInfoW lpStartupInfo, ref ProcessInformation pInfo);
}