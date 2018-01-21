using System;
using CreateFileHookLib.Win32;
using EasyHook;
using EasyHookLib.Hooking;
using CreateFileDelegate = CreateFileHookLib.Delegates.CreateFileDelegate;

namespace CreateFileHookLib
{
    public class CreateFileRemoteHooker : RemoteHookerBase
    {
        public CreateFileRemoteHooker(
            RemoteHooking.IContext inContext,
            string inChannelName) : base(inContext, inChannelName)
        {
        }

        protected override object CallMethod(object[] parameters, out Tuple<string, object>[] tuplesForNotification)
        {
            var fileHooked = Win32Interop.CreateFile(
                (string) parameters[0],
                (uint) parameters[1],
                (uint) parameters[2],
                (IntPtr) parameters[3],
                (uint) parameters[4],
                (uint) parameters[5],
                (IntPtr) parameters[6]);
            tuplesForNotification = new[]
            {
                new Tuple<string, object>("FileName", (string) parameters[0])
            };
            return fileHooked;
        }

        public static IntPtr CreateFile_Handler(string inFileName, uint inDesiredAccess, uint inShareMode,
            IntPtr inSecurityAttributes, uint inCreationDisposition, uint inFlagsAndAttributes, IntPtr inTemplateFile)

        {
            var parameters = new object[]
            {
                inFileName, inDesiredAccess, inShareMode,
                inSecurityAttributes, inCreationDisposition, inFlagsAndAttributes, inTemplateFile
            };
            var processHook = (IntPtr) CallMethodAndNotifyHookerStatic(parameters);
            return processHook;
        }

        public override LocalHook CreateHook()
        {
            var hook = LocalHook.Create(
                LocalHook.GetProcAddress("kernel32.dll", "CreateFileW"),
                new CreateFileDelegate(CreateFile_Handler),
                this);

            hook.ThreadACL.SetExclusiveACL(new Int32[] { 0 });
            return hook;
        }
    }
}