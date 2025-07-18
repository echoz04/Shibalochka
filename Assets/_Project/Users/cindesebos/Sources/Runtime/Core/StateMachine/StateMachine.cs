using UnityEngine;

namespace Sources.Runtime.Core.StateMachine
{
    public abstract class StateMachine
    {
        private State _currentState;

        public void SetState(State newState)
        {
            if (_currentState == newState)
                return;

            _currentState?.Exit();
            _currentState = newState;
            _currentState.Enter();
        }

        public void Tick()
        {
            _currentState?.Tick();
        }
    }
}
