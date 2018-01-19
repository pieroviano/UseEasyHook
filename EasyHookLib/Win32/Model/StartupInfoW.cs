using System;
using System.Runtime.InteropServices;

namespace EasyHookLib.Win32.Model
{
    public struct StartupInfoW
    {
        public uint Cb;
        [MarshalAs(UnmanagedType.LPWStr)] public string LpReserved;
        [MarshalAs(UnmanagedType.LPWStr)] public string LpDesktop;
        [MarshalAs(UnmanagedType.LPWStr)] public string LpTitle;
        public uint DwX;
        public uint DwY;
        public uint DwXSize;
        public uint DwYSize;
        public uint DwXCountChars;
        public uint DwYCountChars;
        public uint DwFillAttribute;
        public uint DwFlags;
        public short WShowWindow;
        public short CbReserved2;
        public IntPtr LpReserved2;
        public IntPtr HStdInput;
        public IntPtr HStdOutput;
        public IntPtr HStdError;
    }
}