//**************************************************************************************
// File: PoisonStatusEffect.cs
//
// Purpose: This defines how the poison status effect works
//
// Written By: Salvatore Hanusiewicz
//**************************************************************************************

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PoisonStatusEffect : StatusEffect
{
	#region Declarations
	#endregion
	
	//***********************************************************************
	// Method: PoisonStatusEffect
	//
	// Purpose: Constructor for class
	//***********************************************************************
	public PoisonStatusEffect(ObjInfo pOriginObj, ObjInfo pSourceObj, DestructibleObjInfo pTargetObj) : base(pOriginObj, pSourceObj, pTargetObj, StatusEffectType.Poison, new DamageContainer(50), 10, 2, false, AudioClipEnum.None, ParticleEffectEnum.None)
    {
        
	}
	
	#region Properties
    #endregion
}