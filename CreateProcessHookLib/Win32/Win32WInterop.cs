using System;
using System.Runtime.InteropServices;
using CreateProcessHookLib.Win32.Model;
using CreateProcessHookLib.Win32.ModelW;

namespace CreateProcessHookLib.Win32
{
    public static class Win32WInterop
    {

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

    }
}