//**************************************************************************************
// File: StreamLineEffectController.cs
//
// Purpose: This script is designed to control simple stream line objects by having 
// them randomize a few factors and flow through the stream, then reset and
// rerandomize when they get to the bottom of the waterfall.
//
// Written By: Salvatore Hanusiewicz
//**************************************************************************************

using UnityEngine;
using System.Collections;
using AdvancedInspector;
using Xft;

[AdvancedInspector]
public class StreamLineEffectController : MonoBehaviour
{
    #region Declarations
    #region Pointers
    [Inspect, Group("Pointers")]
    public Transform m_transform;
    [Inspect, Group("Pointers")]
    public Rigidbody2D m_rigidbody2D;
    [Inspect, Group("Pointers")]
    public XWeaponTrail m_XWeaponTrail;
    #endregion

    #region Stats
    [Inspect, Group("Stats")]
    public Rect m_bounds; //Defines the bounds where the stream line will be reset to upon crossing over
    [Inspect, Group("Stats")]
    public Rect m_spawnArea; //Defines the area where the stream line can randomly reset to
    [Inspect, Group("Stats")]
    public Vector2[] m_velocityRange = new Vector2[2]; //Defines the range of velocities to be randomized between
    [Inspect, Group("Stats")]
    public float[] m_alphaRange = new float[2]; //Defines the range of alpha values to be randomized between
    #endregion

    protected bool m_initialized = false;

    protected bool m_resetting = false;
    #endregion

    /// <summary>
    /// This basically takes the place of a constructor
    /// </summary>
    public void Initialize()
    {
        //Reset();
    }

    /// <summary>
    /// Use this for initialization
    /// </summary>
    void Start()
    {
        if (!m_initialized)
        {
            Initialize();
        }
    }

    /// <summary>
    /// Update is called once per frame
    /// </summary>
    void Update()
    {
        if (OutOfBounds())
        {
            if (!m_resetting)
            {
                //If it's out of bounds, make it invisible (this is so that if I decide to add delays to when they reset, I can do that easily
                m_XWeaponTrail.m_enabled = false;

                //Reset the stream line into it's new randomized state
                StartCoroutine(Reset());
            }
        }

        //Rotate the stream line so that the weapon trail will be generated correctly
        m_transform.up = m_rigidbody2D.velocity.normalized;
    }

    /// <summary>
    /// Check if the stream line is out of bounds
    /// </summary>
    /// <returns>Returns true if the center of the stream line is out of bounds</returns>
    protected bool OutOfBounds()
    {
        return (m_transform.position.x > m_bounds.xMax || m_transform.position.x < m_bounds.xMin || m_transform.position.y > m_bounds.yMax || m_transform.position.y < m_bounds.yMin);
    }


    /// <summary>
    /// Resets the stream line with randomized values so that it can be reused
    /// </summary>
    protected IEnumerator Reset()
    {
        m_resetting = true;

        yield return new WaitForSeconds(m_XWeaponTrail.m_autoFadeTime + Random.Range(.5f, 2));

        //Randomize the velocity
        //m_rigidbody2D.velocity = new Vector2(Random.Range(m_velocityRange[0].x, m_velocityRange[1].x), Random.Range(m_velocityRange[0].y, m_velocityRange[1].y));

        //Randomize the startion position.
        m_transform.position = new Vector3(Random.Range(m_spawnArea.xMin, m_spawnArea.xMax), Random.Range(m_spawnArea.yMin, m_spawnArea.yMax), 1);

        //Reset velocity to 0
        m_rigidbody2D.velocity = Vector2.zero;

        //Re-enable the weapon trail
        m_XWeaponTrail.m_enabled = true;

        m_resetting = false;
    }

    #region Properties
    #endregion
}