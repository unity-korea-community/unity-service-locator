using System;
using System.Collections.Generic;
using UnityEngine;

namespace UNKO.ServiceLocator
{
    public static class IDisposableEx
    {
        public static IDisposable AddToDisable(this IDisposable disposable, MonoBehaviour mono)
        {
            var eventListener = mono.gameObject.GetComponent<UnityEventListener>();
            if (eventListener == null)
            {
                eventListener = mono.gameObject.AddComponent<UnityEventListener>();
            }
            eventListener.AddEvent(UnityEventListener.When.OnDisable, disposable.Dispose);

            return disposable;
        }

        public static IDisposable AddToDisableOnce(this IDisposable disposable, MonoBehaviour mono)
        {
            var eventListener = mono.gameObject.GetComponent<UnityEventListener>();
            if (eventListener == null)
            {
                eventListener = mono.gameObject.AddComponent<UnityEventListener>();
            }
            eventListener.AddOnceEvent(UnityEventListener.When.OnDisable, disposable.Dispose);

            return disposable;
        }

        public static IDisposable AddToDestroy(this IDisposable disposable, MonoBehaviour mono)
        {
            var eventListener = mono.gameObject.GetComponent<UnityEventListener>();
            if (eventListener == null)
            {
                eventListener = mono.gameObject.AddComponent<UnityEventListener>();
            }

            eventListener.AddEvent(UnityEventListener.When.OnDestroy, disposable.Dispose);

            return disposable;
        }

        public static void AddTo(this IDisposable disposable, ICollection<IDisposable> disposables)
        {
            disposables.Add(disposable);
        }
    }
}