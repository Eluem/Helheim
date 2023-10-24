//**************************************************************************************
// File: GhostTrail.cs
//
// Purpose: This class is designed to make it simple to generate ghost trails for
// characters
//
// Written By: Salvatore Hanusiewicz
//**************************************************************************************

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using AdvancedInspector;

[AdvancedInspector]
public class GhostTrail : MonoBehaviour
{
    #region Declarations
    #region Pointers
    [Inspect, Group("Pointers", 1)]
    public Transform m_transform;
    #endregion

    #region Stats
    [Inspect, Group("Stats", 2)]
    public bool m_trailRunning = false;
    [Inspect, Group("Stats")]
    public float m_ghostFadeSpeed = 4f;
    [Inspect, Group("Stats")]
    public float m_ghostSpawnOpacity = .8f;
    [Inspect, Group("Stats")]
    public float m_ghostSpawnDelay = .4f;
    [Inspect, Group("Stats")]
    protected float m_currGhostSpawnDelay = 0f;
    [Inspect, Group("Stats")]
    protected int m_ghostCount = 0;
    #endregion

    protected bool m_trailRunningPrev = false;

    /// <summary>
    /// Sprite renderers to observe for the current trail generation session
    /// </summary>
    protected SpriteRenderer[] m_currObservedSpriteRenderers;

    protected bool m_initialized = false;
    #endregion
	
	/// <summary>
    /// This basically takes the place of a constructor
    /// </summary>
    public void Initialize()
    {
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
		if(m_trailRunning)
        {
            if (!m_trailRunningPrev)
            {
                BeginTrail();
            }
            m_currGhostSpawnDelay -= Time.deltaTime;
            if (m_currGhostSpawnDelay <= 0)
            {
                m_currGhostSpawnDelay = m_ghostSpawnDelay; //TO DO: POSSIBLY REMOVE GHOST SPAWN DELAY FOR EFFICENCY (if i never use it...)!?
                GenerateGhost(m_currObservedSpriteRenderers);
            }
        }

        //UpdateGhosts(Time.deltaTime);

        m_trailRunningPrev = m_trailRunning;
	}

    /// <summary>
    /// Initializes a new trail
    /// </summary>
    protected void BeginTrail()
    {
        m_currObservedSpriteRenderers = GetComponentsInChildren<SpriteRenderer>(false);

    }

    /// <summary>
    /// Generates a ghost from the SpriteRenderers passed
    /// </summary>
    /// <param name="pSpriteRenderers"></param>
    protected void GenerateGhost(SpriteRenderer[] pSpriteRenderers)
    {
        GameObject ghostParent = new GameObject("Ghost_" + name);
        ghostParent.transform.position = new Vector3(m_transform.position.x, m_transform.position.y, m_transform.position.z);

#if UNITY_EDITOR
        //ghostParent.hideFlags = HideFlags.HideInHierarchy;
#endif

        List<SpriteRenderer> tempSpriteRenderers = new List<SpriteRenderer>();
        for (int i = 0; i < pSpriteRenderers.Length; i++)
        {
            GameObject tempGhostChild = new GameObject("GhostChild (" + i + ")");
            SpriteRenderer tempSpriteRenderer = tempGhostChild.AddComponent<SpriteRenderer>();
            tempSpriteRenderer.sprite = pSpriteRenderers[i].sprite;
            tempSpriteRenderer.color = new Color(1, 1, 1, m_ghostSpawnOpacity);
            tempGhostChild.transform.position = pSpriteRenderers[i].transform.position;
            tempGhostChild.transform.rotation = pSpriteRenderers[i].transform.rotation;
            tempGhostChild.transform.localScale = pSpriteRenderers[i].transform.localScale;

            tempGhostChild.transform.parent = ghostParent.transform;

            tempSpriteRenderers.Add(tempSpriteRenderer);
        }

        ghostParent.AddComponent<GhostTrailFader>().Initialize(this, tempSpriteRenderers, m_ghostFadeSpeed);

        ghostParent.transform.position += new Vector3(0, 0, (m_ghostCount + 1) / 10.0f);

        m_ghostCount++;
    }

    ///// <summary>
    ///// Updates all the currently active ghosts
    ///// </summary>
    ///// <param name="pDeltaTime"></param>
    //protected void UpdateGhosts(float pDeltaTime)
    //{
    //    foreach(GameObject ghost in m_ghosts.Keys)
    //    {
    //        float newAlpha = m_ghosts[ghost][0].color.a - m_ghostFadeSpeed * pDeltaTime;

    //        foreach (SpriteRenderer sprite in m_ghosts[ghost])
    //        {
    //            if(newAlpha <= 0)
    //            {
    //                m_ghostsToDestroy.Add(ghost);
    //                break;
    //            }
    //            else
    //            {
    //                sprite.color = new Color(1, 1, 1, newAlpha);
    //            }
    //        }
    //    }

    //    foreach(GameObject ghost in m_ghostsToDestroy)
    //    {
    //        m_ghosts.Remove(ghost);
    //        Destroy(ghost);
    //    }

    //    m_ghostsToDestroy.Clear();
    //}
	
	#region Properties
    public int GhostCount
    {
        get
        {
            return m_ghostCount;
        }
        set
        {
            m_ghostCount = value;
        }
    }
    #endregion
}