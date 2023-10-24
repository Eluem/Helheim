//**************************************************************************************
// File: DamagePacketStatusEffect.cs
//
// Purpose: This class takes advantage of the StatusEffect system to apply delayed
// damage to players. This is meant to allow for simultaneous hits to have a wider
// window.
//
// Written By: Salvatore Hanusiewicz
//**************************************************************************************

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DamagePacketStatusEffect : StatusEffect
{
    #region Declarations
    #endregion

    /// <summary>
    /// Constructor for class
    /// </summary>
    /// <param name="pOriginObj">Original origin of damage (player, turret, ect)</param>
    /// <param name="pSourceObj">Actual object that made contact for damage (used for knockback and such)</param>
    /// <param name="pTargetObj">Object that was hit</param>
    /// <param name="pTicks">Number of ticks of damage</param>
    /// <param name="pTimePerTick">Amount of time before each tick</param>
    /// <param name="pAudioClip">Audio clip to play on tick</param>
    /// <param name="pParticleEffect">Particle effect to play on tick</param>
    /// <param name="pStatusEffectType">Status effect type</param>
    public DamagePacketStatusEffect(ObjInfo pOriginObj, ObjInfo pSourceObj, DestructibleObjInfo pTargetObj, DamageContainer pDamageContainer, int pTicks, float pTimePerTick, AudioClipEnum pAudioClip = AudioClipEnum.None, ParticleEffectEnum pParticleEffect = ParticleEffectEnum.None, KnockbackType pKnockbackType = KnockbackType.SourceRadial, KnockbackType pEffectDirType = KnockbackType.None, Vector3 pExternalDir = default(Vector3), StatusEffectType pStatusEffectType = StatusEffectType.DamagePacket) : base(pOriginObj, pSourceObj, pTargetObj, pStatusEffectType, pDamageContainer, pTicks, pTimePerTick, true, pAudioClip, pParticleEffect, pKnockbackType, pEffectDirType, pExternalDir)
    {
    }
    
    /// <summary>
    /// Causes the status effect to apply all its effects
    /// This also should handle managing remaining ticks and whehter or not
    /// the status effect is done
    /// </summary>
    public override void Tick()
    {
        m_targetObj.PlayAudio(m_audioClip);

        //m_targetObj.PlayParticleEffectWithDirection(m_particleEffect, m_sourceObj.transform.up);
        //m_targetObj.PlayParticleEffect(m_particleEffect);
        //m_targetObj.PlayParticleEffect(m_particleEffect, m_sourceObj.transform.up);

        switch (m_effectDirType)
        {
            case KnockbackType.None:
                m_targetObj.PlayParticleEffect(m_particleEffect);
                break;

            case KnockbackType.OriginFacing:
                m_targetObj.PlayParticleEffectWithDirection(m_particleEffect, m_initOriginUp);
                break;
            case KnockbackType.OriginRadial:
                m_targetObj.PlayParticleEffectWithDirection(m_particleEffect, (m_initTargetPos - m_initOriginPos).normalized);
                break;
            case KnockbackType.SourceFacing:
                m_targetObj.PlayParticleEffectWithDirection(m_particleEffect, m_initSourceUp);
                break;
            case KnockbackType.SourceRadial:
                m_targetObj.PlayParticleEffectWithDirection(m_particleEffect, (m_initTargetPos - m_initSourcePos).normalized);
                break;

            case KnockbackType.CurrOriginFacing:
                m_targetObj.PlayParticleEffectWithDirection(m_particleEffect, m_originObj.Transform.up);
                break;
            case KnockbackType.CurrOriginRadial:
                m_targetObj.PlayParticleEffectWithDirection(m_particleEffect, (m_targetObj.Transform.position - m_originObj.Transform.position).normalized);
                break;
            case KnockbackType.CurrSourceFacing:
                m_targetObj.PlayParticleEffectWithDirection(m_particleEffect, m_sourceObj.Transform.up);
                break;
            case KnockbackType.CurrSourceRadial:
                m_targetObj.PlayParticleEffectWithDirection(m_particleEffect, (m_targetObj.Transform.position - m_sourceObj.Transform.position).normalized);
                break;

            case KnockbackType.ExternalDir:
                m_targetObj.PlayParticleEffectWithDirection(m_particleEffect, m_externalDir.normalized);
                break;
        }

        if (m_damageContainer.Damage > 0)
        {
            m_targetObj.SufferDamage(m_damageContainer.Damage);
        }

        if (m_damageContainer.PoiseDamage > 0)
        {
            m_targetObj.SufferPoiseDamage(m_damageContainer.PoiseDamage);
        }

        if (m_damageContainer.Knockback > 0)
        {
            switch(m_knockbackType)
            {
                case KnockbackType.OriginFacing:
                    m_targetObj.SufferKnockback(m_initOriginUp * m_damageContainer.Knockback);
                    break;
                case KnockbackType.OriginRadial:
                    m_targetObj.SufferKnockback((m_initTargetPos - m_initOriginPos).normalized * m_damageContainer.Knockback);
                    break;
                case KnockbackType.SourceFacing:
                    m_targetObj.SufferKnockback(m_initSourceUp * m_damageContainer.Knockback);
                    break;
                case KnockbackType.SourceRadial:
                    m_targetObj.SufferKnockback((m_initTargetPos - m_initSourcePos).normalized * m_damageContainer.Knockback);
                    break;

                case KnockbackType.CurrOriginFacing:
                    m_targetObj.SufferKnockback(m_originObj.Transform.up * m_damageContainer.Knockback);
                    break;
                case KnockbackType.CurrOriginRadial:
                    m_targetObj.SufferKnockback((m_targetObj.Transform.position - m_originObj.Transform.position).normalized * m_damageContainer.Knockback);
                    break;
                case KnockbackType.CurrSourceFacing:
                    m_targetObj.SufferKnockback(m_sourceObj.Transform.up * m_damageContainer.Knockback);
                    break;
                case KnockbackType.CurrSourceRadial:
                    m_targetObj.SufferKnockback((m_targetObj.Transform.position - m_sourceObj.Transform.position).normalized * m_damageContainer.Knockback);
                    break;

                case KnockbackType.ExternalDir:
                    m_targetObj.SufferKnockback(m_externalDir.normalized * m_damageContainer.Knockback);
                    break;
            }
        }

        if (m_damageContainer.LaunchPower > 0)
        {
            switch (m_knockbackType) //Reusing knockback type since I don't see a reason that they'd ever be different
            {
                //case KnockbackType.OriginFacing:
                //    m_targetObj.SufferLaunch(m_originObj, m_sourceObj, m_originObj.Transform.up * m_damageContainer.LaunchPower, m_damageContainer.LaunchHangTime);
                //    break;
                //case KnockbackType.OriginRadial:
                //    m_targetObj.SufferLaunch(m_originObj, m_sourceObj, (m_targetObj.Transform.position - m_originObj.Transform.position).normalized * m_damageContainer.LaunchPower, m_damageContainer.LaunchHangTime);
                //    break;
                //case KnockbackType.SourceFacing:
                //    m_targetObj.SufferLaunch(m_originObj, m_sourceObj, m_sourceObj.Transform.up * m_damageContainer.LaunchPower, m_damageContainer.LaunchHangTime);
                //    break;
                //case KnockbackType.SourceRadial:
                //    m_targetObj.SufferLaunch(m_originObj, m_sourceObj, (m_targetObj.Transform.position - m_sourceObj.Transform.position).normalized * m_damageContainer.LaunchPower, m_damageContainer.LaunchHangTime);
                //    break;

                case KnockbackType.OriginFacing:
                    m_targetObj.SufferLaunch(m_originObj, m_sourceObj, m_initOriginUp * m_damageContainer.LaunchPower, m_damageContainer.LaunchHangTime);
                    break;
                case KnockbackType.OriginRadial:
                    m_targetObj.SufferLaunch(m_originObj, m_sourceObj, (m_initTargetPos - m_initOriginPos).normalized * m_damageContainer.LaunchPower, m_damageContainer.LaunchHangTime);
                    break;
                case KnockbackType.SourceFacing:
                    m_targetObj.SufferLaunch(m_originObj, m_sourceObj, m_initSourceUp * m_damageContainer.LaunchPower, m_damageContainer.LaunchHangTime);
                    break;
                case KnockbackType.SourceRadial:
                    m_targetObj.SufferLaunch(m_originObj, m_sourceObj, (m_initTargetPos - m_initSourcePos).normalized * m_damageContainer.LaunchPower, m_damageContainer.LaunchHangTime);
                    break;

                case KnockbackType.CurrOriginFacing:
                    m_targetObj.SufferLaunch(m_originObj, m_sourceObj, m_originObj.Transform.up * m_damageContainer.LaunchPower, m_damageContainer.LaunchHangTime);
                    break;
                case KnockbackType.CurrOriginRadial:
                    m_targetObj.SufferLaunch(m_originObj, m_sourceObj, (m_targetObj.Transform.position - m_originObj.Transform.position).normalized * m_damageContainer.LaunchPower, m_damageContainer.LaunchHangTime);
                    break;
                case KnockbackType.CurrSourceFacing:
                    m_targetObj.SufferLaunch(m_originObj, m_sourceObj, m_sourceObj.Transform.up * m_damageContainer.LaunchPower, m_damageContainer.LaunchHangTime);
                    break;
                case KnockbackType.CurrSourceRadial:
                    m_targetObj.SufferLaunch(m_originObj, m_sourceObj, (m_targetObj.Transform.position - m_sourceObj.Transform.position).normalized * m_damageContainer.LaunchPower, m_damageContainer.LaunchHangTime);
                    break;

                case KnockbackType.ExternalDir:
                    m_targetObj.SufferLaunch(m_originObj, m_sourceObj, m_externalDir.normalized * m_damageContainer.LaunchPower, m_damageContainer.LaunchHangTime);
                    break;
            }
        }
        
        if (m_damageContainer.KnockDownPower > 0)
        {
            switch (m_knockbackType) //Reusing knockback type since I don't see a reason that they'd ever be different
            {
                //case KnockbackType.OriginFacing:
                //    m_targetObj.SufferKnockDown(m_originObj, m_sourceObj, m_originObj.Transform.up * m_damageContainer.KnockDownPower, m_damageContainer.KnockDownTime);
                //    break;
                //case KnockbackType.OriginRadial:
                //    m_targetObj.SufferKnockDown(m_originObj, m_sourceObj, (m_targetObj.Transform.position - m_originObj.Transform.position).normalized * m_damageContainer.KnockDownPower, m_damageContainer.KnockDownTime);
                //    break;
                //case KnockbackType.SourceFacing:
                //    m_targetObj.SufferKnockDown(m_originObj, m_sourceObj, m_sourceObj.Transform.up * m_damageContainer.KnockDownPower, m_damageContainer.KnockDownTime);
                //    break;
                //case KnockbackType.SourceRadial:
                //    m_targetObj.SufferKnockDown(m_originObj, m_sourceObj, (m_targetObj.Transform.position - m_sourceObj.Transform.position).normalized * m_damageContainer.KnockDownPower, m_damageContainer.KnockDownTime);
                //    break;

                case KnockbackType.OriginFacing:
                    m_targetObj.SufferKnockDown(m_originObj, m_sourceObj, m_initOriginUp * m_damageContainer.KnockDownPower, m_damageContainer.KnockDownTime);
                    break;
                case KnockbackType.OriginRadial:
                    m_targetObj.SufferKnockDown(m_originObj, m_sourceObj, (m_initTargetPos - m_initOriginPos).normalized * m_damageContainer.KnockDownPower, m_damageContainer.KnockDownTime);
                    break;
                case KnockbackType.SourceFacing:
                    m_targetObj.SufferKnockDown(m_originObj, m_sourceObj, m_initSourceUp * m_damageContainer.KnockDownPower, m_damageContainer.KnockDownTime);
                    break;
                case KnockbackType.SourceRadial:
                    m_targetObj.SufferKnockDown(m_originObj, m_sourceObj, (m_initTargetPos - m_initSourcePos).normalized * m_damageContainer.KnockDownPower, m_damageContainer.KnockDownTime);
                    break;

                case KnockbackType.CurrOriginFacing:
                    m_targetObj.SufferKnockDown(m_originObj, m_sourceObj, m_originObj.Transform.up * m_damageContainer.KnockDownPower, m_damageContainer.KnockDownTime);
                    break;
                case KnockbackType.CurrOriginRadial:
                    m_targetObj.SufferKnockDown(m_originObj, m_sourceObj, (m_targetObj.Transform.position - m_originObj.Transform.position).normalized * m_damageContainer.KnockDownPower, m_damageContainer.KnockDownTime);
                    break;
                case KnockbackType.CurrSourceFacing:
                    m_targetObj.SufferKnockDown(m_originObj, m_sourceObj, m_sourceObj.Transform.up * m_damageContainer.KnockDownPower, m_damageContainer.KnockDownTime);
                    break;
                case KnockbackType.CurrSourceRadial:
                    m_targetObj.SufferKnockDown(m_originObj, m_sourceObj, (m_targetObj.Transform.position - m_sourceObj.Transform.position).normalized * m_damageContainer.KnockDownPower, m_damageContainer.KnockDownTime);
                    break;

                case KnockbackType.ExternalDir:
                    m_targetObj.SufferKnockDown(m_originObj, m_sourceObj, m_externalDir.normalized * m_damageContainer.KnockDownPower, m_damageContainer.KnockDownTime);
                    break;
            }
        }

        if (m_damageContainer.Poison > 0)
        {
            m_targetObj.SufferPoison(m_damageContainer.Poison);
        }

        if (m_damageContainer.Burn > 0)
        {
            //m_targetObj.SufferBurn(m_burn);
        }

        if (m_damageContainer.Bleed > 0)
        {
            //m_targetObj.SufferBleed(m_bleed);
        }

        if (m_damageContainer.Blind > 0)
        {
            //m_targetObj.SufferBlind(m_blind);
        }

        base.Tick();
    }

    #region Properties
    #endregion
}
