//**************************************************************************************
// File: FireBallFlamePoolChargedObjInfo.cs
//
// Purpose: Defines the object type and all it's information for the charged fireball's
// flame pool
//
// Written By: Salvatore Hanusiewicz
//**************************************************************************************

using UnityEngine;
using System.Collections;
using AdvancedInspector;

[AdvancedInspector]
public class FireBallFlamePoolChargedObjInfo : EffectFieldObjInfo
{
    #region Declarations
    #endregion

    /// <summary>
    /// This basically takes the place of a constructor
    /// </summary>
    public void Initialize()
    {
        base.Initialize(false, "FireBallFlamePoolChargedObjInfo");
    }

    /// <summary>
    /// Dictates what happens each time the EffectField ticks
    /// </summary>
    protected override void Tick()
    {
        //TO DO: Should this buffer up and then be called in fixed update or something?
        ClearRecentCollision();

        Collider2D[] tempColliders = Physics2D.OverlapCircleAll(m_transform.position, 1.4f, ACTORHAZARDHAZARD_LAYERMASK);

        for(int i = 0; i < tempColliders.Length; i++)
        {
            OnTriggerEnter2D(tempColliders[i]);
        }
    }

    #region Properties
    #endregion
}