namespace Sources.Signals
{
    public class CameraRotatorStateSignal
    {
        public bool State { get; }
        public CameraRotatorStateSignal(bool state) => State = state;
    }
}