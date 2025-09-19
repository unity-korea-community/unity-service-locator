using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

namespace UNKO.ServiceLocator
{
    public interface IServiceLocator : IDisposable
    {
        bool ApplicationIsQuitting { get; }
        IReadOnlyDictionary<Type, object> AllServices { get; }
        UnityEvent<Type> OnServiceRegistered { get; }

        object GetService(Type type, bool printNotFoundError);
        void RegisterService<T>(T service, params Type[] types) where T : class;
        void RegisterService<T>(UnityEngine.Object unityObject, T service, params Type[] types) where T : class;
        void UnregisterService(System.Type type, bool callDispose);
    }

    public static partial class IServiceLocatorEx
    {
        public static bool IsRegistered<T>(this IServiceLocator locator) where T : class
            => locator.GetService(typeof(T), false) != null;

        public static T GetService<T>(this IServiceLocator locator) where T : class
            => locator?.GetService(typeof(T), true) as T;

        public static T GetService<T>(this IServiceLocator locator, bool printNotFoundError) where T : class
            => locator?.GetService(typeof(T), printNotFoundError) as T;

        /// <summary>
        /// OnGetService 이벤트를 생성합니다. 이 이벤트는 앞으로 서비스의 Regist가 일어날 때마다 호출됩니다. 이미 존재하는 것은 호출되지 않습니다.
        /// </summary>
        public static IDisposable CreateOnGetServiceEvent<T>(this IServiceLocator locator, System.Action<T> onGetService) where T : class
        {
            void NotifyListener(System.Type type)
            {
                if (type == typeof(T))
                {
                    if (locator.TryGetService(out T service))
                    {
                        onGetService?.Invoke(service);
                    }
                    else
                    {
                        Debug.LogError($"CreateOnGetServiceEvent<{typeof(T).Name}> service is null");
                    }
                }
            }

            locator.OnServiceRegistered.AddListener(NotifyListener);
            return new Disposable(() => locator.OnServiceRegistered.RemoveListener(NotifyListener));
        }

        /// <summary>
        /// OnGetService 이벤트를 생성합니다. 이 이벤트는 앞으로 서비스의 Regist가 일어날 때마다 호출됩니다. 이미 존재하는 것은 호출되지 않습니다.
        /// </summary>
        public static IDisposable CreateOnGetServiceEvent(this IServiceLocator locator, System.Type type, System.Action<object> onGetService)
        {
            void NotifyListener(System.Type registeredType)
            {
                if (registeredType == type)
                {
                    if (locator.TryGetService(type, out object service))
                    {
                        onGetService?.Invoke(service);
                    }
                    else
                    {
                        Debug.LogError($"CreateOnGetServiceEvent<{type.Name}> service is null");
                    }
                }
            }

            locator.OnServiceRegistered.AddListener(NotifyListener);
            return new Disposable(() => locator.OnServiceRegistered.RemoveListener(NotifyListener));
        }

        /// <summary>
        /// OnGetService 이벤트를 생성합니다. 이 이벤트는 서비스의 Regist가 일어날 때마다 호출됩니다. 이미 존재하는 경우 바로 호출합니다.
        /// </summary>
        public static IDisposable CreateOnGetServiceEventIncludeAlreadyExist(this IServiceLocator locator, System.Type type, System.Action<object> onGetService)
        {
            if (locator.TryGetService(type, out object service))
            {
                onGetService?.Invoke(service);
            }

            return locator.CreateOnGetServiceEvent(type, onGetService);
        }

        /// <summary>
        /// OnGetService 이벤트를 생성합니다. 이 이벤트는 서비스의 Regist가 일어날 때마다 호출됩니다. 이미 존재하는 경우 바로 호출합니다.
        /// </summary>
        public static IDisposable CreateOnGetServiceEventIncludeAlreadyExist<T>(this IServiceLocator locator, System.Action<T> onGetService) where T : class
        {
            if (locator.TryGetService(out T service))
            {
                onGetService?.Invoke(service);
            }

            return locator.CreateOnGetServiceEvent(onGetService);
        }

        /// <summary>
        /// service를 등록합니다. 이 class 뿐만 아니라 모든 interface들도 같이 등록합니다.
        /// </summary>
        public static void RegisterServiceAndInterfaces<T>(this IServiceLocator locator, T service) where T : class
        {
            RegisterServiceAndInterfaces(locator, service, null);
        }

        /// <summary>
        /// service를 등록합니다. 이 class 뿐만 아니라 모든 interface들도 같이 등록합니다.
        /// </summary>
        public static void RegisterServiceAndInterfaces<T>(this IServiceLocator locator, T service, GameObject gameObject) where T : class
        {
            if (service == null)
            {
                Debug.LogError($"RegisterServiceAndInterfaces<{typeof(T).Name}> service is null");
                return;
            }

            Type[] allTypes = service.GetType().GetInterfaces().Append(service.GetType()).ToArray();
            locator.RegisterService(gameObject, service, allTypes);
            IAwakeAbleAwake(service, gameObject, locator);

            if (gameObject != null)
            {
                locator.RegisterService(gameObject);
            }
        }


        public static void RegisterService<T>(this IServiceLocator locator, T service) where T : class
            => locator.RegisterService(service, typeof(T));

        public static T RegisterServiceFromNew<T>(this IServiceLocator locator) where T : class
        {
            try
            {
                T service = CreateAndResolve<T>(locator);
                locator.RegisterService(service, typeof(T));
                IAwakeAbleAwake(service, null, locator);

                return service;
            }
            catch (Exception exception)
            {
                Debug.LogError($"RegisterServiceFromNew<{typeof(T).Name}> failed " + exception);
                return null;
            }
        }

        public static T RegisterServiceAndInterfacesFromNew<T>(this IServiceLocator locator) where T : class
        {
            T service = CreateAndResolve<T>(locator);
            Type[] allTypes = service.GetType().GetInterfaces().Append(service.GetType()).ToArray();
            locator.RegisterService(service, allTypes);
            IAwakeAbleAwake(service, null, locator);

            return service;
        }

        /// <summary>
        /// <see cref="FromServiceLocatorAttribute"/>에 필요한 Resolve를 전부 다 한 뒤 Regist합니다.
        /// </summary>
        public static async Task<T> RegisterServiceFromNewWaitResolveAllAsync<T>(this IServiceLocator locator) where T : class
            => await RegisterServiceFromNewWaitResolveAllAsync<T>(locator, null);

        /// <summary>
        /// <see cref="FromServiceLocatorAttribute"/>에 필요한 Resolve를 전부 다 한 뒤 Regist합니다.
        /// </summary>
        public static async Task<T> RegisterServiceFromNewWaitResolveAllAsync<T>(this IServiceLocator locator, Component componentOrNull) where T : class
        {
            T service = Activator.CreateInstance<T>();
            await ServiceLocator.ResolveServiceAttributeAsync(service, componentOrNull);
            locator.RegisterService(service, typeof(T));
            IAwakeAbleAwake(service, componentOrNull?.gameObject, locator);

            return service;
        }


        public static void UnregisterService(this IServiceLocator locator, System.Type type)
            => locator.UnregisterService(type, false);

        public static void UnregisterService<T>(this IServiceLocator locator) where T : class
            => locator.UnregisterService(typeof(T), false);

        public static void UnregisterServiceAndInterfaces<T>(this IServiceLocator locator, T instance) where T : class
        {
            Type[] allTypes = instance.GetType().GetInterfaces().Append(instance.GetType()).ToArray();
            foreach (var type in allTypes)
            {
                locator.UnregisterService(type, false);
            }
        }

        public static void UnregisterServiceAndInterfaces<T>(this IServiceLocator locator) where T : class
        {
            T service = locator.GetService<T>();
            if (service == null)
            {
                Debug.LogWarning($"UnregisterServiceAndInterfaces<{typeof(T).Name}> service is null");
                return;
            }

            Type[] allTypes = service.GetType().GetInterfaces().Append(service.GetType()).ToArray();
            foreach (var type in allTypes)
            {
                locator.UnregisterService(type, false);
            }
        }

        public static void UnregisterServiceAll(this IServiceLocator locator)
            => locator.UnregisterServiceAll(false);

        public static void UnregisterServiceAll(this IServiceLocator locator, bool callDispose)
        {
            var allServiceNames = locator.AllServices.Keys.ToList();
            foreach (var serviceName in allServiceNames)
            {
                locator.UnregisterService(serviceName, callDispose);
            }
        }

        public static bool TryGetService<T>(this IServiceLocator locator, out T service) where T : class
        {
            service = locator.GetService<T>(false);
            return service != null;
        }

        public static bool TryGetService(this IServiceLocator locator, System.Type type, out object service)
        {
            service = locator.GetService(type, false);
            return service != null;
        }

        /// <summary>
        /// 생성자 주입을 통해 객체를 생성하고, 주입할 수 있는 서비스가 있으면 주입합니다.
        /// </summary>
        public static T CreateAndResolveComponent<T>(this IServiceLocator locator, params object[] localResolves)
            where T : Component
            => CreateAndResolve<T>(locator, (service) => service, Debug.LogError, localResolves);

        /// <summary>
        /// 생성자 주입을 통해 객체를 생성하고, 주입할 수 있는 서비스가 있으면 주입합니다.
        /// </summary>
        public static T CreateAndResolve<T>(this IServiceLocator locator, params object[] localResolves)
        {
            if (locator is ServiceLocatorGameObject locatorGameObject)
            {
                return CreateAndResolve<T>(locator, (service) => locatorGameObject.transform, Debug.LogError, localResolves);
            }
            else
            {
                return CreateAndResolve<T>(locator, (service) => null, Debug.LogError, localResolves);
            }
        }

        /// <summary>
        /// 생성자 주입을 통해 객체를 생성하고, 주입할 수 있는 서비스가 있으면 주입합니다.
        /// </summary>
        public static T CreateAndResolve<T>(this IServiceLocator locator, Component component, params object[] localResolves)
            => CreateAndResolve<T>(locator, (service) => component, Debug.LogError, localResolves);

        static object[] _emptyObjects = new object[0];

        /// <summary>
        /// 생성자 주입을 통해 객체를 생성하고, 주입할 수 있는 서비스가 있으면 주입합니다.
        /// </summary>
        public static T CreateAndResolve<T>(this IServiceLocator locator, Action<string> onError)
            where T : Component
            => CreateAndResolve<T>(locator, (service) => service, onError, _emptyObjects);

        /// <summary>
        /// 생성자 주입을 통해 객체를 생성하고, 주입할 수 있는 서비스가 있으면 주입합니다.
        /// </summary>
        public static T CreateAndResolve<T>(this IServiceLocator locator, Func<T, Component> getComponent, Action<string> onError, params object[] localResolves)
        {
            // reflection을 통해 생성자 주입
            Type serviceType = typeof(T);
            string errorMessage = $"ServiceLocator.CreateAndResolve<{serviceType.Name}> not found constructor";
            foreach (var constructor in serviceType.GetConstructors().OrderByDescending(constructor => constructor.GetParameters().Length))
            {
                var parameters = constructor.GetParameters();
                // NOTE 생성자의 파라미터가 많은 순부터 시도했기 때문에,
                // 파라미타가 없는 생성자까지 왔다는 건 곧 마지막 생성자라는 뜻
                if (parameters.Length == 0)
                {
                    T serviceWhenZeroConstructorParameter = (T)Activator.CreateInstance(serviceType);
                    Component componentOrNullWhenZeroParam = getComponent(serviceWhenZeroConstructorParameter);
                    ServiceLocator.ResolveServiceAttribute(serviceWhenZeroConstructorParameter, getComponent(serviceWhenZeroConstructorParameter));
                    IAwakeAbleAwake(serviceWhenZeroConstructorParameter, componentOrNullWhenZeroParam?.gameObject, locator);

                    return serviceWhenZeroConstructorParameter;
                }

                if (IsNotResolved(locator, parameters, localResolves))
                {
                    // NOTE TrueForAll은 현재 C# 버전이 지원하지 않음
#pragma warning disable S6603 // The collection-specific "TrueForAll" method should be used instead of the "All" extension
                    var notResolvedParameters = parameters
                        .Where(parameter => locator.TryGetService(parameter.ParameterType, out object paramInstance) == false)
                        .Select(parameter => $"`{parameter.ParameterType.Name}`");
#pragma warning restore S6603 // The collection-specific "TrueForAll" method should be used instead of the "All" extension

                    errorMessage += $", not resolved parameters: {string.Join(", ", notResolvedParameters)}";
                    continue;
                }

                object[] parameterValues = parameters
                    .Select(parameter => ResolveParameter(locator, parameter, localResolves))
                    .ToArray();
                T service = (T)Activator.CreateInstance(serviceType, parameterValues);
                Component componentOrNull = getComponent(service);
                ServiceLocator.ResolveServiceAttribute(service, componentOrNull);
                IAwakeAbleAwake(service, componentOrNull?.gameObject, locator);

                return service;
            }

            onError?.Invoke(errorMessage);
            return default;
        }


        /// <summary>
        /// 생성자 주입을 통해 객체를 생성하고, 주입할 수 있는 서비스가 있으면 주입합니다.
        /// </summary>
        public static object CreateAndResolve(this IServiceLocator locator, Type serviceType, params object[] localResolves)
        {
            if (locator is ServiceLocatorGameObject locatorGameObject)
            {
                return CreateAndResolve(locator, serviceType, (service) => locatorGameObject.transform, Debug.LogError, localResolves);
            }
            else
            {
                return CreateAndResolve(locator, serviceType, (service) => null, Debug.LogError, localResolves);
            }
        }

        static List<object> _usedLocalResolves = new List<object>();

        /// <summary>
        /// 생성자 주입을 통해 객체를 생성하고, 주입할 수 있는 서비스가 있으면 주입합니다.
        /// </summary>
        public static object CreateAndResolve(this IServiceLocator locator, Type serviceType, Func<object, Component> getComponent, Action<string> onError, params object[] localResolves)
        {
            _usedLocalResolves.Clear();
            string errorMessage = $"ServiceLocator.CreateAndResolve<{serviceType.Name}> not found constructor";
            // reflection을 통해 생성자 주입
            foreach (var constructor in serviceType.GetConstructors().OrderByDescending(constructor => constructor.GetParameters().Length))
            {
                var parameters = constructor.GetParameters();
                // NOTE 생성자의 파라미터가 많은 순부터 시도했기 때문에,
                // 파라미타가 없는 생성자까지 왔다는 건 곧 마지막 생성자라는 뜻
                if (parameters.Length == 0)
                {
                    object serviceWhenZeroConstructorParameter = Activator.CreateInstance(serviceType);
                    Component componentOrNullWhenZeroParam = getComponent(serviceWhenZeroConstructorParameter);
                    ServiceLocator.ResolveServiceAttribute(serviceWhenZeroConstructorParameter, componentOrNullWhenZeroParam);
                    IAwakeAbleAwake(serviceWhenZeroConstructorParameter, componentOrNullWhenZeroParam?.gameObject, locator);

                    return serviceWhenZeroConstructorParameter;
                }

                if (IsNotResolved(locator, parameters, localResolves))
                {
                    // NOTE TrueForAll은 현재 C# 버전이 지원하지 않음
#pragma warning disable S6603 // The collection-specific "TrueForAll" method should be used instead of the "All" extension
                    var notResolvedParameters = parameters
                        .Where(parameter => locator.TryGetService(parameter.ParameterType, out object paramInstance) == false)
                        .Select(parameter => $"`{parameter.ParameterType.Name}`");
#pragma warning restore S6603 // The collection-specific "TrueForAll" method should be used instead of the "All" extension

                    errorMessage += $", not resolved parameters: {string.Join(", ", notResolvedParameters)}";
                    continue;
                }

                object[] parameterValues = parameters
                    .Select(parameter => ResolveParameter(locator, parameter, localResolves))
                    .ToArray();
                var service = Activator.CreateInstance(serviceType, parameterValues);
                Component componentOrNull = getComponent(service);
                ServiceLocator.ResolveServiceAttribute(service, componentOrNull);
                IAwakeAbleAwake(service, componentOrNull?.gameObject, locator);

                return service;
            }

            onError?.Invoke(errorMessage);
            return default;
        }

        private static object ResolveParameter(IServiceLocator locator, ParameterInfo parameter, object[] localResolves)
        {
            // 먼저 _usedLocalResolves에 없는 것중에 찾는다
            if (localResolves.FirstOrDefault(localResolve => parameter.ParameterType.IsInstanceOfType(localResolve) && _usedLocalResolves.Contains(localResolve) == false) is object localResolve)
            {
                _usedLocalResolves.Add(localResolve);
                return localResolve;
            }

            // 없으면 localResolves의 마지막 요소를 사용
            if (localResolves.LastOrDefault(localResolve => parameter.ParameterType.IsInstanceOfType(localResolve)) is object lastLocalResolve)
            {
                _usedLocalResolves.Add(lastLocalResolve);
                return lastLocalResolve;
            }

            return locator.GetService(parameter.ParameterType, true);
        }

        private static bool IsNotResolved(IServiceLocator locator, ParameterInfo[] parameters, object[] localResolves)
        {
            return parameters.All(parameter =>
            {
                bool isContainLocator = locator.TryGetService(parameter.ParameterType, out object paramInstance);
                bool isContainLocal = localResolves?.Any(localResolve => parameter.ParameterType.IsInstanceOfType(localResolve)) == true;
                return isContainLocator || isContainLocal;
            }) == false;
        }

        private static void IAwakeAbleAwake(object service, GameObject gameObject, IServiceLocator locator)
        {
            if (service is not IAwakeFromServiceLocator awakeAble)
            {
                return;
            }

            try
            {
                if (gameObject == null && locator is ServiceLocatorObjectBase locatorGameObject)
                {
                    gameObject = locatorGameObject.gameObject;
                }

                awakeAble.AwakeFromServiceLocator(gameObject);
            }
            catch (Exception ex)
            {
                Debug.LogError($"IAwake.Awake() failed for service {service.GetType().Name}: {ex.Message}\n{ex.StackTrace}");
            }
        }
    }
}