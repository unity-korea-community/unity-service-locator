#if UNIRX
using System;
using UniRx;

namespace UNKO.ServiceLocator
{
    public static class IServiceLocatorUniRxEx
    {
        public static IObservable<T> GetServiceFirstObservable<T>(this IServiceLocator locator) where T : class
        {
            if (locator.GetService<T>() != null)
            {
                return Observable.Return(locator.GetService<T>());
            }

            return locator.OnServiceRegistered.AsObservable()
                .Where(type => type == typeof(T))
                .First()
                .Select(_ => locator.GetService<T>());
        }
    }
}
#endif