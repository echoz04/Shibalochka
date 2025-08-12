using UnityEngine;
using Unity.Cinemachine;
using VContainer;

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
            SetActive(true);
        }

        public void SetActive(bool state)
        {
            _cinemachineInputAxisController.enabled = state;

            switch (state)
            {
                case true:
                    _cursorView.Hide();
                    break;
                case false:
                    _cursorView.Show();
                    break;
            }
        }
    }
}
