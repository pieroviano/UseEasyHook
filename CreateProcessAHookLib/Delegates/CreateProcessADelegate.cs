using System;
using System.Runtime.InteropServices;
using CreateProcessHookLib.Win32.Model;
using CreateProcessHookLib.Win32.ModelA;

namespace CreateProcessAHookLib.Delegates
{
    [UnmanagedFunctionPointer(CallingConvention.StdCall,
        CharSet = CharSet.Ansi,
        SetLastError = true)]
    public delegate bool CreateProcessADelegate([MarshalAs(UnmanagedType.LPStr)]string lpApplicationName, [MarshalAs(UnmanagedType.LPStr)] string lpCommandLine,
        IntPtr lpProcessAttributes, IntPtr lpThreadAttributes,
        bool bInheritHandles, uint dwCreationFlags, IntPtr lpEnvironment,
        [MarshalAs(UnmanagedType.LPStr)]string lpCurrentDirectory, ref StartupInfoA lpStartupInfo, ref ProcessInformation pInfo);
}