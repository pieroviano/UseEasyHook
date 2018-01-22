using System;
using CreateFileHookLib.Delegates;
using CreateFileHookLib.Win32;
using EasyHook;
using EasyHookLib.Hooking;

namespace CreateFileHookLib
{
    internal class CreateFileHookerImplementation<T> where T: HookerBase
    {
        private T _t;

        public CreateFileHookerImplementation(T t)
        {
            _t = t;
        }

        public object CallMethod(object[] parameters, out Tuple<string, object>[] tuplesForNotification)
        {
            var fileHooked = Win32Interop.CreateFile(
                (string)parameters[0],
                (uint)parameters[1],
                (uint)parameters[2],
                (IntPtr)parameters[3],
                (uint)parameters[4],
                (uint)parameters[5],
                (IntPtr)parameters[6]);
            tuplesForNotification = new[]
            {
                new Tuple<string, object>("FileName", (string) parameters[0])
            };
            return fileHooked;
        }

        public static IntPtr CreateFileHandlerStatic(string inFileName, uint inDesiredAccess, uint inShareMode,
            IntPtr inSecurityAttributes, uint inCreationDisposition, uint inFlagsAndAttributes, IntPtr inTemplateFile)
        {
            var createFileHookerImplementation = new CreateFileHookerImplementation<T>(null);
            return createFileHookerImplementation.CreateFileHandler(inFileName, inDesiredAccess, inShareMode,
                inSecurityAttributes, inCreationDisposition, inFlagsAndAttributes, inTemplateFile);
        }

        public IntPtr CreateFileHandler(string inFileName, uint inDesiredAccess, uint inShareMode,
            IntPtr inSecurityAttributes, uint inCreationDisposition, uint inFlagsAndAttributes, IntPtr inTemplateFile)
        {
            var parameters = new object[]
            {
                inFileName, inDesiredAccess, inShareMode,
                inSecurityAttributes, inCreationDisposition, inFlagsAndAttributes, inTemplateFile
            };
            IntPtr processHook;
            if (_t == null)
            {
                processHook = (IntPtr)RemoteHookerBase.CallMethodAndNotifyHookerStatic(parameters);
            }
            else
            {
                processHook = (IntPtr)_t.CallMethodAndNotifyHooker(parameters);
            }
            return processHook;
        }

        public LocalHook CreateHook(T This, Delegate @delegate)
        {
            return _t.HookMethod("kernel32.dll", "CreateFileW",
                @delegate==null?new CreateFileDelegate(CreateFileHookerImplementation<CreateFileRemoteHooker>.CreateFileHandlerStatic):@delegate, This);
        }
    }
}