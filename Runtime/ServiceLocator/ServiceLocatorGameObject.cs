using System.Linq;

namespace UNKO.ServiceLocator
{
    public class ServiceLocatorGameObject : ServiceLocatorObjectBase
    {
        public override T GetService<T>()
        {
            return base.GetService<T>() ??
                GetServiceFromParents<T>() ??
                ServiceLocator.SceneOf(this).GetService<T>() ??
                ServiceLocator.Global.GetService<T>();
        }

        private T GetServiceFromParents<T>() where T : class
        {
            return GetComponentsInParent<ServiceLocatorGameObject>()
                .FirstOrDefault(x => x != this)
                ?.GetService<T>();
        }
    }
}
