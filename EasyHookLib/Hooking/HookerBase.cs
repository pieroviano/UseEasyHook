using System;
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

        public virtual void NotifyMethodHooked(params Tuple<string, object>[] args)
        {
            CreatedEventArgs = new HookedEventArgs(args);

            OnMethodHooked(CreatedEventArgs);
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

        public event EventHandler<HookedEventArgs> MethodHooked;

        protected virtual void OnMethodHooked(HookedEventArgs e)
        {
            MethodHooked?.Invoke(this, e);
        }
    }
}