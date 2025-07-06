namespace Sources.Runtime.Core.StateMachine
{
    public class State
    {
        public virtual void Enter() { }
        public virtual void Exit() { }
        public virtual void Tick() { }
    }
}
