using System;
using CreateFileHookLib.Delegates;
using EasyHook;
using EasyHookLib.Hooking;

namespace CreateFileHookLib
{
    public class CreateFileRemoteHooker : RemoteHookerBase
    {
        private readonly CreateFileHookerImplementation<CreateFileRemoteHooker> _createFileHookerImplementation;

        public CreateFileRemoteHooker(
            RemoteHooking.IContext inContext,
            string inChannelName) : base(inContext, inChannelName)
        {
            _createFileHookerImplementation = new CreateFileHookerImplementation<CreateFileRemoteHooker>(this);
        }

        protected override object CallMethod(object[] parameters, out Tuple<string, object>[] tuplesForNotification)
        {
            return _createFileHookerImplementation.CallMethod(parameters, out tuplesForNotification);
        }

        public static IntPtr CreateFileHandler(string inFileName, uint inDesiredAccess, uint inShareMode,
            IntPtr inSecurityAttributes, uint inCreationDisposition, uint inFlagsAndAttributes, IntPtr inTemplateFile)

        {
            return CreateFileHookerImplementation<CreateFileRemoteHooker>.CreateFileHandlerStatic(inFileName,
                inDesiredAccess, inShareMode,
                inSecurityAttributes, inCreationDisposition, inFlagsAndAttributes, inTemplateFile);
        }

        public override LocalHook CreateHook()
        {
            return _createFileHookerImplementation.CreateHook(null, new CreateFileDelegate(CreateFileHandler));
        }
    }
}