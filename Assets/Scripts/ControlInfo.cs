//**************************************************************************************
// File: ControlInfo.cs
//
// Purpose: This is the abstract base class for all controls
//
// Written By: Salvatore Hanusiewicz
//**************************************************************************************

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class ControlInfo
{
    #region Declarations
    protected string m_controlName; //Name of the control
    protected InputManager m_inputManager; //Stores a pointer to the input manager

    protected bool m_isMutedDirect; //Direct mute boolean (if this is true OR this control is muted by the InputMuter, it's considered muted
    #endregion

    //***********************************************************************
    // Method: ControlInfo
    //
    // Purpose: Constructor for class
    //***********************************************************************
    public ControlInfo(InputManager pInputManager, string pControlName)
	{
        m_inputManager = pInputManager;
        m_controlName = pControlName;

        m_isMutedDirect = false;
	}

    //***********************************************************************
    // Method: SetIsMutedDirect
    //
    // Purpose: Sets the value of m_isMutedDirect
    //***********************************************************************
    public void SetIsMutedDirect(bool pIsMutedDirect)
    {
        m_isMutedDirect = pIsMutedDirect;
    }

    #region Properties
    public string ControlName
    {
        get
        {
            return m_controlName;
        }
    }

    public string FullControlName
    {
        get
        {
            return m_controlName + "_" + m_inputManager.InputSuffix;
        }
    }

    public bool IsMuted
    {
        get
        {
            return (m_isMutedDirect || m_inputManager.InputMuter.IsParamMuted(m_controlName));
        }
    }
    #endregion
}
