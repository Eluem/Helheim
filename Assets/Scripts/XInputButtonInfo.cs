//**************************************************************************************
// File: XInputButtonInfo.cs
//
// Purpose: Handles checking if a button is being held or tapped in addition to
// other basic button modularization
//
// Written By: Salvatore Hanusiewicz
//**************************************************************************************

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using XInputDotNetPure;

public class XInputButtonInfo : ButtonInfo
{
    #region Declarations
    #endregion

    /// <summary>
    /// Constructor for class
    /// </summary>
    /// <param name="pInputManager"></param>
    /// <param name="pControlName"></param>
    /// <param name="pHoldTime"></param>
    /// <param name="pTapTime"></param>
    public XInputButtonInfo(InputManager pInputManager, string pControlName, float pHoldTime, float pTapTime = -1) : base(pInputManager, pControlName, pHoldTime, pTapTime)
    {
	}

    /// <summary>
    /// This should be called at the top of the update function
    /// in the object where you're tracking the button press.
    /// It accepts the delta time since the last frame and checks the current
    /// button state to determine if it was pressed
    /// </summary>
    /// <param name="pDeltaTime"></param>
    /// <param name="pXInputButtonState"></param>
    public override void Update(float pDeltaTime, XInputDotNetPure.ButtonState pXInputButtonState)
    {
        m_buttonState = (pXInputButtonState == XInputDotNetPure.ButtonState.Pressed);

        m_isDown = m_buttonState && !m_prevButtonState;
        m_isUp = !m_buttonState && m_prevButtonState;

#if UNITY_EDITOR
        if (m_forcePressTime > 0)
        {
            m_forcePressTime -= pDeltaTime;
            m_buttonState = true;
            m_isDown = !m_prevButtonState;
            m_isUp = false;
        }
#endif

        UpdateComplexDetails(pDeltaTime);

        m_prevButtonState = m_buttonState;
    }

    #region Properties
    #endregion
}