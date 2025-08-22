using DG.Tweening;
using Sources.Runtime.Gameplay.Configs;
using Sources.Signals;
using UnityEngine.InputSystem;
using VContainer;

namespace Sources.UI.Services.PowerBar
{
    public class PowerBarService : IUIService
    {

        private ISignalBus _signalBus;
        private PowerBarView _view;
        private CharacterInput _characterInput;
        private Tween _tween;
        private ProjectConfig _projectConfig;

        private float _powerValue;
        
        public IUIView View { get; }

        [Inject]
        private void Construct(
            PowerBarView view,
            ISignalBus signalBus,
            CharacterInput characterInput,
            ProjectConfig projectConfig
        )
        {
            _view = view;
            _signalBus = signalBus;
            _characterInput = characterInput;
            _projectConfig = projectConfig;
        }

        public void Enable()
        {
            Subscribe();
        }

        public void Disable()
        {
            Unsubscribe();
        }

        private void Subscribe()
        {
            _characterInput.Game.StartPowerBar.started += StartThrow;
            _characterInput.Game.StartPowerBar.canceled += LaunchFishing;
        } 
        
        private void Unsubscribe()
        {
            _characterInput.Game.StartPowerBar.started -= StartThrow;
            _characterInput.Game.StartPowerBar.canceled -= LaunchFishing;
        }

        private void StartThrow(InputAction.CallbackContext obj)
        {
            _tween.Kill();
            _view.Show();
            _tween = DOTween.Sequence()
                .Append(DOVirtual.Float(0f, 1f, _projectConfig.UIConfig.StaminaFillDuration,
                    v =>
                    {
                        _view.SetBarValue(v);
                        SetPowerValue(v);
                    }).SetEase(Ease.Linear))
                .Append(DOVirtual.Float(1f, 0f, _projectConfig.UIConfig.StaminaDrainDuration,
                    v =>
                    {
                        _view.SetBarValue(v);
                        SetPowerValue(v);
                    }).SetEase(Ease.Linear))
                .SetLoops(-1);
        }

        private void SetPowerValue(float value)
        {
            _powerValue = value;
        }

        private void LaunchFishing(InputAction.CallbackContext obj)
        {
            _tween.Kill();
            _view.Hide();
            
            
        }
    }
}