//**************************************************************************************
// File: FireBallObjInfo.cs
//
// Purpose: Defines the FireBall object type and all it's information
//
// Written By: Salvatore Hanusiewicz
//**************************************************************************************

using UnityEngine;
using System.Collections;
using AdvancedInspector;

[AdvancedInspector]
public class FireBallObjInfo : ProjectileObjInfo
{
    #region Declarations
    #endregion

    /// <summary>
    /// This basically takes the place of a constructor
    /// </summary>
    public void Initialize()
    {
        base.Initialize(false, "FireBallObjInfo");
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
    /// Handles what happens when a hazard object is destroyed
    /// </summary>
    protected override void OnDeath()
    {
        base.OnDeath();

        ObjectFactory.CreateFireBallExplosion(m_originObjInfo, this, m_transform.position);
    }

    #region Properties
    #endregion
}