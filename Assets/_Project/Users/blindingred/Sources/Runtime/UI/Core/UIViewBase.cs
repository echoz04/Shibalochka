using System;
using System.Collections.Generic;
using UnityEngine;

namespace Sources
{
    public abstract class UIViewBase : MonoBehaviour, IUIView
    {
        protected Dictionary<string, Action> _actions = new();
        public IReadOnlyDictionary<string, Action> Actions => _actions;

        public virtual void Show(Action onComplete = null)
        {
            gameObject.SetActive(true);
            onComplete?.Invoke();
        }

        public virtual void Hide(Action onComplete = null)
        {
            gameObject.SetActive(false);
            onComplete?.Invoke();
        }

        public virtual void Subscribe(string key, Action action)
        {
            if (_actions.ContainsKey(key))
            {
                _actions[key] += action;
            }
        }

        public virtual void Unsubscribe(string key, Action action)
        {
            if (_actions.ContainsKey(key))
            {
                _actions[key] -= action;
            }
        }
    }
}