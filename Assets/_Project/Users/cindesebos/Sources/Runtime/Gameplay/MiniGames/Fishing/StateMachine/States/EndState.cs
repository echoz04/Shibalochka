using System;
using FMODUnity;
using Sources.Runtime.Core.StateMachine;

namespace Sources.Runtime.Gameplay.MiniGames.Fishing.StateMachine.States
{
    public class EndState : BaseState
    {
        public event Action OnEnded;

        private readonly FishingMiniGameDependencies _dependencies;

        private bool _gameplayResult;

        public EndState(FishingMiniGameDependencies dependencies)
        {
            _dependencies = dependencies;
        }

        public override void Enter()
        {
            if (_gameplayResult)
            {
                _dependencies.InventoryRoot.TryAddItem(_dependencies.RewardService.GetRandomItem());
                //RuntimeManager.PlayOneShot("event:/SFX/MiniGames/MG_Win");
            }
            else
            {
                //RuntimeManager.PlayOneShot("event:/SFX/MiniGames/MG_Lose");
            }

            _dependencies.StateMachine.CurrentMiniGame.End(_gameplayResult);

            OnEnded?.Invoke();
        }

        public override void Exit()
        {
        }

        public void SetGameplayResult(bool result)
        {
            _gameplayResult = result;
        }
    }
}