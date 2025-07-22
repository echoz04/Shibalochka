using UnityEngine;
using Zenject;
using UnityEngine.InputSystem;
using Sources.Runtime.Services.ProjectConfigLoader;
using Sources.Runtime.Gameplay.Configs;

namespace Sources.Runtime.Gameplay.Camera
{
    public class CameraRotator : MonoBehaviour
    {
        [SerializeField] private Transform _holder;
        [SerializeField] private Transform _cameraPivot;

        private CharacterInput _characterInput;
        private CursorView _cursorView;
        private CameraConfig _cameraConfig;

        private float _xRotation;
        private bool _isRotating = false;
        private bool _canRotate = true;

        [Inject]
        private void Construct(CharacterInput characterInput, CursorView cursorView, IProjectConfigLoader projectConfigLoader)
        {
            _characterInput = characterInput;
            _cursorView = cursorView;
            _cameraConfig = projectConfigLoader.ProjectConfig.CameraConfig;
        }

        private void Awake()
        {
            Initialize();

            _characterInput.Camera.Rotate.started += StopWorking;
            _characterInput.Camera.Rotate.canceled += StartWorking;
        }

        private void OnDestroy()
        {
            _characterInput.Camera.Rotate.started -= StopWorking;
            _characterInput.Camera.Rotate.canceled -= StartWorking;
        }

        private void Initialize()
        {
            _isRotating = true;
            _canRotate = true;

            _cursorView.Hide();
        }

        private void Update()
        {
            if (_isRotating == false || _canRotate == false)
                return;

            Vector2 mouseDelta = Mouse.current.delta.ReadValue();

            Rotate(mouseDelta);
        }

        private void Rotate(Vector2 delta)
        {
            float mouseX = delta.x * _cameraConfig.Sensitivity * Time.deltaTime;
            float mouseY = delta.y * _cameraConfig.Sensitivity * Time.deltaTime;

            _holder.Rotate(Vector3.up * mouseX);

            _xRotation -= mouseY;
            _xRotation = Mathf.Clamp(_xRotation, _cameraConfig.MinVerticalAngle, _cameraConfig.MaxVerticalAngle);
            _cameraPivot.localRotation = Quaternion.Euler(_xRotation, 0f, 0f);
        }

        public void OnPanelShow()
        {
            _canRotate = false;
        }

        public void OnPanelHide()
        {
            _canRotate = true;
        }

        private void StartWorking(InputAction.CallbackContext context)
        {
            _cursorView.Hide();

            _isRotating = true;
        }

        private void StopWorking(InputAction.CallbackContext context)
        {
            _cursorView.Show();

            _isRotating = false;
        }
    }
}
