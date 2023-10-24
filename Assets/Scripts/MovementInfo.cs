//**************************************************************************************
// File: MovementInfo.cs
//
// Purpose: Handles analyzing a gamepad left thumbstick for movement
//
// Written By: Salvatore Hanusiewicz
//**************************************************************************************

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MovementInfo : ControlInfo
{
	#region Declarations
	#endregion
	
	//***********************************************************************
	// Method: MovementInfo
	//
	// Purpose: Constructor for class
	//***********************************************************************
	public MovementInfo(InputManager pInputManager) : base(pInputManager, "Movement")
	{
        
	}

    //***********************************************************************
    // Method: Update
    //
    // Purpose: This should be called at the top of the update function
    // in the object where you're tracking the movement.
    //
    // NOTE: This version is to allow easy polymorphism for the
    // XInputMovementInfo
    //***********************************************************************
    public virtual void Update(float pX, float pY)
    {
    }

    #region Properties
    public virtual float Horizontal
    {
        get
        {
            return Input.GetAxis("Horizontal" + "_" + m_inputManager.InputSuffix);
        }
    }

    public virtual float Vertical
    {
        get
        {
            return Input.GetAxis("Vertical" + "_" + m_inputManager.InputSuffix);
        }
    }

    public virtual Vector2 Direction
    {
        get
        {
            return new Vector2(Horizontal, Vertical);
        }
    }
    #endregion
}
