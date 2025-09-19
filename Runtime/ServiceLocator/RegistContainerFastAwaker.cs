using System.Collections.Generic;
using UnityEngine;

namespace UNKO.ServiceLocator
{
    public interface IFastAwakener
    {
        bool IsAwaked { get; }

        void Awake();
    }

    public class RegistContainerFastAwaker : ServiceLocatorRegistBase
    {
        [SerializeField]
        List<GameObject> _awakeTargets = new List<GameObject>();

        public override void RegisterServices(IServiceLocator serviceLocator)
        {
            foreach (var target in _awakeTargets)
            {
                target.SendMessage("Awake", SendMessageOptions.DontRequireReceiver);
            }
        }
    }
}
