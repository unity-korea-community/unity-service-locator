#if UNIRX
using System;
using UniRx;

namespace UNKO.ServiceLocator
{
    public static class IServiceLocatorUniRxEx
    {
        public static IObservable<T> GetServiceObservable<T>(this IServiceLocator locator) where T : class
        {
            return locator.OnServiceRegistered.AsObservable()
                .Where(type => type == typeof(T))
                .Select(_ => locator.GetService<T>())
                .Where(service => service != null);
        }

        public static IObservable<T> GetServiceObservableFirst<T>(this IServiceLocator locator) where T : class
        {
            var service = locator.GetService<T>();
            if (service != null)
            {
                return Observable.Return(service);
            }

            return locator.OnServiceRegistered.AsObservable()
                .Where(type => type == typeof(T))
                .Select(_ => locator.GetService<T>())
                .Where(service => service != null)
                .First();
        }

        public static IObservable<(T1, T2)> GetServiceObservableFirst<T1, T2>(this IServiceLocator locator)
            where T1 : class
            where T2 : class
        {
            var service1 = locator.GetService<T1>();
            var service2 = locator.GetService<T2>();
            if (service1 != null && service2 != null)
            {
                return Observable.Return((service1, service2));
            }

            return locator.OnServiceRegistered.AsObservable()
                .Where(type => type == typeof(T1) || type == typeof(T2))
                .Select(_ => (locator.GetService<T1>(), locator.GetService<T2>()))
                .Where(services => services.Item1 != null && services.Item2 != null)
                .First();
        }

        public static IObservable<(T1, T2, T3)> GetServiceObservableFirst<T1, T2, T3>(this IServiceLocator locator)
            where T1 : class
            where T2 : class
            where T3 : class
        {
            var service1 = locator.GetService<T1>();
            var service2 = locator.GetService<T2>();
            var service3 = locator.GetService<T3>();
            if (service1 != null && service2 != null && service3 != null)
            {
                return Observable.Return((service1, service2, service3));
            }

            return locator.OnServiceRegistered.AsObservable()
                .Where(type => type == typeof(T1) || type == typeof(T2) || type == typeof(T3))
                .Select(_ => (locator.GetService<T1>(), locator.GetService<T2>(), locator.GetService<T3>()))
                .Where(services => services.Item1 != null && services.Item2 != null && services.Item3 != null)
                .First();
        }

        public static IObservable<(T1, T2, T3, T4)> GetServiceObservableFirst<T1, T2, T3, T4>(this IServiceLocator locator)
            where T1 : class
            where T2 : class
            where T3 : class
            where T4 : class
        {
            var service1 = locator.GetService<T1>();
            var service2 = locator.GetService<T2>();
            var service3 = locator.GetService<T3>();
            var service4 = locator.GetService<T4>();
            if (service1 != null && service2 != null && service3 != null && service4 != null)
            {
                return Observable.Return((service1, service2, service3, service4));
            }

            return locator.OnServiceRegistered.AsObservable()
                .Where(type => type == typeof(T1) || type == typeof(T2) || type == typeof(T3) || type == typeof(T4))
                .Select(_ => (locator.GetService<T1>(), locator.GetService<T2>(), locator.GetService<T3>(), locator.GetService<T4>()))
                .Where(services => services.Item1 != null && services.Item2 != null && services.Item3 != null && services.Item4 != null)
                .First();
        }
    }
}
#endif