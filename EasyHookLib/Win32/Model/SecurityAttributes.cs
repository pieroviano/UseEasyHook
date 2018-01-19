using System;

namespace EasyHookLib.Win32.Model
{
    public struct SecurityAttributes
    {
        public int length;
        public IntPtr lpSecurityDescriptor;
        public bool bInheritHandle;
    }
}