using UnityEngine;
using UnityEngine.InputSystem;

namespace Sources
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerController : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Transform cameraTransform;

        [Header("Movement")]
        [SerializeField] private float speed = 5f;
        [SerializeField] private float acceleration = 10f;
        [SerializeField] private float deceleration = 10f;

        [Header("Rotation")]
        [SerializeField] private float rotationSpeed = 10f;

        [Header("Jump & Gravity")]
        [SerializeField] private float jumpHeight = 2f;
        [SerializeField] private float gravity = -9.81f;

        private CharacterController controller;
        private Vector2 moveInput;
        private Vector3 velocity;
        private Vector3 currentMove;
        private float smoothVelocity;

        void Awake()
        {
            controller = GetComponent<CharacterController>();
        }

        public void OnMove(InputAction.CallbackContext context)
        {
            moveInput = context.ReadValue<Vector2>();
        }

        public void OnJump(InputAction.CallbackContext context)
        {
            if (context.performed && controller.isGrounded)
            {
                velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            }
        }

        void Update()
        {
            HandleMovement();
            HandleGravity();
        }

        private void HandleMovement()
        {
            Vector3 forward = cameraTransform.forward;
            Vector3 right = cameraTransform.right;
            forward.y = 0f;
            right.y = 0f;
            forward.Normalize();
            right.Normalize();

            Vector3 inputDir = new Vector3(moveInput.x, 0f, moveInput.y).normalized;
            Vector3 targetMove = (forward * moveInput.y + right * moveInput.x);

            float targetSpeed = targetMove.magnitude * speed;
            float currentSpeed = currentMove.magnitude;
            float smoothSpeed = Mathf.SmoothDamp(currentSpeed, targetSpeed, ref smoothVelocity,
                targetSpeed > currentSpeed ? 1f / acceleration : 1f / deceleration);

            currentMove = targetMove.normalized * smoothSpeed;
            controller.Move(currentMove * Time.deltaTime);

            Vector3 horizontalVelocity = new Vector3(currentMove.x, 0, currentMove.z);
            if (horizontalVelocity.sqrMagnitude > 0.001f)
            {
                Quaternion targetRotation = Quaternion.LookRotation(horizontalVelocity);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            }
        }

        private void HandleGravity()
        {
            if (controller.isGrounded && velocity.y < 0)
                velocity.y = -2f;

            velocity.y += gravity * Time.deltaTime;
            controller.Move(velocity * Time.deltaTime);
        }
    }
}
