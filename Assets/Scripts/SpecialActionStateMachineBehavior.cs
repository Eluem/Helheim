//**************************************************************************************************************
// File: SpecialActionStateMachineBehavior.cs
//
// Purpose: This class handles turning the SpecialAction boolean off to cause it to act a little more like
// a trigger. It needed to be a boolean so that it could pass through multiple states.
// This also handles hiding the character's current weapon and showing the unarmed, and then
// toggling back at the end.
//
// Written By: Salvatore Hanusiewicz
//**************************************************************************************************************

using UnityEngine;
using System.Collections;

public class SpecialActionStateMachineBehavior : StateMachineBehaviour
{
    #region Declarations
    #endregion

    // OnStateEnter is called before OnStateEnter is called on any state inside this state machine
    //override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	//{
    //    
	//}

    // OnStateUpdate is called before OnStateUpdate is called on any state inside this state machine
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //
    //}

    // OnStateExit is called before OnStateExit is called on any state inside this state machine
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //
    //}

    // OnStateMove is called before OnStateMove is called on any state inside this state machine
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //
    //}

    // OnStateIK is called before OnStateIK is called on any state inside this state machine
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //
    //}

    // OnStateMachineEnter is called when entering a statemachine via its Entry Node
    override public void OnStateMachineEnter(Animator animator, int stateMachinePathHash)
    {
        animator.SetBool("SpecialAction", false);
    }

    // OnStateMachineExit is called when exiting a statemachine via its Exit Node
    //override public void OnStateMachineExit(Animator animator, int stateMachinePathHash)
    //{
    //     
    //}

    #region Properties
    #endregion
}
