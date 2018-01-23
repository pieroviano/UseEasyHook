using System;
using System.IO;

namespace HookCreateFileW
{
    internal static class ConsoleHelper
    {
        public static string AskTargetExe(ref string[] args)
        {
            if (args.Length != 1 || !File.Exists(args[0]))
            {
                Console.WriteLine();
                Console.WriteLine("Usage: FileMon %PID%");
                Console.WriteLine("   or: FileMon PathToExecutable");
                Console.WriteLine();
                Console.Write("Please enter a process Id or path to executable: ");

                args = new[] {Console.ReadLine()};

                if (String.IsNullOrEmpty(args[0]))
                {
                    return null;
                }
            }
            return args[0];
        }
    }
}