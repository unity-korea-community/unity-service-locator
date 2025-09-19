using UnityEngine;

namespace UNKO.ServiceLocator.Sample
{
    public class GlobalSample : MonoBehaviour
    {
        void Awake()
        {
            // NOTE global에 ISample을 regist하는 곳은 RegistContainer에
            ServiceLocator.GetService<ISample>(this).Print(name);
        }
    }
}
