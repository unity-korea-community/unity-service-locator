using System;
using UnityEngine;
using UnityEngine.Events;

namespace UNKO.ServiceLocator
{
    public class ServiceLocatorObjectBase : MonoBehaviour, IServiceLocator
    {
        public UnityEvent<Type> OnServiceRegistered => _logic.OnServiceRegistered;
        ServiceLocatorLogic _logic = new ServiceLocatorLogic();

        bool _isAlreadyAwake;

        protected virtual void Awake()
        {
            if (_isAlreadyAwake)
            {
                return;
            }
            _isAlreadyAwake = true;

            _logic.RegisterService<IServiceLocator>(this);
        }

        protected virtual void OnDestroy()
        {
            _logic.UnregisterService<IServiceLocator>();
        }

        virtual public T GetService<T>()
            where T : class
        {
            return _logic.GetService<T>();
        }

        virtual public void RegisterService<T>(T service, params Type[] types) where T : class
        {
            _logic.RegisterService(service, types);
        }

        virtual public void UnregisterService<T>()
            where T : class
        {
            _logic.UnregisterService<T>();
        }
    }
}