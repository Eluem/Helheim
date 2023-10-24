//**************************************************************************************
// File: ShurikenThrowSpecialAbilityInfo.cs
//
// Purpose: Defines the ShurikenThrow special ability.
//
// Written By: Salvatore Hanusiewicz
//**************************************************************************************

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ShurikenThrowSpecialAbilityInfo : SpecialAbilityInfo
{
    #region Declarations
    #endregion

    //***********************************************************************
    // Method: ShurikenThrowSpecialAbilityInfo
    //
    // Purpose: Constructor for class
    //***********************************************************************
    public ShurikenThrowSpecialAbilityInfo(CharObjInfo pCharObjInfo, Loadout pLoadout, int pSlot) : base(pCharObjInfo, pLoadout, pSlot, 3, "ShurikenThrow", DisplayMode.Ammo)
    {
        m_ammo = 3;
        m_maxAmmo = 3;
    }

    //***********************************************************************
    // Method: IsAllowed
    //
    // Purpose: Defines whether or not the ability is currently allowed to
    // be activated. This will be determined by conditions such as having
    // enough mana, stamina or other resource... or other conditions
    //***********************************************************************
    public override bool IsAllowed()
    {
        return m_charObjInfo.Mana >= 25 || m_ammo > 0;
    }

    #region Properties
    #endregion
}