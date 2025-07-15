using Sources.Runtime.Gameplay.Camera;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using Zenject;
using Sources.Runtime.Services.ProjectConfigLoader;
using DG.Tweening;
using Cysharp.Threading.Tasks;
using Sources.Runtime.Gameplay.MiniGames.Fishing;

namespace Sources.Runtime.Gameplay.MiniGames
{
    public class StaminaHandler : MonoBehaviour
    {
        [SerializeField] private GameObject _stamina;
        [SerializeField] private Image _sliderImage;

        private CursorHandler _cursorHandler;
        private CharacterInput _characterInput;
        private IProjectConfigLoader _projectConfigLoader;
        private FishingMiniGameBootstrapper _fishingMiniGameBootstrapper;
        private CameraRotator _cameraRotator;

        private Tween _staminaTween;
        private bool _hasAlreadyShown;

        [Inject]
        private void Construct(CursorHandler cursorHandler, CharacterInput characterInput, IProjectConfigLoader projectConfigLoader,
        FishingMiniGameBootstrapper fishingMiniGameBootstrapper, CameraRotator cameraRotator)
        {
            _cursorHandler = cursorHandler;
            _characterInput = characterInput;
            _projectConfigLoader = projectConfigLoader;
            _fishingMiniGameBootstrapper = fishingMiniGameBootstrapper;
            _cameraRotator = cameraRotator;
        }

        private void Start()
        {
            _characterInput.MiniGames.ShowStamina.started += Show;
            _characterInput.MiniGames.ShowStamina.canceled += Hide;

            _fishingMiniGameBootstrapper.OnEnded += ResetShownState;
        }

        private void Show(InputAction.CallbackContext context)
        {
            if (_hasAlreadyShown == true || _cursorHandler.IsInteractable == true)
                return;

            _cursorHandler.SetCanHandle(false);
            _cameraRotator.OnPanelShow();

            _stamina.SetActive(true);
            _sliderImage.fillAmount = 0f;

            _staminaTween = DOTween.Sequence()
                .Append(_sliderImage.DOFillAmount(1f, _projectConfigLoader.ProjectConfig.UIConfig.StaminaFillDuration).SetEase(Ease.Linear))
                .Append(_sliderImage.DOFillAmount(0f, _projectConfigLoader.ProjectConfig.UIConfig.StaminaDrainDuration).SetEase(Ease.Linear))
                .SetLoops(-1);
        }

        private void Hide(InputAction.CallbackContext context)
        {
            if (_hasAlreadyShown)
                return;

            if (_staminaTween != null && _staminaTween.IsActive())
                    _staminaTween.Kill();

            _hasAlreadyShown = true;

            float result = _sliderImage.fillAmount;

            _stamina.SetActive(false);

            _fishingMiniGameBootstrapper.LaunchRandomMiniGame(result).Forget();
        }

        private void ResetShownState() => _hasAlreadyShown = false;

        private void OnDestroy()
        {
            _characterInput.MiniGames.ShowStamina.started -= Show;
            _characterInput.MiniGames.ShowStamina.canceled -= Hide;

            _fishingMiniGameBootstrapper.OnEnded -= ResetShownState;

            if (_staminaTween != null && _staminaTween.IsActive())
                _staminaTween.Kill();
        }
    }
}
