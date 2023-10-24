//**************************************************************************************
// File: XInputMovementInfo.cs
//
// Purpose: Handles analyzing a gamepad left thumbstick for movement
//
// Written By: Salvatore Hanusiewicz
//**************************************************************************************

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class XInputMovementInfo : MovementInfo
{
    #region Declarations
    protected float m_horizontal;
    protected float m_vertical;
    #endregion
	
	//***********************************************************************
	// Method: XInputMovementInfo
	//
	// Purpose: Constructor for class
	//***********************************************************************
	public XInputMovementInfo(InputManager pInputManager) : base(pInputManager)
	{
        m_horizontal = 0;
        m_vertical = 0;
    }

    //***********************************************************************
    // Method: Update
    //
    // Purpose: This should be called at the top of the update function
    // in the object where you're tracking the movement.
    //***********************************************************************
    public override void Update(float pX, float pY)
    {
        m_horizontal = pX;
        m_vertical = pY;
    }

    #region Properties
    public override float Horizontal
    {
        get
        {
            return m_horizontal;
        }
    }

    public override float Vertical
    {
        get
        {
            return m_vertical;
        }
    }
    #endregion
}