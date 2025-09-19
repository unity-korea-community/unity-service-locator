using UnityEngine;

namespace UNKO.ServiceLocator.Sample
{
    public class GrandParentSample : MonoBehaviour
    {
        void Awake()
        {
            // ServiceLocator.GameObjectOf 두번째 인자로 find in parent를 true로 해도, 루트 오브젝트이기 때문에 자기 자신에 ServiceLocator를 추가한다
            ServiceLocator.GameObjectOf(this).RegisterService(this);
        }

        public void Print(string callerName)
        {
            Debug.Log($"'GrandParent' Sample.Print '{name}' from '{callerName}'", this);
        }
    }
}
