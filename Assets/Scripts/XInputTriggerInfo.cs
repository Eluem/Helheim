//**************************************************************************************
// File: XInputTriggerInfo.cs
//
// Purpose: Handles analyzing a gamepad trigger.
//
// Written By: Salvatore Hanusiewicz
//**************************************************************************************

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class XInputTriggerInfo : TriggerInfo
{
    #region Declarations
    #endregion
	
	//***********************************************************************
	// Method: XInputTriggerInfo
	//
	// Purpose: Constructor for class
	//***********************************************************************
	public XInputTriggerInfo(InputManager pInputManager, string pControlName, float pHoldTime, float pIsDownThreshold = 0.4f, float pTapTime = -1) : base(pInputManager, pControlName, pHoldTime, pIsDownThreshold, pTapTime)
	{
	}

    //***********************************************************************
    // Method: Update
    //
    // Purpose: This should be called at the top of the update function
    // in the object where you're tracking the trigger value.
    // It accepts the delta time since the last frame and checks the current
    // trigger state to determine the details
    //***********************************************************************
    public override void Update(float pDeltaTime, float pTriggerValue)
    {
        m_buttonState = pTriggerValue >= m_isDownThreshold;

        m_isDown = m_buttonState && !m_prevButtonState;
        m_isUp = !m_buttonState && m_prevButtonState;

        UpdateComplexDetails(pDeltaTime);

        m_prevButtonState = m_buttonState;
    }

    #region Properties
    #endregion
}