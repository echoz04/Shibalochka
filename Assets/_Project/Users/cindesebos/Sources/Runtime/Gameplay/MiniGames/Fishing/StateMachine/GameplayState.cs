using System;
using System.Diagnostics;
using Sources.Runtime.Core.StateMachine;
using Sources.Runtime.Gameplay.MiniGames.Fishing.Types;
using UnityEngine.InputSystem;

namespace Sources.Runtime.Gameplay.MiniGames.Fishing
{
    public class GameplayState : State
    {
        private readonly FishingMiniGameDependencies _dependencies;

        private bool _canMovePointer = true;
        private float _timeToAllowMovePointer = 0;

        public GameplayState(FishingMiniGameDependencies dependencies)
        {
            _dependencies = dependencies;
        }

        public override void Enter()
        {
            _dependencies.CharacterInput.MiniGames.UseMovingPointer.performed += OnUseMovingPointer;
        }

        public override void Exit()
        {
            _dependencies.CharacterInput.MiniGames.UseMovingPointer.performed -= OnUseMovingPointer;
        }

        public override void Tick()
        {
            if (_canMovePointer == true)
                return;

            if (_timeToAllowMovePointer > 0)
                _timeToAllowMovePointer -= UnityEngine.Time.deltaTime;
            else
                _canMovePointer = true;
        }

        private void OnUseMovingPointer(InputAction.CallbackContext context)
        {
            if (_canMovePointer == false)
                return;

            _canMovePointer = false;

            _timeToAllowMovePointer = _dependencies.ProjectConfigLoader.ProjectConfig.UIConfig.FishingClickCooldown;

            FishSlot caughtFish = TryCatchFish();

            UpdateProgressView(caughtFish);

            CheckGamepalyResult();
        }

        private FishSlot TryCatchFish()
        {
            float pointerValue = _dependencies.PointerSlider.value;

            FishSlot caughtSlot = null;

            foreach (var slot in _dependencies.FishSlots)
            {
                if (slot.IsCaught(pointerValue))
                {
                    caughtSlot = slot;

                    return caughtSlot;
                }
            }

            return null;
        }

        private void UpdateProgressView(FishSlot caughtFish)
        {
            if (caughtFish != null)
            {
                if (caughtFish.CurrentFish.Type == FishType.Common)
                    _dependencies.ProgressView.AddValue(_dependencies.ProjectConfigLoader.ProjectConfig.UIConfig.ValueToAddOnCommonCatch);
                else
                    _dependencies.ProgressView.AddValue(_dependencies.ProjectConfigLoader.ProjectConfig.UIConfig.ValueToAddOnGoldCatch);
            }
            else
            {
                _dependencies.ProgressView.RemoveValue(_dependencies.ProjectConfigLoader.ProjectConfig.UIConfig.ValueToRemoveOnMiss);
            }
        }

        private void CheckGamepalyResult()
        {
            if (_dependencies.ProgressView.Value <= 0)
            {
                UnityEngine.Debug.Log("You lose");

                _dependencies.StateMachine.SetState(_dependencies.StateMachine.EndState, false);
            }
            else if (_dependencies.ProgressView.Value >= 100)
            {
                UnityEngine.Debug.Log("You win");

                _dependencies.StateMachine.SetState(_dependencies.StateMachine.EndState, true);
            }
        }
    }
}