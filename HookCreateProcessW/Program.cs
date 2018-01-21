using System;
using CreateProcessHookLib.Win32;
using CreateProcessHookLib.Win32.Model;
using CreateProcessHookWLib;
using EasyHookLib.Model;

namespace HookCreateProcessW
{

    public class Program
    {
        private static void DoCreateProcessW()
        {
            var si = new StartupInfoW();
            var pi = new ProcessInformation();
            Win32Interop.CreateProcessW("C:\\WINDOWS\\SYSTEM32\\Cmd.exe", "/c Notepad.exe", IntPtr.Zero, IntPtr.Zero, false, 0, IntPtr.Zero,
                null, ref si, ref pi);
        }

        private static void DoCreateProcessA()
        {
            var si = new StartupInfoA();
            var pi = new ProcessInformation();
            Win32Interop.CreateProcessA("C:\\WINDOWS\\SYSTEM32\\Cmd.exe", "/c Notepad.exe", IntPtr.Zero, IntPtr.Zero, false, 0, IntPtr.Zero,
                null, ref si, ref pi);
        }

        public static void Main()
        {
            var createProcessWHooker = new CreateProcessWHooker();
            createProcessWHooker.MethodHooked += CreateProcessWHooker_ProcessCreated;
            var hook = createProcessWHooker.CreateHook();

            DoCreateProcessW();

            Console.Write("\nPress <enter> to uninstall hook and exit.");
            Console.ReadLine();
            hook.Dispose();
            Console.ReadLine();
        }

        private static void CreateProcessWHooker_ProcessCreated(object sender, HookedEventArgs e)
        {
            Console.WriteLine("Process ID (PID): " + e.Entries["DwProcessId"]);
            Console.WriteLine("Process Handle : " + e.Entries["HProcess"]);
        }
    }
}