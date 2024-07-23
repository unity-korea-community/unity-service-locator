using System.Collections.Generic;
using UnityEngine;

namespace UNKO.ServiceLocator
{
    [CreateAssetMenu(fileName = "ServiceLocatorGlobalSetting", menuName = "UNKO/ServiceLocator/ServiceLocatorGlobalSetting")]
    public class ServiceLocatorGlobalSetting : ScriptableObject
    {
        public static ServiceLocatorGlobalSetting Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = Resources.Load<ServiceLocatorGlobalSetting>("ServiceLocatorGlobalSetting");
                }

                return _instance;
            }
        }

        static ServiceLocatorGlobalSetting _instance;

        public IReadOnlyList<GameObject> InstantiateObjectsWhenAwake => _instantiateObjectsWhenAwake;

        [SerializeField]
        List<GameObject> _instantiateObjectsWhenAwake = new List<GameObject>();
    }
}
