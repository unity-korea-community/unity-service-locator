using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace UNKO.ServiceLocator
{
    public class ServiceLocatorObjectBase : MonoBehaviour, IServiceLocator
    {
        public bool IsAlreadyAwake { get; private set; }

        public IReadOnlyDictionary<Type, object> AllServices => _logic.AllServices;
        public UnityEvent<Type> OnServiceRegistered => _logic.OnServiceRegistered;

        public bool ApplicationIsQuitting => _logic.ApplicationIsQuitting;
        [SerializeField]
        protected ServiceLocatorLogic _logic = new ServiceLocatorLogic();

        [SerializeField]
        protected List<ServiceLocatorRegistSOBase> _registList = new List<ServiceLocatorRegistSOBase>();

        public virtual void Awake()
        {
            if (IsAlreadyAwake)
            {
                return;
            }
            IsAlreadyAwake = true;

            foreach (ServiceLocatorRegistSOBase registSO in _registList)
            {
                registSO.RegisterServices(this);
            }

            OnAwake();
        }

        protected virtual void OnAwake()
        {
            _logic.SetMono(this);
        }

        protected virtual void OnDestroy()
        {
            this.Dispose();
        }

        virtual public object GetService(Type type, bool printNotFoundError)
        {
            Awake();
            return _logic.GetService(type, printNotFoundError);
        }

        virtual public void RegisterService<T>(T service, params Type[] types) where T : class
        {
            Awake();
            _logic.RegisterService(service, types.Append(typeof(T)).ToArray());
        }

        virtual public void RegisterService<T>(UnityEngine.Object unityObject, T service, params Type[] types) where T : class
        {
            Awake();
            _logic.RegisterService(unityObject, service, types.Append(typeof(T)).ToArray());
        }

        virtual public void UnregisterService(System.Type type, bool callDispose)
        {
            _logic.UnregisterService(type, callDispose);
        }

        public void Dispose()
        {
            _logic.Dispose();
        }

    }
}