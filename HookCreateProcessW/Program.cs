using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using CreateProcessHookLib.Win32;
using CreateProcessHookLib.Win32.Model;
using CreateProcessHookLib.Win32.ModelW;
using EasyHook;
using EasyHookLib.Model;
using EasyHookLib.RemoteInjection;
using EasyHookLib.Utility;
using UtilityLibrary.Configuration;
using UtilityLibrary.Configuration.Arguments;

namespace HookCreateProcessW
{
    public class Program
    {
        private static bool _noHook;
        private static readonly string _dllWithHook = "CreateProcessWHookLib.dll";

        public static void CreateProcessWHooker_ProcessCreated(object sender, HookedEventArgs e)
        {
            var processId = Convert.ToInt32(e.Entries["DwProcessId"]);
            Console.WriteLine($"Process ID (PID): {processId}");
            Console.WriteLine($"Process Handle: {e.Entries["HProcess"]}");
            Console.WriteLine("Process Thread : " + e.Entries["HThread"]);
            IntPtr threadHandle = (IntPtr)e.Entries["HThread"];
            var dllToInject = _dllWithHook;
            var notifyClient = new NotifyClient();
            string channelName;
            var formattableString =
                RemoteInjector.InjectDll(dllToInject, "", ref processId, out channelName, notifyClient);
            if (!string.IsNullOrEmpty(formattableString))
            {
                Console.WriteLine(formattableString);
            }
        }

        private static void DoCreateProcessW(string exeToLaunch, string exeArguments)
        {
            var si = new StartupInfoW();
            var pi = new ProcessInformation();
            bool processW = Win32WInterop.CreateProcessW(exeToLaunch, exeArguments, IntPtr.Zero, IntPtr.Zero,
                false, 0, IntPtr.Zero,
                null, ref si, ref pi);
            Debug.Assert(processW, "Error creating process");
        }

        public static void Main(string[] args)
        {
            var exeToLaunch = ArgumentGetter.Instance.GetValueFromArguments(args, "ExeToLaunch");
            exeToLaunch = string.Format(exeToLaunch, Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
#if DEBUG
                "Debug"
#else
                "Release"
#endif
                );
            var exeArguments = ArgumentGetter.Instance.GetValueFromArguments(args, "ExeArguments");
            LocalHook hook = null;
            _noHook = ArgumentGetter.Instance.GetValueFromArguments(args, "NoHook") == "true";
            if (!_noHook)
            {
                object createProcessWHooker;
                var type = EventHandlerInjector.AttachHandlerToEventDynamically(_dllWithHook, "CreateProcessWHooker", "MethodHookedEvent", typeof(Program),
                    "CreateProcessWHooker_ProcessCreated", null, out createProcessWHooker);
                var memberInfo = type.GetMethod("CreateHook");
                Debug.Assert(memberInfo != null, "memberInfo != null");
                hook = (LocalHook) memberInfo.Invoke(createProcessWHooker, new object[0]);
            }

            DoCreateProcessW(exeToLaunch, exeArguments);

            Console.Write("\nPress <enter> to uninstall hook and exit.");
            Console.ReadLine();
            hook?.Dispose();
            Console.ReadLine();
        }
    }
}