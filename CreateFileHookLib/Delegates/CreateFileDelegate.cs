using System;
using System.Runtime.InteropServices;

namespace CreateFileHookLib.Delegates
{
    [UnmanagedFunctionPointer(CallingConvention.StdCall,
        CharSet = CharSet.Unicode,
        SetLastError = true)]
    public delegate IntPtr CreateFileDelegate(string inFileName, uint inDesiredAccess, uint inShareMode,
        IntPtr inSecurityAttributes, uint inCreationDisposition, uint inFlagsAndAttributes, IntPtr inTemplateFile);
}