using Sources.Runtime.Gameplay.Camera;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using Zenject;
using Sources.Runtime.Services.ProjectConfigLoader;
using DG.Tweening;

namespace Sources.Runtime.Gameplay.FishingMiniGames
{
    public class StaminaHandler : MonoBehaviour
    {
        [SerializeField] private GameObject _stamina;
        [SerializeField] private Image _sliderImage;
        [SerializeField] private float _fillDuration = 2f;
        [SerializeField] private float _drainDuration = 2f;

        private CursorHandler _cursorHandler;
        private CharacterInput _characterInput;
        private IProjectConfigLoader _projectConfigLoader;

        private Tween _staminaTween;
        private bool _hasAlreadyShown;

        [Inject]
        private void Construct(CursorHandler cursorHandler, CharacterInput characterInput, IProjectConfigLoader projectConfigLoader)
        {
            _cursorHandler = cursorHandler;
            _characterInput = characterInput;
            _projectConfigLoader = projectConfigLoader;
        }

        private void Start()
        {
            _characterInput.MiniGames.ShowStamina.started += Show;
            _characterInput.MiniGames.ShowStamina.canceled += Hide;
        }

        private void Show(InputAction.CallbackContext context)
        {
            if (_hasAlreadyShown)
                return;

            _cursorHandler.SetCanHandle(false);
            _hasAlreadyShown = true;

            _stamina.SetActive(true);
            _sliderImage.fillAmount = 0f;

            _staminaTween = DOTween.Sequence()
                .Append(_sliderImage.DOFillAmount(1f, _fillDuration).SetEase(Ease.Linear))
                .Append(_sliderImage.DOFillAmount(0f, _drainDuration).SetEase(Ease.Linear))
                .SetLoops(-1);
        }

        private void Hide(InputAction.CallbackContext context)
        {
            if (_staminaTween != null && _staminaTween.IsActive())
                _staminaTween.Kill();

            float result = _sliderImage.fillAmount;
            
            Debug.Log($"[StaminaHandler] Released at value: {result:F2}");

            _stamina.SetActive(false);
            _hasAlreadyShown = false;
            _cursorHandler.SetCanHandle(true);
        }

        private void OnDestroy()
        {
            _characterInput.MiniGames.ShowStamina.started -= Show;
            _characterInput.MiniGames.ShowStamina.canceled -= Hide;

            if (_staminaTween != null && _staminaTween.IsActive())
                _staminaTween.Kill();
        }
    }
}
