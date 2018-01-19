using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyHookLib.RemoteInjection
{
    public class PostbackMessageHandler : MarshalByRefObject
    {
        public void IsInstalled(int inClientPid)
        {
            Debug.Write($"Hooking has been installed in target {inClientPid}.\r\n" );
        }

        public void ProcessMessages(int inClientPid, string[] messsages)
        {
            for (var i = 0; i < messsages.Length; i++)
            {
                Debug.Write(messsages[i]);
            }
        }

        public void Ping()
        {
            Debug.Write("I'm alive");
        }

        public void ReportException(Exception inInfo)
        {
            Debug.Write("The target process has reported an error:\r\n" + inInfo);
        }
    }

}
