using UnityEngine;
using UnityEngine.UI;
using Zenject;
using System;
using Sources.Runtime.Services.ProjectConfigLoader;
using Sources.Runtime.Gameplay.Camera;
using Sources.Runtime.Core.ObjectPool;

namespace Sources.Runtime.Gameplay.MiniGames.Fishing
{
    public class FishingMiniGameBootstrapper : MonoBehaviour
    {
        [SerializeField] private FishingMiniGameDependencies _dependencies;

        private StaminaHandler _staminaHandler;

        [Inject]
        private void Construct(CharacterInput characterInput, IProjectConfigLoader projectConfigLoader, CameraRotator cameraRotator,
        StaminaHandler staminaHandler)
        {
            _dependencies.CharacterInput = characterInput;
            _dependencies.ProjectConfigLoader = projectConfigLoader;
            _dependencies.CameraRotator = cameraRotator;
            _staminaHandler = staminaHandler;
        }

        public void Initialize()
        {
            _dependencies.Camera ??= UnityEngine.Camera.main;

            _dependencies.StateMachine = new FishingMiniGameStateMachine(_dependencies);
        }

        public void Launch(float force)
        {
            _dependencies.StateMachine.SetState(_dependencies.StateMachine.LaunchState);

            _dependencies.StateMachine.EndState.OnEnded += OnEnded;
        }

        private void Update() =>
            _dependencies.StateMachine.Tick();

        public void OnEnded()
        {
            _staminaHandler.ResetShownState();
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

        public ObjectPool<Fish> CommonFishPool;
        public ObjectPool<Fish> GoldFishPool;
    }
}