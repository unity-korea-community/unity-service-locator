using UnityEngine;

namespace UNKO.ServiceLocator
{
    public interface ISample
    {
        void Print(string callerName);
    }

    public class PureClassSample : ISample
    {
        public void Print(string callerName)
        {
            Debug.Log($"'PureClass' Sample.Print '{callerName}'");
        }
    }

    [CreateAssetMenu(menuName = "UNKO/ServiceLocator/Sample/RegistContainerSample")]
    public class RegistContainerSample : ServiceLocatorRegistSOBase
    {
        public override void RegisterServices(IServiceLocator serviceLocator)
        {
            serviceLocator.RegisterServiceAndInterfacesFromNew<PureClassSample>();
        }
    }
}
