using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UNKO.ServiceLocator
{
    public static partial class ServiceLocator
    {
        static IServiceLocator _global;
        public static IServiceLocator Global
        {
            get
            {
                if (_global == null)
                {
                    ServiceLocatorGlobal global = GameObject.FindFirstObjectByType<ServiceLocatorGlobal>();
                    if (global == null)
                    {
                        var go = new UnityEngine.GameObject(nameof(ServiceLocatorGlobal));
                        global = go.AddComponent<ServiceLocatorGlobal>();
                    }

                    if (global.IsAlreadyAwake == false)
                    {
                        global.Awake();
                    }

                    SceneManager.sceneLoaded += (scene, mode) =>
                    {
                        _isSceneDestroyed[scene] = false;
                    };

                    SceneManager.sceneUnloaded += (scene) =>
                    {
                        _isSceneDestroyed[scene] = true;
                    };
                }

                return _global;
            }
        }

        static Dictionary<Scene, bool> _isSceneDestroyed = new Dictionary<Scene, bool>();

        public static void DefaultSetTimeoutSeconds(float seconds)
            => IServiceLocatorEx.DefaultSetTimeoutSeconds(seconds);

        public static void ResetTimeoutSeconds()
            => IServiceLocatorEx.ResetTimeoutSeconds();

        internal static void ConfigureGlobal(ServiceLocatorGlobal globalLocator)
        {
            if (_global != null)
            {
                Debug.LogError("ServiceLocator.ConfigureGlobal: Another ServiceLocator is already configured as global", globalLocator);
                return;
            }

            _global = globalLocator;

            if (ServiceLocatorGlobalSetting.Instance != null)
            {
                foreach (var go in ServiceLocatorGlobalSetting.Instance.InstantiateObjectsWhenAwake)
                {
                    UnityEngine.Object.Instantiate(go, globalLocator.transform);
                }
            }
        }

        internal static void UnconfigureGlobal(ServiceLocatorGlobal globalLocator)
        {
            if (_global == null || (object)_global == globalLocator)
            {
                _global = null;
            }
            else
            {
                Debug.LogError("ServiceLocator.UnconfigureGlobal: Another ServiceLocator is already configured as global", globalLocator);
            }
        }

        static Dictionary<Scene, IServiceLocator> _serviceByScene = new Dictionary<Scene, IServiceLocator>();

        public static IServiceLocator SceneOf(GameObject gameObject)
            => SceneOf(gameObject.transform);

        public static IServiceLocator SceneOf(string sceneName)
        {
            Scene scene = SceneManager.GetSceneByName(sceneName);
            if (scene.IsValid() == false)
            {
                Debug.LogWarning($"ServiceLocator.SceneOf: Scene '{sceneName}' is not valid. Returning Global.");
                return Global;
            }

            if (_serviceByScene.TryGetValue(scene, out var container) == false)
            {
                container = GameObject.FindFirstObjectByType<ServiceLocatorScene>();
                if (container == null)
                {
                    var go = new GameObject(nameof(ServiceLocatorScene) + "_" + scene.name);
                    container = go.AddComponent<ServiceLocatorScene>();
                }
                else
                {
                    _serviceByScene.Add(scene, container);
                }
            }

            return container;
        }

        public static IServiceLocator SceneOf(Component mono)
        {
            Scene scene = mono.gameObject.scene;
            if (_isSceneDestroyed.TryGetValue(scene, out bool isDestroyed) && isDestroyed)
            {
                Debug.LogWarning($"ServiceLocator.SceneOf: Scene '{scene.name}' is already destroyed. Returning null.");
                return Global;
            }

            if (scene.name == null)
            {
                return Global;
            }

            if (_serviceByScene.TryGetValue(scene, out var container) == false)
            {
                container = GameObject.FindFirstObjectByType<ServiceLocatorScene>();
                if (container == null)
                {
                    var go = new GameObject(nameof(ServiceLocatorScene) + "_" + scene.name);
                    container = go.AddComponent<ServiceLocatorScene>();
                }
                else
                {
                    _serviceByScene.Add(scene, container);
                }
            }

            return container;
        }

        internal static void ConfigureScene(ServiceLocatorScene sceneLocator)
        {
            Scene scene = sceneLocator.gameObject.scene;

            if (_serviceByScene.ContainsKey(scene))
            {
                Debug.LogError("ServiceLocator.ConfigureForScene: Another ServiceLocator is already configured for this scene", sceneLocator);
                return;
            }

            _serviceByScene.Add(scene, sceneLocator);
        }

        internal static void UnconfigureScene(ServiceLocatorScene sceneLocator)
        {
            Scene scene = sceneLocator.gameObject.scene;
            if (scene.name == null)
            {
                return;
            }

            if (_serviceByScene.ContainsKey(scene) == false)
            {
                Debug.LogError("ServiceLocator.UnconfigureScene: ServiceLocator is not configured for this scene", sceneLocator);
                return;
            }

            _serviceByScene.Remove(scene);
        }

        public static IServiceLocator GameObjectOf(Component component, bool findInParentObject = true)
            => GameObjectOf(component, findInParentObject, true);

        public static IServiceLocator GameObjectOf(Component component, bool findInParentObject, bool isThrowNotFound)
        {
            if (component == null)
            {
                return Global;
            }

            if (component.TryGetComponent(out ServiceLocatorGameObject locator))
            {
                return locator;
            }

            if (findInParentObject)
            {
                var parentLocator = component.GetComponentInParent<ServiceLocatorGameObject>(true);
                if (parentLocator != null)
                {
                    return parentLocator;
                }
            }

            if (isThrowNotFound)
            {
                Debug.LogException(
                    new System.Exception($"{component.name}에서 {typeof(ServiceLocatorGameObject).Name}를 못찾았습니다. Inspector에 {typeof(ServiceLocatorGameObject).Name}를 미리 추가해주세요.\nfindInParentObject:{findInParentObject}"),
                    component);
            }

            return null;
        }

        public static T GetService<T>() where T : class
            => Global.GetService<T>();

        public static T GetService<T>(Component component) where T : class
        {
            if (component == null)
            {
                return Global.GetService<T>();
            }

            IServiceLocator locatorOrNull = GameObjectOf(component, true, false);
            if (locatorOrNull != null)
            {
                return locatorOrNull.GetService<T>();
            }

            return SceneOf(component).GetService<T>();
        }

        public static async Task<T> GetServiceAsync<T>() where T : class
            => await Global.GetServiceAsync<T>();

        public static async Task<T> GetServiceAsync<T>(Component component) where T : class
        {
            IServiceLocator locatorOrNull = GameObjectOf(component, true, false);
            if (locatorOrNull != null)
            {
                return await locatorOrNull.GetServiceAsync<T>();
            }

            return await SceneOf(component).GetServiceAsync<T>();
        }

        public static void RegisterServiceToGlobal<T>(T service) where T : class
            => Global.RegisterService(service);

        public static void DisposeAllServiceLocator()
        {
            foreach (var scene in _serviceByScene.Values)
            {
                scene?.Dispose();
            }
            _serviceByScene.Clear();
            _global?.Dispose();

            foreach (var go in GameObject.FindObjectsByType<ServiceLocatorGameObject>(FindObjectsSortMode.None))
            {
                go?.Dispose();
            }
        }
    }
}