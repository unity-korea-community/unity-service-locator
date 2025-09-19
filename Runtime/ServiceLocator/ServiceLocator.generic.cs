using System.Threading.Tasks;
using UnityEngine;

#pragma warning disable S2436 // Reduce the number of generic parameters in the 'ServiceLocator.GetService' method to no more than the 3 authorized.
namespace UNKO.ServiceLocator
{
    public static partial class ServiceLocator
    {
        public static (T1, T2) GetService<T1, T2>()
            where T1 : class where T2 : class
            => (Global.GetService<T1>(), Global.GetService<T2>());

        public static (T1, T2) GetService<T1, T2>(GameObject gameObject)
            where T1 : class where T2 : class
            => GetService<T1, T2>(gameObject.transform);

        public static (T1, T2) GetService<T1, T2>(Component component)
            where T1 : class where T2 : class
            => (ServiceLocator.GetService<T1>(component), ServiceLocator.GetService<T2>(component));

        public static (T1, T2, T3) GetService<T1, T2, T3>(Component component)
            where T1 : class where T2 : class where T3 : class
            => (ServiceLocator.GetService<T1>(component), ServiceLocator.GetService<T2>(component), ServiceLocator.GetService<T3>(component));

        public static (T1, T2, T3, T4) GetService<T1, T2, T3, T4>(Component component)
            where T1 : class where T2 : class where T3 : class where T4 : class
            => (ServiceLocator.GetService<T1>(component), ServiceLocator.GetService<T2>(component), ServiceLocator.GetService<T3>(component), ServiceLocator.GetService<T4>(component));

        public static (T1, T2, T3, T4, T5) GetService<T1, T2, T3, T4, T5>(Component component)
            where T1 : class where T2 : class where T3 : class where T4 : class where T5 : class
            => (ServiceLocator.GetService<T1>(component), ServiceLocator.GetService<T2>(component), ServiceLocator.GetService<T3>(component), ServiceLocator.GetService<T4>(component), ServiceLocator.GetService<T5>(component));

        public static (T1, T2, T3, T4, T5, T6) GetService<T1, T2, T3, T4, T5, T6>(Component component)
            where T1 : class where T2 : class where T3 : class where T4 : class where T5 : class where T6 : class
            => (ServiceLocator.GetService<T1>(component), ServiceLocator.GetService<T2>(component), ServiceLocator.GetService<T3>(component), ServiceLocator.GetService<T4>(component), ServiceLocator.GetService<T5>(component), ServiceLocator.GetService<T6>(component));

        public static (T1, T2, T3, T4, T5, T6, T7) GetService<T1, T2, T3, T4, T5, T6, T7>(Component component)
            where T1 : class where T2 : class where T3 : class where T4 : class where T5 : class where T6 : class where T7 : class
            => (ServiceLocator.GetService<T1>(component), ServiceLocator.GetService<T2>(component), ServiceLocator.GetService<T3>(component), ServiceLocator.GetService<T4>(component), ServiceLocator.GetService<T5>(component), ServiceLocator.GetService<T6>(component), ServiceLocator.GetService<T7>(component));

        public static (T1, T2, T3, T4, T5, T6, T7, T8) GetService<T1, T2, T3, T4, T5, T6, T7, T8>(Component component)
            where T1 : class where T2 : class where T3 : class where T4 : class where T5 : class where T6 : class where T7 : class where T8 : class
            => (ServiceLocator.GetService<T1>(component), ServiceLocator.GetService<T2>(component), ServiceLocator.GetService<T3>(component), ServiceLocator.GetService<T4>(component), ServiceLocator.GetService<T5>(component), ServiceLocator.GetService<T6>(component), ServiceLocator.GetService<T7>(component), ServiceLocator.GetService<T8>(component));

        #region async
        public static async Task<(T1, T2)> GetServiceAsync<T1, T2>()
            where T1 : class where T2 : class
            => (await ServiceLocator.GetServiceAsync<T1>(), await ServiceLocator.GetServiceAsync<T2>());

        public static async Task<(T1, T2)> GetServiceAsync<T1, T2>(Component component)
            where T1 : class where T2 : class
            => (await ServiceLocator.GetServiceAsync<T1>(component), await ServiceLocator.GetServiceAsync<T2>(component));

        public static async Task<(T1, T2, T3)> GetServiceAsync<T1, T2, T3>(Component component)
            where T1 : class where T2 : class where T3 : class
            => (await ServiceLocator.GetServiceAsync<T1>(component), await ServiceLocator.GetServiceAsync<T2>(component), await ServiceLocator.GetServiceAsync<T3>(component));

        public static async Task<(T1, T2, T3, T4)> GetServiceAsync<T1, T2, T3, T4>(Component component)
            where T1 : class where T2 : class where T3 : class where T4 : class
            => (await ServiceLocator.GetServiceAsync<T1>(component), await ServiceLocator.GetServiceAsync<T2>(component), await ServiceLocator.GetServiceAsync<T3>(component), await ServiceLocator.GetServiceAsync<T4>(component));

        public static async Task<(T1, T2, T3, T4, T5)> GetServiceAsync<T1, T2, T3, T4, T5>(Component component)
            where T1 : class where T2 : class where T3 : class where T4 : class where T5 : class
            => (await ServiceLocator.GetServiceAsync<T1>(component), await ServiceLocator.GetServiceAsync<T2>(component), await ServiceLocator.GetServiceAsync<T3>(component), await ServiceLocator.GetServiceAsync<T4>(component), await ServiceLocator.GetServiceAsync<T5>(component));

        public static async Task<(T1, T2, T3, T4, T5, T6)> GetServiceAsync<T1, T2, T3, T4, T5, T6>(Component component)
            where T1 : class where T2 : class where T3 : class where T4 : class where T5 : class where T6 : class
            => (await ServiceLocator.GetServiceAsync<T1>(component), await ServiceLocator.GetServiceAsync<T2>(component), await ServiceLocator.GetServiceAsync<T3>(component), await ServiceLocator.GetServiceAsync<T4>(component), await ServiceLocator.GetServiceAsync<T5>(component), await ServiceLocator.GetServiceAsync<T6>(component));

        public static async Task<(T1, T2, T3, T4, T5, T6, T7)> GetServiceAsync<T1, T2, T3, T4, T5, T6, T7>(Component component)
            where T1 : class where T2 : class where T3 : class where T4 : class where T5 : class where T6 : class where T7 : class
            => (await ServiceLocator.GetServiceAsync<T1>(component), await ServiceLocator.GetServiceAsync<T2>(component), await ServiceLocator.GetServiceAsync<T3>(component), await ServiceLocator.GetServiceAsync<T4>(component), await ServiceLocator.GetServiceAsync<T5>(component), await ServiceLocator.GetServiceAsync<T6>(component), await ServiceLocator.GetServiceAsync<T7>(component));

        public static async Task<(T1, T2, T3, T4, T5, T6, T7, T8)> GetServiceAsync<T1, T2, T3, T4, T5, T6, T7, T8>(Component component)
            where T1 : class where T2 : class where T3 : class where T4 : class where T5 : class where T6 : class where T7 : class where T8 : class
            => (await ServiceLocator.GetServiceAsync<T1>(component), await ServiceLocator.GetServiceAsync<T2>(component), await ServiceLocator.GetServiceAsync<T3>(component), await ServiceLocator.GetServiceAsync<T4>(component), await ServiceLocator.GetServiceAsync<T5>(component), await ServiceLocator.GetServiceAsync<T6>(component), await ServiceLocator.GetServiceAsync<T7>(component), await ServiceLocator.GetServiceAsync<T8>(component));

        public static async Task<(T1, T2, T3, T4, T5, T6, T7, T8, T9)> GetServiceAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9>(Component component)
            where T1 : class where T2 : class where T3 : class where T4 : class where T5 : class where T6 : class where T7 : class where T8 : class where T9 : class
            => (await ServiceLocator.GetServiceAsync<T1>(component), await ServiceLocator.GetServiceAsync<T2>(component), await ServiceLocator.GetServiceAsync<T3>(component), await ServiceLocator.GetServiceAsync<T4>(component), await ServiceLocator.GetServiceAsync<T5>(component), await ServiceLocator.GetServiceAsync<T6>(component), await ServiceLocator.GetServiceAsync<T7>(component), await ServiceLocator.GetServiceAsync<T8>(component), await ServiceLocator.GetServiceAsync<T9>(component));

        #endregion
    }
}
#pragma warning restore S2436 // Reduce the number of generic parameters in the 'ServiceLocator.GetService' method to no more than the 3 authorized.
