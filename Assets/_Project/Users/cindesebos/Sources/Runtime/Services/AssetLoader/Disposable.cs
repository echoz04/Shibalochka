using System;

namespace Sources.Runtime.Services.AssetLoader
{
    public class Disposable<T> : IDisposable where T : UnityEngine.Object
    {
        public T Value { get; }
        
        private Action _onDispose;

        public Disposable(T value, Action onDispose)
        {
            Value = value;
            _onDispose = onDispose;
        }

        public void Dispose()
        {
            _onDispose?.Invoke();
        }
    }
}