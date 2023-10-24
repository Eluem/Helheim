//**************************************************************************************
// File: ObstacleObjInfo.cs
//
// Purpose: This is the base class for all objects that should be treated as
// indestructible obstacles (such as walls)
//
// Written By: Salvatore Hanusiewicz
//**************************************************************************************

using UnityEngine;
using System.Collections;

public abstract class ObstacleObjInfo : ObjInfo
{
    #region Declarations
    #endregion

    //***********************************************************************
    // Method: Initialize
    //
    // Purpose: This basically takes the place of a constructor
    //***********************************************************************
    public void Initialize(string pObjType = "ObstacleObjInfo")
    {
        base.Initialize(false, pObjType, true);
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
        if (pObjType == "ObstacleObjInfo" || pObjType == ObjType)
        {
            return true;
        }

        return false;
    }

    /// <summary>
    /// Initializes all the interactions for this object
    /// </summary>
    protected override void InitializeInteractionData()
    {
        //Set default interaction audio clips
        m_interactAudioClips[(int)InteractionType.SharpLight] = AudioClipEnum.DamageTest;
        m_interactAudioClips[(int)InteractionType.SharpMedium] = AudioClipEnum.DamageTest;
        m_interactAudioClips[(int)InteractionType.SharpHeavy] = AudioClipEnum.DamageTest;
        m_interactAudioClips[(int)InteractionType.BluntLight] = AudioClipEnum.DamageTest;
        m_interactAudioClips[(int)InteractionType.BluntMedium] = AudioClipEnum.DamageTest;
        m_interactAudioClips[(int)InteractionType.BluntHeavy] = AudioClipEnum.DamageTest;

        //Set default interaction particle effects
        m_interactParticleEffects[(int)InteractionType.SharpLight] = ParticleEffectEnum.HitSparks;
        m_interactParticleEffects[(int)InteractionType.SharpMedium] = ParticleEffectEnum.HitSparks;
        m_interactParticleEffects[(int)InteractionType.SharpHeavy] = ParticleEffectEnum.HitSparks;
        m_interactParticleEffects[(int)InteractionType.BluntLight] = ParticleEffectEnum.HitDust;
        m_interactParticleEffects[(int)InteractionType.BluntMedium] = ParticleEffectEnum.HitDust;
        m_interactParticleEffects[(int)InteractionType.BluntHeavy] = ParticleEffectEnum.HitDust;
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

    #region Properties
    #endregion
}
