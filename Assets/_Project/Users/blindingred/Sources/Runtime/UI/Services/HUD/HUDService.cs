using Sources.Signals;
using UnityEngine;
using UnityEngine.InputSystem;
using VContainer;

namespace Sources.UI.Services
{
    public class HUDService : IUIService
    {
        private CharacterInput _characterInput;
        private ISignalBus _signalBus;
        
        private HUDView _view;
        IUIView IUIService.View => _view;

        private bool _isCursorVisible;

        [Inject]
        private void Construct(
            HUDView hudView,
            CharacterInput characterInput,
            ISignalBus signalBus)
        {
            _characterInput = characterInput;
            _view = hudView;
            _signalBus = signalBus;
        }

        public void Enable()
        {
            Subscribe();
            _view.Show();
            _characterInput.Game.Enable();
            _isCursorVisible = false;
            SwitchCursorState(_isCursorVisible);
        }

        public void Disable()
        {
            Unsubscribe();
            _view.Hide();
            _characterInput.Game.Disable();
        }

        private void Subscribe()
        {
            _view.Subscribe(_view.InventoryKey, OnInventory);
            _view.Subscribe(_view.SettingsKey, OnSettings);
            _characterInput.Game.SwitchCursor.performed += SwitchCursor;
        }

        private void Unsubscribe()
        {
            _view.Unsubscribe(_view.InventoryKey, OnInventory);
            _view.Unsubscribe(_view.SettingsKey, OnSettings);
            _characterInput.Game.SwitchCursor.performed -= SwitchCursor;
        }
        
        private void SwitchCursor(InputAction.CallbackContext ctx)
        {
            SwitchCursorState(!_isCursorVisible);
        }

        private void SwitchCursorState(bool isVisible)
        {
            _signalBus.Fire(new SwitchCursorStateSignal(isVisible));
            _signalBus.Fire(new CameraRotatorStateSignal(!isVisible));
            _isCursorVisible = !_isCursorVisible;
        }

        private void OnInventory()
        {
            Debug.Log("inventory");
        }

        private void OnSettings()
        {
            Debug.Log("Settings");
        }
    }
}