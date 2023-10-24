//**************************************************************************************
// File: TriggerInfo.cs
//
// Purpose: Handles analyzing a gamepad trigger.
//
// Written By: Salvatore Hanusiewicz
//**************************************************************************************

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TriggerInfo : ButtonInfo
{
    #region Declarations
    protected float m_isDownThreshold; //Threshold that trigger must pass before counting as being "down"
	#endregion
	
	//***********************************************************************
	// Method: TriggerInfo
	//
	// Purpose: Constructor for class
	//***********************************************************************
	public TriggerInfo(InputManager pInputManager, string pControlName, float pHoldTime, float pIsDownThreshold = 0.4f, float pTapTime = -1) : base(pInputManager, pControlName, pHoldTime, pTapTime)
	{
        m_isDownThreshold = pIsDownThreshold;
    }

    //***********************************************************************
    // Method: Update
    //
    // Purpose: This should be called at the top of the update function
    // in the object where you're tracking the trigger value.
    // It accepts the delta time since the last frame and checks the current
    // trigger state to determine the details
    //***********************************************************************
    public override void Update(float pDeltaTime)
    {
        m_buttonState = Input.GetAxis(FullControlName) >= m_isDownThreshold;
        
        m_isDown = m_buttonState && !m_prevButtonState;
        m_isUp = !m_buttonState && m_prevButtonState;

        UpdateComplexDetails(pDeltaTime);

        m_prevButtonState = m_buttonState;
    }

    //***********************************************************************
    // Method: Update
    //
    // Purpose: This should be called at the top of the update function
    // in the object where you're tracking the trigger value.
    // It accepts the delta time since the last frame and checks the current
    // trigger state to determine the details
    //
    // NOTE: This version is to allow easy polymorphism for the
    // XInputTriggerInfo
    //***********************************************************************
    public virtual void Update(float pDeltaTime, float pTriggerValue)
    {
    }

        #region Properties
        #endregion
    }
