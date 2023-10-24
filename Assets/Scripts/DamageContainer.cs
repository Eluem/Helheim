//**************************************************************************************
// File: DamageContainer.cs
//
// Purpose: This is basically simply meant to be a way to encapsulate all the different
// damage type variables so that I don't need to copy and paste their definitions
// and pass throughs to each and every class and function that needs to pass all
// this damage info through. It should make it easier to add new types of damage since
// I'll only need to add it here and add the specific code for responding to it.
//
// Note: This is sort of like a damage "packet" in the sense that it's a packet of
// damage info... but the DamagePacketStatusEffect is already using the
// name DamagePacket to describe something that passes generic damage info to another
// object lol
//
// Written By: Salvatore Hanusiewicz
//**************************************************************************************

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DamageContainer
{
    #region Declarations
    protected int m_damage; //Damage to health
    protected int m_poiseDamage; //Damage to poise
    protected int m_knockback; //Knockback effect
    protected int m_poison; //Poison status
    protected int m_burn; //Burn status
    protected int m_bleed; //Bleed status
    protected int m_blind; //Blind status
    protected int m_launchPower; //Launch power
    protected float m_launchHangTime; //Launch hang time (in 10ths of a second)
    protected int m_knockDownPower; //KnockDown power
    protected float m_knockDownTime; //KnockDown time (in 10ths of a second)
    #endregion

    //***********************************************************************
    // Method: DamagePacketStatusEffect
    //
    // Purpose: Constructor for class
    //***********************************************************************
    public DamageContainer()
    {
        m_damage = 0; //Damage to health
        m_poiseDamage = 0; //Damage to poise
        m_knockback = 0; //Knockback effect
        m_poison = 0; //Poison status
        m_burn = 0; //Burn status
        m_bleed = 0; //Bleed status
        m_blind = 0; //Blind status
        m_launchPower = 0; //Launch power
        m_launchHangTime = 1; //Launch hang time (in 10ths of a second)
        m_knockDownPower = 0; //KnockDown power
        m_knockDownTime = 1; //KnockDown time (in 10ths of a second)
    }

    //***********************************************************************
    // Method: DamagePacketStatusEffect
    //
    // Purpose: Constructor for class
    //***********************************************************************
    public DamageContainer(int pDamage, int pPoiseDamage = 0, int pKnockback = 0, int pPoison = 0, int pBurn = 0, int pBleed = 0, int pBlind = 0, int pLaunchPower = 0, float pLaunchHangTime = 1, int pKnockDownPower = 0, float pKnockDownTime = 1)
    {
        m_damage = pDamage;
        m_poiseDamage = pPoiseDamage;
        m_knockback = pKnockback;
        m_poison = pPoison;
        m_burn = pBurn;
        m_bleed = pBleed;
        m_blind = pBlind;
        m_launchPower = pLaunchPower;
        m_launchHangTime = pLaunchHangTime;
        m_knockDownPower = pKnockDownPower;
        m_knockDownTime = pKnockDownTime;
    }

    #region Properties
    public int Damage
    {
        get
        {
            return m_damage;
        }
    }

    public int PoiseDamage
    {
        get
        {
            return m_poiseDamage;
        }
    }

    public int Knockback
    {
        get
        {
            return m_knockback;
        }
    }

    public int Poison
    {
        get
        {
            return m_poison;
        }
    }

    public int Burn
    {
        get
        {
            return m_burn;
        }
    }

    public int Bleed
    {
        get
        {
            return m_bleed;
        }
    }

    public int Blind
    {
        get
        {
            return m_blind;
        }
    }

    public int LaunchPower
    {
        get
        {
            return m_launchPower;
        }
    }

    public float LaunchHangTime
    {
        get
        {
            return m_launchHangTime;
        }
    }

    public int KnockDownPower
    {
        get
        {
            return m_knockDownPower;
        }
    }

    public float KnockDownTime
    {
        get
        {
            return m_knockDownTime;
        }
    }
    #endregion
}