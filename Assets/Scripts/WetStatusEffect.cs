//**************************************************************************************
// File: WetStatusEffect.cs
//
// Purpose: This defines how the wet status effect works
//
// Written By: Salvatore Hanusiewicz
//**************************************************************************************

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WetStatusEffect : StatusEffect
{
    #region Declarations
    #endregion

    /// <summary>
    /// Constructor for class
    /// </summary>
    /// <param name="pOriginObj"></param>
    /// <param name="pSourceObj"></param>
    /// <param name="pTargetObj"></param>
    public WetStatusEffect(ObjInfo pOriginObj, ObjInfo pSourceObj, DestructibleObjInfo pTargetObj) : base(pOriginObj, pSourceObj, pTargetObj, StatusEffectType.Wet, new DamageContainer(0), 1, 3, false, AudioClipEnum.None, ParticleEffectEnum.None, KnockbackType.None, KnockbackType.None)
    {
        
	}

    /// <summary>
    /// Called once per frame by the parent StatusEffectManager
    /// </summary>
    /// <param name="pDeltaTime"></param>
    public override void Update(float pDeltaTime)
    {
        if (Dripping)
        {
            m_currTickTime -= pDeltaTime;

            if (m_currTickTime <= 0)
            {
                Tick();

                m_currTickTime = m_timePerTick;
            }
        }
    }

    /// <summary>
    /// Causes the status effect to apply all its effects
    /// This also should handle managing remaining ticks and whehter or not
    /// the status effect is done
    /// </summary>
    public override void Tick()
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
    public override void Merge(StatusEffect pStatusEffect)
    {
        m_currTickTime = m_timePerTick; //Reset the timer before the wet effect wears off
    }

    #region Properties
    //Returns whehter or not the target is dripping (wet, but out of water)
    public bool Dripping
    {
        get
        {
            return !m_targetObj.Animator.GetBool("InWater");
        }
    }
    #endregion
}