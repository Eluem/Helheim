//**************************************************************************************
// File: GamepadInputManager.cs
//
// Purpose: This is the input manager for gamepad devices
//
// Written By: Salvatore Hanusiewicz
//**************************************************************************************

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GamepadInputManager : InputManager
{
    #region Declarations    
    #endregion

    //***********************************************************************
    // Method: GamepadInputManager
    //
    // Purpose: Constructor for class
    //***********************************************************************
    public GamepadInputManager(string pInputSuffix) : base(pInputSuffix)
    {
        m_evade = new ButtonInfo(this, "Evade", 0.5f, 0.2f);
        m_interact = new ButtonInfo(this, "Interact", 0.5f);
        m_specialAction1 = new ButtonInfo(this, "SpecialAction1", 0.5f);
        m_specialAction2 = new ButtonInfo(this, "SpecialAction2", 0.5f);
        m_attackBasicLight = new ButtonInfo(this, "AttackBasicLight", 0.5f);
        m_attackSpecialLight = new ButtonInfo(this, "AttackSpecialLight", 0.5f);
        m_attackBasicHeavy = new TriggerInfo(this, "AttackBasicHeavy", 0.5f);
        m_attackSpecialHeavy = new TriggerInfo(this, "AttackSpecialHeavy", 0.5f);
        m_movement = new MovementInfo(this);
    }

    //***********************************************************************
    // Method: Update
    //
    // Purpose: Once per frame updates
    //***********************************************************************
    public override void Update(float pDeltaTime)
    {
        //Button Updates
        m_evade.Update(pDeltaTime);
        m_interact.Update(pDeltaTime);
        m_specialAction1.Update(pDeltaTime);
        m_specialAction2.Update(pDeltaTime);
        m_attackBasicLight.Update(pDeltaTime);
        m_attackSpecialLight.Update(pDeltaTime);

        //Trigger Updates
        m_attackBasicHeavy.Update(pDeltaTime);
        m_attackSpecialHeavy.Update(pDeltaTime);
        
        
        //m_movement.Update(pDeltaTime);
    }

    #region Properties
    #endregion
}
