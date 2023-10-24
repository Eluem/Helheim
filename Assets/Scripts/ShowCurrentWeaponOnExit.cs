//**************************************************************************************
// File: ShowCurrentWeaponOnExit.cs
//
// Purpose: This script is meant to show the player's current weapon on exiting
// the state. This is mainly used for exiting a special action since special actions
// are generally aniamted using the Unarmed hands.
//
// Written By: Salvatore Hanusiewicz
//**************************************************************************************

using UnityEngine;
using System.Collections;

public class ShowCurrentWeaponOnExit : StateMachineBehaviourWithCharObjInfoPointer
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
        //((CharObjInfo)animator.gameObject.GetComponent(typeof(CharObjInfo))).EnableCurrentDisableUnarmed();
        m_charObjInfo.EnableCurrentDisableUnarmed();
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