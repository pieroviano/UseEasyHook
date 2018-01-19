using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EasyHook;

namespace EasyHookLib
{
    public class HookerBase : IDisposable
    {
        protected LocalHook Hook;

        protected LocalHook HookMethod(IntPtr inTargetProc, Delegate delegateToCall)
        {
            Hook = LocalHook.Create(
                inTargetProc,
                delegateToCall,
                null);
            Hook.ThreadACL.SetInclusiveACL(new int[] { 0 });
            return Hook;
        }

        public void Dispose()
        {
            if (Hook != null)
            {
                Hook.Dispose();
                Hook = null;
            }
        }

        ~HookerBase()
        {
            Dispose();
        }
    }
}
