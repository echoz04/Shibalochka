using System;

namespace Sources
{
    public interface IUIView
    {
        void Show(Action onComplete);
        void Hide(Action onComplete);

        void Subscribe(string key, Action action);
        void Unsubscribe(string key, Action action);
    }
}