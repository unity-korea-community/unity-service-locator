using System;

namespace UNKO.ServiceLocator
{
    public class Disposable : IDisposable
    {
        Action _onDispose;

        public Disposable(Action onDispose)
        {
            _onDispose = onDispose;
        }

        public void Dispose()
        {
            _onDispose?.Invoke();
        }
    }
}