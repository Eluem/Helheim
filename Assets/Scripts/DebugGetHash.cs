using UnityEngine;
using System.Collections;

public class DebugGetHash : StateMachineBehaviour
{
    /// <summary>
    /// OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    /// Debug function to get the hash of the state this is attached to.
    /// </summary>
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Debug.Log(stateInfo.fullPathHash);
	}
}
