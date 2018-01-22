using System;
using System.Runtime.InteropServices;
using CreateProcessHookLib.Win32.Model;
using CreateProcessWHookLib.Win32.Model;

namespace CreateProcessWHookLib.Delegates
{
    [UnmanagedFunctionPointer(CallingConvention.StdCall,
        CharSet = CharSet.Unicode,
        SetLastError = true)]
    public delegate bool CreateProcessWDelegate([MarshalAs(UnmanagedType.LPWStr)]string lpApplicationName, [MarshalAs(UnmanagedType.LPWStr)] string lpCommandLine,
        IntPtr lpProcessAttributes, IntPtr lpThreadAttributes,
        bool bInheritHandles, uint dwCreationFlags, IntPtr lpEnvironment,
        [MarshalAs(UnmanagedType.LPWStr)]string lpCurrentDirectory, ref StartupInfoW lpStartupInfo, ref ProcessInformation pInfo);
}