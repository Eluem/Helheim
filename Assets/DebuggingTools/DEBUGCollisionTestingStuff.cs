//**************************************************************************************
// File: DEBUGCollisionTestingStuff.cs
//
// Purpose: 
//
// Written By: Salvatore Hanusiewicz
//**************************************************************************************

using UnityEngine;
using System.Collections;
using AdvancedInspector;

[AdvancedInspector]
public class DEBUGCollisionTestingStuff : MonoBehaviour
{
    #region Declarations
    [Inspect]
    public string m_layer = "Floor";
    [Inspect]
    public LayerMask m_layermask;
    [Inspect]
    public float m_radius = .4f;
    #endregion
	
	/// <summary>
    /// This basically takes the place of a constructor
    /// </summary>
    public void Initialize()
    {
		
	}

	/// <summary>
    /// Use this for initialization
    /// </summary>
	void Start()
	{
		
	}
	
	/// <summary>
    /// Update is called once per frame
    /// </summary>
	void Update()
	{
		
	}

    [Inspect]
    public void Inspector_OverlapCheck()
    {
        Debug.Log(Physics2D.OverlapCircleAll((Vector2)transform.position, m_radius, LayerMask.NameToLayer(m_layer)).Length);
    }

    [Inspect]
    public void Inspector_OverlapCheckLayermask()
    {
        Debug.Log(Physics2D.OverlapCircleAll((Vector2)transform.position, m_radius, m_layermask).Length);
    }

    [Inspect]
    public void Inspector_OverlapCheckNoMask()
    {
        Debug.Log(Physics2D.OverlapCircleAll((Vector2)transform.position, m_radius).Length);//, LayerMask.NameToLayer(m_layer)).Length);
    }

    #region Properties
    #endregion
}