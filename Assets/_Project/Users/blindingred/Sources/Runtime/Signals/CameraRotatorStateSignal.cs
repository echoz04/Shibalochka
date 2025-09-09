namespace Sources.Signals
{
    public struct CameraRotatorStateSignal
    {
        public bool State { get; }
        public CameraRotatorStateSignal(bool state) => State = state;
    }
}