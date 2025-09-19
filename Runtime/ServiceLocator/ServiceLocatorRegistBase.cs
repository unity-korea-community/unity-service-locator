using UnityEngine;

namespace UNKO.ServiceLocator
{
    public abstract class ServiceLocatorRegistBase : MonoBehaviour
    {
        public virtual int Priority { get;  } = 0;

        public abstract void RegisterServices(IServiceLocator serviceLocator);
    }
}
