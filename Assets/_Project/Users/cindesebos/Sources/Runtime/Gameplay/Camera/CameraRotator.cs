using UnityEngine;
using Zenject;
using UnityEngine.InputSystem;
using Sources.Runtime.Services.ProjectConfigLoader;
using Unity.Cinemachine;

namespace Sources.Runtime.Gameplay.Camera
{
    public class CameraRotator : MonoBehaviour
    {
        [SerializeField] private CinemachineInputAxisController _cinemachineInputAxisController;

        private CursorView _cursorView;

        [Inject]
        private void Construct(CursorView cursorView)
        {
            _cursorView = cursorView;
        }

        private void Start()
        {
            Enable();
        }

        public void Enable()
        {
            _cursorView.Hide();

            _cinemachineInputAxisController.enabled = true;
        }

        public void Disable()
        {
            _cursorView.Show();

            _cinemachineInputAxisController.enabled = false;
        }
    }
}
