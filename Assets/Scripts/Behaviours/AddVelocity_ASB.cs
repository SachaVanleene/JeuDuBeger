using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Behaviours
{
    public class AddVelocity_ASB : StateMachineBehaviour
    {

        public float life = 0.4f;
        public float force = 6;
        public Vector3 direction;
        [Space]
        [Header("This will override the direction")]
        public bool useTransformForward;
        public bool additive;
        public bool onEnter;
        public bool onExit;
        [Header("When ending, applying velocity, not anim state")]
        public bool onEndClampVelocity;

        TPC.StateManager states;
        TPC.HandleMovement_Player ply;

        // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (onEnter)
            {
                if (useTransformForward && !additive)
                    direction = animator.transform.forward;

                if (useTransformForward && additive)
                    direction += animator.transform.forward;

                if (states == null)
                    states = animator.transform.GetComponent<TPC.StateManager>();

                if (!states.aiming)//.isPlayer)
                    return;

                if (ply == null)
                    ply = animator.transform.GetComponent<TPC.HandleMovement_Player>();

                ply.AddVelocity(direction, life, force, onEndClampVelocity);
            }
        }

        // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
        override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (onEnter)
            {
                if (useTransformForward && !additive)
                    direction = animator.transform.forward;

                if (useTransformForward && additive)
                    direction += animator.transform.forward;

                if (states == null)
                    states = animator.transform.GetComponent<TPC.StateManager>();

                if (!states.aiming)//.isPlayer)
                    return;

                if (ply == null)
                    ply = animator.transform.GetComponent<TPC.HandleMovement_Player>();


                //ply.AddVelocity(direction, life, force, onEndClampVelocity);
            }
        }

        // OnStateMove is called right after Animator.OnAnimatorMove(). Code that processes and affects root motion should be implemented here
        //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        //
        //}

        // OnStateIK is called right after Animator.OnAnimatorIK(). Code that sets up animation IK (inverse kinematics) should be implemented here.
        //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        //
        //}
    }
}