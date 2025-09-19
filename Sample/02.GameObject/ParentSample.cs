using UnityEngine;

namespace UNKO.ServiceLocator.Sample
{
    public class ParentSample : MonoBehaviour
    {
        void Awake()
        {
            // ServiceLocator.GameObjectOf 두번째 인자로 find in parent를 false로 하면 자기 자신에 ServiceLocator를 추가한다
            ServiceLocator.GameObjectOf(this, false).RegisterService(this);
        }

        public void Print(string callerName)
        {
            Debug.Log($"'Parent' Sample.Print '{name}' from '{callerName}'", this);

            ServiceLocator.GetService<GrandParentSample>(this).Print(name);
        }
    }
}
