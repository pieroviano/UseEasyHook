using System;

namespace CreateProcessHookLib.Win32.Model
{
    public struct SecurityAttributes
    {
        public int length;
        public IntPtr lpSecurityDescriptor;
        public bool bInheritHandle;
    }
}