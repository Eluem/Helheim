//**************************************************************************************
// File: CloudEffectController.cs
//
// Purpose: This script is designed to control simple cloud objects by having them
// randomize a few factors and scroll across the screen, then reset and
// rerandomize.
//
// Written By: Salvatore Hanusiewicz
//**************************************************************************************

using UnityEngine;
using System.Collections;
using AdvancedInspector;

[AdvancedInspector]
public class CloudEffectController : MonoBehaviour
{
    #region Declarations
    #region Pointers
    [Inspect, Group("Pointers")]
    public Transform m_transform;
    [Inspect, Group("Pointers")]
    public Rigidbody2D m_rigidbody2D;
    [Inspect, Group("Pointers")]
    public SpriteRenderer m_spriteRenderer;
    #endregion

    #region Stats
    [Inspect, Group("Stats")]
    public Rect m_bounds; //Defines the bounds where the cloud will be reset to upon crossing over
    [Inspect, Group("Stats")]
    public Rect m_spawnArea; //Defines the area where the cloud can randomly reset to
    [Inspect, Group("Stats")]
    public Vector2[] m_velocityRange = new Vector2[2]; //Defines the range of velocities to be randomized between
    [Inspect, Group("Stats")]
    public float[] m_alphaRange = new float[2]; //Defines the range of alpha values to be randomized between
    [Inspect, Group("Stats")]
    public Sprite[] m_cloudSprites; //Array of all possible cloud sprites to be randomized between
    [Inspect, Group("Stats")]
    public bool m_randomizeInitialSpawn = true;
    #endregion

    protected bool m_initialized = false;
    #endregion

    /// <summary>
    /// This basically takes the place of a constructor
    /// </summary>
    public void Initialize()
    {
        //Back up the spawn area, and use the bounds as the initial spawn area
        Rect tempSpawnArea = m_spawnArea;

        if (m_randomizeInitialSpawn)
        {
            m_spawnArea = m_bounds;
        }

        Reset();
        m_spawnArea = tempSpawnArea;

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
		if(OutOfBounds())
        {
            //If it's out of bounds, make it invisible (this is so that if I decide to add delays to when they reset, I can do that easily
            m_spriteRenderer.enabled = false;

            //Reset the cloud into it's new randomized state
            Reset();
        }
	}

    /// <summary>
    /// Check if the cloud is out of bounds
    /// </summary>
    /// <returns>Returns true if the center of the cloud is out of bounds</returns>
    protected bool OutOfBounds()
    {
        return (m_transform.position.x > m_bounds.xMax || m_transform.position.x < m_bounds.xMin || m_transform.position.y > m_bounds.yMax || m_transform.position.y < m_bounds.yMin);
    }
	

    /// <summary>
    /// Resets the cloud with randomized values so that it can be reused
    /// </summary>
    protected void Reset()
    {
        //Randomize the velocity
        m_rigidbody2D.velocity = new Vector2(Random.Range(m_velocityRange[0].x, m_velocityRange[1].x), Random.Range(m_velocityRange[0].y, m_velocityRange[1].y));

        //Randomize the alpha for the color
        m_spriteRenderer.color = new Color(m_spriteRenderer.color.r, m_spriteRenderer.color.b, m_spriteRenderer.color.g, Random.Range(m_alphaRange[0], m_alphaRange[1]));

        //Randomize the sprite
        m_spriteRenderer.sprite = m_cloudSprites[Random.Range(0, 5)];

        //Randomize the startion position. Z is hardcoded to 0-5 simply to allow randomized layering
        m_transform.position = new Vector3(Random.Range(m_spawnArea.xMin, m_spawnArea.xMax), Random.Range(m_spawnArea.yMin, m_spawnArea.yMax), Random.Range(0, 5f));

        //Re-enable the renderer so that the cloud will be visible
        m_spriteRenderer.enabled = true;
    }
	#region Properties
    #endregion
}