using System;

namespace Sources.Signals
{
    public class ScreenChangeSignal
    {
        public Type ScreenType { get; }

        public ScreenChangeSignal(Type screenType)
        {
            ScreenType = screenType;
        }
    }
}