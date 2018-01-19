using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Remoting;
using System.Text;
using System.Threading.Tasks;
using EasyHook;

namespace EasyHookLib.RemoteInjection
{
    public static class RemoteInjector
    {
        public static string InjectDll(string dllToInject, string targetExe, int targetPid, out string channelName)
        {
            channelName = null;
            RemoteHooking.IpcCreateServer<PostbackMessageHandler>(ref channelName, WellKnownObjectMode.SingleCall);

            var location = Assembly.GetExecutingAssembly().Location;
            var directoryName = Path.GetDirectoryName(location);
            var directoryIsNotNull = directoryName != null;
            string formattableString = null;
            if (directoryIsNotNull)
            {
                var injectionLibrary = Path.Combine(directoryName, dllToInject);
                if (File.Exists(injectionLibrary))
                {
                    if (string.IsNullOrEmpty(targetExe))
                    {
                        RemoteHooking.Inject(
                            targetPid,
                            injectionLibrary,
                            injectionLibrary,
                            channelName);

                        formattableString = $"Injected to process {targetPid}";
                    }
                    else if (File.Exists(targetExe))
                    {
                        RemoteHooking.CreateAndInject(targetExe, "", 0, InjectionOptions.DoNotRequireStrongName,
                            injectionLibrary, injectionLibrary, out targetPid, channelName);
                        formattableString = $"Created and injected process {targetPid}";
                    }
                }
            }
            return formattableString;
        }
    }
}
