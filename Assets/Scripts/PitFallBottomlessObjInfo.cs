//**************************************************************************************
// File: PitFallBottomlessObjInfo.cs
//
// Purpose: Defines a pit terrain type that objects can "fall" into
//
// Written By: Salvatore Hanusiewicz
//**************************************************************************************

using UnityEngine;
using System.Collections;

public class PitFallBottomlessObjInfo : TerrainObjInfo
{
    #region Declarations
    #endregion

    /// <summary>
    /// This basically takes the place of a constructor
    /// </summary>
    public void Initialize()
    {
        base.Initialize("PitFallBottomlessObjInfo");
	}

    /// <summary>
    /// Accepts an object type and returns true if one of the types
    /// that it is matches the type passed.
    /// i.e. PlayerObjInfo is also a CharObjInfo and a DestructibleObjInfo
    /// </summary>
    /// <param name="pObjType"></param>
    /// <returns></returns>
    public override bool IsType(string pObjType)
    {
        if (pObjType == "TerrainObjInfo" || pObjType == "PitFallBottomlessObjInfo" || pObjType == ObjType)
        {
            return true;
        }

        return false;
    }

    /// <summary>
    /// Use this for initialization
    /// </summary>
    protected override void Start()
    {
        if (!m_initialized)
        {
            Initialize();
        }
    }

    /// <summary>
    /// Update is called once per frame
    /// </summary>
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
            ((DestructibleObjInfo)otherObjInfo).SufferPitFallBottomless();
        }

        if(otherObjInfo.IsType("FlagObjInfo"))
        {
            ((FlagObjInfo)otherObjInfo).Reset();
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
    //    pObjInfo.SufferPitFallBottomless();
    //}

    #region Properties
    #endregion
}