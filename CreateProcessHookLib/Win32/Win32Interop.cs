using System;
using System.Runtime.InteropServices;
using CreateProcessHookLib.Win32.Model;

namespace CreateProcessHookLib.Win32
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

        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode, EntryPoint = "CreateProcessW")]
        public static extern bool CreateProcessW(
            string lpApplicationName,
            string lpCommandLine,
            IntPtr lpProcessAttributes,
            IntPtr lpThreadAttributes,
            bool bInheritHandles,
            uint dwCreationFlags,
            IntPtr lpEnvironment,
            string lpCurrentDirectory,
            [In] ref StartupInfoW lpStartupInfo,
            ref ProcessInformation lpProcessInformation);

        [DllImport("kernel32.dll")]
        public static extern uint ResumeThread(IntPtr hThread);
    }
}