//**************************************************************************************
// File: ForceBlastObjInfo.cs
//
// Purpose: Defines the object type and all it's information for ForceBlast
//
// Written By: Salvatore Hanusiewicz
//**************************************************************************************

using UnityEngine;
using System.Collections;
using AdvancedInspector;

[AdvancedInspector]
public class ForceBlastObjInfo : ExplosionObjInfo
{
    #region Declarations
    #endregion

    /// <summary>
    /// This basically takes the place of a constructor
    /// </summary>
    public void Initialize()
    {
        base.Initialize(false, "ForceBlastObjInfo");
    }

    /// <summary>
    /// Handles detecting a valid hit
    /// </summary>
    /// <param name="pOther"></param>
    /// <param name="pOtherObjInfo"></param>
    protected override void HitDetected(Collider2D pOther, ObjInfo pOtherObjInfo)
    {
        //This allows the force blast to deflect projectiles
        if (pOtherObjInfo.IsType("ProjectileObjInfo"))
        {
            Vector2 tempNewDirection = ((Vector2)(pOtherObjInfo.Transform.position - m_transform.position)).normalized;

            ((ProjectileObjInfo)pOtherObjInfo).Rigidbody2D.velocity = tempNewDirection * ((ProjectileObjInfo)pOtherObjInfo).Rigidbody2D.velocity.magnitude;

            ((ProjectileObjInfo)pOtherObjInfo).AliveTime = 0;
        }
        else
        {
            base.HitDetected(pOther, pOtherObjInfo);
        }
    }

    #region Properties
    #endregion
}