using UnityEngine;

namespace UNKO.ServiceLocator
{
    /// <summary>
    /// <see cref="FromServiceLocatorAttribute"/>를 사용하는 모노비헤비어들을 찾아 Resolve
    /// </summary>
    [DefaultExecutionOrder(Constants.EXECUTION_ORDER_RESOLVER)]
    public class ServiceLocatorAttributeResolver : MonoBehaviour
    {
        private void Awake()
        {
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
