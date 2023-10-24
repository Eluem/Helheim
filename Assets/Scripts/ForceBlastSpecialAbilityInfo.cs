//**************************************************************************************
// File: ForceBlastSpecialAbilityInfo.cs
//
// Purpose: Defines the ForceBlast special ability.
//
// Written By: Salvatore Hanusiewicz
//**************************************************************************************

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ForceBlastSpecialAbilityInfo : SpecialAbilityInfo
{
    #region Declarations
    #endregion
	
	//***********************************************************************
	// Method: ForceBlastSpecialAbilityInfo
	//
	// Purpose: Constructor for class
	//***********************************************************************
	public ForceBlastSpecialAbilityInfo(CharObjInfo pCharObjInfo, Loadout pLoadout, int pSlot) : base(pCharObjInfo, pLoadout, pSlot, 2, "ForceBlast")
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
        return m_charObjInfo.Mana >= 25;
    }

    #region Properties
    #endregion
}