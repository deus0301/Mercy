using Mercy.Core;
using UnityEngine;

namespace Mercy.Animation
{
    public class Animate : MonoBehaviour
    {
        private Animator animator;

        void Awake()
        {
            animator = GetComponent<Animator>();
        }
        
        public void SetSpeed(float speed)
        {
            animator.SetFloat("Speed", speed);
        }
        public void IsGrounded(bool value)
        {
            animator.SetBool(GameConstants.animatorGroundCheck, value);
        }
    }    
}
