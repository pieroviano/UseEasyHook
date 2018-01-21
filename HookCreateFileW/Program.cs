using System;
using System.IO;
using EasyHookLib;
using EasyHookLib.RemoteInjection;

namespace FileMonitor
{
    internal class Program
    {
        private static string _channelName;

        public static void Main(string[] args)
        {
            int targetPid;
            string targetExe;
            ConsoleAsker.GetTargetExeOrPid(args, out targetExe, out targetPid);

            try
            {
                var dllToInject = "CreateFileHookLib.dll";
                var notifyClient = new NotifyClient();
                var formattableString = RemoteInjector.InjectDll(dllToInject, targetExe, ref targetPid, out _channelName, notifyClient);
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