using UnityEngine;

namespace UNKO.ServiceLocator
{
    public class ServiceLocatorGlobal : ServiceLocatorObjectBase
    {
        public event System.Action OnApplicationQuitting;

        bool _applicationQuitting = false;

        protected override void OnAwake()
        {
            base.OnAwake();

            if (_applicationQuitting)
            {
                return;
            }
            ServiceLocator.ConfigureGlobal(this);

            if (Application.isPlaying)
            {
                DontDestroyOnLoad(gameObject);
            }

            Application.quitting += _logic.SetApplicationIsQuitting;
        }

        private void OnApplicationQuit()
        {
            _applicationQuitting = true;
            OnApplicationQuitting?.Invoke();
        }

        protected override void OnDestroy()
        {
            if (_applicationQuitting == false)
            {
                ServiceLocator.UnconfigureGlobal(this);
            }
            base.OnDestroy();
        }
    }
}