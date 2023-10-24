//**************************************************************************************
// File: ShurikenObjInfo.cs
//
// Purpose: Defines the Shuriken object type and all it's information
//
// Written By: Salvatore Hanusiewicz
//**************************************************************************************

using UnityEngine;
using System.Collections;
using AdvancedInspector;

[AdvancedInspector]
public class ShurikenObjInfo : ProjectileObjInfo
{
    #region Declarations
    #endregion

    /// <summary>
    /// This basically takes the place of a constructor
    /// </summary>
    public void Initialize()
    {
        base.Initialize(false, "ShurikenObjInfo");
    }

    /// <summary>
    /// This is the code that runs each time this object is spawned
    /// </summary>
    /// <param name="pOriginObjInfo"></param>
    /// <param name="pSourceObjInfo"></param>
    /// <param name="pDirection"></param>
    public void Spawn(ObjInfo pOriginObjInfo, ObjInfo pSourceObjInfo, Vector3 pDirection)
    {
        base.Spawn(pOriginObjInfo, pSourceObjInfo, pDirection, 20);

        LaunchProjectile();
    }

    /// <summary>
    /// Handles detecting a valid hit
    /// </summary>
    /// <param name="pOther"></param>
    /// <param name="pOtherObjInfo"></param>
    protected override void HitDetected(Collider2D pOther, ObjInfo pOtherObjInfo)
    {
        if (pOtherObjInfo.RigidBody2D != null && pOtherObjInfo.RigidBody2D.constraints != RigidbodyConstraints2D.FreezeAll)
        {
            m_lingerTime = 0;
        }

        base.HitDetected(pOther, pOtherObjInfo);
    }

    #region Properties
    #endregion
}