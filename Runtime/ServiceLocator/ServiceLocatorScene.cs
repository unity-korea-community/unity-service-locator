using UnityEngine;

namespace UNKO.ServiceLocator
{
    public class ServiceLocatorScene : ServiceLocatorObjectBase
    {
        public override T GetService<T>()
        {
            return base.GetService<T>() ?? ServiceLocator.Global.GetService<T>();
        }

        protected override void Awake()
        {
            base.Awake();

            if (ServiceLocator.Global == null)
            {
                Debug.LogError("ServiceLocatorScene.Awake: ServiceLocator.Global is null", this);
            }

            ServiceLocator.ConfigureScene(this);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            ServiceLocator.UnconfigureScene(this);
        }
    }
}
