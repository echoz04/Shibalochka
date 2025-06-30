using UnityEngine;
using Zenject;
using UnityEngine.InputSystem;

namespace Sources.Runtime.Gameplay.Camera
{
    public class CursorHandler : MonoBehaviour
    {
        public bool IsInteractable { get; private set; }

        public bool CanHandle { get; private set; } = true;

        private CharacterInput _characterInput;

        [Inject]
        private void Construct(CharacterInput characterInput)
        {
            _characterInput = characterInput;
        }

        private void Awake()
        {
            _characterInput.Camera.ToggleCursor.started += Unlock;
            _characterInput.Camera.ToggleCursor.canceled += Lock;

            IsInteractable = false;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        private void Lock(InputAction.CallbackContext context)
        {
            if (CanHandle == false)
                return;

            IsInteractable = false;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        private void Unlock(InputAction.CallbackContext context)
        {
            if (CanHandle == false)
                return;

            IsInteractable = true;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        public void SetCanHandle(bool value)
        {
            CanHandle = value;
        }

        private void OnDestroy()
        {
            _characterInput.Camera.ToggleCursor.started -= Unlock;
            _characterInput.Camera.ToggleCursor.canceled -= Lock;
        }

    }
}
