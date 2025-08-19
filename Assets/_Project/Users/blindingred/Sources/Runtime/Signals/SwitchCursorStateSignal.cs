namespace Sources.Signals
{
    public class SwitchCursorStateSignal
    {
        public bool State { get; }
        public SwitchCursorStateSignal(bool state) => State = state;
    }
}