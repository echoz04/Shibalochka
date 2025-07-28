namespace Sources.Runtime.Core.StateMachine
{
    public abstract class BaseState
    {
        public virtual void Enter() { }
        public virtual void Exit() { }
        public virtual void Tick() { }
    }
}
