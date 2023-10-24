//**************************************************************************************
// File: ButtonInfo.cs
//
// Purpose: Handles checking if a button is being held or tapped in addition to
// other basic button modularization
//
// Written By: Salvatore Hanusiewicz
//**************************************************************************************

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ButtonInfo : ControlInfo
{
    #region Declarations
    protected float m_holdTime; //How long it takes for the button to be pressed to be considered held
    protected float m_tapTime; //How long it takes for the button to be pressed before it won't be considered tapped when released

    protected float m_timePressed;

    protected bool m_buttonState;
    protected bool m_prevButtonState; //State of the button from the last update

    protected bool m_isDown;
    protected bool m_isUp;
    protected bool m_isHeld;
    protected bool m_isTapped;
    protected bool m_lastFrameState;

#if UNITY_EDITOR
    protected float m_forcePressTime;
#endif
#endregion

    /// <summary>
    /// Constructor for class
    /// </summary>
    /// <param name="pInputManager"></param>
    /// <param name="pControlName"></param>
    /// <param name="pHoldTime"></param>
    /// <param name="pTapTime"></param>
    public ButtonInfo(InputManager pInputManager, string pControlName, float pHoldTime, float pTapTime = -1) : base(pInputManager, pControlName)
    {
        m_holdTime = pHoldTime;

        if (pTapTime != -1)
        {
            m_tapTime = pTapTime;
        }
        else
        {
            m_tapTime = m_holdTime;
        }

        m_timePressed = 0;

        m_buttonState = false;
        m_isDown = false;
        m_isUp = false;
        m_isHeld = false;
        m_isTapped = false;
        m_lastFrameState = false;
    }

    /// <summary>
    /// This should be called at the top of the update function
    /// in the object where you're tracking the button press.
    /// It accepts the delta time since the last frame and checks the current
    /// button state to determine if it was pressed
    /// </summary>
    /// <param name="pDeltaTime"></param>
    public virtual void Update(float pDeltaTime)
    {
        m_buttonState = Input.GetButton(FullControlName);
        m_isDown = Input.GetButtonDown(FullControlName);
        m_isUp = Input.GetButtonUp(FullControlName);

#if UNITY_EDITOR
        if(m_forcePressTime > 0)
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

    /// <summary>
    /// This should be called at the top of the update function
    /// in the object where you're tracking the button press.
    /// It accepts the delta time since the last frame and checks the current
    /// button state to determine if it was pressed
    ///
    /// NOTE: This version is to allow easy polymorphism for the
    /// XInputButtonInfo
    /// </summary>
    /// <param name="pDeltaTime"></param>
    /// <param name="pXInputButtonState"></param>
    public virtual void Update(float pDeltaTime, XInputDotNetPure.ButtonState pXInputButtonState)
    {
    }

    /// <summary>
    /// Updates the more complex isHeld and isTapped details
    /// </summary>
    /// <param name="pDeltaTime"></param>
    protected void UpdateComplexDetails(float pDeltaTime)
    {
        m_isHeld = false;
        m_isTapped = false;

        if (m_buttonState)
        {
            m_timePressed += pDeltaTime;

            if (m_timePressed >= m_holdTime)
            {
                m_isHeld = true;
            }

            m_lastFrameState = true;
        }
        else
        {
            if (m_lastFrameState && m_timePressed < m_tapTime)
            {
                m_isTapped = true;
            }

            m_timePressed = 0;

            m_lastFrameState = false;
        }
    }

#if UNITY_EDITOR
    /// <summary>
    /// This function exists mainly for debugging purposes and causes the button to act as if it detected a press from the controller
    /// </summary>
    public virtual void ForcePress(float pDuration)
    {
        m_forcePressTime = pDuration;
    }
#endif

    #region Properties
    public bool ButtonState
    {
        get
        {
            return m_buttonState;
        }
    }

    public bool IsDown
    {
        get
        {
            return m_isDown;
        }
    }

    public bool IsUp
    {
        get
        {
            return m_isUp;
        }
    }

    public bool IsHeld
    {
        get
        {
            return m_isHeld;
        }
    }

    public bool IsTapped
    {
        get
        {
            return m_isTapped;
        }
    }

    public float HoldTime
    {
        get
        {
            return m_holdTime;
        }
    }

    public float TimePressed
    {
        get
        {
            return m_timePressed;
        }
    }

    public bool LastFrameState
    {
        get
        {
            return m_lastFrameState;
        }
    }
    #endregion
}
