using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using Zenject;
using System;
using Sources.Runtime.Services.ProjectConfigLoader;
using Sources.Runtime.Gameplay.Camera;
using Sources.Runtime.Utilities;

namespace Sources.Runtime.Gameplay.MiniGames.Fishing
{
    public class FishingMiniGameBootstrapper : MonoBehaviour
    {
        private const float InitialProgressValue = 0;
        private const float InitialSliderValue = 50;

        public event Action OnEnded;

        [field: SerializeField] public FishSlot[] FishSlots { get; private set; }
        [SerializeField] private Transform[] _edgePoints;

        [SerializeField] private FishingMiniGameView _view;
        [Space]

        [SerializeField] private FishingMiniGameProgressView _fishingMiniGameProgressView;
        [SerializeField] private Slider _pointerSlider;
        [Space]

        [SerializeField] private Fish _commonFishPrefab;
        [SerializeField] private Fish _goldFishPrefab;

        private IMiniGame _currentMiniGame;
        private CharacterInput _characterInput;
        private IProjectConfigLoader _projectConfigLoader;
        private CursorHandler _cursorHandler;
        private CameraRotator _cameraRotator;
        private UnityEngine.Camera _camera;

        [Inject]
        private void Construct(CharacterInput characterInput, IProjectConfigLoader projectConfigLoader, CursorHandler cursorHandler, CameraRotator cameraRotator)
        {
            _characterInput = characterInput;
            _projectConfigLoader = projectConfigLoader;
            _cursorHandler = cursorHandler;
            _cameraRotator = cameraRotator;
        }

        public void Initialize()
        {
            _camera ??= UnityEngine.Camera.main;

            _fishingMiniGameProgressView.SetValue(InitialProgressValue);

            _pointerSlider.value = InitialSliderValue;
        }

        public async UniTask LaunchRandomMiniGame(float power)
        {
            Initialize();

            SpawnFishes();

            int randomIndex = UnityEngine.Random.Range(0, 2);

            Debug.Log("Random Mini Game Index is " + randomIndex);

            if (randomIndex == 0)
            {
                _currentMiniGame = new MovablePointerFishingMiniGame(_pointerSlider, _characterInput, _projectConfigLoader);
            }
            else
            {
                _currentMiniGame = new MovableFishesMiniGame(FishSlots, _edgePoints, _projectConfigLoader, _pointerSlider, _camera);
            }

            _view.Initialize(_projectConfigLoader, this);

            await _view.OnShow();

            _currentMiniGame.Launch();

            _currentMiniGame.OnEnded += OnEndMiniGame;

            _characterInput.MiniGames.UseMovingPointer.performed += OnUseMovingPointer;

        }

        private void OnUseMovingPointer(InputAction.CallbackContext context)
        {

            float pointerValue = _pointerSlider.value;

            FishSlot caughtSlot = null;

            foreach (var slot in FishSlots)
            {
                if (slot.IsCaught(pointerValue))
                {
                    caughtSlot = slot;

                    break;
                }
            }

            if (caughtSlot != null)
            {
                Debug.Log($"Caught a fish: {caughtSlot.CurrentFish.name}");

                if (caughtSlot.CurrentFish.Type == FishType.Common)
                {
                    _fishingMiniGameProgressView.AddValue(_projectConfigLoader.ProjectConfig.UIConfig.ValueToAddOnCommonCatch);
                }
                else
                {
                    _fishingMiniGameProgressView.AddValue(_projectConfigLoader.ProjectConfig.UIConfig.ValueToAddOnGoldCatch);
                }
            }
            else
            {
                _fishingMiniGameProgressView.RemoveValue(_projectConfigLoader.ProjectConfig.UIConfig.ValueToRemoveOnMiss);
            }

            if (_fishingMiniGameProgressView.Value <= 0)
                _currentMiniGame.End(false);
            else if (_fishingMiniGameProgressView.Value >= 100)
                _currentMiniGame.End(true);
        }

        public void SpawnFishes()
        {
            int goldFishIndex = UnityEngine.Random.Range(0, FishSlots.Length);

            for (int i = 0; i < FishSlots.Length; i++)
            {
                FishSlot slot = FishSlots[i];
                if (slot.IsOccupied) continue;

                bool isGold = i == goldFishIndex;

                Fish prefab = isGold ? _goldFishPrefab : _commonFishPrefab;
                slot.CurrentFish = Instantiate(prefab, slot.SpawnPoint);
                slot.CatchRange = prefab.CatchRange;
                slot.IsOccupied = true;

                Vector3 fishWorldPos = slot.CurrentFish.transform.position;

                Vector2 screenPos = RectTransformUtility.WorldToScreenPoint(_camera, fishWorldPos);

                float pointerValue = Extensions.MapScreenPositionToSliderValue(screenPos.x, _pointerSlider, _camera);

                slot.CatchCenterValue = pointerValue;
            }
        }

        private void OnEndMiniGame(bool isWin)
        {
            OnEnded?.Invoke();
            _characterInput.MiniGames.UseMovingPointer.performed -= OnUseMovingPointer;

            foreach (var slot in FishSlots)
            {
                if (slot.CurrentFish != null)
                    Destroy(slot.CurrentFish.gameObject);

                slot.IsOccupied = false;
            }

            _cursorHandler.SetCanHandle(true);
            _cameraRotator.OnPanelHide();

            if (_currentMiniGame != null)
                _currentMiniGame.OnEnded -= OnEndMiniGame;
        }

        private void OnDestroy()
        {
            OnEndMiniGame(false);
        }
    }

    [Serializable]
    public class FishSlot
    {
        public Fish CurrentFish;
        public Transform SpawnPoint;
        public bool IsOccupied = false;

        [Range(0, 100)] public float CatchCenterValue;
        public float CatchRange;

        public bool IsCaught(float pointerValue) => Mathf.Abs(pointerValue - CatchCenterValue) <= CatchRange;
    }
}