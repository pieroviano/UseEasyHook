using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using EasyHook;
using EasyHookLib;
using EasyHookLib.Win32;
using EasyHookLib.Win32.Model;

namespace UseEasyHook
{

    public class Program
    {
        private static void DoCreateProcessW()
        {
            var si = new StartupInfoW();
            var pi = new ProcessInformation();
            Win32Interop.CreateProcessW("C:\\WINDOWS\\SYSTEM32\\Calc.exe", null, IntPtr.Zero, IntPtr.Zero, false, 0, IntPtr.Zero,
                null, ref si, ref pi);
        }

        private static void DoCreateProcessA()
        {
            var si = new StartupInfoA();
            var pi = new ProcessInformation();
            Win32Interop.CreateProcessA("C:\\WINDOWS\\SYSTEM32\\Calc.exe", null, IntPtr.Zero, IntPtr.Zero, false, 0, IntPtr.Zero,
                null, ref si, ref pi);
        }

        public static void Main()
        {
            var createProcessWHooker = new CreateProcessWHooker();
            createProcessWHooker.ProcessCreated += CreateProcessWHooker_ProcessCreated;
            var hook = createProcessWHooker.HookCreateProcess();

            DoCreateProcessW();

            Console.Write("\nPress <enter> to uninstall hook.");
            Console.ReadLine();
            hook.Dispose();

            var createProcessAHooker = new CreateProcessAHooker();
            createProcessAHooker.ProcessCreated += CreateProcessWHooker_ProcessCreated;
            var hookA = createProcessAHooker.HookCreateProcess();

            DoCreateProcessA();

            Console.Write("\nPress <enter> to uninstall hook and exit.");
            Console.ReadLine();
            hookA.Dispose();
            Console.ReadLine();
        }

        private static void CreateProcessWHooker_ProcessCreated(object sender, ProcessCreatedEventArgs e)
        {
            Console.WriteLine("Process ID (PID): " + e.ProcessId);
            Console.WriteLine("Process Handle : " + e.HProcess);
        }
    }
}