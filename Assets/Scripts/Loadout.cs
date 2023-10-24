//**************************************************************************************
// File: Loadout.cs
//
// Purpose: Holds all the information about a character's current loadout
//
// Written By: Salvatore Hanusiewicz
//**************************************************************************************

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Loadout
{
    #region Declarations
    protected CharObjInfo m_charObjInfo; //Stores a pointer to the character using this weapon
    protected GameObject m_gameObject; //Stores a pointer to the game object for the character using this weapon

    protected WeaponInfo m_unarmed; //This weapon is always instantiated, it handles any situation where the player gets disarmed or something like that
    protected WeaponInfo m_weapon;
    protected WeaponInfo m_altWeapon; //This is only used if the player has chosen the hammerspace scabbard thingy (I don't know what it's named but it takes all slots to let you use 2 weapons)
    protected int m_selectedWeapon = 0; //Determines which of the two weapons the player currently has selected. 0 = unarmed, 1 = m_weapon, 2 = m_altWeapon.
                                        //It can only become 2 if they're using a specific passive ability

    protected int m_prevSelectedWeapon = 0; //Stores the value of the previously selected weapon

    protected PassiveAbilityInfo m_passiveAbility;

    protected SpecialAbilityInfo m_specialAbility1;
    protected SpecialAbilityInfo m_specialAbility2;

    protected List<CarriableObjInfo> m_carriedObjInfos; //List of all currently carried objects
    #endregion

    /// <summary>
    /// Constructor for class
    /// </summary>
    /// <param name="pCharObjInfo"></param>
    /// <param name="pGameObject"></param>
    /// <param name="pSerializedLoadout"></param>
    public Loadout(CharObjInfo pCharObjInfo, GameObject pGameObject, string pSerializedLoadout)
    {
        m_charObjInfo = pCharObjInfo;
        m_gameObject = pGameObject;

        m_unarmed = m_charObjInfo.GetComponentInChildren<UnarmedWeaponInfo>();
        m_unarmed.Equip();

        DeserializeStringIntoLoadout(pSerializedLoadout, ref m_weapon, ref m_altWeapon, ref m_passiveAbility, ref m_specialAbility1, ref m_specialAbility2);

        m_carriedObjInfos = new List<CarriableObjInfo>();
    }

    /// <summary>
    /// Accepts a comma separated string of values indicating
    /// what the player's loadout will be and splits it, then instantiates
    /// the correct classes for the player
    ///
    /// Format: Weapon,AltWeapon,PassiveAbility,SpecialAbility1,SpecialAbility2
    /// Weapon: -1 = null, 0 = unarmed
    /// Passive Ability: -1 = null
    /// Special Ability: -1 = null
    /// </summary>
    /// <param name="pSerializedLoadout"></param>
    /// <param name="pWeapon"></param>
    /// <param name="pAltWeapon"></param>
    /// <param name="pPassiveAbility"></param>
    /// <param name="pSpecialAbility1"></param>
    /// <param name="pSpecialAbility2"></param>
    private void DeserializeStringIntoLoadout(string pSerializedLoadout, ref WeaponInfo pWeapon, ref WeaponInfo pAltWeapon, ref PassiveAbilityInfo pPassiveAbility, ref SpecialAbilityInfo pSpecialAbility1, ref SpecialAbilityInfo pSpecialAbility2)
    {
        string[] splitLoadout = pSerializedLoadout.Split(',');

        pWeapon = null;
        pAltWeapon = null;
        pPassiveAbility = null;
        pSpecialAbility1 = null;
        pSpecialAbility2 = null;

        WeaponInfo tempWeapon = null;
        WeaponInfo tempAltWeapon = null;
        PassiveAbilityInfo tempPassiveAbility = null;
        SpecialAbilityInfo tempSpecialAbility1 = null;
        SpecialAbilityInfo tempSpecialAbility2 = null;

        tempWeapon = GetWeaponFromString(splitLoadout[0]);
        if (tempWeapon != null)
        {
            pWeapon = tempWeapon;
        }

        tempAltWeapon = GetWeaponFromString(splitLoadout[1]);
        if (tempAltWeapon != null)
        {
            pAltWeapon = tempAltWeapon;
        }

        tempPassiveAbility = GetPassiveAbilityFromString(splitLoadout[2]);
        if (tempPassiveAbility != null)
        {
            pPassiveAbility = tempPassiveAbility;
        }

        tempSpecialAbility1 = GetSpecialAbilityFromString(splitLoadout[3], 1);
        if (tempSpecialAbility1 != null)
        {
            pSpecialAbility1 = tempSpecialAbility1;
        }

        tempSpecialAbility2 = GetSpecialAbilityFromString(splitLoadout[4], 2);
        if (tempSpecialAbility2 != null)
        {
            pSpecialAbility2 = tempSpecialAbility2;
        }
    }

    /// <summary>
    /// Accepts a string and returns a WeaponInfo
    /// </summary>
    /// <param name="pWeaponIndex"></param>
    /// <returns></returns>
    public WeaponInfo GetWeaponFromString(string pWeaponIndex)
    {
        switch (pWeaponIndex)
        {
            case "0":
                return m_charObjInfo.GetComponentsInChildren<UnarmedWeaponInfo>(true)[0];
            case "1":
                return m_charObjInfo.GetComponentsInChildren<ZweihanderWeaponInfo>(true)[0];
            case "2":
                return m_charObjInfo.GetComponentsInChildren<UltraGreatswordWeaponInfo>(true)[0];
            case "3":
                return m_charObjInfo.GetComponentsInChildren<DualDaggerWeaponInfo>(true)[0];
        }
        return null;
    }

    /// <summary>
    /// Accepts a string and returns a PassiveAbilityInfo
    /// </summary>
    /// <param name="pPassiveAbilityIndex"></param>
    /// <returns></returns>
    public PassiveAbilityInfo GetPassiveAbilityFromString(string pPassiveAbilityIndex)
    {
        switch (pPassiveAbilityIndex)
        {
            case "0":
                //return new PassiveAbilityInfo;
                break; //TO DO: REMOVE ME
        }
        return null;
    }

    /// <summary>
    /// Accepts a string and returns a SpecialAbilityInfo
    /// </summary>
    /// <param name="pSpecialAbilityIndex"></param>
    /// <param name="pSlot"></param>
    /// <returns></returns>
    public SpecialAbilityInfo GetSpecialAbilityFromString(string pSpecialAbilityIndex, int pSlot)
    {
        switch (pSpecialAbilityIndex)
        {
            case "0":
                return new SwitchWeaponSpecialAbilityInfo(m_charObjInfo, this, pSlot);
            case "1":
                return new FireBallSpecialAbilityInfo(m_charObjInfo, this, pSlot);
            case "2":
                return new ForceBlastSpecialAbilityInfo(m_charObjInfo, this, pSlot);
            case "3":
                return new ShurikenThrowSpecialAbilityInfo(m_charObjInfo, this, pSlot);
        }
        return null;
    }

    /// <summary>
    /// Accepts an integer and switches to the weapon in that slot
    /// 0 = Unarmed, 1 = Main Weapon, 2 = Alternate Weapon
    /// </summary>
    /// <param name="pSelectedWeapon"></param>
    public void SwitchWeapons(int pSelectedWeapon)
    {
        m_prevSelectedWeapon = m_selectedWeapon;
        m_selectedWeapon = pSelectedWeapon;

        m_charObjInfo.m_animator.SetInteger("Stance", (int)Weapon.WeaponStance);
    }

    /// <summary>
    /// This applies the unequip and equip effect of the previous
    /// and current weapon respectively.
    ///
    /// Note: Due to this delay, nothing should be done with assuming the
    /// weapon has been switched. Need to make sure this delay doesn't casuse
    /// any issues with the way it's implemented.
    /// </summary>
    public void ApplyDelayedWeaponSwitch()
    {
        PrevWeapon.Unequip();
        Weapon.Equip();
    }

    /// <summary>
    /// Enables the unarmed weapon and disables the current.
    /// The primary purpose of this method is to be used when using a
    /// special ability. Special abilities are generally animated with the
    /// assumption that your character's hands are currently empty.
    /// </summary>
    public void EnableUnarmedDisableCurrent()
    {
        Weapon.Unequip();
        m_unarmed.Equip();
    }

    /// <summary>
    /// Enables the current weapon and disables the unarmed.
    /// The primary purpose of this method is to be used when exiting a
    /// special ability. Special abilities are generally animated with the
    /// assumption that your character's hands are currently empty.
    /// </summary>
    public void EnableCurrentDisableUnarmed()
    {
        m_unarmed.Unequip();
        Weapon.Equip();
    }

    /// <summary>
    /// Picks up the passed CarriablObjInfo
    /// </summary>
    /// <param name="pCarriableObjInfo">CarriableObjInfo to pick up</param>
    public void PickUp(CarriableObjInfo pCarriableObjInfo)
    {
        pCarriableObjInfo.CarryBegin(m_charObjInfo);
        m_carriedObjInfos.Add(pCarriableObjInfo);
    }

    /// <summary>
    /// Causes the passed CarriableObjInfo to be dropped
    /// </summary>
    /// <param name="pCarriableObjInfo">CarriableObjInfo to drop</param>
    public void DropOff(CarriableObjInfo pCarriableObjInfo)
    {
        m_carriedObjInfos.Remove(pCarriableObjInfo);
        pCarriableObjInfo.CarryEnd();
    }

    /// <summary>
    /// Causes the first CarriableObjInfo of type "pType" to be dropped
    /// </summary>
    /// <param name="pType">Type of CarriableObjInfo to be dropped</param>
    public void DropOff(string pType)
    {
        CarriableObjInfo carriableObjInfoToDrop = null;
        foreach (CarriableObjInfo carriableObjInfo in m_carriedObjInfos)
        {
            if (carriableObjInfo.IsType(pType))
            {
                carriableObjInfoToDrop = carriableObjInfo;
                break;
            }
        }

        if (carriableObjInfoToDrop != null)
        {
            DropOff(carriableObjInfoToDrop);
        }
    }

    /// <summary>
    /// Causes CharObjInfo to drop all carried objects. (Usually used on death)
    /// </summary>
    public void DropAll()
    {
        foreach (CarriableObjInfo carriableObjInfo in m_carriedObjInfos)
        {
            carriableObjInfo.CarryEnd();
        }
    }

    /// <summary>
    /// Returns the first CarriableObjInfo whose type matches the passed type
    /// </summary>
    /// <param name="pType">Type to return</param>
    /// <returns></returns>
    public CarriableObjInfo GetCarriedObject(string pType)
    {
        foreach (CarriableObjInfo carriableObjInfo in m_carriedObjInfos)
        {
            if (carriableObjInfo.IsType(pType))
            {
                return carriableObjInfo;
            }
        }

        return null;
    }

    /// <summary>
    /// Checks if the CharObjInfo is carrying an object with the passed type
    /// </summary>
    /// <param name="pType">Type to check if the CharObjInfo is carrying</param>
    /// <returns></returns>
    public bool IsCarrying(string pType)
    {
        foreach (CarriableObjInfo carriableObjInfo in m_carriedObjInfos)
        {
            if (carriableObjInfo.IsType(pType))
            {
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// Cleans up anything that might need to be taken care of when
    /// a loadout's parent is going to be destroyed
    /// </summary>
    public void CleanUp()
    {
        if (m_weapon != null)
        {
            m_weapon.CleanUp();
        }

        if (m_altWeapon != null)
        {
            m_altWeapon.CleanUp();
        }

        DropAll();
    }

    #region Properties
    public WeaponInfo Weapon
    {
        get
        {
            if (m_selectedWeapon == 0)
            {
                return m_unarmed;
            }
            else if (m_selectedWeapon == 1)
            {
                return m_weapon;
            }
            return m_altWeapon;
        }
    }

    public WeaponInfo PrevWeapon
    {
        get
        {
            if (m_prevSelectedWeapon == 0)
            {
                return m_unarmed;
            }
            else if (m_prevSelectedWeapon == 1)
            {
                return m_weapon;
            }
            return m_altWeapon;
        }
    }

    public SpecialAbilityInfo SpecialAbility1
    {
        get
        {
            return m_specialAbility1;
        }
    }

    public SpecialAbilityInfo SpecialAbility2
    {
        get
        {
            return m_specialAbility2;
        }
    }

    public PassiveAbilityInfo PassiveAbility
    {
        get
        {
            return m_passiveAbility;
        }
    }

    public int SelectedWeapon
    {
        get
        {
            return m_selectedWeapon;
        }
    }
    #endregion
}