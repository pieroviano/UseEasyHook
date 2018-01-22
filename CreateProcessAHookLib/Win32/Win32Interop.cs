using System;
using System.Runtime.InteropServices;
using CreateProcessAHookLib.Win32.Model;
using CreateProcessHookLib.Win32.Model;

namespace CreateProcessHookALib.Win32
{
    public static class Win32Interop
    {
        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Ansi, EntryPoint = "CreateProcessA")]
        public static extern bool CreateProcessA(
            string lpApplicationName,
            string lpCommandLine,
            IntPtr lpProcessAttributes,
            IntPtr lpThreadAttributes,
            bool bInheritHandles,
            uint dwCreationFlags,
            IntPtr lpEnvironment,
            string lpCurrentDirectory,
            [In] ref StartupInfoA lpStartupInfo,
            ref ProcessInformation lpProcessInformation);

    }
}