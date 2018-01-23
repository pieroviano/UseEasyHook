using System.IO;

namespace HookCreateFileW
{
    internal static class ConsoleAsker
    {
        public static void GetTargetExeOrPid(string[] args, out string targetExe, out int targetPid)
        {
            targetPid = -1;
            targetExe = null;
            if (args.Length > 0 && File.Exists(args[0]))
            {
                targetExe = args[0];
            }
            // Load the parameter
            while (args.Length < 1 || !int.TryParse(args[0], out targetPid) && !File.Exists(args[0]))
            {
                if (targetPid > 0)
                {
                    break;
                }
                targetExe = ConsoleHelper.AskTargetExe(ref args);
            }
        }
    }
}