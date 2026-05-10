using Mercy.Core;
using UnityEngine;

namespace Mercy.Movement
{
    [RequireComponent(typeof(CharacterController))]
    public class Mover : MonoBehaviour
    {
        private CharacterController characterController;
        private Vector3 velocity;

        private float speed;

        void Awake()
        {
            characterController = GetComponent<CharacterController>();
            speed = GameConstants.speed;
        }

        public void Sprinting(bool isSprinting)
        {
            if(isSprinting) speed = GameConstants.sprintSpeed;
            else speed = GameConstants.speed;
        }

        public void Move(Vector3 motion)
        {
            Vector3 horizontalMove = motion * speed;

            if(IsGrounded)
                velocity.y = -2.0f;
            else    
               velocity.y += GameConstants.gravity * Time.deltaTime;

            Vector3 finalMove = (horizontalMove + velocity) * Time.deltaTime;
            characterController.Move(finalMove);
        }

        public void Jump(float jump)
        {
            if(!IsGrounded) return;
            velocity.y = jump;
        }

        public bool IsGrounded => characterController.isGrounded;
        public float Speed => speed;
    }    
}
