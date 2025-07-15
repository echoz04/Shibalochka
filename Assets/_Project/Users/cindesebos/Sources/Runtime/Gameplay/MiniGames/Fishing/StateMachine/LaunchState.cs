using System;
using System.Linq;
using Cysharp.Threading.Tasks;
using Sources.Runtime.Core.ObjectPool;
using Sources.Runtime.Core.StateMachine;
using Sources.Runtime.Gameplay.MiniGames.Fishing.Types;
using Sources.Runtime.Utilities;
using UnityEngine;

namespace Sources.Runtime.Gameplay.MiniGames.Fishing
{
    public class LaunchState : State
    {
        private readonly FishingMiniGameDependencies _dependencies;

        private MovableFishesMiniGame _movableFishesMiniGame;
        private MovablePointerFishingMiniGame _movablePointerFishingMiniGame;

        private ObjectPool<Fish> _fishPool;

        public LaunchState(FishingMiniGameDependencies dependencies)
        {
            _dependencies = dependencies;

            _fishPool = new ObjectPool<Fish>();

            SpawnFishes();
            CreateMiniGames();
        }

        public override void Enter()
        {
            LaunchRandomMiniGame().Forget();
        }

        private void CreateMiniGames()
        {
            _movablePointerFishingMiniGame = new MovablePointerFishingMiniGame(_dependencies.PointerSlider, _dependencies.CharacterInput,
            _dependencies.ProjectConfigLoader);

            _movableFishesMiniGame = new MovableFishesMiniGame(_dependencies.FishSlots, _dependencies.EdgePoints, _dependencies.ProjectConfigLoader,
            _dependencies.PointerSlider, _dependencies.Camera);
        }

        private async UniTask LaunchRandomMiniGame()
        {
            InitializeView();

            int randomIndex = UnityEngine.Random.Range(0, 2);

            Debug.Log("Random Mini Game Index is " + randomIndex);

            if (randomIndex == 0)
                _dependencies.StateMachine.CurrentMiniGame = _movablePointerFishingMiniGame;
            else
                _dependencies.StateMachine.CurrentMiniGame = _movableFishesMiniGame;

            await _dependencies.View.Show();

            _dependencies.StateMachine.CurrentMiniGame.Launch();

            _dependencies.StateMachine.SetState(_dependencies.StateMachine.GameplayState);
        }

        private void InitializeView()
        {
            _dependencies.View.Initialize(_dependencies.ProjectConfigLoader);

            _dependencies.ProgressView.SetValue(FishingMiniGameDependencies.INITIAL_PROGRESS_VALUE);
            _dependencies.PointerSlider.value = FishingMiniGameDependencies.INITIAL_SLIDER_VALUE;
        }

        private void SpawnFishes()
        {
            int goldFishIndex = UnityEngine.Random.Range(0, _dependencies.FishSlots.Length);

            for (int i = 0; i < _dependencies.FishSlots.Length; i++)
            {
                FishSlot slot = _dependencies.FishSlots[i];

                if (slot.IsOccupied == true) continue;

                CreateFishInstanceInPool(i, goldFishIndex, slot);

                Vector3 fishWorldPos = slot.CurrentFish.transform.position;

                Vector2 screenPos = RectTransformUtility.WorldToScreenPoint(_dependencies.Camera, fishWorldPos);

                float pointerValue = Extensions.MapScreenPositionToSliderValue(screenPos.x, _dependencies.PointerSlider, _dependencies.Camera);

                slot.CatchCenterValue = pointerValue;
            }
        }

        private void CreateFishInstanceInPool(int currentIndex, int goldFishIndex, FishSlot targetSlot)
        {
            bool isGold = currentIndex == goldFishIndex;

            Fish prefab = isGold ? _dependencies.GoldFishPrefab : _dependencies.CommonFishPrefab;

            targetSlot.CurrentFish = _fishPool.CreateInstance(prefab, targetSlot.SpawnPoint, true);
            targetSlot.CatchRange = prefab.CatchRange;
            targetSlot.IsOccupied = true;
        }
    }
}