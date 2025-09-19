using UnityEngine;
using UnityEngine.SceneManagement;

namespace UNKO.ServiceLocator
{
    public class ServiceLocatorScene : ServiceLocatorObjectBase
    {
        static bool _isApplicationQuit = false;

        public override object GetService(System.Type type, bool printNotFoundError)
            => base.GetService(type, false) ?? ServiceLocator.Global.GetService(type, printNotFoundError);

        // 1) 플레이 모드 시작 전에 실행되어 sceneUnloaded 콜백을 미리 등록
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void RegisterSceneUnloadedCallback()
        {
            SceneManager.sceneUnloaded += scene =>
            {
                // 언로드된 씬에 속한 모든 ServiceLocatorScene 오브젝트를 찾아서 즉시 파괴
                var locators = GameObject.FindObjectsOfType<ServiceLocatorScene>();
                foreach (var locator in locators)
                {
                    if (locator.gameObject.scene == scene)
                    {
                        Object.Destroy(locator.gameObject);
                    }
                }
            };
        }

        protected override void OnAwake()
        {
            if (_isApplicationQuit)
            {
                return;
            }

            base.OnAwake();

            if (ServiceLocator.Global == null)
            {
                Debug.LogError("ServiceLocatorScene.Awake: ServiceLocator.Global is null", this);
            }

            ServiceLocator.ConfigureScene(this);

            Application.quitting += _logic.SetApplicationIsQuitting;
        }

        private void OnApplicationQuit()
        {
            _isApplicationQuit = true;
        }

        protected override void OnDestroy()
        {
            if (_isApplicationQuit)
            {
                return;
            }

            base.OnDestroy();

            ServiceLocator.UnconfigureScene(this);
        }
    }
}
