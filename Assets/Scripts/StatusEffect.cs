//**************************************************************************************
// File: StatusEffect.cs
//
// Purpose: This is the base class for all StatusEffects
//
// Written By: Salvatore Hanusiewicz
//**************************************************************************************

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum StatusEffectType
{
    DamagePacket = 0,
    Poison = 1,
    Burn = 2,
    Bleed = 3,
    Blind = 4,
    Wet = 5
}

public enum KnockbackType
{
    None = -1,
    OriginFacing = 0,
    OriginRadial = 1,
    SourceFacing = 2,
    SourceRadial = 3,
    CurrOriginFacing = 4,
    CurrOriginRadial = 5,
    CurrSourceFacing = 6,
    CurrSourceRadial = 7,
    ExternalDir = 8
}

public abstract class StatusEffect
{
    #region Declarations
    #region Pointers
    protected ObjInfo m_originObj;
    protected ObjInfo m_sourceObj;
    protected DestructibleObjInfo m_targetObj;
    #endregion

    protected Vector3 m_initOriginPos; //Stores the initial position of the origin
    protected Vector3 m_initSourcePos; //Stores the initial position of the source
    protected Vector3 m_initTargetPos; //Stores the initial position of the target

    protected Vector3 m_initOriginUp; //Stores the initial Up vector of the origin
    protected Vector3 m_initSourceUp; //Stores the initial Up vector of the source
    protected Vector3 m_initTargetUp; //Stores the initial Up vector of the target

    protected StatusEffectType m_effectType; //Indicates the type of status effectthis is

    protected DamageContainer m_damageContainer;

    protected int m_ticks; //Ticks remaining (when it hits 0, it will be destroyed.. generally)
    protected float m_timePerTick; //Time that a tick takes to resolve (-1 means that it is resolved immediately)
    protected float m_currTickTime; //Time remaining for the current tick

    protected bool m_canStack; //Defines whether or not the status effect can have multiple instances running side by side

    protected AudioClipEnum m_audioClip; //This is the audioclip that will be played each tick
    protected ParticleEffectEnum m_particleEffect; //This is the particle effect that will be played each tick

    protected KnockbackType m_knockbackType; //This is the type of knockback that will be applied if there is any to apply

    protected KnockbackType m_effectDirType; //This determines how the effect's direction will be determined

    protected Vector3 m_externalDir; //External direction passed in for use with knockback and effect direction

    protected bool m_done; //Inidicates that the effect has finished it's process and should be removed from the StatusEffectManager
    #endregion

    /// <summary>
    /// Constructor for class
    /// </summary>
    /// <param name="pOriginObj"></param>
    /// <param name="pSourceObj"></param>
    /// <param name="pTargetObj"></param>
    /// <param name="pStatusEffectType"></param>
    /// <param name="pDamageContainer"></param>
    /// <param name="pTicks"></param>
    /// <param name="pTimePerTick"></param>
    /// <param name="pCanStack"></param>
    /// <param name="pAudioClip"></param>
    /// <param name="pParticleEffect"></param>
    /// <param name="pKnockbackType"></param>
    public StatusEffect(ObjInfo pOriginObj, ObjInfo pSourceObj, DestructibleObjInfo pTargetObj, StatusEffectType pStatusEffectType, DamageContainer pDamageContainer, int pTicks, float pTimePerTick, bool pCanStack, AudioClipEnum pAudioClip = AudioClipEnum.None, ParticleEffectEnum pParticleEffect = ParticleEffectEnum.None, KnockbackType pKnockbackType = KnockbackType.SourceRadial, KnockbackType pEffectDirType = KnockbackType.None, Vector3 pExternalDir = default(Vector3))
    {
        m_originObj = pOriginObj;
        m_sourceObj = pSourceObj;
        m_targetObj = pTargetObj;

        m_initTargetPos = m_targetObj.Transform.position;
        m_initTargetUp = m_targetObj.Transform.up;

        if(m_sourceObj == null)
        {
            m_initSourcePos = m_initTargetPos;
            m_initSourceUp = m_initTargetUp;
        }
        else
        {
            m_initSourcePos = m_sourceObj.Transform.position;
            m_initSourceUp = m_sourceObj.Transform.up;
        }

        if (m_originObj == null)
        {
            m_initOriginPos = m_initSourcePos;
            m_initOriginUp = m_initSourceUp;
        }
        else
        {
            m_initOriginPos = m_originObj.Transform.position;
            m_initOriginUp = m_originObj.Transform.up;
        }

        m_effectType = pStatusEffectType;

        m_damageContainer = pDamageContainer;

        m_ticks = pTicks;
        m_timePerTick = pTimePerTick;
        m_currTickTime = m_timePerTick;

        m_canStack = pCanStack;
        m_audioClip = pAudioClip;
        m_particleEffect = pParticleEffect;

        m_knockbackType = pKnockbackType;

        m_effectDirType = pEffectDirType;

        m_externalDir = pExternalDir;

        m_done = false;
    }

    /// <summary>
    /// Called once per frame by the parent StatusEffectManager
    /// </summary>
    /// <param name="pDeltaTime"></param>
    public virtual void Update(float pDeltaTime)
    {
        m_currTickTime -= pDeltaTime;
        
        if (m_currTickTime <= 0)
        {
            Tick();

            m_currTickTime = m_timePerTick;
        }
    }

    /// <summary>
    /// Causes the status effect to apply all its effects
    /// This also should handle managing remaining ticks and whehter or not
    /// the status effect is done
    /// </summary>
    public virtual void Tick()
    {
        m_ticks--;

        if (m_ticks < 1)
        {
            m_done = true;
        }
    }

    /// <summary>
    /// Accepts another status effect (it should be the same type
    /// as this effect) and handles merging it. This is mainly used for
    /// non-stackable status effects (most things other than an initial hit..
    /// generally anything like poison, bleed, ect will be non-stackable
    /// and instead will absorb any additional similar status effects)
    /// 
    /// Note: Depending on the specific type of status effect and it's
    /// current state, it may simply ignore the attempt to merge or do
    /// something else.
    /// EX: With blind damage, if the player had their blind effect pop and
    /// it's currently applying to them, additional blind damage will be
    /// ignored. However, with burn damage, you can keep extending the burn
    /// timer by hitting them with more burn damage.
    /// Another example could be a status effect that acts differently when
    /// it's triggered, converting the effect damage to direct damage.
    /// </summary>
    /// <param name="pStatusEffect"></param>
    public virtual void Merge(StatusEffect pStatusEffect)
    {

    }

    #region Properties
    public StatusEffectType EffectType
    {
        get
        {
            return m_effectType;
        }
    }

    public bool Done
    {
        get
        {
            return m_done;
        }
    }

    public int Ticks
    {
        get
        {
            return m_ticks;
        }
    }

    public float TimePerTick
    {
        get
        {
            return m_timePerTick;
        }
    }

    public bool CanStack
    {
        get
        {
            return m_canStack;
        }
    }
    #endregion
}
