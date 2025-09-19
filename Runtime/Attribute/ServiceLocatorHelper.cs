using UnityEngine;

namespace UNKO.ServiceLocator
{
    /// <summary>
    /// <see cref="ServiceLocatorRegister"/>와 <see cref="ServiceLocatorAttributeResolver"/>를 합친 클래스
    /// </summary>
    [DefaultExecutionOrder(Constants.EXECUTION_ORDER_RESOLVER)]
    public class ServiceLocatorHelper : ServiceLocatorRegister
    {
        protected override void Awake()
        {
            base.Awake();

            var components = GetComponents<MonoBehaviour>();
            foreach (var component in components)
            {
                if (component == this)
                {
                    continue;
                }

                ServiceLocator.ResolveServiceAttribute(component);
            }
        }
    }
}
