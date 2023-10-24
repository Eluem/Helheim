//**************************************************************************************
// File: WaterDeepObjInfo.cs
//
// Purpose: Defines deep water, which will cause objects to be treated as if they are
// "floating" in it... the player, for example, should begin struggling for air
//
// Written By: Salvatore Hanusiewicz
//**************************************************************************************

using UnityEngine;
using System.Collections;

public class WaterDeepObjInfo : TerrainObjInfo
{
    #region Declarations
    #endregion

    /// <summary>
    /// This basically takes the place of a constructor
    /// </summary>
    public override void Initialize(string pObjType = "WaterDeepObjInfo")
    {
        base.Initialize(pObjType);
    }

    /// <summary>
    /// The TerrainObjInfo base class handles checking if an ActorObjInfo has this TerrainObjInfo in one of its lists. If it does, then this isn't called
    /// </summary>
    /// <param name="pOther">Other object's collider</param>
    protected override void OnTriggerEnter2D_DupePruned(Collider2D pOther)
    {
        //base.OnTriggerEnter2D(pOther);
        base.OnTriggerEnter2D_DupePruned(pOther);

        ObjInfo otherObjInfo = GetObjInfoFromCollider(pOther);

        if (otherObjInfo.IsType("DestructibleObjInfo"))
        {
            ((DestructibleObjInfo)otherObjInfo).SufferDrowning();
        }
    }

    /// <summary>
    /// The TerrainObjInfo base class handles checking if an ActorObjInfo has this TerrainObjInfo in one of its lists. If it doesn't, then this isn't called
    /// </summary>
    /// <param name="pOther">Other object's collider</param>
    protected override void OnTriggerExit2D_DupePruned(Collider2D pOther)
    {
        //base.OnTriggerExit2D(pOther);
        base.OnTriggerExit2D_DupePruned(pOther);

        ObjInfo otherObjInfo = GetObjInfoFromCollider(pOther);

        if (otherObjInfo.IsType("DestructibleObjInfo"))
        {
            ((DestructibleObjInfo)otherObjInfo).RelieveDrowning();
        }
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