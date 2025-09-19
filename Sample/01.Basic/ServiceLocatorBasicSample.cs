using UnityEngine;

namespace UNKO.ServiceLocator.Sample
{
    public class ServiceLocatorBasicSample : MonoBehaviour
    {
        void Awake()
        {
            {
                ServiceLocator.Global.RegisterServiceAndInterfaces(new SampleServiceA());
                var returnValue = ServiceLocator.Global.GetService<ISampleService>().ReturnString();
                Debug.Log("global new:" + returnValue);
            }

            // 이미 Regist된 서비스는 덮어쓰기가 된다.
            {
                ServiceLocator.Global.RegisterServiceAndInterfaces(new SampleServiceB());
                var returnValue = ServiceLocator.Global.GetService<ISampleService>().ReturnString();
                Debug.Log("global override:" + returnValue); // 덮어쓴 B 출력
            }

            // Global 하위로 Scene이 있다. Scene에 없으면 global로부터 얻어온다.
            {
                var returnValue = ServiceLocator.GetService<ISampleService>(this).ReturnString();
                Debug.Log("scene(global):" + returnValue); // global의 B 출력
            }

            // Scene에 있으면 Scene에서 얻어온다. global까지 가지 않는다.
            {
                ServiceLocator.SceneOf(this).RegisterServiceAndInterfaces(new SampleServiceC());
                var returnValue = ServiceLocator.GetService<ISampleService>(this).ReturnString();
                Debug.Log("scene:" + returnValue); // scene의 C 출력
            }

            // Scene 하위로 GameObject가 있다. GameObject에 없으면 Scene으로부터 얻어온다.
            // Global이든 Scene이든 GameObject든 ServiceLocator.GetService<ISampleService>(this)를 통해 찾는다.
            {
                ServiceLocator.GameObjectOf(this).RegisterServiceAndInterfaces(new SampleServiceD());
                var returnValue = ServiceLocator.GetService<ISampleService>(this).ReturnString();
                Debug.Log("gameobject:" + returnValue); // scene의 D 출력
            }

            // GameObject를 Unregist하면 Scene으로부터 얻어온다.
            {
                ServiceLocator.GameObjectOf(this).UnregisterService(typeof(ISampleService));
                var returnValue = ServiceLocator.GetService<ISampleService>(this).ReturnString();
                Debug.Log("gameobject unregist:" + returnValue); // scene의 C 출력
            }
        }
    }
}
