using UnityEngine;

namespace Sources.Runtime.Core.StateMachine
{
    public abstract class BaseStateMachine
    {
        private BaseState _currentState;

        public void SetState(BaseState newState)
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
