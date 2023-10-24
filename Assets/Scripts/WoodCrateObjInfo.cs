//**************************************************************************************
// File: WoodCrateObjInfo.cs
//
// Purpose: Simple wooden crate
//
// Written By: Salvatore Hanusiewicz
//**************************************************************************************

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WoodCrateObjInfo : DestructibleObjInfo
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
        base.Initialize(false, "WoodCrateObjInfo");

        //TO DO: Change Interaction thing,there's a function to override now
        //Set default interaction particle effects
        m_interactParticleEffects[(int)InteractionType.SharpLight] = ParticleEffectEnum.Blood;
        m_interactParticleEffects[(int)InteractionType.SharpMedium] = ParticleEffectEnum.None;
        m_interactParticleEffects[(int)InteractionType.SharpHeavy] = ParticleEffectEnum.Blood;
        m_interactParticleEffects[(int)InteractionType.BluntLight] = ParticleEffectEnum.None;
        m_interactParticleEffects[(int)InteractionType.BluntMedium] = ParticleEffectEnum.None;
        m_interactParticleEffects[(int)InteractionType.BluntHeavy] = ParticleEffectEnum.None;
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
        if (pObjType == "ActorObjInfo" || pObjType == "DestructibleObjInfo" || pObjType == "CharObjInfo" || pObjType == "WoodCrateObjInfo" || pObjType == ObjType)
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
    // Method: OnDeath
    //
    // Purpose: Handles what happens when a destructible object runs out
    // of health
    //***********************************************************************
    protected override void OnDeath()
    {
        OnDeathFinal();
    }

    //***********************************************************************
    // Method: OnDeathFinal
    //
    // Purpose: This is used so that OnDeath can trigger an animation which
    // can trigger OnDeathFinalAnim which will call OnDeathFinal
    //***********************************************************************
    protected override void OnDeathFinal()
    {
        DestroyMe();
    }

    #region Properties
    #endregion
}
