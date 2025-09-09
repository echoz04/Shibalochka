using System;

namespace Sources.Signals
{
    public struct ScreenChangeSignal
    {
        public Type ScreenType { get; }

        public ScreenChangeSignal(Type screenType)
        {
            ScreenType = screenType;
        }
    }
}