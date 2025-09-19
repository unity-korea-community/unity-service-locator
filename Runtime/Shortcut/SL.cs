using System.Threading.Tasks;
using UnityEngine;

namespace UNKO.ServiceLocator
{
    /// <summary>
    /// <see cref="ServiceLocator"/> 이름을 단축했습니다. 기능은 동일 
    /// </summary>
    public static partial class SL
    {
        static IServiceLocator _global;
        public static IServiceLocator Global
        {
            get
            {
                if (_global == null)
                {
                    _global = GameObject.FindFirstObjectByType<ServiceLocatorGlobal>();
                    if (_global == null)
                    {
                        var go = new GameObject(nameof(ServiceLocatorGlobal));
                        _global = go.AddComponent<ServiceLocatorGlobal>();
                    }
                }

                return _global;
            }
        }

        public static IServiceLocator SceneOf(Component component)
            => ServiceLocator.SceneOf(component);

        public static IServiceLocator SceneOf(string sceneName)
            => ServiceLocator.SceneOf(sceneName);

        public static IServiceLocator TryGameObjectOf(Component gameObject, bool findInParentObject = true)
            => GameObjectOf(gameObject, findInParentObject, false);

        public static IServiceLocator TryGameObjectOf(GameObject gameObject, bool findInParentObject = true)
            => GameObjectOf(gameObject.transform, findInParentObject, false);

        /// <summary>
        /// 인자의 <see cref="ServiceLocatorGameObject"/>로부터 서비스를 가져옵니다.
        /// </summary>
        public static bool TryGetService<T>(Component gameObject, out T service)
            where T : class
        {
            service = null;
            return GameObjectOf(gameObject, true, false)?.TryGetService(out service) ?? false;
        }

        /// <summary>
        /// 인자의 <see cref="ServiceLocatorGameObject"/>로부터 서비스를 가져옵니다.
        /// </summary>
        public static bool TryGetServiceFromSLGameObject<T>(Component gameObject, bool findInParentObject, out T service)
            where T : class
        {
            service = null;
            return GameObjectOf(gameObject, findInParentObject, false)?.TryGetService(out service) ?? false;
        }

        /// <summary>
        /// 인자의 <see cref="ServiceLocatorGameObject"/>로부터 서비스를 가져옵니다.
        /// </summary>
        public static bool TryGetServiceFromSLGameObject<T>(GameObject gameObject, out T service)
            where T : class
            => TryGetServiceFromSLGameObject(gameObject, true, out service);

        /// <summary>
        /// 인자의 <see cref="ServiceLocatorGameObject"/>로부터 서비스를 가져옵니다.
        /// </summary>
        public static bool TryGetServiceFromSLGameObject<T>(GameObject gameObject, bool findInParentObject, out T service)
            where T : class
            => TryGetServiceFromSLGameObject(gameObject.transform, findInParentObject, out service);

        public static IServiceLocator GameObjectOf(GameObject gameObject, bool findInParentObject = true)
            => GameObjectOf(gameObject.transform, findInParentObject, true);

        public static IServiceLocator GameObjectOf(Component component, bool findInParentObject = true)
            => GameObjectOf(component, findInParentObject, true);

        public static IServiceLocator GameObjectOf(Component component, bool findInParentObject, bool isThrowNotFound)
            => ServiceLocator.GameObjectOf(component, findInParentObject, isThrowNotFound);

        public static T GetService<T>() where T : class
            => ServiceLocator.GetService<T>(null);

        public static T GetService<T>(Component component) where T : class
        {
            if (component == null)
            {
                Debug.LogError("Component is null. Cannot get service.");
                return null;
            }

            return ServiceLocator.GetService<T>(component);
        }

        public static T GetService<T>(GameObject gameObject) where T : class
        {
            if (gameObject == null)
            {
                Debug.LogError("GameObject is null. Cannot get service.");
                return null;
            }

            return ServiceLocator.GetService<T>(gameObject.transform);
        }

        public static async Task<T> GetServiceAsync<T>() where T : class
            => await ServiceLocator.GetServiceAsync<T>(null);

        public static async Task<T> GetServiceAsync<T>(Component component) where T : class
            => await ServiceLocator.GetServiceAsync<T>(component);

        public static async Task<T> GetServiceAsync<T>(GameObject gameObject) where T : class
        {
            if (gameObject == null)
            {
                Debug.LogError("GameObject is null. Cannot get service.");
                return null;
            }

            return await ServiceLocator.GetServiceAsync<T>(gameObject.transform);
        }

        public static void RegisterServiceToGlobal<T>(T service) where T : class
            => ServiceLocator.RegisterServiceToGlobal(service);

        public static void ResolveServiceAttribute(Component component)
            => ServiceLocator.ResolveServiceAttribute(component);

        public static async Task ResolveServiceAttributeAsync(Component component)
            => await ServiceLocator.ResolveServiceAttributeAsync(component, component);
    }
}