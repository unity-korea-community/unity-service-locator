using System;
using System.Collections.Generic;
using UnityEngine;

namespace UNKO.ServiceLocator
{
    /// <summary>
    /// Destroy, Disable 시에 이벤트를 받을 수 있는 컴포넌트
    /// </summary>
    public class UnityEventListener : MonoBehaviour
    {
        public enum When
        {
            OnDestroy,
            OnDisable,
        }

        [System.Serializable]
        public struct WhenEvent
        {
            [SerializeField]
            string _name; public string Name => _name;
            [SerializeField]
            When _when; public When When => _when;
            [SerializeField]
            System.Action _action; public System.Action Action => _action;
            [SerializeField]
            bool _isOnce; public bool IsOnce => _isOnce;

            public WhenEvent(When when, Action action)
            {
                _when = when;
                _action = action;
                _isOnce = false;
                _name = when.ToString() + action.Method.Name;
            }

            public WhenEvent(When when, Action action, bool isOnce)
            {
                _when = when;
                _action = action;
                _isOnce = isOnce;
                _name = when.ToString() + action.Method.Name;
            }
        }

        [SerializeField]
        List<WhenEvent> _whenEvents = new List<WhenEvent>();

        public void AddEvent(When when, System.Action action)
        {
            _whenEvents.Add(new WhenEvent(when, action));
        }

        public void AddOnceEvent(When when, System.Action action)
        {
            _whenEvents.Add(new WhenEvent(when, action, true));
        }

        void OnDisable()
        {
            CallEvent(When.OnDisable);
        }

        void OnDestroy()
        {
            CallEvent(When.OnDestroy);
        }

        private void CallEvent(When when)
        {
            foreach (var whenEvent in _whenEvents)
            {
                if (whenEvent.When == when)
                {
                    whenEvent.Action?.Invoke();
                }
            }
            _whenEvents.RemoveAll(x => x.When == when && x.IsOnce);
        }
    }
}