using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UNKO.ServiceLocator
{
    public interface IAwakeFromServiceLocator
    {
        void AwakeFromServiceLocator(GameObject gameObjectFromLocator = null);
    }
}
