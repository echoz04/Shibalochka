using UnityEngine;
using Zenject;
using UnityEngine.InputSystem;

namespace Sources.Runtime.Gameplay.Camera
{
    public class CameraRotator : MonoBehaviour
    {
        [SerializeField] private float _rotationSpeed = 100f;
        [SerializeField] private Transform _holder;

        private CharacterInput _characterInput;
        private CursorHandler _cursorHandler;

        private bool _isWorking;

        [Inject]
        private void Construct(CharacterInput characterInput, CursorHandler cursorHandler)
        {
            _characterInput = characterInput;
            _cursorHandler = cursorHandler;
        }

        private void Awake()
        {
            _characterInput.Camera.ToggleCursor.started += StartWorking;
            _characterInput.Camera.ToggleCursor.canceled += StopWorking;
        }

        private void Update()
        {
            Debug.Log(GetCameraLook());

            if (_isWorking == false || _cursorHandler.IsInteractable == false)
                return;

            Rotate();
        }

        private void Rotate()
        {
            Vector2 mousePosition = Mouse.current.position.ReadValue();
            float screenMiddle = Screen.width / 2f;

            float direction = 0;

            if (mousePosition.x < screenMiddle - 2f) direction = 1f;
            else if (mousePosition.x > screenMiddle + 2f) direction = -1f;

            if (direction != 0)
            {
                _holder.Rotate(Vector3.up * direction * _rotationSpeed * Time.deltaTime);
            }
        }

        private void StartWorking(InputAction.CallbackContext context)
        {
            _isWorking = true;
        }

        private void StopWorking(InputAction.CallbackContext context)
        {
            _isWorking = false;
        }

        private void OnDestroy()
        {
            _characterInput.Camera.ToggleCursor.started -= StartWorking;
            _characterInput.Camera.ToggleCursor.canceled -= StopWorking;
        }

        private float GetCameraLook() => _characterInput.Camera.Look.ReadValue<float>();
    }
}
