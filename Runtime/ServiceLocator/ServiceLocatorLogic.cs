using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace UNKO.ServiceLocator
{
    [System.Serializable]
    public sealed class ServiceLocatorLogic : IServiceLocator
    {
        [System.Serializable]
        public struct ServiceInfoForDebug
        {
            // NOTE inspector에서 collection의 요소를 편하게 보기 위해 필드 순서의 맨 위에 string 배치
            [SerializeField]
            string _typeName;
            [SerializeField]
            Type _type; public Type Type => _type;
            [SerializeField]
            object _object;
            [SerializeField]
            UnityEngine.Object _asUnityObject;
            [SerializeField]
            UnityEngine.Object _unityContext;
            [SerializeField]
            int _registIndex;
            [SerializeField]
            bool _isCurrentRegistered;

            public ServiceInfoForDebug(Type type, object obj, UnityEngine.Object unityContext, int registIndex, bool isCurrentRegistered)
            {
                _type = type;

                if (type.GenericTypeArguments.Length > 0)
                {
                    _typeName = $"{type.Name}<{string.Join(",", type.GenericTypeArguments.Select(type => type.Name))}>";
                }
                else
                {
                    _typeName = type.Name;
                }

                _object = obj;
                _asUnityObject = obj as UnityEngine.Object;
                _unityContext = unityContext;
                _registIndex = registIndex;
                _isCurrentRegistered = isCurrentRegistered;
            }

            public void SetCurrentRegistFalse()
            {
                _isCurrentRegistered = false;
            }
        }

        public IReadOnlyDictionary<Type, object> AllServices => _services;
        readonly Dictionary<Type, object> _services = new Dictionary<Type, object>();

        public UnityEvent<Type> OnServiceRegistered { get; private set; } = new UnityEvent<Type>();

        [SerializeField]
        List<ServiceInfoForDebug> _allServicesForDebug = new List<ServiceInfoForDebug>();

        public bool ApplicationIsQuitting { get; private set; }
        Dictionary<Type, IDisposable> _disposableByType = new Dictionary<Type, IDisposable>();

        // NOTE 같은 IDisposable을 여러 타입에서 등록할 수 있는 경우가 있어서
        Dictionary<IDisposable, List<Type>> _typeByDisposable = new Dictionary<IDisposable, List<Type>>();

        MonoBehaviour _mono;

        public void SetMono(MonoBehaviour owner)
        {
            _mono = owner;
        }

        public object GetService(Type type, bool printNotFoundError)
        {
            if (_services.TryGetValue(type, out object obj) == false)
            {
                if (printNotFoundError && ApplicationIsQuitting == false)
                {
                    Debug.LogError($"{_mono?.name} ServiceLocator.GetService: service not found, type:{type.Name}", _mono);
                }
                return null;
            }

            return obj;
        }

        public void RegisterService<T>(T service, params Type[] types) where T : class
            => RegisterService(null, service, types);

        public void RegisterService<T>(UnityEngine.Object unityObject, T service, params Type[] types) where T : class
        {
            if (service == null)
            {
                string typeName = typeof(T).Name;
                Debug.LogWarning($"{_mono?.name} RegisterService<{typeName}> service is null", _mono);
            }

            foreach (Type type in types.Distinct())
            {
                if (_services.ContainsKey(type))
                {
                    _services.Remove(type);
                }

                _services.Add(type, service);
                OnServiceRegistered.Invoke(type);

#if UNITY_EDITOR
                int count = 0;
                foreach (var sameService in _allServicesForDebug.Where(x => x.Type == type))
                {
                    sameService.SetCurrentRegistFalse();
                    count++;
                }
                _allServicesForDebug.Add(new ServiceInfoForDebug(type, service, unityObject, count, true));
#endif

                var disposable = service as IDisposable;
                if (disposable != null)
                {
                    if (_disposableByType.TryAdd(type, disposable) == false)
                    {
                        Debug.LogWarning($"{_mono?.name} ServiceLocatorObjectBase.RegisterService: {type} is already registered as a disposable service. It will be replaced.", _mono);
                    }

                    if (_typeByDisposable.TryGetValue(disposable, out List<Type> disposableTypes) == false)
                    {
                        disposableTypes = new List<Type>();
                        _typeByDisposable.Add(disposable, disposableTypes);
                    }
                    disposableTypes.Add(type);
                }
            }
        }

        public void UnregisterService(System.Type type, bool callDispose)
        {
            if (type == null)
            {
                return;
            }

            if (callDispose)
            {
                if (_disposableByType.TryGetValue(type, out IDisposable disposable))
                {
                    disposable.Dispose();

                    _typeByDisposable.TryGetValue(disposable, out List<Type> types);
                    types?.Clear();
                }
            }

            _services.Remove(type);
            _disposableByType.Remove(type);

#if UNITY_EDITOR
            _allServicesForDebug.RemoveAll(x => x.Type == type);
#endif
        }

        public void Dispose()
        {
            foreach (IDisposable disposable in _disposableByType.Values)
            {
                disposable.Dispose();
            }
            _disposableByType.Clear();
            foreach (List<Type> types in _typeByDisposable.Values)
            {
                types.Clear();
            }
            _typeByDisposable.Clear();

            this.UnregisterServiceAll();
            _services.Clear();
            OnServiceRegistered.RemoveAllListeners();

        }

        public void SetApplicationIsQuitting()
        {
            ApplicationIsQuitting = true;
        }
    }
}