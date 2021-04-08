using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleBehaviour : StateMachineBehaviour
{
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
        HeroCombat.instance.canReceiveInput = true;


    }

    //OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //Basic Attack in idle o Crouch
        if (HeroCombat.instance.inputBA)
        {
            Debug.Log("Idle Behaviour Attacke one");
            animator.SetTrigger("Attack1");
            HeroCombat.instance.InputManager();
            HeroCombat.instance.inputBA = false;
        }

        //Up Attack in idle
        if (HeroCombat.instance.inputUpBA)
        {
            Debug.Log("Idle Behaviour Attacke one");
            animator.SetTrigger("UpAttack");
            HeroCombat.instance.InputManager();
            HeroCombat.instance.inputUpBA = false;
        }

    }

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
