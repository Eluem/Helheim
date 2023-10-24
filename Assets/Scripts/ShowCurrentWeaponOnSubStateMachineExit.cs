//**************************************************************************************
// File: ShowCurrentWeaponOnSubStateMachineExit.cs
//
// Purpose: This script is meant to show the player's current weapon on exiting
// the subStateMachine.
//
// Written By: Salvatore Hanusiewicz
//**************************************************************************************

using UnityEngine;
using System.Collections;

public class ShowCurrentWeaponOnSubStateMachineExit : StateMachineBehaviourWithCharObjInfoPointer
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

    ///// <summary>
    ///// Called when a transition ends and the state machine
    ///// finishes evaluating this state
    ///// </summary>
    ///// <param name="animator"></param>
    ///// <param name="stateInfo"></param>
    ///// <param name="layerIndex"></param>
    //public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    Debug.Log(stateInfo.fullPathHash + " |||| " + stateInfo.shortNameHash + " |||| " + stateInfo.fullPathHash.ToString());
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

    /// <summary>
    /// Called when exiting a statemachine via its Exit Node
    /// </summary>
    public override void OnStateMachineExit(Animator animator, int stateMachinePathHash)
    {
        m_charObjInfo.EnableCurrentDisableUnarmed();
    }

    #region Properties
    #endregion
}