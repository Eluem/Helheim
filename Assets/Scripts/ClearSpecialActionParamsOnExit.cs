//**************************************************************************************
// File: ClearSpecialActionParamsOnExit.cs
//
// Purpose: When exiting the SpecialAction SubStateMachine, clears out all the
// paramters that indicates what specialActions were being used.
//
// This allows more complex abilities (with two buttons) to do strange combos within
// the SpecialAction SubStateMachine but resets everything once completed.
//
// Written By: Salvatore Hanusiewicz
//**************************************************************************************

using UnityEngine;
using System.Collections;

public class ClearSpecialActionParamsOnExit : StateMachineBehaviour
{
    #region Declarations
    #endregion

    //***********************************************************************
    // Method: OnStateEnter
    //
    // Purpose: Called before OnStateEnter is called on any state
    // inside this state machine
    //***********************************************************************
    //override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //	
    //}

    //***********************************************************************
    // Method: OnStateUpdate
    //
    // Purpose: Called before OnStateUpdate is called on any state
    // inside this state machine
    //***********************************************************************
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //	
    //}

    //***********************************************************************
    // Method: OnStateExit
    //
    // Purpose: Called before OnStateExit is called on any state
    // inside this state machine
    //***********************************************************************
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //	
    //}

    //***********************************************************************
    // Method: OnStateMove
    //
    // Purpose: Called before OnStateMove is called on any state
    // inside this state machine
    //***********************************************************************
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //	
    //}

    //***********************************************************************
    // Method: OnStateIK
    //
    // Purpose: Called before OnStateIK is called on any state
    // inside this state machine
    //***********************************************************************
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //	
    //}

    //***********************************************************************
    // Method: OnStateMachineEnter
    //
    // Purpose: Called when entering a statemachine via its Entry Node
    //***********************************************************************
    //override public void OnStateMachineEnter(Animator animator, int stateMachinePathHash)
    //{
    //	
    //}

    //***********************************************************************
    // Method: OnStateMachineExit
    //
    // Purpose: Called when exiting a statemachine via its Exit Node
    //***********************************************************************
    override public void OnStateMachineExit(Animator animator, int stateMachinePathHash)
    {
        //If the current SpecialActionMajor is pressed, then allow it to be comboed into and don't clear all the info out
        if (!animator.GetBool("SpecialActionMajorPressed"))
        {
            if (animator.GetBool("SpecialActionMinorPressed"))
            {
                animator.SetTrigger("SpecialActionMajorPressed");
                animator.SetBool("SpecialActionMajorState", animator.GetBool("SpecialActionMinorState"));
                animator.SetInteger("SpecialActionMajorChoice", animator.GetInteger("SpecialActionMinorChoice"));
                animator.SetInteger("SpecialActionMajorSlot", animator.GetInteger("SpecialActionMinorSlot"));
                animator.SetInteger("SpecialActionMajorAmmo", animator.GetInteger("SpecialActionMinorAmmo"));
            }
            else
            {
                //animator.ResetTrigger("SpecialActionMajorPressed");
                animator.SetBool("SpecialActionMajorState", false); //Should this be left alone and allow the player controller to manage it when the player releases the button?
                animator.SetInteger("SpecialActionMajorChoice", 0); //-1? could be removed for efficiency. The player controller would alter this anytime it would trigger any special action
                animator.SetInteger("SpecialActionMajorSlot", 0);
                animator.SetInteger("SpecialActionMajorAmmo", 0);
            }
        }

        //TO DO: Should I do something similar with special action minor being pressed and not clearing the info? should it depend on special action major? I'll have to think about this more when I actually
        //(if I actually) use the minor in some advanced ability combo system
        animator.ResetTrigger("SpecialActionMinorPressed");
        animator.SetBool("SpecialActionMinorState", false); //Should this be left alone and allow the player controller to manage it when the player releases the button?
        animator.SetInteger("SpecialActionMinorChoice", 0); //-1? could be removed for efficiency. The player controller would alter this anytime it would trigger any special action
        animator.SetInteger("SpecialActionMinorSlot", 0);
        animator.SetInteger("SpecialActionMinorAmmo", 0);
    }

    #region Properties
    #endregion
}