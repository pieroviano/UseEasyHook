using System;
using System.Diagnostics;
using EasyHook;
using EasyHookLib.Interfaces;
using EasyHookLib.Model;
using EasyHookLib.Utility;

namespace EasyHookLib.Hooking
{
    public abstract class HookerBase : IDisposable, INotifyClient
    {

        protected bool CallBeforeNotify = true;
        protected LocalHook Hook;

        public HookedEventArgs CreatedEventArgs { get; set; }

        public virtual void Dispose()
        {
            if (Hook != null)
            {
                Hook.Dispose();
                Hook = null;
            }
        }

        protected abstract object CallMethod(object[] parameters, out Tuple<string, object>[] tuplesForNotification);

        public abstract LocalHook CreateHook();

        protected object CallMethodAndNotifyHooker(params object[] parameters)
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
                if (!CallBeforeNotify)
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

        ~HookerBase()
        {
            Dispose();
        }

        public virtual LocalHook HookMethod(string dllName, string methodName, Delegate delegateToCall, object inCallback)
        {
            var inTargetProc = LocalHook.GetProcAddress(dllName, methodName);
            Hook = LocalHook.Create(
                inTargetProc,
                delegateToCall,
                inCallback);
            Hook.ThreadACL.SetInclusiveACL(new[] { 0 });
            return Hook;
        }

        public event EventHandler<HookedEventArgs> MethodHooked;

        protected virtual void OnMethodHooked(HookedEventArgs e)
        {
            MethodHooked?.Invoke(this, e);
        }

        public virtual void NotifyMethodHooked(params Tuple<string, object>[] args)
        {
            CreatedEventArgs = new HookedEventArgs(args);

            OnMethodHooked(CreatedEventArgs);
        }
    }
}