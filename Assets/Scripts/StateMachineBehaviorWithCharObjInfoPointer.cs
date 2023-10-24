//**************************************************************************************
// File: StateMachineBehaviorWithCharObjInfoPointer.cs
//
// Purpose: This is to be used by StateMachineBehaviors that include a pointer to 
// their CharObjInfo.
//
// This is mainly to be used by StateMachineBehaviors that need to communicate with
// the CharObjInfo that they're animator is related to. This helps prevent calling
// GetComponent every time they would need to reference it and allows the CharObjInfo
// to initialize all of these in a loop within it's Initialize function.
//
// Written By: Salvatore Hanusiewicz
//**************************************************************************************

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StateMachineBehaviourWithCharObjInfoPointer : StateMachineBehaviour
{
    #region Declarations
    protected CharObjInfo m_charObjInfo;
    #endregion


    #region Properties
    public CharObjInfo CharObjInfo
    {
        get
        {
            return m_charObjInfo;
        }

        set
        {
            m_charObjInfo = value;
        }
    }
    #endregion
}