using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NZ 
{
    public class ResetActionFlag : StateMachineBehaviour
    {
        CharacterManager characterManager;
        // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (characterManager == null)
            {
                characterManager = animator.GetComponent<CharacterManager>();
            }
            //当准备进入并执行状态时，这里是Empty,将flag设为false
            characterManager.isPerformingAction = false;
            characterManager.applyRootMotion = false;
            characterManager.canRotate = true;
            characterManager.canMove = true;
            characterManager.characterLocomotionManager.isRolling = false;
            characterManager.characterAnimatorManager.DisableCanDoCombo();

            if (characterManager.IsOwner)
            {
                characterManager.characterNetworkManager.isJumping.Value = false;
            }
            
        }

        // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
        //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        //{
        //    
        //}

        // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
        //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        //{
        //    
        //}

        // OnStateMove is called right after Animator.OnAnimatorMove()
        //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        //{
        //    // Implement code that processes and affects root motion
        //}

        // OnStateIK is called right after Animator.OnAnimatorIK()
        //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        //{
        //    // Implement code that sets up animation IK (inverse kinematics)
        //}
    }
}
