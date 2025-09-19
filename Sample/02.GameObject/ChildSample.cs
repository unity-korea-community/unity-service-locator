using UnityEngine;

namespace UNKO.ServiceLocator.Sample
{
    public class ChildSample : MonoBehaviour
    {
        void Start()
        {
            ServiceLocator.GetService<ParentSample>(this).Print(this.name);
        }
    }
}
