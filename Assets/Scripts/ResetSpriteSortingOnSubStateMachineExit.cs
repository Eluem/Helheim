//**************************************************************************************
// File: ResetSpriteSortingOnSubStateMachineExit.cs
//
// Purpose: Resets the object's sprite sorting order to default upon
// exiting a subStateMachine
//
// Written By: Salvatore Hanusiewicz
//**************************************************************************************

using UnityEngine;
using System.Collections;

public class ResetSpriteSortingOnSubStateMachineExit : StateMachineBehaviourWithCharObjInfoPointer
{
    #region Declarations
    #endregion

    /// <summary>
    /// Called when a transition starts and the state machine starts to evaluate this state
    /// </summary>
    //override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //	
    //}

    /// <summary>
    /// Called on each Update frame between OnStateEnter and OnStateExit callbacks
    /// </summary>
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //	
    //}

    /// <summary>
    /// Called when a transition ends and the state machine finishes evaluating this state
    /// </summary>
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    /// <summary>
    /// Called right after Animator.OnAnimatorMove(). Code that
    /// processes and affects root motion should be implemented here
    /// </summary>
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //	
    //}

    /// <summary>
    /// Called right after Animator.OnAnimatorIK(). Code that
    /// sets up animation IK (inverse kinematics) should be implemented here.
    /// </summary>
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //	
    //}


    /// <summary>
    /// Called when exiting a statemachine via its Exit Node
    /// </summary>
    public override void OnStateMachineExit(Animator animator, int stateMachinePathHash)
    {
        m_charObjInfo.ResetSpriteSorting();
    }

    #region Properties
    #endregion
}