using UnityEngine;

namespace UNKO.ServiceLocator
{
    public abstract class ServiceLocatorRegistSOBase : ScriptableObject
    {
        public abstract void RegisterServices(IServiceLocator serviceLocator);
    }
}
