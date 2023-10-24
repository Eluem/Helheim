//**************************************************************************************
// File: ClearAttackStateInfoOnExitState.cs
//
// Purpose: This script is meant to clear any information that is specific to an attack
// animation at the end of the attack
//
// Written By: Salvatore Hanusiewicz
//**************************************************************************************

using UnityEngine;
using System.Collections;

public class ClearAttackStateInfoOnExitState : StateMachineBehaviour
{
    #region Declarations
    protected bool m_initialized = false;
    protected CharObjInfo m_charObjInfo;
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
        if(!m_initialized)
        {
            m_charObjInfo = (CharObjInfo)animator.gameObject.GetComponent(typeof(CharObjInfo));
        }

        //TO DO: Consider possible way to allow passing these values through to another state if the state is exited in specific ways
        animator.SetInteger("BouncePower", 0);
        animator.SetInteger("HitsRegistered", 0);
        animator.SetInteger("HitsRegisteredCharacter", 0);
        animator.SetInteger("HitsRegisteredDestructible", 0);
        animator.SetInteger("HitsRegisteredObstacle", 0);
        //animator.SetInteger("HitsRegisteredHazard", 0);

        m_charObjInfo.ClearAttackStateInfo();
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