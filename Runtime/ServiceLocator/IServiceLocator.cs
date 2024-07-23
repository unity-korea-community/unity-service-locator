using System;
using UnityEngine.Events;

namespace UNKO.ServiceLocator
{
    public interface IServiceLocator
    {
        UnityEvent<Type> OnServiceRegistered { get; }

        T GetService<T>() where T : class;
        void RegisterService<T>(T service, params Type[] types) where T : class;
        void UnregisterService<T>() where T : class;
    }

    public static class IServiceLocatorEx
    {
        public static void RegisterServiceAndInterfaces<T>(this IServiceLocator locator, T service) where T : class
        {
            Type[] interfaces = service.GetType().GetInterfaces();
            locator.RegisterService(service, interfaces);
        }

        public static void RegisterService<T>(this IServiceLocator locator, T service) where T : class
            => locator.RegisterService(service, typeof(T));

        public static bool TryGetService<T>(this IServiceLocator locator, out T service) where T : class
        {
            service = locator.GetService<T>();
            return service != null;
        }
    }
}