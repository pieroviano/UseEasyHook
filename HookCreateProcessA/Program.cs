using System;
using CreateProcessAHookLib;
using CreateProcessHookLib.Win32;
using CreateProcessHookLib.Win32.Model;
using CreateProcessHookLib.Win32.ModelA;
using EasyHookLib.Model;

namespace HookCreateProcessA
{

    public class Program
    {

        private static void DoCreateProcessA()
        {
            var si = new StartupInfoA();
            var pi = new ProcessInformation();
            Win32Interop.CreateProcessA("c:\\windows\\system32\\Notepad.exe","", IntPtr.Zero, IntPtr.Zero, false, 0, IntPtr.Zero,
                null, ref si, ref pi);
        }

        public static void Main()
        {
            var createProcessAHooker = new CreateProcessAHooker();
            createProcessAHooker.MethodHookedEvent += CreateProcessAHooker_ProcessCreated;
            var hookA = createProcessAHooker.CreateHook();

            DoCreateProcessA();

            Console.Write("\nPress <enter> to uninstall hook and exit.");
            Console.ReadLine();
            hookA.Dispose();
            Console.ReadLine();
        }

        private static void CreateProcessAHooker_ProcessCreated(object sender, HookedEventArgs e)
        {
            Console.WriteLine("Process ID (PID): " + e.Entries["DwProcessId"]);
            Console.WriteLine("Process Handle : " + e.Entries["HProcess"]);
            Console.WriteLine("Process Thread : " + e.Entries["HThread"]);
            var threadHandle = (IntPtr)e.Entries["HThread"];
            Win32Utility.ResumeThread(threadHandle);
        }
    }
}