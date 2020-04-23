using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player {
    [RequireComponent(typeof(Rigidbody2D))]
    public class JumpPlayer : MonoBehaviour
    {

        private float x, y, z;
        public float speed;
        public float jumpTakeOffSpeed = 7;
        //public JumpState jumpState = JumpState.Grounded;
        private bool stopJump;
        Rigidbody2D rb;
        public Vector2 jump;
        public float jumpForce = 2.0f;
        public Collider2D ground;

        private bool grounded;

        // Start is called before the first frame update
        void Start()
        {
            Debug.Log("hello");
            rb = GetComponent<Rigidbody2D>();
            jump = new Vector2(0.0f, jumpTakeOffSpeed);
            grounded = true;
        }

        // Update is called once per frame
        void Update()
        {
            grounded = rb.IsTouching(ground);

            if (Input.GetButtonDown("Jump") && grounded)
            {
                //jumpState = JumpState.PrepareToJump;
                rb.AddForce(jump * jumpForce, ForceMode2D.Impulse);
            }
            //UpdateJumpState();
        }

    /*
            void UpdateJumpState()
            {
                jump = false;
                switch (jumpState)
                {
                    case JumpState.PrepareToJump:
                        jumpState = JumpState.Jumping;
                        jump = true;
                        stopJump = false;
                        break;
                    case JumpState.Jumping:
                        if (!IsGrounded)
                        {
                            Schedule<PlayerJumped>().player = this;
                            jumpState = JumpState.InFlight;
                        }
                        break;
                    case JumpState.InFlight:
                        if (IsGrounded)
                        {
                            Schedule<PlayerLanded>().player = this;
                            jumpState = JumpState.Landed;
                        }
                        break;
                    case JumpState.Landed:
                        jumpState = JumpState.Grounded;
                        break;
                }
            }

            protected override void ComputeVelocity()
            {
                if (jump && IsGrounded)
                {
                    velocity.y = jumpTakeOffSpeed * model.jumpModifier;
                    jump = false;
                }
                else if (stopJump)
                {
                    stopJump = false;
                    if (velocity.y > 0)
                    {
                        velocity.y = velocity.y * model.jumpDeceleration;
                    }
                }

                targetVelocity = move * maxSpeed;
            }

            public enum JumpState
            {
                Grounded,
                PrepareToJump,
                Jumping,
                InFlight,
                Landed
            }*/
        //}
    }
}