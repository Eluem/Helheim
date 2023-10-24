//**************************************************************************************
// File: Vector2Extensions.cs
//
// Purpose: Extends Vector2
//
// Written By: Salvatore Hanusiewicz
//**************************************************************************************

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class Vector2Extensions
{
    #region Declarations
    #endregion

    //***********************************************************************
    // Method: Rotate
    //
    // Purpose: Rotates the vector by the degrees passed
    //***********************************************************************
    public static Vector2 Rotate(this Vector2 pV, float pDegrees)
    {
        return Quaternion.Euler(0, 0, pDegrees) * pV;
    }

    //***********************************************************************
    // Method: GetAngle
    //
    // Purpose: Gets the angle that this 2d vector represents if you treat
    // "North" as 0
    //
    // TO DO: Figure out why I have to negate
    //***********************************************************************
    public static float GetAngle(this Vector2 pV)
    {
        return -1 * Mathf.Atan2(pV.x, pV.y) * 180 / Mathf.PI;
    }

    //***********************************************************************
    // Method: GetRotation
    //
    // Purpose: Gets the 3d rotation that will make a transform's up value
    // face in the same 2d direction as this vector relative to the
    // currently established systems
    //***********************************************************************
    public static Quaternion GetRotation(this Vector2 pV)
    {
        return Quaternion.Euler(0, 0, GetAngle(pV));
    }

    #region Properties
    #endregion
}