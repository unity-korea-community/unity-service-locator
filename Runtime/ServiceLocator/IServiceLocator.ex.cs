using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace UNKO.ServiceLocator
{
    /// <summary>
    /// <see cref="GetServiceAsync"/> 만 따로 모음
    /// </summary>
    public static partial class IServiceLocatorEx
    {
        const float READONLY_DEFAULT_TIMEOUT_SECONDS = 2f;
        static float DEFAULT_TIMEOUT_SECONDS = READONLY_DEFAULT_TIMEOUT_SECONDS;

        public static void DefaultSetTimeoutSeconds(float seconds)
        {
            DEFAULT_TIMEOUT_SECONDS = seconds;
        }

        /// <summary>
        /// 타임아웃의 시간 기준을 기본값으로 되돌립니다.
        /// </summary>
        public static void ResetTimeoutSeconds()
        {
            DEFAULT_TIMEOUT_SECONDS = READONLY_DEFAULT_TIMEOUT_SECONDS;
        }

        public static async Task<T> GetServiceWithoutTimemoutAsync<T>(this IServiceLocator locator) where T : class
        {
            T service = null;
            while (locator.TryGetService(out service) == false)
            {
                await Task.Yield();
            }

            return service;
        }

        /// <summary>
        /// 서비스를 가져옵니다. 타임아웃이 걸리면 Exception이 발생합니다.
        /// </summary>
        public static async Task<object> GetServiceAsync(this IServiceLocator locator, System.Type type)
            => await GetServiceAsync(locator, type, DEFAULT_TIMEOUT_SECONDS);

        /// <summary>
        /// 서비스를 가져옵니다. 타임아웃이 걸리면 Exception이 발생합니다.
        /// </summary>
        public static async Task<object> GetServiceAsync(this IServiceLocator locator, System.Type type, float timeoutSeconds)
        {
            object service = null;

            using (var cancellationTokenSource = new CancellationTokenSource(System.TimeSpan.FromSeconds(timeoutSeconds)))
            {
                var token = cancellationTokenSource.Token;
                while (locator.TryGetService(type, out service) == false)
                {
#if UNITY_EDITOR
                    if (Application.isEditor && Application.isPlaying == false)
                    {
                        break;
                    }
#endif

                    if (token.IsCancellationRequested)
                    {
                        // Debug.LogException(new System.TimeoutException($"GetServiceAsync<{type.Name}> Timeout"));
                        throw new System.TimeoutException($"GetServiceAsync({type.Name}) Timeout");
                    }
                    await Task.Yield();
                }
            }

            return service;
        }

        /// <summary>
        /// 서비스를 가져옵니다. 타임아웃이 걸리면 Exception이 발생합니다.
        /// </summary>
        public static async Task<T> GetServiceAsync<T>(this IServiceLocator locator) where T : class
            => await GetServiceAsync<T>(locator, DEFAULT_TIMEOUT_SECONDS);

        /// <summary>
        /// 서비스를 가져옵니다. 타임아웃이 걸리면 Exception이 발생합니다.
        /// </summary>
        public static async Task<T> GetServiceAsync<T>(this IServiceLocator locator, float timeoutSeconds) where T : class
        {
            T service = null;

            using (var cancellationTokenSource = new CancellationTokenSource(System.TimeSpan.FromSeconds(timeoutSeconds)))
            {
                var token = cancellationTokenSource.Token;
                while (locator.TryGetService(out service) == false)
                {
                    if (token.IsCancellationRequested)
                    {
                        // Debug.LogException(new System.TimeoutException($"GetServiceAsync<{typeof(T).Name}> Timeout"));
                        // break;

                        if (locator.ApplicationIsQuitting == false)
                        {
                            if (locator is UnityEngine.Object locatorObject)
                            {
                                // Debug.LogException(new System.TimeoutException($"GetServiceAsync<{typeof(T).Name}> Timeout"), locatorObject);
                                throw new System.TimeoutException($"GetServiceAsync<{typeof(T).Name}> Timeout");
                                break;
                            }
                            else
                            {
                                throw new System.TimeoutException($"GetServiceAsync<{typeof(T).Name}> Timeout");
                            }
                        }
                    }
                    await Task.Yield();
                }
            }

            return service;
        }

        public static (T1 result1, T2 result2) GetService<T1, T2>(this IServiceLocator locator) where T1 : class where T2 : class
            => (locator.GetService<T1>(), locator.GetService<T2>());

        public static async Task<(T1 result1, T2 result2)> GetServiceAsync<T1, T2>(this IServiceLocator locator) where T1 : class where T2 : class
            => await GetServiceAsync<T1, T2>(locator, DEFAULT_TIMEOUT_SECONDS);

        public static async Task<(T1 result1, T2 result2)> GetServiceAsync<T1, T2>(this IServiceLocator locator, float timeoutSeconds) where T1 : class where T2 : class
            => (await locator.GetServiceAsync<T1>(timeoutSeconds), await locator.GetServiceAsync<T2>(timeoutSeconds));


        public static (T1 result1, T2 result2, T3 result3) GetService<T1, T2, T3>(this IServiceLocator locator) where T1 : class where T2 : class where T3 : class
            => (locator.GetService<T1>(), locator.GetService<T2>(), locator.GetService<T3>());

        public static async Task<(T1 result1, T2 result2, T3 result3)> GetServiceAsync<T1, T2, T3>(this IServiceLocator locator) where T1 : class where T2 : class where T3 : class
            => await GetServiceAsync<T1, T2, T3>(locator, DEFAULT_TIMEOUT_SECONDS);

        public static async Task<(T1 result1, T2 result2, T3 result3)> GetServiceAsync<T1, T2, T3>(this IServiceLocator locator, float timeoutSeconds) where T1 : class where T2 : class where T3 : class
            => (await locator.GetServiceAsync<T1>(timeoutSeconds), await locator.GetServiceAsync<T2>(timeoutSeconds), await locator.GetServiceAsync<T3>(timeoutSeconds));


        public static async Task<(T1 result1, T2 result2, T3 result3, T4)> GetServiceAsync<T1, T2, T3, T4>(this IServiceLocator locator) where T1 : class where T2 : class where T3 : class where T4 : class
            => await GetServiceAsync<T1, T2, T3, T4>(locator, DEFAULT_TIMEOUT_SECONDS);

        public static async Task<(T1 result1, T2 result2, T3 result3, T4)> GetServiceAsync<T1, T2, T3, T4>(this IServiceLocator locator, float timeoutSeconds) where T1 : class where T2 : class where T3 : class where T4 : class
            => (await locator.GetServiceAsync<T1>(timeoutSeconds), await locator.GetServiceAsync<T2>(timeoutSeconds), await locator.GetServiceAsync<T3>(timeoutSeconds), await locator.GetServiceAsync<T4>(timeoutSeconds));


        public static async Task<(T1 result1, T2 result2, T3 result3, T4 result4, T5 result5)> GetServiceAsync<T1, T2, T3, T4, T5>(this IServiceLocator locator) where T1 : class where T2 : class where T3 : class where T4 : class where T5 : class
            => await GetServiceAsync<T1, T2, T3, T4, T5>(locator, DEFAULT_TIMEOUT_SECONDS);

        public static async Task<(T1 result1, T2 result2, T3 result3, T4 result4, T5 result5)> GetServiceAsync<T1, T2, T3, T4, T5>(this IServiceLocator locator, float timeoutSeconds) where T1 : class where T2 : class where T3 : class where T4 : class where T5 : class
            => (await locator.GetServiceAsync<T1>(timeoutSeconds), await locator.GetServiceAsync<T2>(timeoutSeconds), await locator.GetServiceAsync<T3>(timeoutSeconds), await locator.GetServiceAsync<T4>(timeoutSeconds), await locator.GetServiceAsync<T5>(timeoutSeconds));


        public static async Task<(T1 result1, T2 result2, T3 result3, T4 result4, T5 result5, T6 result6)> GetServiceAsync<T1, T2, T3, T4, T5, T6>(this IServiceLocator locator) where T1 : class where T2 : class where T3 : class where T4 : class where T5 : class where T6 : class
            => await GetServiceAsync<T1, T2, T3, T4, T5, T6>(locator, DEFAULT_TIMEOUT_SECONDS);

        public static async Task<(T1 result1, T2 result2, T3 result3, T4 result4, T5 result5, T6 result6)> GetServiceAsync<T1, T2, T3, T4, T5, T6>(this IServiceLocator locator, float timeoutSeconds) where T1 : class where T2 : class where T3 : class where T4 : class where T5 : class where T6 : class
            => (await locator.GetServiceAsync<T1>(timeoutSeconds), await locator.GetServiceAsync<T2>(timeoutSeconds), await locator.GetServiceAsync<T3>(timeoutSeconds), await locator.GetServiceAsync<T4>(timeoutSeconds), await locator.GetServiceAsync<T5>(timeoutSeconds), await locator.GetServiceAsync<T6>(timeoutSeconds));
    }
}