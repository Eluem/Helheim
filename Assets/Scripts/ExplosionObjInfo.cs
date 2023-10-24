//**************************************************************************************
// File: ExplosionObjInfo.cs
//
// Purpose: Defines the base class for all explosions. Any typical explosion will
// simply customize the paramters of this class.
//
// Written By: Salvatore Hanusiewicz
//**************************************************************************************

using UnityEngine;
using System.Collections;

public abstract class ExplosionObjInfo : AoEObjInfo
{
    #region Declarations
    #endregion

    /// <summary>
    /// This basically takes the place of a constructor
    /// </summary>
    /// <param name="pPooled"></param>
    /// <param name="pObjType"></param>
    public override void Initialize(bool pPooled, string pObjType = "ExplosionObjInfo")
    {
        base.Initialize(pPooled, pObjType);
	}

    /// <summary>
    /// Accepts an object type and returns true if one of the types
    /// that it is matches the type passed.
    /// i.e. PlayerObjInfo is also a CharObjInfo and a DestructibleObjInfo
    /// </summary>
    /// <param name="pObjType">Object Type string to check</param>
    /// <returns></returns>
    public override bool IsType(string pObjType)
    {
        if (pObjType == "HazardObjInfo" || pObjType == "ExplosionObjInfo" || pObjType == ObjType)
        {
            return true;
        }

        return false;
    }

    #region Properties
    #endregion
}