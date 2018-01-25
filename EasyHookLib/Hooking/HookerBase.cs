using System;
using EasyHook;
using EasyHookLib.Interfaces;
using EasyHookLib.Model;
using EasyHookLib.Utility;

namespace EasyHookLib.Hooking
{
    public abstract class HookerBase : NotifyEventsBase, IDisposable, INotifyClient
    {
        protected bool CallBeforeNotify = true;
        protected LocalHook Hook;

        public event EventHandler<HookedEventArgs> IsInstalledEvent;
        public event EventHandler<HookedEventArgs> PingEvent;
        public event EventHandler<HookedEventArgs> ReportExceptionEvent;
        public event EventHandler<HookedEventArgs> MethodHookedEvent;

        public virtual void Dispose()
        {
            if (Hook != null)
            {
                Hook.Dispose();
                Hook = null;
            }
        }

        public override void IsInstalled(int inClientPid)
        {
            IsInstalledEvent?.Invoke(this,
                new HookedEventArgs(new Tuple<string, object>("ClientPid", inClientPid)));
        }

        public override void NotifyMethodHooked(params Tuple<string, object>[] args)
        {
            OnMethodHooked(new HookedEventArgs(args));
        }

        protected abstract object CallMethod(object[] parameters, out Tuple<string, object>[] tuplesForNotification);

        public object CallMethodAndNotifyHooker(params object[] parameters)
        {
            object returnValue = null;
            Tuple<string, object>[] tuplesToRaiseEvent = null;
            try
            {
                if (CallBeforeNotify)
                {
                    returnValue = CallMethod(parameters, out tuplesToRaiseEvent);
                    NotifyMethodHooked(tuplesToRaiseEvent);
                }
                else
                {
                    NotifyMethodHooked(tuplesToRaiseEvent);
                    returnValue = CallMethod(parameters, out tuplesToRaiseEvent);
                }
            }
            catch (Exception e)
            {
                Tracer.WriteLine(e);
            }
            return returnValue;
        }

        public abstract LocalHook CreateHook();

        ~HookerBase()
        {
            Dispose();
        }

        public virtual LocalHook HookMethod(string dllName, string methodName, Delegate delegateToCall,
            object inCallback)
        {
            var inTargetProc = LocalHook.GetProcAddress(dllName, methodName);
            Hook = LocalHook.Create(
                inTargetProc,
                delegateToCall,
                inCallback);
            if (inCallback == null)
            {
                Hook.ThreadACL.SetInclusiveACL(new[] {0});
            }
            else
            {
                Hook.ThreadACL.SetExclusiveACL(new[] {0});
            }
            return Hook;
        }


        protected virtual void OnMethodHooked(HookedEventArgs e)
        {
            MethodHookedEvent?.Invoke(this, e);
        }

    }
}