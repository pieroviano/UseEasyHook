using System;
using System.IO;
using EasyHookLib.RemoteInjection;

namespace FileMonitor
{
    internal class Program
    {
        private static string _channelName;

        private static string AskTargetExe(ref string[] args)
        {
            if (args.Length != 1 || !File.Exists(args[0]))
            {
                Console.WriteLine();
                Console.WriteLine("Usage: FileMon %PID%");
                Console.WriteLine("   or: FileMon PathToExecutable");
                Console.WriteLine();
                Console.Write("Please enter a process Id or path to executable: ");

                args = new[] {Console.ReadLine()};

                if (string.IsNullOrEmpty(args[0]))
                {
                    return null;
                }
            }
            return args[0];
        }

        private static void GetTargetExeOrPid(string[] args, out string targetExe, out int targetPid)
        {
            targetPid = 0;
            targetExe = null;

            // Load the parameter
            while (args.Length < 1 || (!int.TryParse(args[0], out targetPid) && !File.Exists(args[0])))
            {
                if (targetPid > 0)
                {
                    break;
                }
                targetExe = AskTargetExe(ref args);
            }
        }

        public static void Main(string[] args)
        {
            int targetPid;
            string targetExe;
            GetTargetExeOrPid(args, out targetExe, out targetPid);

            try
            {
                var dllToInject = "FileMonInject.dll";
                var formattableString = RemoteInjector.InjectDll(dllToInject, targetExe, targetPid, out _channelName);
                if (!string.IsNullOrEmpty(formattableString))
                {
                    Console.WriteLine(formattableString);
                }
                Console.WriteLine("<Press any key to exit>");
                Console.ReadKey();
            }
            catch (Exception extInfo)
            {
                Console.WriteLine("There was an error while connecting to target:\r\n{0}", extInfo);
                Console.WriteLine("<Press any key to exit>");
                Console.ReadKey();
            }
        }
    }
}