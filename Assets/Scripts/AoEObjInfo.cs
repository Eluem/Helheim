//**************************************************************************************
// File: AoEObjInfo.cs
//
// Purpose: Defines the base class for all AoEs. Explosions, EffectFields and other
// types of AoE hazards will inherit this base class.
//
// TO DO:(Should terrain inherit this as well? Not all terrain are acutally hazards
// though.... hmmm should hazards be renamed to something like...
// Interactible? or something)
//
// Written By: Salvatore Hanusiewicz
//**************************************************************************************

using UnityEngine;
using System.Collections;

public class AoEObjInfo : HazardObjInfo
{
    #region Declarations
    #endregion

    /// <summary>
    /// This basically takes the place of a constructor
    /// </summary>
    /// <param name="pOriginObjInfo"></param>
    /// <param name="pSourceObjInfo"></param>
    /// <param name="pObjType"></param>
    public override void Initialize(bool pPooled, string pObjType = "AoEObjInfo")
    {
        base.Initialize(pPooled, pObjType);

        //Debug.Log("Initialize: " + Physics2D.OverlapPoint(transform.position, GROUNDALL_LAYERMASK)); TO DO: REMOVE ME
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
        if (pObjType == "HazardObjInfo" || pObjType == "AoEObjInfo" || pObjType == ObjType)
        {
            return true;
        }

        return false;
    }

    ///// <summary>
    ///// Checks if the collision to the passed object is blocked by something in the way
    ///// </summary>
    ///// <param name="pOther">Object to check against</param>
    ///// <returns></returns>
    //protected override bool IsCollisionBlocked(Collider2D pOther, ObjInfo pOtherObjInfo)
    //{
    //    Vector2 tempDir = pOtherObjInfo.Transform.position - m_transform.position;

    //    RaycastHit2D[] tempRaycastHit2D = Physics2D.RaycastAll(m_transform.position, tempDir, tempDir.magnitude, GROUNDALL_LAYERMASK);

    //    if(tempRaycastHit2D.Length > 0)
    //    {
    //        return true;
    //    }

    //    //foreach (RaycastHit2D raycastHit2D in tempRaycastHit2D)
    //    //{
    //    //    //TO DO: Add code to allow some obstacles to prevent blocking hazards.... keep in mind.. this should probably be something directly inside the obstacle class, if it's done at all...
    //    //    //because this system only checks against the All and Ground layers, to save cpu cycles... if I want players and other objects to have the ability to block hazards, then I need to remove the layer mask
    //    //    //as of right now, only objects with colliders on the All and Ground layers will block collisions.... and every single one will.
    //    //    //Keep in mind, if I want a player to be able to block hazards, it's a bad idea to put an All or Ground collider on them... since then they'll start receiving double collisions and other such stuff...
    //    //    //It's possible to toggle between ActorHazard and Ground... possibly... idk thought... probably would need to turn off a few others too.. like Actor (to Actor)
    //    //    //if (GetObjInfoFromCollider(raycastHit2D.collider).BlocksHazards)
    //    //    //{
    //    //    //    return true;
    //    //    //}

    //    //    return true;
    //    //}

    //    return false;
    //}

    #region Properties
    #endregion
}