namespace Sources.Signals
{
    public struct SwitchCursorStateSignal
    {
        public bool State { get; }
        public SwitchCursorStateSignal(bool state) => State = state;
    }
}