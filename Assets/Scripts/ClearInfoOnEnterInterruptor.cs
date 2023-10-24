//**************************************************************************************
// File: ClearInfoOnEnterInterruptor.cs
//
// Purpose: Clears all information that needs to be cleared when any interruptor
// is triggered
//
// Also clears the triggers for all interruptors(just in case multiple are
// set at once.. though this should probably be prevented elsewhere)
//
// TO DO: Look into preventing multiple interruptors being set simultaneously
// AND look into stun lock prevention mechanisms such as small amounts of
// immunity to all types of stun? (maybe just inexpensive stuns... mainly stagger)
//
// Written By: Salvatore Hanusiewicz
//**************************************************************************************

using UnityEngine;
using System.Collections;

public class ClearInfoOnEnterInterruptor : StateMachineBehaviour
{
	#region Declarations
    #endregion
	
	//***********************************************************************
    // Method: OnStateEnter
    //
    // Purpose: Called when a transition starts and the state machine
	// starts to evaluate this state
    //***********************************************************************
	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
        animator.ResetTrigger("AttackBasicLightPressed");
        animator.ResetTrigger("AttackBasicHeavyPressed");
        animator.ResetTrigger("AttackSpecialLightPressed");
        animator.ResetTrigger("AttackSpecialHeavyPressed");
        animator.ResetTrigger("InteractPressed");
        animator.ResetTrigger("EvadePressedDuringSprint");
        animator.ResetTrigger("EvadeTapped");

        animator.ResetTrigger("SpecialActionMajorPressed");
        animator.SetBool("SpecialActionMajorState", false);
        animator.SetInteger("SpecialActionMajorChoice", 0); //-1? could be removed for efficiency. The player controller would alter this anytime it would trigger any special action
        animator.SetInteger("SpecialActionMajorSlot", 0);

        animator.ResetTrigger("SpecialActionMinorPressed");
        animator.SetBool("SpecialActionMinorState", false);
        animator.SetInteger("SpecialActionMinorChoice", 0); //-1? could be removed for efficiency. The player controller would alter this anytime it would trigger any special action
        animator.SetInteger("SpecialActionMinorSlot", 0);

        animator.ResetTrigger("Stagger");
        animator.ResetTrigger("Launch");
        animator.ResetTrigger("KnockDown");
    }
	
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
	//override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	//{
	//	
	//}
	
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