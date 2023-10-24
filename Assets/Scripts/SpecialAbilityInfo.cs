//**************************************************************************************
// File: SpecialAbilityInfo.cs
//
// Purpose: This is the base class for all special ability definitions. It stores the
// information about the current state and other details of a special. It also handles
// collisions and all other interactions with the special.
//
// Written By: Salvatore Hanusiewicz
//**************************************************************************************

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class SpecialAbilityInfo
{
    public enum DisplayMode
    {
        /// <summary>
        /// (Default) Display nothing
        /// </summary>
        None = 0,
        /// <summary>
        /// Display an ammo meter
        /// </summary>
        Ammo = 1,
        /// <summary>
        /// Display a gradient energy (generic resource) meter
        /// </summary>
        Energy = 2,
        /// <summary>
        /// Display a channel bar
        /// </summary>
        Channel = 3,
        /// <summary>
        /// Display a cooldown bar
        /// </summary>
        Cooldown = 4,
        /// <summary>
        /// Display an ammo meter except that the last unfilled bullet is treated as a cooldown bar
        /// </summary>
        AmmoCooldown = 5
    }

    #region Declarations
    #region Pointers
    protected CharObjInfo m_charObjInfo; //Points to the character that owns this special ability
    protected Loadout m_loadout; //Points to the loadout that this special ability is contained within
    #endregion

    protected int m_ID; //Used to pass to the animator to direct it to the correct state
    protected string m_name;

    #region Resources //TO DO: implement basic code to be used for all the resources (i.e. basic EnergyUpdate, ChannelUpdate, CooldownUpdate, ect so that others may call them)
    protected int m_ammo; //Stores the ammo for this special ability
    protected int m_maxAmmo; //Stores the maximum amount of ammo for this special ability

    protected float m_energy; //Stores a generic "energy" resource
    protected int m_maxEnergy; //Stores the maximum amount of generic "energy" this special ability can contain
    protected float m_energyRechargeRate; //Stores the rate at which energy will recharge
    protected float m_energyRechargeDelay; //Stores the amount of delay time before the energy will begin to recharge again after spending energy
    protected float m_energyRechargeDelayCurr; //Stores the current amount of time waited since last spending energy

    protected float m_channel; //Stores a generic channel value
    protected float m_channelTime; //Time it takes for the channel to complete

    protected float m_cooldown; //Stores the current time before this ability finishes cooling down
    protected float m_cooldownDuration; //Stores the amount of time this ability will be set to cooldown when put on a cd
    #endregion

    protected int m_slot; //Indicates which slot this ability is in (by defualt slot 1 is X and slot 2 is Y on an xbox gamepad)

    protected DisplayMode m_displayMode; //Stores which mode of display this special ability uses (default is none)
	#endregion
	
	//***********************************************************************
	// Method: SpecialAbilityInfo
	//
	// Purpose: Constructor for class
	//***********************************************************************
	public SpecialAbilityInfo(CharObjInfo pCharObjInfo, Loadout pLoadout, int pSlot, int pID, string pName, DisplayMode pDisplayMode = DisplayMode.None)
	{
        m_charObjInfo = pCharObjInfo;
        m_loadout = pLoadout;

        m_ID = pID;
        m_name = pName;

        m_ammo = 0;
        m_maxAmmo = 0;

        m_energy = 0;
        m_maxEnergy = 0;
        m_energyRechargeRate = 0;
        m_energyRechargeDelay = 0;
        m_energyRechargeDelayCurr = 0;

        m_channel = 0;
        m_channelTime = 0;

        m_cooldown = 0;
        m_cooldownDuration = 0;

        m_slot = pSlot;

        m_displayMode = pDisplayMode;
	}

    //***********************************************************************
    // Method: IsAllowed
    //
    // Purpose: Defines whether or not the ability is currently allowed to
    // be activated. This will be determined by conditions such as having
    // enough mana, stamina or other resource... or other conditions
    //***********************************************************************
    public abstract bool IsAllowed();

    //***********************************************************************
    // Method: GainAmmo
    //
    // Purpose: Accepts an integer for ammo to gain and adds it
    //***********************************************************************
    public virtual void GainAmmo(int pAmmo)
    {
        m_ammo += pAmmo;

        if(m_ammo > m_maxAmmo)
        {
            m_ammo = m_maxAmmo;
        }

        //Update the animator to reflect the correct ammo (normally it is set on button press, but if it changes after a button is pressed AND after a trigger is set but before it's consumed, it needs this to update it)
        if (m_charObjInfo.Animator.GetInteger("SpecialActionMajorSlot") == m_slot)
        {
            m_charObjInfo.Animator.SetInteger("SpecialActionMajorAmmo", m_ammo);
        }
        if (m_charObjInfo.Animator.GetInteger("SpecialActionMinorSlot") == m_slot)
        {
            m_charObjInfo.Animator.SetInteger("SpecialActionMinorAmmo", m_ammo);
        }
    }

    //***********************************************************************
    // Method: SpendAmmo
    //
    // Purpose: Accepts an integer for ammo to spend and spends it
    //***********************************************************************
    public virtual void SpendAmmo(int pAmmo)
    {
        m_ammo -= pAmmo;

        if(m_ammo < 0)
        {
            m_ammo = 0;
        }

        //Update the animator to reflect the correct ammo (normally it is set on button press, but if it changes after a button is pressed AND after a trigger is set but before it's consumed, it needs this to update it)
        if (m_charObjInfo.Animator.GetInteger("SpecialActionMajorSlot") == m_slot)
        {
            m_charObjInfo.Animator.SetInteger("SpecialActionMajorAmmo", m_ammo);

            if(!IsAllowed())
            {
                m_charObjInfo.Animator.ResetTrigger("SpecialActionMajorPressed");
            }
        }
        if (m_charObjInfo.Animator.GetInteger("SpecialActionMinorSlot") == m_slot)
        {
            m_charObjInfo.Animator.SetInteger("SpecialActionMinorAmmo", m_ammo);

            if (!IsAllowed())
            {
                m_charObjInfo.Animator.ResetTrigger("SpecialActionMinorPressed");
            }
        }
    }

    #region Properties
    public int ID
    {
        get
        {
            return m_ID;
        }
    }

    public string Name
    {
        get
        {
            return m_name;
        }
    }

    public int Ammo
    {
        get
        {
            return m_ammo;
        }
    }

    public int MaxAmmo
    {
        get
        {
            return m_maxAmmo;
        }
    }

    public float EnergyFloat
    {
        get
        {
            return m_energy;
        }
    }

    public int Energy
    {
        get
        {
            return (int)m_energy;
        }
    }

    public int MaxEnergy
    {
        get
        {
            return MaxEnergy;
        }
    }

    public float Channel
    {
        get
        {
            return m_channel;
        }
    }

    public float ChannelTime
    {
        get
        {
            return m_channelTime;
        }
    }

    public float Cooldown
    {
        get
        {
            return m_cooldown;
        }
    }

    public float CooldownDuration
    {
        get
        {
            return m_cooldownDuration;
        }
    }

    public DisplayMode GUIDisplayMode
    {
        get
        {
            return m_displayMode;
        }
    }
#endregion
}