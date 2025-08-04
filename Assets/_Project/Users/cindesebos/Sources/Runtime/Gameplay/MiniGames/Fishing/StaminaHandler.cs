using Cysharp.Threading.Tasks;
using Sources.Runtime.Gameplay.Camera;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using Sources.Runtime.Services.ProjectConfigLoader;
using DG.Tweening;
using Sources.Runtime.Gameplay.Inventory;
using VContainer;
using VContainer.Unity;

namespace Sources.Runtime.Gameplay.MiniGames.Fishing
{
    public class StaminaHandler : MonoBehaviour, IStartable
    {
        public bool IsStarted => _canHandle == false;

        [SerializeField] private GameObject _stamina;
        [SerializeField] private Image _sliderImage;

        private CharacterInput _characterInput;
        private IProjectConfigLoader _projectConfigLoader;
        private FishingMiniGameBootstrapper _fishingMiniGameBootstrapper;
        private CameraRotator _cameraRotator;
        private InventoryRoot _inventoryRoot;

        private Tween _staminaTween;
        [SerializeField] private bool _canHandle = true;

        [Inject]
        private void Construct(CharacterInput characterInput, IProjectConfigLoader projectConfigLoader,
        FishingMiniGameBootstrapper fishingMiniGameBootstrapper, CameraRotator cameraRotator, InventoryRoot inventoryRoot)
        {
            _characterInput = characterInput;
            _projectConfigLoader = projectConfigLoader;
            _fishingMiniGameBootstrapper = fishingMiniGameBootstrapper;
            _cameraRotator = cameraRotator;
            _inventoryRoot = inventoryRoot;
        }
        
        void IStartable.Start()
        {
            _canHandle = true;

            _characterInput.MiniGames.ShowStamina.started += Handle;
            _characterInput.MiniGames.ShowStamina.canceled += BoostrapFishingMiniGame;
        }

        private void Handle(InputAction.CallbackContext context)
        {
            if (_canHandle == false || _inventoryRoot.IsVisible == true)
                return;

            _cameraRotator.SetActive(false);

            _stamina.SetActive(true);
            _sliderImage.fillAmount = 0f;

            _staminaTween = DOTween.Sequence()
                .Append(_sliderImage.DOFillAmount(1f, _projectConfigLoader.ProjectConfig.UIConfig.StaminaFillDuration).SetEase(Ease.Linear))
                .Append(_sliderImage.DOFillAmount(0f, _projectConfigLoader.ProjectConfig.UIConfig.StaminaDrainDuration).SetEase(Ease.Linear))
                .SetLoops(-1);
        }

        private void BoostrapFishingMiniGame(InputAction.CallbackContext context)
        {
            if (_canHandle == false || _inventoryRoot.IsVisible == true)
                return;

            if (_staminaTween != null && _staminaTween.IsActive())
                _staminaTween.Kill();

            float result = _sliderImage.fillAmount;

            _canHandle = false;

            _stamina.SetActive(false);

            _fishingMiniGameBootstrapper.Launch(result).Forget();
        }

        public void AllowHandle()
        {
            _canHandle = true;

            _cameraRotator.SetActive(true);
        }

        private void OnDestroy()
        {
            _characterInput.MiniGames.ShowStamina.started -= Handle;
            _characterInput.MiniGames.ShowStamina.canceled -= BoostrapFishingMiniGame;

            if (_staminaTween != null && _staminaTween.IsActive())
                _staminaTween.Kill();
        }
    }
}
