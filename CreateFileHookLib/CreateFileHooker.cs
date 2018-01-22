using System;
using CreateFileHookLib.Delegates;
using EasyHook;
using EasyHookLib.Hooking;

namespace CreateFileHookLib
{
    public class CreateFileHooker : HookerBase
    {
        private readonly CreateFileHookerImplementation<CreateFileHooker> _createFileHookerImplementation;

        public CreateFileHooker()
        {
            _createFileHookerImplementation = new CreateFileHookerImplementation<CreateFileHooker>(this);
        }

        protected override object CallMethod(object[] parameters, out Tuple<string, object>[] tuplesForNotification)
        {
            return _createFileHookerImplementation.CallMethod(parameters, out tuplesForNotification);
        }

        public override LocalHook CreateHook()
        {
            return _createFileHookerImplementation.CreateHook(null, new CreateFileDelegate(CreateFileHandler));
        }

        public IntPtr CreateFileHandler(string inFileName, uint inDesiredAccess, uint inShareMode,
            IntPtr inSecurityAttributes, uint inCreationDisposition, uint inFlagsAndAttributes, IntPtr inTemplateFile)

        {
            return CreateFileHookerImplementation<CreateFileRemoteHooker>.CreateFileHandlerStatic(inFileName,
                inDesiredAccess, inShareMode,
                inSecurityAttributes, inCreationDisposition, inFlagsAndAttributes, inTemplateFile);
        }
    }
}