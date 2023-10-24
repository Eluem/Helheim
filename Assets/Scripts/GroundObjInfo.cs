//**************************************************************************************
// File: GroundObjInfo.cs
//
// Purpose: Defines a simple type of ground terrain which explicitly exists to
// preclude other terrain triggers so that this may act like "solid ground"
//
// Written By: Salvatore Hanusiewicz
//**************************************************************************************

using UnityEngine;
using System.Collections;

public class GroundObjInfo : TerrainObjInfo
{
    #region Declarations
    #endregion

    /// <summary>
    /// This basically takes the place of a constructor
    /// </summary>
    public override void Initialize(string pObjType = "GroundObjInfo")
    {
        base.Initialize(pObjType);
    }

    /// <summary>
    /// Builds a damage packet and passes it to the
    /// DestructibleObjInfo so that it can handle responding to it correctly
    /// (I'm most likely going to have characters delay responding to the
    /// damage packets to make it easy to allow players to kill each other
    /// or to come up with a good way to handle clashing (if I do that))
    /// </summary>
    /// <param name="pObjInfo"></param>
    /// <param name="pDamageContainer"></param>
    /// <param name="pInteractionType"></param>
    //protected override void RegisterHit(DestructibleObjInfo pObjInfo, DamageContainer pDamageContainer, InteractionType pInteractionType = InteractionType.None)
    //{
    //}

    #region Properties
    #endregion
}