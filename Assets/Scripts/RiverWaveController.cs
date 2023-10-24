//**************************************************************************************
// File: RiverWaveController.cs
//
// Purpose: Controls the animated River Waves
//
// Written By: Salvatore Hanusiewicz
//**************************************************************************************

using UnityEngine;
using System.Collections;

public class RiverWaveController : MonoBehaviour
{
    #region Declarations
    public Animator m_animator;
    public float m_startDelay;

    protected bool m_initialized = false;
    #endregion
	
	/// <summary>
    /// This basically takes the place of a constructor
    /// </summary>
    public void Initialize()
    {
        m_animator = GetComponent<Animator>();

        m_startDelay = Random.Range(0.1f, 2);

        m_initialized = true;
	}

	/// <summary>
    /// Use this for initialization
    /// </summary>
	void Start()
	{
		if(!m_initialized)
        {
            Initialize();
        }
	}
	
	/// <summary>
    /// Update is called once per frame
    /// </summary>
	void Update()
	{
        if(m_startDelay > 0)
        {
            m_startDelay -= Time.deltaTime;

            if(m_startDelay <= 0)
            {
                m_animator.SetTrigger("Start");
            }
        }
	}
	
	#region Properties
    #endregion
}