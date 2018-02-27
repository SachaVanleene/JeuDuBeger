using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TPC
{
    public class HandleMovement_Player : MonoBehaviour
    {

        StateManager states;
        Rigidbody rb;

        public bool doAngleCheck = true;

        [SerializeField]
        float degreeRunTreshold = 8f;
        [SerializeField]
        bool useDot = true;

        bool overrideForce;
        bool inAngle;
        bool playingStopAnimation;

        float rotateTimer;
        float velocityChange;
        bool applyJumpForce;

        Vector3 storeDirection;
        InputHandler ih;
        Vector3 curVelocity;
        Vector3 targetVelocity;
        float prevAngle;
        Vector3 prevDir;

        Vector3 overrideDirection;
        float overrideSpeed;
        float forceOverrideTimer;
        float forceOverrideLife;
        bool stopVelocity;

        public void Init(StateManager st, InputHandler inh)
        {
            ih = inh;
            states = st;
            rb = st.rb;
            states.anim.applyRootMotion = false;
        }

        public void Tick()
        {
            if (!overrideForce)
            {
                HandleDrag();
                if (states.onLocomotion)
                    MovementNormal();
                //HandleJump();
            }
            else
            {
                states.horizontal = 0;
                states.vertical = 0;
                OverrideLogic();
            }
        }

        void MovementNormal()
        {
            inAngle = states.inAngle_MoveDir;

            Vector3 h = ih.camTrans.right * states.horizontal;
            Vector3 v = ih.camTrans.forward * states.vertical;

            h.y = 0;
            v.y = 0;

            if (states.onGround)
            {
                if (states.onLocomotion)
                    HandleRotation_Normal(h, v);

                float targetSpeed = states.walkSpeed;

                if (states.run && states.groundAngle <= 5)
                    targetSpeed = states.runSpeed;


                HandleVelocity_Normal(h, v, targetSpeed);

            }

            HandleAnimations_Normal();
        }

        void HandleVelocity_Normal(Vector3 h, Vector3 v, float speed)
        {
            Vector3 curVelocity = rb.velocity;

            if (states.moving)
            {
                targetVelocity = (h + v).normalized * speed;
                velocityChange = 3;
            }
            else
            {
                velocityChange = 2;
                targetVelocity = Vector3.zero;
            }

            if (states.obstacleForward || states.shooting)
            {
                rb.velocity = Vector3.zero;
            }
            else
            {
                Vector3 vel = Vector3.Lerp(curVelocity, targetVelocity, Time.deltaTime * velocityChange);
                rb.velocity = vel;
            }


        }

        void HandleRotation_Normal(Vector3 h, Vector3 v)
        {
            if (states.moving)
            {
                storeDirection = (h + v).normalized;

                float targetAngle = Mathf.Atan2(storeDirection.x, storeDirection.z) * Mathf.Rad2Deg;

                if (states.run && doAngleCheck)
                {
                    if (!useDot)
                    {
                        if (Mathf.Abs(prevAngle - targetAngle) > degreeRunTreshold)
                        {
                            prevAngle = targetAngle;
                            PlayAnimSpecial(AnimSpecials.runToStop, false);
                            return;
                        }
                    }
                    else
                    {
                        float dot = Vector3.Dot(prevDir, states.moveDirection);
                        if (dot < 0)
                        {
                            prevDir = states.moveDirection;
                            PlayAnimSpecial(AnimSpecials.runToStop);
                            return;
                        }
                    }
                }

                prevDir = states.moveDirection;
                prevAngle = targetAngle;

                storeDirection += transform.position;
                Vector3 targetDir = (storeDirection - transform.position).normalized;
                targetDir.y = 0;

                if (targetDir == Vector3.zero)
                    targetDir = transform.forward;

                if (states.vertical > 0)
                {
                    Quaternion targetRot = Quaternion.LookRotation(targetDir);
                    transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, Time.deltaTime * velocityChange);
                }

                else
                {

                    Vector3 lookAt = (states.lookHitPosition - transform.position).normalized;
                    lookAt.y = 0;
                    Debug.DrawRay(transform.position, lookAt, Color.cyan);
                    Quaternion targetRot = Quaternion.LookRotation(lookAt);
                    transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, Time.deltaTime * velocityChange);
                }

            }
            else
            {
                Vector3 lookAt = (states.lookHitPosition - transform.position).normalized;
                lookAt.y = 0;
                Quaternion targetRot = Quaternion.LookRotation(lookAt);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, Time.deltaTime * velocityChange);
            }
        }

        void HandleAnimations_Normal()
        {
            Vector3 relativeDirection = transform.InverseTransformDirection(states.moveDirection);

            float h = relativeDirection.x;
            float v = relativeDirection.z;

            if (states.obstacleForward)
                v = 0;

            states.anim.SetFloat(Statics.vertical, v, 0.2f, Time.deltaTime);
            states.anim.SetFloat(Statics.horizontal, h, 0.2f, Time.deltaTime);
            states.anim.SetBool(Statics.moving, states.moving);
        }

        void HandleJump()
        {
            if (states.onGround && states.canJump)
            {
                if (states.jumpInput && !states.jumping && states.onLocomotion
                    && states.curState != StateManager.CharStates.hold
                    && states.curState != StateManager.CharStates.onAir)
                {
                    if (states.curState == StateManager.CharStates.idle)
                    {
                        //TODO: Fix bug.
                        //states.anim.SetBool(Statics.special, true);
                        //states.anim.SetInteger(Statics.specialType, Statics.GetAnimSpecialType(AnimSpecials.jump_idle));
                    }

                    if (states.curState == StateManager.CharStates.moving)
                    {
                        states.LegFront();
                        states.jumping = true;
                        states.anim.SetBool(Statics.special, true);
                        states.anim.SetInteger(Statics.specialType, Statics.GetAnimSpecialType(AnimSpecials.run_jump));
                        states.curState = StateManager.CharStates.hold;
                        states.anim.SetBool(Statics.onAir, true);
                        states.canJump = false;
                    }
                }
            }

            if (states.jumping)
            {
                if (states.onGround)
                {
                    if (!applyJumpForce)
                    {
                        StartCoroutine(AddJumpForce(0));
                        applyJumpForce = true;
                    }
                }
                else
                {
                    states.jumping = false;
                }
            }
            else
            {

            }
        }

        IEnumerator AddJumpForce(float delay)
        {
            yield return new WaitForSeconds(delay);
            rb.drag = 0;
            Vector3 vel = rb.velocity;
            Vector3 forward = transform.forward;
            vel = forward * 3;
            vel.y = states.jumpForce;
            rb.velocity = vel;
            StartCoroutine(CloseJump());
        }

        IEnumerator CloseJump()
        {
            yield return new WaitForSeconds(0.3f);
            states.curState = StateManager.CharStates.onAir;

            states.jumping = false;
            applyJumpForce = false;
            states.canJump = false;

            StartCoroutine(EnableJump());
        }

        IEnumerator EnableJump()
        {
            yield return new WaitForSeconds(1.3f);
            states.canJump = true;
        }

        void HandleDrag()
        {
            if (states.moving || states.onGround == false)
                rb.drag = 0;
            else
                rb.drag = 4;
        }

        public void PlayAnimSpecial(AnimSpecials t, bool sptrue = true)
        {
            int n = Statics.GetAnimSpecialType(t);
            states.anim.SetBool(Statics.special, sptrue);
            states.anim.SetInteger(Statics.specialType, n);
            StartCoroutine(CloseSpecialOnAnim(0.4f));
        }

        IEnumerator CloseSpecialOnAnim(float t)
        {
            yield return new WaitForSeconds(t);
            states.anim.SetBool(Statics.special, false);
        }

        public void AddVelocity(Vector3 direction, float t, float force, bool clamp)
        {
            forceOverrideLife = t;
            overrideSpeed = force;
            overrideForce = true;
            forceOverrideTimer = 0;
            overrideDirection = direction;
            rb.velocity = Vector3.zero;
            stopVelocity = clamp;
        }

        void OverrideLogic()
        {
            rb.drag = 0;
            rb.velocity = overrideDirection * overrideSpeed;

            forceOverrideTimer += Time.deltaTime;

            if (forceOverrideTimer > forceOverrideLife)
            {
                if (stopVelocity)
                    rb.velocity = Vector3.zero;

                stopVelocity = false;
                overrideForce = false;
            }
        }
    }
}