using System.Threading.Tasks;
using UnityEngine;

#pragma warning disable S2436 // Reduce the number of generic parameters in the 'ServiceLocator.GetService' method to no more than the 3 authorized.
namespace UNKO.ServiceLocator
{
    /// <summary>
    /// <see cref="ServiceLocator"/> 이름을 단축했습니다. 기능은 동일 
    /// </summary>
    public static partial class SL
    {
        public static (T1, T2) GetService<T1, T2>()
            where T1 : class where T2 : class
            => ServiceLocator.GetService<T1, T2>();

        public static (T1, T2) GetService<T1, T2>(Component component)
            where T1 : class where T2 : class
            => ServiceLocator.GetService<T1, T2>(component);

        public static (T1, T2, T3) GetService<T1, T2, T3>(Component component)
            where T1 : class where T2 : class where T3 : class
            => ServiceLocator.GetService<T1, T2, T3>(component);

        public static (T1, T2, T3) GetService<T1, T2, T3>(GameObject gameObject)
            where T1 : class where T2 : class where T3 : class
            => ServiceLocator.GetService<T1, T2, T3>(gameObject.transform);

        public static (T1, T2, T3, T4) GetService<T1, T2, T3, T4>(Component component)
            where T1 : class where T2 : class where T3 : class where T4 : class
            => ServiceLocator.GetService<T1, T2, T3, T4>(component);

        public static (T1, T2, T3, T4) GetService<T1, T2, T3, T4>(GameObject gameObject)
            where T1 : class where T2 : class where T3 : class where T4 : class
            => ServiceLocator.GetService<T1, T2, T3, T4>(gameObject.transform);

        public static (T1, T2, T3, T4, T5) GetService<T1, T2, T3, T4, T5>(Component component)
            where T1 : class where T2 : class where T3 : class where T4 : class where T5 : class
            => ServiceLocator.GetService<T1, T2, T3, T4, T5>(component);

        public static (T1, T2, T3, T4, T5, T6) GetService<T1, T2, T3, T4, T5, T6>(Component component)
            where T1 : class where T2 : class where T3 : class where T4 : class where T5 : class where T6 : class
            => ServiceLocator.GetService<T1, T2, T3, T4, T5, T6>(component);

        public static (T1, T2, T3, T4, T5, T6, T7) GetService<T1, T2, T3, T4, T5, T6, T7>(Component component)
            where T1 : class where T2 : class where T3 : class where T4 : class where T5 : class where T6 : class where T7 : class
            => ServiceLocator.GetService<T1, T2, T3, T4, T5, T6, T7>(component);

        public static (T1, T2, T3, T4, T5, T6, T7, T8) GetService<T1, T2, T3, T4, T5, T6, T7, T8>(Component component)
            where T1 : class where T2 : class where T3 : class where T4 : class where T5 : class where T6 : class where T7 : class where T8 : class
            => ServiceLocator.GetService<T1, T2, T3, T4, T5, T6, T7, T8>(component);

        #region async

        public static async Task<(T1, T2)> GetServiceAsync<T1, T2>()
            where T1 : class where T2 : class
            => await ServiceLocator.GetServiceAsync<T1, T2>(null);

        public static async Task<(T1, T2)> GetServiceAsync<T1, T2>(Component component)
            where T1 : class where T2 : class
            => await ServiceLocator.GetServiceAsync<T1, T2>(component);

        public static async Task<(T1, T2)> GetServiceAsync<T1, T2>(GameObject gameObject)
            where T1 : class where T2 : class
            => await ServiceLocator.GetServiceAsync<T1, T2>(gameObject.transform);

        public static async Task<(T1, T2, T3)> GetServiceAsync<T1, T2, T3>()
            where T1 : class where T2 : class where T3 : class
            => await ServiceLocator.GetServiceAsync<T1, T2, T3>(null);

        public static async Task<(T1, T2, T3)> GetServiceAsync<T1, T2, T3>(Component component)
            where T1 : class where T2 : class where T3 : class
            => await ServiceLocator.GetServiceAsync<T1, T2, T3>(component);

        public static async Task<(T1, T2, T3)> GetServiceAsync<T1, T2, T3>(GameObject gameObject)
            where T1 : class where T2 : class where T3 : class
            => await ServiceLocator.GetServiceAsync<T1, T2, T3>(gameObject.transform);

        public static async Task<(T1, T2, T3, T4)> GetServiceAsync<T1, T2, T3, T4>(Component component)
            where T1 : class where T2 : class where T3 : class where T4 : class
            => await ServiceLocator.GetServiceAsync<T1, T2, T3, T4>(component);

        public static async Task<(T1, T2, T3, T4)> GetServiceAsync<T1, T2, T3, T4>(GameObject gameObject)
            where T1 : class where T2 : class where T3 : class where T4 : class
            => await ServiceLocator.GetServiceAsync<T1, T2, T3, T4>(gameObject.transform);

        public static async Task<(T1, T2, T3, T4, T5)> GetServiceAsync<T1, T2, T3, T4, T5>(Component component)
            where T1 : class where T2 : class where T3 : class where T4 : class where T5 : class
            => await ServiceLocator.GetServiceAsync<T1, T2, T3, T4, T5>(component);

        public static async Task<(T1, T2, T3, T4, T5)> GetServiceAsync<T1, T2, T3, T4, T5>(GameObject gameObject)
            where T1 : class where T2 : class where T3 : class where T4 : class where T5 : class
            => await ServiceLocator.GetServiceAsync<T1, T2, T3, T4, T5>(gameObject.transform);

        public static async Task<(T1, T2, T3, T4, T5, T6)> GetServiceAsync<T1, T2, T3, T4, T5, T6>(Component component)
            where T1 : class where T2 : class where T3 : class where T4 : class where T5 : class where T6 : class
            => await ServiceLocator.GetServiceAsync<T1, T2, T3, T4, T5, T6>(component);

        public static async Task<(T1, T2, T3, T4, T5, T6, T7)> GetServiceAsync<T1, T2, T3, T4, T5, T6, T7>(Component component)
            where T1 : class where T2 : class where T3 : class where T4 : class where T5 : class where T6 : class where T7 : class
            => await ServiceLocator.GetServiceAsync<T1, T2, T3, T4, T5, T6, T7>(component);

        public static async Task<(T1, T2, T3, T4, T5, T6, T7, T8)> GetServiceAsync<T1, T2, T3, T4, T5, T6, T7, T8>(Component component)
            where T1 : class where T2 : class where T3 : class where T4 : class where T5 : class where T6 : class where T7 : class where T8 : class
            => await ServiceLocator.GetServiceAsync<T1, T2, T3, T4, T5, T6, T7, T8>(component);

        public static async Task<(T1, T2, T3, T4, T5, T6, T7, T8, T9)> GetServiceAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9>(Component component)
            where T1 : class where T2 : class where T3 : class where T4 : class where T5 : class where T6 : class where T7 : class where T8 : class where T9 : class
            => await ServiceLocator.GetServiceAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9>(component);

        #endregion
    }
}
#pragma warning restore S2436 // Reduce the number of generic parameters in the 'ServiceLocator.GetService' method to no more than the 3 authorized.
