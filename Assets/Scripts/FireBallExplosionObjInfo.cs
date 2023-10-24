//**************************************************************************************
// File: FireBallExplosionObjInfo.cs
//
// Purpose: Defines the object type and all it's information for the fireball's
// explosion
//
// Written By: Salvatore Hanusiewicz
//**************************************************************************************

using UnityEngine;
using System.Collections;
using AdvancedInspector;

[AdvancedInspector]
public class FireBallExplosionObjInfo : ExplosionObjInfo
{
    #region Declarations
    #endregion

    /// <summary>
    /// This basically takes the place of a constructor
    /// </summary>
    public void Initialize()
    {
        base.Initialize(false, "FireBallExplosionObjInfo");
    }

    #region Properties
    #endregion
}