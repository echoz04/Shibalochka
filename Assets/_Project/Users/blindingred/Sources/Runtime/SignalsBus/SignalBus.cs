using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Sources.Signals
{
    public class SignalBus : ISignalBus
    {
        private readonly Dictionary<Type, List<Delegate>> _subscribers = new();
        private readonly Dictionary<Type, object> _lastSignals = new();
        
        public void Fire<TSignal>(TSignal signal)
        {
            if (!_subscribers.TryGetValue(typeof(TSignal), out var handlers))
            {
                Debug.Log($"No handlers found for {signal.GetType().Name}. Saving for later.");
                _lastSignals[typeof(TSignal)] = signal;
                return;
            }

            foreach (var handler in handlers.Cast<Action<TSignal>>())
            {
                handler.Invoke(signal);
            }
        }

        public void Subscribe<TSignal>(Action<TSignal> signal, bool replayLast = true)
        {
            Debug.Log($"Subscribing to {signal.GetType().Name}");
            if (!_subscribers.TryGetValue(typeof(TSignal), out var handlers))
            {
                _subscribers[typeof(TSignal)] = handlers = new List<Delegate>();
            }
            Debug.Log($"Adding to handlers {signal.GetType().Name}");
            handlers.Add(signal);
            
            if (replayLast && _lastSignals.TryGetValue(typeof(TSignal), out var last))
            {
                Debug.Log($"Replaying last signal {typeof(TSignal).Name}");
                signal((TSignal)last);
            }
        }

        public void Unsubscribe<TSignal>(Action<TSignal> signal)
        {
            if (_subscribers.TryGetValue(typeof(TSignal), out var handlers))
            {
                handlers.Remove(signal);
            }
        }
    }
}