//**************************************************************************************
// File: FireBallExplosionChargedObjInfo.cs
//
// Purpose: Defines the object type and all it's information for the charged fireball's
// explosion
//
// Written By: Salvatore Hanusiewicz
//**************************************************************************************

using UnityEngine;
using System.Collections;
using AdvancedInspector;

[AdvancedInspector]
public class FireBallExplosionChargedObjInfo : ExplosionObjInfo
{
    #region Declarations
    #endregion

    /// <summary>
    /// This basically takes the place of a constructor
    /// </summary>
    public void Initialize()
    {
        base.Initialize(false, "FireBallExplosionChargedObjInfo");
    }

    #region Properties
    #endregion
}