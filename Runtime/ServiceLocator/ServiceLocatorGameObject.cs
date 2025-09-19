using System.Linq;
using UnityEngine;

namespace UNKO.ServiceLocator
{
    public class ServiceLocatorGameObject : ServiceLocatorObjectBase
    {
        [SerializeField]
        bool _isRegistTransformGameObject = false;

        protected override void OnAwake()
        {
            base.OnAwake();

            if (_isRegistTransformGameObject)
            {
                RegisterService(transform);
                RegisterService(gameObject);
            }

            Application.quitting += _logic.SetApplicationIsQuitting;
        }

        public override object GetService(System.Type type, bool printNotFoundError)
        {
            // NOTE 게임오브젝트부터 씬까지 찾아보고 없으면 전역에서 찾으면서 전역에도 없으면 printNotFoundError에 따라 에러를 출력
            return base.GetService(type, false) ??
                GetServiceFromParents(type) ??
                ServiceLocator.SceneOf(this).GetService(type, false) ??
                ServiceLocator.Global.GetService(type, printNotFoundError);
        }

        private object GetServiceFromParents(System.Type type)
        {
            return GetComponentsInParent<ServiceLocatorGameObject>()
                .FirstOrDefault(x => x != this)
                ?.GetService(type, true);
        }
    }
}
