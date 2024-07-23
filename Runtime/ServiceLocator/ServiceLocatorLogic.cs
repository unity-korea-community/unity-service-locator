using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace UNKO.ServiceLocator
{
    [System.Serializable]
    public class ServiceLocatorLogic : IServiceLocator
    {
        readonly Dictionary<Type, object> _services = new Dictionary<Type, object>();
        public UnityEvent<Type> OnServiceRegistered { get; private set; } = new UnityEvent<Type>();

        public T GetService<T>()
            where T : class
        {
            Type type = typeof(T);
            if (_services.TryGetValue(type, out object obj) == false)
            {
                return null;
            }

            return obj as T;
        }

        public void RegisterService<T>(T service)
            where T : class
        {
            if (service == null)
            {
                Debug.LogError($"ServiceLocatorLogic.RegisterService<{typeof(T).Name}> service is null");
                return;
            }

            Type type = typeof(T);
            if (_services.ContainsKey(type))
            {
                _services.Remove(type);
            }

            _services.Add(type, service);
            OnServiceRegistered.Invoke(type);
        }

        public void RegisterService<T>(T service, params Type[] types) where T : class
        {
            RegisterService(service);

            foreach (Type type in types)
            {
                if (_services.ContainsKey(type))
                {
                    _services.Remove(type);
                }

                _services.Add(type, service);
                OnServiceRegistered.Invoke(type);
            }
        }

        public void UnregisterService<T>()
            where T : class
        {
            Type type = typeof(T);
            _services.Remove(type);
        }
    }
}