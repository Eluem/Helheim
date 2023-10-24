//**************************************************************************************
// File: GhostTrailFader.cs
//
// Purpose: This class handles fading out and then destroying a GhostTrail element.
// This is to prevent the destruction of the parent object from interfering with the
// trail fade.
//
// Written By: Salvatore Hanusiewicz
//**************************************************************************************

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GhostTrailFader : MonoBehaviour
{
    #region Declarations
    protected GhostTrail m_parent;
    protected List<SpriteRenderer> m_sprites;
    protected float m_ghostFadeSpeed;
    #endregion

    /// <summary>
    /// This basically takes the place of a constructor
    /// </summary>
    public void Initialize(GhostTrail pParent, List<SpriteRenderer> pSprites, float pGhostFadeSpeed)
    {
        m_parent = pParent;
        m_sprites = pSprites;
        m_ghostFadeSpeed = pGhostFadeSpeed;
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
        float newAlpha = m_sprites[0].color.a - m_ghostFadeSpeed * Time.deltaTime;

        if (newAlpha <= 0)
        {
            m_parent.GhostCount--;
            Destroy(gameObject);
            return;
        }

        foreach (SpriteRenderer sprite in m_sprites)
        {
            sprite.color = new Color(1, 1, 1, newAlpha);
        }
    }

    #region Properties
    #endregion
}