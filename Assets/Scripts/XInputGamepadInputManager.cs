//**************************************************************************************
// File: XInputGamepadInputManager.cs
//
// Purpose: This is the input manager for gamepad devices using XInput.Net
//
// Written By: Salvatore Hanusiewicz
//**************************************************************************************

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using XInputDotNetPure;

public class XInputGamepadInputManager : InputManager
{
    #region Declarations
    GamePadState m_gamePadState; //So I don't keep remaking the variable
    PlayerIndex m_xInputPlayerIndex;
    #endregion

    //***********************************************************************
    // Method: XInputGamepadInputManager
    //
    // Purpose: Constructor for class
    //***********************************************************************
    public XInputGamepadInputManager(string pInputSuffix, PlayerIndex pXInputPlayerIndex) : base(pInputSuffix)
    {
        m_xInputPlayerIndex = pXInputPlayerIndex;

        m_evade = new XInputButtonInfo(this, "Evade", 0.5f, 0.2f);
        m_interact = new XInputButtonInfo(this, "Interact", 0.5f);
        m_specialAction1 = new XInputButtonInfo(this, "SpecialAction1", 0.5f);
        m_specialAction2 = new XInputButtonInfo(this, "SpecialAction2", 0.5f);
        m_attackBasicLight = new XInputButtonInfo(this, "AttackBasicLight", 0.5f);
        m_attackSpecialLight = new XInputButtonInfo(this, "AttackSpecialLight", 0.5f);
        m_attackBasicHeavy = new XInputTriggerInfo(this, "AttackBasicHeavy", 0.5f);
        m_attackSpecialHeavy = new XInputTriggerInfo(this, "AttackSpecialHeavy", 0.5f);
        m_movement = new XInputMovementInfo(this);
    }

    //***********************************************************************
    // Method: Update
    //
    // Purpose: Once per frame updates
    //***********************************************************************
    public override void Update(float pDeltaTime)
    {
        m_gamePadState = GamePad.GetState(m_xInputPlayerIndex);

        //Button Updates
        m_evade.Update(pDeltaTime, m_gamePadState.Buttons.B);
        m_interact.Update(pDeltaTime, m_gamePadState.Buttons.A);
        m_specialAction1.Update(pDeltaTime, m_gamePadState.Buttons.X);
        m_specialAction2.Update(pDeltaTime, m_gamePadState.Buttons.Y);
        m_attackBasicLight.Update(pDeltaTime, m_gamePadState.Buttons.RightShoulder);
        m_attackSpecialLight.Update(pDeltaTime, m_gamePadState.Buttons.LeftShoulder);

        //Trigger Updates
        m_attackBasicHeavy.Update(pDeltaTime, m_gamePadState.Triggers.Right);
        m_attackSpecialHeavy.Update(pDeltaTime, m_gamePadState.Triggers.Left);


        m_movement.Update(m_gamePadState.ThumbSticks.Left.X, m_gamePadState.ThumbSticks.Left.Y);
    }

    //***********************************************************************
    // Method: Matches
    //
    // Purpose: Checks if the XInputPlayerIndex matches this InputManager
    //***********************************************************************
    public override bool Matches(XInputDotNetPure.PlayerIndex pXInputPlayerIndex)
    {
        return m_xInputPlayerIndex == pXInputPlayerIndex;
    }

    #region Properties
    #endregion
}
