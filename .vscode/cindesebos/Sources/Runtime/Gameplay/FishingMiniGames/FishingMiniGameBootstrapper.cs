using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using Zenject;
using System;
using Sources.Runtime.Services.ProjectConfigLoader;
using Sources.Runtime.Gameplay.FishingMiniGames.Types;

namespace Sources.Runtime.Gameplay.FishingMiniGames
{
    public class FishingMiniGameBootstrapper : MonoBehaviour
    {
        private const float InitialProgressValue = 0;
        private const float InitialSliderValue = 50;
        [SerializeField] private FishingMiniGameView _view;
        [Space]

        [SerializeField] private Slider _progressSlider;
        [SerializeField] private TextMeshProUGUI _progressText;
        [SerializeField] private Slider _pointerSlider;
        [Space]

        [SerializeField] private Fish _commonFishPrefab;
        [SerializeField] private Fish _goldFishPrefab;
        [SerializeField] private FishSlot[] _fishSlots;

        private IFishingMiniGame _currentMiniGame;
        private CharacterInput _characterInput;
        private IProjectConfigLoader _projectConfigLoader;

        [Inject]
        private void Construct(CharacterInput characterInput, IProjectConfigLoader projectConfigLoader)
        {
            _characterInput = characterInput;
            _projectConfigLoader = projectConfigLoader;
        }

        private void Start()
        {
            Initialize();

            Launch();
        }

        public void Initialize()
        {
            _progressSlider.value = InitialProgressValue;
            _progressText.text = $"{Mathf.RoundToInt(InitialProgressValue)}%";
            _pointerSlider.value = InitialSliderValue;

            _currentMiniGame = new MovablePointerFishingMiniGame(_progressSlider, _progressText, _pointerSlider, _characterInput, _projectConfigLoader);
        }

        public async UniTask Launch()
        {
            _view.Initialize(_projectConfigLoader);

            SpawnFishes();

            await _view.OnShow();

            _characterInput.MiniGames.StopMovingPointer.performed += OnStopMovingPointer;

            _currentMiniGame.Launch();
        }

        private void OnStopMovingPointer(InputAction.CallbackContext context)
        {
            _characterInput.MiniGames.StopMovingPointer.performed -= OnStopMovingPointer;

            _currentMiniGame.End();
        }

        public void SpawnFishes()
        {
            int goldFishIndex = UnityEngine.Random.Range(0, _fishSlots.Length);

            foreach (var slot in _fishSlots)
            {
                if (slot.IsOccupied == true) continue;

                slot.CurrentFish = Instantiate(slot == _fishSlots[goldFishIndex] ? _commonFishPrefab : _goldFishPrefab, slot.SpawnPoint);
                slot.CurrentFish.transform.SetParent(slot.SpawnPoint);

                slot.IsOccupied = true;
            }
        }

        private void OnDestroy()
        {
            _characterInput.MiniGames.StopMovingPointer.performed -= OnStopMovingPointer;
        }
    }

    [Serializable]
    public class FishSlot
    {
        public Fish CurrentFish;
        public Transform SpawnPoint;
        public bool IsOccupied = false;
    }
}