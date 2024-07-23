using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UNKO.ServiceLocator
{
    public static class ServiceLocator
    {
        static IServiceLocator _global;
        public static IServiceLocator Global
        {
            get
            {
                if (_global == null)
                {
                    _global = UnityEngine.GameObject.FindObjectOfType<ServiceLocatorGlobal>();
                    if (_global == null)
                    {
                        var go = new UnityEngine.GameObject(nameof(ServiceLocatorGlobal));
                        _global = go.AddComponent<ServiceLocatorGlobal>();
                    }
                }

                return _global;
            }
        }

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
            if (_global == globalLocator)
            {
                _global = null;
            }
            else
            {
                Debug.LogError("ServiceLocator.UnconfigureGlobal: Another ServiceLocator is already configured as global", globalLocator);
            }
        }

        static Dictionary<Scene, IServiceLocator> _serviceByScene = new Dictionary<Scene, IServiceLocator>();
        public static IServiceLocator SceneOf(MonoBehaviour mono)
        {
            Scene scene = mono.gameObject.scene;

            if (_serviceByScene.TryGetValue(scene, out var container) == false)
            {
                container = UnityEngine.GameObject.FindObjectOfType<ServiceLocatorScene>();
                if (container == null)
                {
                    var go = new UnityEngine.GameObject(nameof(ServiceLocatorScene) + "_" + scene.name);
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

            if (_serviceByScene.ContainsKey(scene) == false)
            {
                Debug.LogError("ServiceLocator.UnconfigureScene: ServiceLocator is not configured for this scene", sceneLocator);
                return;
            }

            _serviceByScene.Remove(scene);
        }

        public static IServiceLocator GameObjectOf(MonoBehaviour mono, bool findInParent = true)
        {
            if (mono.TryGetComponent(out IServiceLocator locator))
            {
                return locator;
            }

            if (findInParent)
            {
                var parentLocator = mono.GetComponentInParent<IServiceLocator>();
                if (parentLocator != null)
                {
                    return parentLocator;
                }
            }

            return mono.gameObject.AddComponent<ServiceLocatorGameObject>();
        }

        public static T GetService<T>(MonoBehaviour mono) where T : class
        {
            if (mono.TryGetComponent(out IServiceLocator locator))
            {
                return locator.GetService<T>();
            }
            else
            {
                return GameObjectOf(mono).GetService<T>();
            }
        }

        public static void RegisterServiceToGlobal<T>(T service) where T : class
        {
            Global.RegisterService(service);
        }
    }
}