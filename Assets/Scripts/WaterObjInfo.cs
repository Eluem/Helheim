//**************************************************************************************
// File: WaterObjInfo.cs
//
// Purpose: Defines a type of water object that should cause actors to become "wet"
// and stay "wet" for as long as they are in the water and for some time after leaving
// The effectiveness of the "wet" status effect decays as long as it isn't being
// refreshed.
//
// TO DO: Consider "knee deep (need better name)" status effect for being IN the water
// or have the "wet" effect cause a stronger that drops off extremely quickly
//
// Written By: Salvatore Hanusiewicz
//**************************************************************************************

using UnityEngine;
using System.Collections;

public class WaterObjInfo : TerrainObjInfo
{
    #region Declarations
    #endregion

    /// <summary>
    /// This basically takes the place of a constructor
    /// </summary>
    public override void Initialize(string pObjType = "WaterObjInfo")
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

    /// <summary>
    /// As long as another trigger collider is in contact with a collider attached to this object, this function will be called with that trigger collider as pOther.
    /// </summary>
    /// <param name="pOther"></param>
    //public void OnTriggerStay2D(Collider2D pOther)
    //{
    //    ObjInfo otherObjInfo = GetObjInfoFromCollider(pOther);
    //    if(otherObjInfo.IsType("ActorObjInfo"))
    //    {
    //        //Spawn a ripple in the water at the center of any actor touching this object
    //        ObjectFactory.CreateWaterRipple(otherObjInfo.Transform.position);
    //        //ParticleEffectManager.PlayParticleEffect(ParticleEffectEnum.WaterRipple, otherObjInfo.Transform.position);
    //    }
    //}

    /// <summary>
    /// When the trigger collider overlaps another collider, this
    /// function will run with that collider as the parameter.
    /// </summary>
    /// <param name="pOther">Other object's collider</param>
    protected override void OnTriggerEnter2D(Collider2D pOther)
    {
        base.OnTriggerEnter2D(pOther);

        ObjInfo otherObjInfo = GetObjInfoFromCollider(pOther);
        if (otherObjInfo.IsType("ActorObjInfo"))
        {
            ((ActorObjInfo)otherObjInfo).EnterWater();
        }
    }

    /// <summary>
    /// When the trigger collider stops overlapping another collider, this
    /// function will run with that collider as the parameter.
    /// </summary>
    /// <param name="pOther">Other object's collider</param>
    protected override void OnTriggerExit2D(Collider2D pOther)
    {
        base.OnTriggerExit2D(pOther);

        ObjInfo otherObjInfo = GetObjInfoFromCollider(pOther);
        if (otherObjInfo.IsType("ActorObjInfo"))
        {
            ((ActorObjInfo)otherObjInfo).ExitWater();
        }
    }

    #region Properties
    #endregion
}