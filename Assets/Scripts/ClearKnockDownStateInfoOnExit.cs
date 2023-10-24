//**************************************************************************************
// File: ClearKnockDownStateInfoOnExit.cs
//
// Purpose: Clears the KnockDownAnimSpeedModifier on exiting the KnockDown state
//
// Written By: Salvatore Hanusiewicz
//**************************************************************************************

using UnityEngine;
using System.Collections;

public class ClearKnockDownStateInfoOnExit : StateMachineBehaviour
{
    #region Declarations
    #endregion

    //***********************************************************************
    // Method: OnStateEnter
    //
    // Purpose: Called when a transition starts and the state machine
    // starts to evaluate this state
    //***********************************************************************
    //override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //	
    //}

    //***********************************************************************
    // Method: OnStateUpdate
    //
    // Purpose: Called on each Update frame between OnStateEnter
    // and OnStateExit callbacks
    //***********************************************************************
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //	
    //}

    //***********************************************************************
    // Method: OnStateExit
    //
    // Purpose: Called when a transition ends and the state machine
    // finishes evaluating this state
    //***********************************************************************
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (animator.GetBool("KnockDown"))
        {
            Debug.Log(animator.gameObject.name + " knockdown queued on exit");
        }

        Debug.Log(stateInfo.fullPathHash + ", " + animator.GetCurrentAnimatorStateInfo(0).fullPathHash);

        animator.SetFloat("KnockDownAnimSpeedModifier", 0);
    }

    //***********************************************************************
    // Method: OnStateMove
    //
    // Purpose: Called right after Animator.OnAnimatorMove(). Code that
    // processes and affects root motion should be implemented here
    //***********************************************************************
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //	
    //}


    //***********************************************************************
    // Method: OnStateIK
    //
    // Purpose: Called right after Animator.OnAnimatorIK(). Code that
    // sets up animation IK (inverse kinematics) should be implemented here.
    //***********************************************************************
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //	
    //}

    #region Properties
    #endregion
}