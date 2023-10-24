//**************************************************************************************
// File: EffectObjInfo.cs
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
public class EffectObjInfo : ObjInfo
{
    #region Declarations
    #region Pointers
    [Inspect, Group("Pointers")]
    public SpriteRenderer m_spriteRenderer;
    #endregion

    #region Stats
    [Inspect, Group("Stats", 2)]
    public float m_lifeTime = -1; //Time the effect exists for, default is -1 (forever)

    [Inspect, Group("Stats")]
    public float m_fadeStartTime = -1; //Time until the effect begins to fade, default is -1 (never)

    [Inspect, Group("Stats")]
    protected float m_timeAlive = 0; //Time the effect has existed for
    #endregion
    #endregion

    /// <summary>
    /// This basically takes the place of a constructor
    /// </summary>
    public virtual void Initialize(string pObjType = "EffectObjInfo")
    {
        base.Initialize(true, pObjType);
	}

    /// <summary>
    /// This is the code that runs each time this object is spawned
    /// </summary>
    public virtual void Spawn()
    {
        base.Spawn(0);

        m_timeAlive = 0;
        m_spriteRenderer.color = new Color(m_spriteRenderer.color.r, m_spriteRenderer.color.g, m_spriteRenderer.color.b, 1);
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
        if (pObjType == "EffectObjInfo" || pObjType == ObjType)
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
	
	/// <summary>
    /// Update is called once per frame
    /// </summary>
	protected override void Update()
	{
        if(m_timeAlive < m_lifeTime)
        {
            m_timeAlive += Time.deltaTime;

            if(m_fadeStartTime != -1)
            {
                m_spriteRenderer.color = new Color(m_spriteRenderer.color.r, m_spriteRenderer.color.g, m_spriteRenderer.color.b, (m_lifeTime - m_timeAlive) / (m_lifeTime - m_fadeStartTime));
            }

            if(m_timeAlive >= m_lifeTime)
            {
                DestroyMe();
            }
        }
	}
	
	#region Properties
    public SpriteRenderer SpriteRenderer
    {
        get
        {
            return m_spriteRenderer;
        }
        set
        {
            m_spriteRenderer = value;
        }
    }
    #endregion
}