using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;

namespace EasyHookLib.Utility
{
    public class EventHandlerInjector
    {
        public static Type AttachHandlerToEventDynamically(string dllWithEvent, string typeName, string eventName,
            Type typeHavingHandler, string handlerName, object objectHavingHandler, out object createProcessWHooker)
        {
            var directoryName = Path.GetDirectoryName(Assembly.GetCallingAssembly().Location);
            Debug.Assert(directoryName != null, "directoryName != null");
            var combine = Path.Combine(directoryName, dllWithEvent);
            var assembly = Assembly.LoadFrom(combine);
            var type = assembly.GetTypes().FirstOrDefault(t => t.Name == typeName);
            Debug.Assert(type != null, "type != null");
            var constructorInfo = type.GetConstructor(new Type[0]);
            Debug.Assert(constructorInfo != null, "constructorInfo != null");
            createProcessWHooker = constructorInfo.Invoke(new object[0]);
            //var createProcessWHooker = new CreateProcessWHooker();
            var eventInfo = createProcessWHooker.GetType().GetEvent(eventName);

            var methodInfo = typeHavingHandler.GetMethod(handlerName,
                BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance);
            Debug.Assert(methodInfo != null, "methodInfo != null");
            var handler =
                Delegate.CreateDelegate(eventInfo.EventHandlerType,
                    objectHavingHandler,
                    methodInfo);
            eventInfo.AddEventHandler(createProcessWHooker, handler);
            return type;
        }
    }
}