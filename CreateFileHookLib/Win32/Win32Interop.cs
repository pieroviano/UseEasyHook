using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace CreateFileHookLib.Win32
{
    public class Win32Interop
    {
        [DllImport("kernel32.dll",
            CharSet = CharSet.Unicode,
            SetLastError = true,
            CallingConvention = CallingConvention.StdCall)]
        public static extern IntPtr CreateFile(string inFileName, uint inDesiredAccess, uint inShareMode,
            IntPtr inSecurityAttributes, uint inCreationDisposition, uint inFlagsAndAttributes, IntPtr inTemplateFile);

    }
}
