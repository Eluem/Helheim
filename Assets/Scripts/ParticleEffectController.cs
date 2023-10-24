//**************************************************************************************
// File: ParticleEffectController.cs
//
// Purpose: This will be used as an interface and controller of basic particle effects
// such as blood spray from attack hits, rock dust effects from swords hitting walls,
// and potentially for other more specific effects like those from weapons or abilities
//
// TO DO: Consider not using one giant pool for all particle effects and instead
// allow each object to manage they're own mini pools of effects that they might use?
//
// TO DO: Make this into a more generalized object pooling system for all types
// of objects? Merge with ObjectFactory?????
//
// Written By: Salvatore Hanusiewicz
//**************************************************************************************

using UnityEngine;
using System.Collections;
using AdvancedInspector;

[AdvancedInspector]
public class ParticleEffectController : MonoBehaviour
{
    #region Declarations
    #region Pointers
    [Inspect, Group("Pointers", 1)]
    private ParticleEffectManager m_particleEffectManager; //Pointer to parent particle effect manager
    [Inspect, Group("Pointers")]
    public ParticleSystem m_particleSystem;
    #endregion

    #region Info
    [Inspect, Group("Info", 2)]
    public Vector3 m_manualOffset = new Vector3();
    [Inspect, Group("Info")]
    public bool m_allowDirChange = false; //Determines whether or not this effect will respond to having it's direction changed
    #endregion

    private bool m_tracking; //Indicates whether or not this controller is tracking a target (as opposed to being parented directly to it)

    [Inspect]
    private Transform m_target; //Target that this particle effect will track

    private Vector3 m_offset; //This is the offset with which the target will be tracked

    [Inspect]
    private bool m_inUse;

    [Inspect]
    private ParticleEffectEnum m_particleEffectType;
    #endregion

    /// <summary>
    /// This basically takes the place of a constructor
    /// </summary>
    /// <param name="pParticleEffectManager"></param>
    /// <param name="pParticleEffectType"></param>
    public void Initialize(ParticleEffectManager pParticleEffectManager, ParticleEffectEnum pParticleEffectType)
    {
        m_particleEffectManager = pParticleEffectManager;
        m_particleEffectType = pParticleEffectType;

        gameObject.SetActive(false);
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
        if (m_tracking && m_target != null)
        {
            transform.position = m_target.position + m_offset; //TO DO: Consider making offset act relative to rotation as well?
        }

        m_inUse = m_particleSystem.isPlaying;

        if (!m_inUse)
        {
            Deactivate();
        }
    }

    /// <summary>
    /// Plays the particle effect in the passed direction
    /// </summary>
    /// <param name="pDirection"></param>
    private void Play(Vector3 pDirection)
    {
        gameObject.SetActive(true);

        if (pDirection != Vector3.zero && m_allowDirChange)
        {
            //gameObject.transform.rotation = Quaternion.LookRotation(pDirection, Vector3.up); TO DO: Should I allow a way to set the forward rotation as well?

            //gameObject.transform.rotation = new Vector2(pDirection.x, pDirection.y).GetRotation(); //TO DO: Use a 2d vector to begin with
            gameObject.transform.forward = pDirection;

            ////TO DO: REMOVE ME
            //if (m_particleEffectType == ParticleEffectEnum.Blood)
            //{
            //    Debug.Log("particle effect played with a direction " + pDirection);
            //}
        }
        else
        {
            if (m_particleEffectType == ParticleEffectEnum.Blood)
            {
                Debug.Log("particle effect played without a direction " + pDirection + " (Allow Dir Change = " + m_allowDirChange + ")");
            }
        }

        m_particleSystem.Play();
    }

    /// <summary>
    /// Moves the effect to the passed position and plays it in the
    /// passed direction
    /// </summary>
    /// <param name="pPosition"></param>
    /// <param name="pDirection"></param>
    public void Play(Vector3 pPosition, Vector3 pDirection)
    {
        gameObject.transform.position = pPosition;
        Play(pDirection);
    }

    /// <summary>
    /// Attaches the particle effect to the passed target, facing
    /// in the passed direction, and plays it
    /// </summary>
    /// <param name="pTarget"></param>
    /// <param name="pDirection"></param>
    /// <param name="pOffset"></param>
    /// <param name="pSetTargetAsParent"></param>
    public void Play(Transform pTarget, Vector3 pDirection, Vector3 pOffset, bool pSetTargetAsParent = false)
    {
        m_tracking = true;
        m_target = pTarget;
        m_offset = pOffset + m_manualOffset;
        transform.position = m_target.position + m_offset;

        if (pSetTargetAsParent)
        {
            transform.parent = m_target;
        }
        Play(pDirection);
    }

    /// <summary>
    /// Deactivates this ParticleEffectController and releases it
    /// back to the pool of available ParticleEffectController
    /// </summary>
    private void Deactivate()
    {
        m_tracking = false;
        m_target = null;
        transform.parent = m_particleEffectManager.transform;
        transform.position = Vector3.zero;
        transform.rotation = Quaternion.Euler(Vector3.zero);
        transform.localScale = Vector3.one;
        m_particleEffectManager.FreeParticleEffectController(this);
        gameObject.SetActive(false);
    }

    #region Properties
    public ParticleEffectEnum ParticleEffectType
    {
        get
        {
            return m_particleEffectType;
        }
    }
    #endregion
}
