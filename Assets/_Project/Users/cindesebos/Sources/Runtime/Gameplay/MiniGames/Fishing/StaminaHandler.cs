using Cysharp.Threading.Tasks;
using Sources.Runtime.Gameplay.Camera;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using DG.Tweening;
using Sources.Runtime.Gameplay.Configs;
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
        private ProjectConfig _projectConfig;
        private FishingMiniGameBootstrapper _fishingMiniGameBootstrapper;
        private CameraRotator _cameraRotator;

        private Tween _staminaTween;
        [SerializeField] private bool _canHandle = true;

        [Inject]
        private void Construct(CharacterInput characterInput, ProjectConfig projectConfig,
        FishingMiniGameBootstrapper fishingMiniGameBootstrapper, CameraRotator cameraRotator)
        {
            _characterInput = characterInput;
            _projectConfig = projectConfig;
            _fishingMiniGameBootstrapper = fishingMiniGameBootstrapper;
            _cameraRotator = cameraRotator;
        }
        
        void IStartable.Start()
        {
            _canHandle = true;

            _characterInput.Game.StartPowerBar.started += Handle;
            _characterInput.Game.StartPowerBar.canceled += LaunchFishing;
        }

        private void Handle(InputAction.CallbackContext context)
        {
            // if (_canHandle == false || _inventoryRoot.IsVisible)
            if (_canHandle == false)
                return;

            _cameraRotator.SetState(false);

            _stamina.SetActive(true);
            _sliderImage.fillAmount = 0f;

            _staminaTween = DOTween.Sequence()
                .Append(_sliderImage.DOFillAmount(1f, _projectConfig.UIConfig.StaminaFillDuration).SetEase(Ease.Linear))
                .Append(_sliderImage.DOFillAmount(0f, _projectConfig.UIConfig.StaminaDrainDuration).SetEase(Ease.Linear))
                .SetLoops(-1);
        }

        private void LaunchFishing(InputAction.CallbackContext context)
        {
            // if (_canHandle == false || _inventoryRoot.IsVisible == true)
            if (_canHandle == false)
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

            _cameraRotator.SetState(true);
        }

        private void OnDestroy()
        {
            _characterInput.Game.StartPowerBar.started -= Handle;
            _characterInput.Game.StartPowerBar.canceled -= LaunchFishing;

            if (_staminaTween != null && _staminaTween.IsActive())
                _staminaTween.Kill();
        }
    }
}
