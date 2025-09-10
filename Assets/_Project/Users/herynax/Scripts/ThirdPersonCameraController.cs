using System;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Sources
{
    public class ThirdPersonCameraController : MonoBehaviour
    {
        [SerializeField] private float zoomSpeed = 2f;
        [SerializeField] private float zoomLerpSpeed = 10f;
        [SerializeField] private float minDistance = 3f;
        [SerializeField] private float maxDistance = 15;

        private CharacterInput controls;

        private CinemachineCamera cam;
        private CinemachineOrbitalFollow orbital;
        private Vector2 scrollDelta;

        private float targetZoom;
        private float currentZoom;

        void Start()
        {
            controls = new CharacterInput();
            controls.Enable();
            controls.Camera.MouseZoom.performed += HandleMouseScroll;

            Cursor.lockState = CursorLockMode.Locked;

            cam = GetComponent<CinemachineCamera>();
            orbital = cam.GetComponent<CinemachineOrbitalFollow>();

            targetZoom = currentZoom = orbital.Radius;

        }

        private void HandleMouseScroll(InputAction.CallbackContext context)
        {
            scrollDelta = context.ReadValue<Vector2>();

            
        }

        void Update()
        {
            if (scrollDelta.y != 0)
            {
                if(orbital != null)
                {
                    targetZoom = Mathf.Clamp(orbital.Radius - scrollDelta.y * zoomSpeed, minDistance, maxDistance);
                    scrollDelta = Vector2.zero;
                }
            }

            float bumperDelta = controls.Camera.GamepadZoom.ReadValue<float>();
            if (bumperDelta != 0)
            {
                targetZoom = Mathf.Clamp (orbital.Radius - bumperDelta * zoomSpeed, minDistance, maxDistance);
            }

            currentZoom = Mathf.Lerp(currentZoom, targetZoom, Time.deltaTime * zoomLerpSpeed);
            orbital.Radius = currentZoom;
        }
    }
}
