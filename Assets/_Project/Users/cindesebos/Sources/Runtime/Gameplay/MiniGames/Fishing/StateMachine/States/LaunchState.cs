using Cysharp.Threading.Tasks;
using Sources.Runtime.Core.ObjectPool;
using Sources.Runtime.Core.StateMachine;
using Sources.Runtime.Gameplay.MiniGames.Fishing.FishTypes;
using Sources.Runtime.Gameplay.MiniGames.Fishing.Types;
using Sources.Runtime.Utilities;
using UnityEngine;

namespace Sources.Runtime.Gameplay.MiniGames.Fishing.StateMachine.States
{
    public class LaunchState : BaseState
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

        private void SpawnFishes()
        {
            int goldFishIndex = UnityEngine.Random.Range(0, _dependencies.FishSlots.Length);

            for (int i = 0; i < _dependencies.FishSlots.Length; i++)
            {
                FishSlot slot = _dependencies.FishSlots[i];

                if (slot.IsOccupied == true) continue;

                CreateFishInstanceInPool(i, goldFishIndex, slot);

                Vector3 fishWorldPos = slot.CurrentFish.transform.position;

                Vector2 screenPos = RectTransformUtility.WorldToScreenPoint(null, fishWorldPos);

                float pointerValue = Extensions.MapWorldPositionToSliderValue(slot.CurrentFish.transform.position, _dependencies.PointerSlider);

                slot.CatchCenterValue = pointerValue;
            }
        }

        private void CreateMiniGames()
        {
            _movablePointerFishingMiniGame = new MovablePointerFishingMiniGame(_dependencies.PointerSlider,
            _dependencies.ProjectConfig);

            _movableFishesMiniGame = new MovableFishesMiniGame(_dependencies.FishSlots, _dependencies.EdgePoints, _dependencies.ProjectConfig,
            _dependencies.PointerSlider, _dependencies.Camera);
        }

        private async UniTask LaunchRandomMiniGame()
        {
            InitializeView();

            int randomIndex = UnityEngine.Random.Range(0, 2);

            Debug.Log("Random Mini Game Index is " + randomIndex);

            //RuntimeManager.PlayOneShot("event:/SFX/GameSFX/Fishing_Rod");

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
            _dependencies.ProgressView.SetValue(FishingMiniGameDependencies.INITIAL_PROGRESS_VALUE);
            _dependencies.PointerSlider.value = FishingMiniGameDependencies.INITIAL_SLIDER_VALUE;
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