using System;
using CreateProcessHookALib;
using CreateProcessHookLib.Win32;
using CreateProcessHookLib.Win32.Model;
using EasyHookLib.Model;

namespace HookCreateProcessA
{

    public class Program
    {

        private static void DoCreateProcessA()
        {
            var si = new StartupInfoA();
            var pi = new ProcessInformation();
            Win32Interop.CreateProcessA("C:\\WINDOWS\\SYSTEM32\\Cmd.exe", "/c Notepad.exe", IntPtr.Zero, IntPtr.Zero, false, 0, IntPtr.Zero,
                null, ref si, ref pi);
        }

        public static void Main()
        {
            var createProcessAHooker = new CreateProcessAHooker();
            createProcessAHooker.MethodHooked += CreateProcessWHooker_ProcessCreated;
            var hookA = createProcessAHooker.CreateHook();

            DoCreateProcessA();

            Console.Write("\nPress <enter> to uninstall hook and exit.");
            Console.ReadLine();
            hookA.Dispose();
            Console.ReadLine();
        }

        private static void CreateProcessWHooker_ProcessCreated(object sender, HookedEventArgs e)
        {
            Console.WriteLine("Process ID (PID): " + e.Entries["DwProcessId"]);
            Console.WriteLine("Process Handle : " + e.Entries["HProcess"]);
        }
    }
}