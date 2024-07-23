using System.Collections.Generic;
using UnityEngine;

namespace UNKO.ServiceLocator
{
    [CreateAssetMenu(fileName = "ServiceLocatorInstantiatePrefabBehaviour", menuName = "UNKO/ServiceLocator/InstantiatePrefabBehaviour")]
    public class ServiceLocatorInstantiatePrefabBehaviour : ServiceLocatorBehaviourBase
    {
        [SerializeField]
        List<GameObject> _instantiatedList = new List<GameObject>();

        public override void OnAwakeScene()
        {
            base.OnAwakeScene();

            foreach (var prefab in _instantiatedList)
            {
                Instantiate(prefab);
            }
        }
    }
}
