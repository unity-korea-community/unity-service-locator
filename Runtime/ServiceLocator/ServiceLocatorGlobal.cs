using System.Collections.Generic;

namespace UNKO.ServiceLocator
{
    public class ServiceLocatorGlobal : ServiceLocatorObjectBase
    {
        List<ServiceLocatorBehaviourBase> _onAwakeBehaviours = new List<ServiceLocatorBehaviourBase>();

        protected override void Awake()
        {
            base.Awake();

            ServiceLocator.ConfigureGlobal(this);
            DontDestroyOnLoad(gameObject);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            ServiceLocator.UnconfigureGlobal(this);
        }
    }
}