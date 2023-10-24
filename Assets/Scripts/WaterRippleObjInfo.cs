//**************************************************************************************
// File: WaterRippleObjInfo.cs
//
// Purpose: Basic effect class which other effects can inherit from, or it can be
// used directly for all very simple effects.
//
// Written By: Salvatore Hanusiewicz
//**************************************************************************************

using UnityEngine;
using System.Collections;
using AdvancedInspector;

[AdvancedInspector]
public class WaterRippleObjInfo : EffectObjInfo
{
    #region Declarations
    #endregion

    /// <summary>
    /// This basically takes the place of a constructor
    /// </summary>
    public override void Initialize(string pObjType = "WaterRippleObjInfo")
    {
        base.Initialize(true, pObjType);
	}

    /// <summary>
    /// This is the code that runs each time this object is spawned
    /// </summary>
    public override void Spawn()
    {
        base.Spawn();

        m_spriteRenderer.color = new Color(m_spriteRenderer.color.r, m_spriteRenderer.color.g, m_spriteRenderer.color.b, 0);
        m_transform.localScale = new Vector3(0.1f, 0.1f, 1);
    }

    /// <summary>
    /// Accepts an object type and returns true if one of the types
    /// that it is matches the type passed.
    /// i.e. PlayerObjInfo is also a CharObjInfo and a DestructibleObjInfo
    /// </summary>
    /// <param name="pObjType">Object Type string to check</param>
    /// <returns></returns>
    public override bool IsType(string pObjType)
    {
        if (pObjType == "WaterRippleObjInfo" || pObjType == "EffectObjInfo" || pObjType == ObjType)
        {
            return true;
        }

        return false;
    }

    /// <summary>
    /// Use this for initialization
    /// </summary>
    protected override void Start()
	{
        base.Start();
	}
	
	#region Properties
    #endregion
}