using Mercy.Core;
using Mercy.Movement;
using Mercy.Animation;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Mercy.Control
{
    [RequireComponent(typeof(Mover))]
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private Transform cameraTarget;
        [SerializeField] private Transform playerMesh;

        private Mover mover;
        private Animate animate;
        private float currentAnimSpeed;
        private PlayerInputActions inputActions;
        private Vector2 moveInput;

        void Awake()
        {
            mover = GetComponent<Mover>();
            animate = GetComponentInChildren<Animate>();

            inputActions = new PlayerInputActions();
        }

        void OnEnable()
        {
            inputActions.Player.Enable();
            inputActions.Player.Jump.performed += Jump;
        }

        void Update()
        {
            Move();
            UpdateAnimator();
        }

        private void Move()
        {
            moveInput = inputActions.Player.Move.ReadValue<Vector2>();
            Vector3 inputDir = new Vector3(moveInput.x, 0, moveInput.y);

            Vector3 camForward = Camera.main.transform.forward; camForward.y = 0; camForward.Normalize();
            Vector3 camRight = Camera.main.transform.right; camRight.y = 0; camRight.Normalize();
            Vector3 worldDir = (camForward * inputDir.z + camRight * inputDir.x).normalized;

            if (worldDir.sqrMagnitude > 0.01f)
            {
                Quaternion targetRotation = Quaternion.LookRotation(worldDir);
                playerMesh.rotation = Quaternion.Slerp(playerMesh.rotation, targetRotation, 10f * Time.deltaTime);
            }

            mover.Sprinting(inputActions.Player.Sprint.IsPressed());
            mover.Move(worldDir);
        }

        void OnDisable()
        {
            inputActions.Player.Jump.performed -= Jump;
            inputActions.Player.Disable();
        }

        private void Jump(InputAction.CallbackContext ctx)
        {
            mover.Jump(GameConstants.jumpForce);
        }
        private void UpdateAnimator()
        {
            float targetSpeed = moveInput.magnitude * mover.Speed;
            currentAnimSpeed = Mathf.Lerp(currentAnimSpeed, targetSpeed, 10f * Time.deltaTime);
            animate.SetSpeed(currentAnimSpeed);
            animate.IsGrounded(mover.IsGrounded);
        }
    }    
}
