using System;

namespace Sources.Signals
{
    public interface ISignalBus
    {
        void Fire<TSignal>(TSignal signal);
        void Subscribe<TSignal>(Action<TSignal> signal, bool replayLast);
        void Unsubscribe<TSignal>(Action<TSignal> signal);
    }
}