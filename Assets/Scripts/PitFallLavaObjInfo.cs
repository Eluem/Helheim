//**************************************************************************************
// File: PitFallLavaObjInfo.cs
//
// Purpose: Defines a lava pit terrain type that objects can "fall" into
//
// Written By: Salvatore Hanusiewicz
//**************************************************************************************

using UnityEngine;
using System.Collections;

public class PitFallLavaObjInfo : TerrainObjInfo
{
    #region Declarations
    #endregion

    //***********************************************************************
    // Method: Initialize
    //
    // Purpose: This basically takes the place of a constructor
    //***********************************************************************
    public void Initialize()
    {
        base.Initialize("PitFallLavaObjInfo");
    }

    //***********************************************************************
    // Method: IsType
    //
    // Purpose: Accepts an object type and returns true if one of the types
    // that it is matches the type passed.
    // i.e. PlayerObjInfo is also a CharObjInfo and a DestructibleObjInfo
    //***********************************************************************
    public override bool IsType(string pObjType)
    {
        if (pObjType == "TerrainObjInfo" || pObjType == "PitFallLavaObjInfo" || pObjType == ObjType)
        {
            return true;
        }

        return false;
    }

    //***********************************************************************
    // Method: Start
    //
    // Purpose: Use this for initialization
    //***********************************************************************
    protected override void Start()
    {
        if (!m_initialized)
        {
            Initialize();
        }
    }

    //***********************************************************************
    // Method: Update
    //
    // Purpose: Update is called once per frame
    //***********************************************************************
    protected override void Update()
    {
        base.Update();
    }

    /// <summary>
    /// When the trigger collider overlaps another collider, this
    /// function will run with that collider as the parameter.
    /// </summary>
    /// <param name="pOther">Other object's collider</param>
    protected override void OnTriggerEnter2D(Collider2D pOther)
    {
        base.OnTriggerEnter2D(pOther);

        ObjInfo otherObjInfo = GetObjInfoFromCollider(pOther);

        if (otherObjInfo.IsType("DestructibleObjInfo"))
        {
            ((DestructibleObjInfo)otherObjInfo).SufferPitFallLava();
        }
    }

    //***********************************************************************
    // Method: RegisterHit
    //
    // Purpose: Builds a damage packet and passes it to the
    // DestructibleObjInfo so that it can handle responding to it correctly
    // (I'm most likely going to have characters delay responding to the
    // damage packets to make it easy to allow players to kill each other
    // or to come up with a good way to handle clashing (if I do that))
    //***********************************************************************
    //protected override void RegisterHit(DestructibleObjInfo pObjInfo, DamageContainer pDamageContainer, InteractionType pInteractionType = InteractionType.None)
    //{
    //    pObjInfo.SufferPitFallLava();
    //}

    #region Properties
    #endregion
}