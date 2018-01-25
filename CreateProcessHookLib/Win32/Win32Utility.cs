using System;
using System.Runtime.InteropServices;
using CreateProcessHookLib.Win32.Model;

namespace CreateProcessHookLib.Win32
{
    public static class Win32Utility
    {

        [DllImport("kernel32.dll")]
        public static extern uint ResumeThread(IntPtr hThread);
    }
}