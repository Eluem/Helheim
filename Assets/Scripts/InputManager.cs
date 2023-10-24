//**************************************************************************************
// File: InputManager.cs
//
// Purpose: This class will be used as an abstract class to modularize the inputs
// that the PlayerController reads from.
// This will allow for brute networking (just sending input values as a string and
// then deserializing them into a specialized InputManager... though it might be
// better to use state syncing and such...), creating a special AI InputManager,
// simply allowing me to keep the input management separate so that I can use other
// tools to manage input easily, ect.
//
// Written By: Salvatore Hanusiewicz
//**************************************************************************************

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class InputManager
{
    #region Declarations
    protected InputMuter m_inputMuter;

    protected ButtonInfo m_evade;
    protected ButtonInfo m_interact;
    protected ButtonInfo m_specialAction1;
    protected ButtonInfo m_specialAction2;
    protected ButtonInfo m_attackBasicLight;
    protected ButtonInfo m_attackSpecialLight;
    protected TriggerInfo m_attackBasicHeavy;
    protected TriggerInfo m_attackSpecialHeavy;
    protected MovementInfo m_movement;

    private string m_inputSuffix;
    #endregion

    /// <summary>
    /// Constructor for class
    /// </summary>
    /// <param name="pInputSuffix"></param>
    public InputManager(string pInputSuffix)
    {
        m_inputSuffix = pInputSuffix;

        m_inputMuter = new InputMuter();
    }

    /// <summary>
    /// Once per frame updates
    /// </summary>
    /// <param name="pDeltaTime"></param>
    public abstract void Update(float pDeltaTime);

    /// <summary>
    /// Once per frame updates
    /// This handles direct boolean mutes for each input as well
    /// If the AllActionIsMutedDirect flag is true, then it overrides all
    /// the other IsMutedDirect flags EXCEPT for MovementIsMutedDirect
    /// </summary>
    /// <param name="pDeltaTime"></param>
    /// <param name="pAllActionIsMutedDirect"></param>
    /// <param name="pEvadeIsMutedDirect"></param>
    /// <param name="pInteractIsMutedDirect"></param>
    /// <param name="pSpecialAction1IsMutedDirect"></param>
    /// <param name="pSpecialAction2IsMutedDirect"></param>
    /// <param name="pAttackBasicLightIsMutedDirect"></param>
    /// <param name="pAttackSpecialLightIsMutedDirect"></param>
    /// <param name="pAttackBasicHeavyIsMutedDirect"></param>
    /// <param name="pAttackSpecialHeavyIsMutedDirect"></param>
    /// <param name="pMovementIsMutedDirect"></param>
    public virtual void Update(float pDeltaTime, bool pAllActionIsMutedDirect, bool pEvadeIsMutedDirect, bool pInteractIsMutedDirect, bool pSpecialAction1IsMutedDirect, bool pSpecialAction2IsMutedDirect, bool pAttackBasicLightIsMutedDirect, bool pAttackSpecialLightIsMutedDirect, bool pAttackBasicHeavyIsMutedDirect, bool pAttackSpecialHeavyIsMutedDirect, bool pMovementIsMutedDirect)
    {
        //This could have been done by passing (pAllActionIsMuteDirect || p[InputName]IsMutedDirect)... however, I believe this is more efficient
        if (pAllActionIsMutedDirect)
        {
            m_evade.SetIsMutedDirect(true);
            m_interact.SetIsMutedDirect(true);
            m_specialAction1.SetIsMutedDirect(true);
            m_specialAction2.SetIsMutedDirect(true);
            m_attackBasicLight.SetIsMutedDirect(true);
            m_attackSpecialLight.SetIsMutedDirect(true);
            m_attackBasicHeavy.SetIsMutedDirect(true);
            m_attackSpecialHeavy.SetIsMutedDirect(true);
        }
        else
        {
            m_evade.SetIsMutedDirect(pEvadeIsMutedDirect);
            m_interact.SetIsMutedDirect(pInteractIsMutedDirect);
            m_specialAction1.SetIsMutedDirect(pSpecialAction1IsMutedDirect);
            m_specialAction2.SetIsMutedDirect(pSpecialAction2IsMutedDirect);
            m_attackBasicLight.SetIsMutedDirect(pAttackBasicLightIsMutedDirect);
            m_attackSpecialLight.SetIsMutedDirect(pAttackSpecialLightIsMutedDirect);
            m_attackBasicHeavy.SetIsMutedDirect(pAttackBasicHeavyIsMutedDirect);
            m_attackSpecialHeavy.SetIsMutedDirect(pAttackSpecialHeavyIsMutedDirect);
        }

        m_movement.SetIsMutedDirect(pMovementIsMutedDirect);

        Update(pDeltaTime);
    }

    /// <summary>
    /// Checks if the input suffix matches this InputManager
    /// </summary>
    /// <param name="pInputSuffix"></param>
    /// <returns></returns>
    public virtual bool Matches(string pInputSuffix)
    {
        return m_inputSuffix == pInputSuffix;
    }

    /// <summary>
    /// Checks if the XInputPlayerIndex matches this InputManager
    /// </summary>
    /// <param name="pXInputPlayerIndex"></param>
    /// <returns></returns>
    public virtual bool Matches(XInputDotNetPure.PlayerIndex pXInputPlayerIndex)
    {
        return false;
    }

    #region Properties
    public ButtonInfo Evade
    {
        get
        {
            return m_evade;
        }
    }

    public ButtonInfo Interact
    {
        get
        {
            return m_interact;
        }
    }

    public ButtonInfo SpecialAction1
    {
        get
        {
            return m_specialAction1;
        }
    }

    public ButtonInfo SpecialAction2
    {
        get
        {
            return m_specialAction2;
        }
    }

    public ButtonInfo AttackBasicLight
    {
        get
        {
            return m_attackBasicLight;
        }
    }

    public ButtonInfo AttackSpecialLight
    {
        get
        {
            return m_attackSpecialLight;
        }
    }

    public TriggerInfo AttackBasicHeavy
    {
        get
        {
            return m_attackBasicHeavy;
        }
    }

    public TriggerInfo AttackSpecialHeavy
    {
        get
        {
            return m_attackSpecialHeavy;
        }
    }

    public MovementInfo Movement
    {
        get
        {
            return m_movement;
        }
    }

    public InputMuter InputMuter
    {
        get
        {
            return m_inputMuter;
        }
    }

    public string InputSuffix
    {
        get
        {
            return m_inputSuffix;
        }
        set
        {
            m_inputSuffix = value;
        }
    }
    #endregion
}
