//**************************************************************************************
// File: SwitchWeaponSpecialAbilityInfo.cs
//
// Purpose: Defines the SwitchWeapon special ability.
//
// Written By: Salvatore Hanusiewicz
//**************************************************************************************

//TO DO: Replace this with an ability kit thingy for a.. like.. hammer space backpack thing or something.. takes up all your ability slots.. still need to define that whole system for taking which slots with each piece of gear

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SwitchWeaponSpecialAbilityInfo : SpecialAbilityInfo
{
	#region Declarations
	#endregion
	
	//***********************************************************************
	// Method: SwitchWeaponSpecialAbilityInfo
	//
	// Purpose: Constructor for class
	//***********************************************************************
	public SwitchWeaponSpecialAbilityInfo(CharObjInfo pCharObjInfo, Loadout pLoadout, int pSlot) : base(pCharObjInfo, pLoadout, pSlot, 0, "SwitchWeapon") //TO DO: Rename this ability
	{
        
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
        return true;
    }

    #region Properties
    #endregion
}
