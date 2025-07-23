using UnityEngine;
using UnityEngine.UI;
using Zenject;
using System;
using Sources.Runtime.Services.ProjectConfigLoader;
using Sources.Runtime.Gameplay.Camera;
using Sources.Runtime.Core.ObjectPool;
using Cysharp.Threading.Tasks;
using UnityEngine.InputSystem;
using Sources.Runtime.Gameplay.MiniGames.Fishing.StateMachine;
using Sources.Runtime.Gameplay.Inventory;

namespace Sources.Runtime.Gameplay.MiniGames.Fishing
{
    public class FishingMiniGameBootstrapper : MonoBehaviour
    {
        public event Action OnCatchTimeStarted;
        public event Action OnCatchTiming;
        public event Action OnCatchTimeEnded;

        [SerializeField] private FishingMiniGameDependencies _dependencies;

        private StaminaHandler _staminaHandler;

        private bool _isAlreadyLaunched = false;
        private bool _isSubscribed = false;

        [Inject]
        private void Construct(CharacterInput characterInput, IProjectConfigLoader projectConfigLoader, CameraRotator cameraRotator,
        StaminaHandler staminaHandler, IMiniGameRewardService rewardService, InventoryRoot inventoryRoot)
        {
            _dependencies.CharacterInput = characterInput;
            _dependencies.ProjectConfigLoader = projectConfigLoader;
            _dependencies.CameraRotator = cameraRotator;
            _staminaHandler = staminaHandler;
            _dependencies.RewardService = rewardService;
            _dependencies.InventoryRoot = inventoryRoot;
        }

        public void Initialize()
        {
            _dependencies.Camera ??= UnityEngine.Camera.main;

            _dependencies.StateMachine = new FishingMiniGameStateMachine(_dependencies);
            _dependencies.View.Initialize(this, _dependencies.ProjectConfigLoader);
        }

        public async UniTask Launch(float force)
        {
            _isAlreadyLaunched = false;

            await WaitForCatchingFish();
        }

        public async UniTask WaitForCatchingFish()
        {
            OnCatchTimeStarted?.Invoke();

            var uiConfig = _dependencies.ProjectConfigLoader.ProjectConfig.UIConfig;
            float waitingTime = UnityEngine.Random.Range(uiConfig.MinimumWaitingTime, uiConfig.MaximumWaitingTime);

            await UniTask.WaitForSeconds(waitingTime);

            OnCatchTiming?.Invoke();

            _dependencies.CharacterInput.MiniGames.CatchFish.performed += CatchFish;
            _isSubscribed = true;

            await UniTask.WaitForSeconds(uiConfig.TimeToCatchFish);

            if (_isAlreadyLaunched == false)
            {
                OnCatchTimeEnded?.Invoke();
                _staminaHandler.AllowHandle();
            }

            DisposeSubscribe();
        }

        private void CatchFish(InputAction.CallbackContext context)
        {
            if (_isAlreadyLaunched)
                return;

            _isAlreadyLaunched = true;
            OnCatchTimeEnded?.Invoke();
            Debug.Log("Ты поймал рыбу");

            DisposeSubscribe();

            _dependencies.StateMachine.SetState(_dependencies.StateMachine.LaunchState);
            _dependencies.StateMachine.EndState.OnEnded += OnEnded;
        }

        private void DisposeSubscribe()
        {
            if (_isSubscribed == true)
            {
                _dependencies.CharacterInput.MiniGames.CatchFish.performed -= CatchFish;
                _isSubscribed = false;
            }
        }

        private void Update() =>
            _dependencies.StateMachine.Tick();

        public void OnEnded()
        {
            _staminaHandler.AllowHandle();
            _dependencies.View.Hide();

            _dependencies.StateMachine.EndState.OnEnded -= OnEnded;
        }

        private void OnDisable()
        {
            _dependencies.StateMachine.EndState.OnEnded -= OnEnded;
        }
    }

    [Serializable]
    public class FishingMiniGameDependencies
    {
        public const float INITIAL_PROGRESS_VALUE = 0;
        public const float INITIAL_SLIDER_VALUE = 50;

        [field: SerializeField] public FishSlot[] FishSlots;
        [field: SerializeField] public Transform[] EdgePoints;
        [field: Space]

        [field: SerializeField] public FishingMiniGameView View;
        [field: Space]

        [field: SerializeField] public FishingMiniGameProgressView ProgressView;
        [field: SerializeField] public Slider PointerSlider;
        [field: Space]

        [field: SerializeField] public Fish CommonFishPrefab;
        [field: SerializeField] public Fish GoldFishPrefab;
        [field: Space]

        [field: SerializeField] public CameraRotator CameraRotator;
        [field: SerializeField] public UnityEngine.Camera Camera;

        public CharacterInput CharacterInput;
        public IProjectConfigLoader ProjectConfigLoader;
        public FishingMiniGameStateMachine StateMachine;
        public IMiniGameRewardService RewardService;
        public InventoryRoot InventoryRoot;

        public ObjectPool<Fish> CommonFishPool;
        public ObjectPool<Fish> GoldFishPool;
    }
}