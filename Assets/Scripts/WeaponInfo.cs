//**************************************************************************************
// File: WeaponInfo.cs
//
// Purpose: This is the base class for all weapon definitions. It stores the
// information about the current state and other details of a weapon. It also handles
// collisions and all other interactions with the weapon.
//
// Written By: Salvatore Hanusiewicz
//**************************************************************************************

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using AdvancedInspector;

public enum Stance
{
    Unarmed,
    BlackZweihander,
    UltraGreatsword,
    DualDagger
}

[AdvancedInspector]
public abstract class WeaponInfo : MonoBehaviour
{
    #region Declarations
    #region Pointers
    [Inspect, Group("Pointers", 1)]
    public CharObjInfo m_charObjInfo; //Stores a pointer to the character using this weapon

    [Inspect]
    public bool m_obstacleMultiHitDisabled = false;
    #endregion

    [Inspect(2)]
    public Stance m_stance; //Stores the stance that this weapon should evoke when equipped

    [Inspect(2)]
    public InteractionType m_defaultInteractionType; //Stores the default type of interaction this sword will be considered to be making when colliding with an object (this can be overridden by individual parts)

    protected InteractionType m_interactionType; //Stores the current type of interaction this sword will be considered to be making when colliding with an object (this can be overridden by individual parts)

    protected Dictionary<string, List<ObjInfo>> m_recentCollisions; //Dictionary that stores which collide detector/objInfo combinations should be ignored until it is reset 
                                                                    //(usually resets at the end of an attack animation but can be in the middle (if it's some spinning multi-hit attack or something))

    protected Dictionary<string, WeaponPartObjInfo> m_weaponPartObjInfos = new Dictionary<string, WeaponPartObjInfo>(); //Dictionary of weapon parts

    protected bool m_initialized = false; //Indicates if this object has been initialized
    #endregion

    /// <summary>
    /// This basically takes the place of a constructor
    /// </summary>
    public void Initialize()
    {
        /*
        m_charObjInfo = pCharObjInfo;
        m_gameObject = pGameObject;
        m_rigidbody = m_gameObject.GetComponent<Rigidbody2D>();
        m_transform = m_gameObject.GetComponent<Transform>();
        m_audioSource = m_gameObject.GetComponent<AudioSource>();
        m_animator = m_gameObject.GetComponent<Animator>();

        m_stance = pStance;
        */

        ResetInteractionType("");

        m_recentCollisions = new Dictionary<string, List<ObjInfo>>();
        m_recentCollisions.Add("Default", new List<ObjInfo>());

        m_initialized = true;
    }

    /// <summary>
    /// Use this for early initialization (even when disabled)
    /// </summary>
    void Awake()
    {
        if (!m_initialized)
        {
            Initialize();
        }
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
    /// Adds a weapon part info to the weapon part info dictionary
    /// </summary>
    /// <param name="pWeaponPartObjInfo"></param>
    public void RegisterWeaponPartObjInfo(WeaponPartObjInfo pWeaponPartObjInfo)
    {
        m_weaponPartObjInfos.Add(pWeaponPartObjInfo.PartName, pWeaponPartObjInfo);

        //Make sure the RecentCollisionsGroup this WeaponPartObjInfo uses exists
        if(!m_recentCollisions.ContainsKey(pWeaponPartObjInfo.RecentCollisionsGroup))
        {
            m_recentCollisions.Add(pWeaponPartObjInfo.RecentCollisionsGroup, new List<ObjInfo>());
        }
    }

    /// <summary>
    /// This function should be called when a player draws this
    /// weapon into their hands. It will create all the objects and set up
    /// all the configurations required to make the weapon work correctly
    /// 
    /// Note: Doesn't run if the gameobject is already active
    /// </summary>
    public virtual void Equip()
    {
        if (gameObject.activeSelf)
        {
            return;
        }

        //m_charObjInfo.m_animator.SetInteger("Stance", (int)m_stance);
        gameObject.SetActive(true);
    }

    /// <summary>
    /// This function should be called when a player switches from
    /// this weapon. This will hide the weapon and remove anything necessary.
    /// 
    /// Note: Doesn't run if the gameobject is not currently active
    /// </summary>
    public virtual void Unequip()
    {
        if(!gameObject.activeSelf)
        {
            return;
        }

        gameObject.SetActive(false);
    }

    /// <summary>
    /// Cleans up anything that the weapon would need to take care
    /// of if it's going to be destroyed (usually because the parent is
    /// being destroyed)
    /// </summary>
    public virtual void CleanUp()
    {
        
    }

    /// <summary>
    /// Handles the collisions from the WeaponPartObjInfos
    /// </summary>
    /// <param name="pWeaponPartObjInfo"></param>
    /// <param name="pCollisionNodeHit2D"></param>
    /// <param name="pOtherObjInfo"></param>
    /// <param name="pPartName"></param>
    /// <param name="pDamageContainer"></param>
    /// <param name="pKnockbackType"></param>
    /// <param name="pInteractionType"></param>
    public virtual void HandleCollision(WeaponPartObjInfo pWeaponPartObjInfo, CollisionNodeHit2D pCollisionNodeHit2D, ObjInfo pOtherObjInfo, string pPartName, DamageContainer pDamageContainer, KnockbackType pKnockbackType, InteractionType pInteractionType = InteractionType.None)
    {
        //Debug.Log("(" + UnityEngine.Time.frameCount + ") " + pWeaponPartObjInfo.PartName + " " + !HasRecentCollision(pWeaponPartObjInfo, pOtherObjInfo)); //TO DO: REMOVE ME (This is here to test the collision order of different weapon parts)
        if (IsValid(pWeaponPartObjInfo, pCollisionNodeHit2D.Collider, pOtherObjInfo))
        {
            //Debug.Log("(" + UnityEngine.Time.frameCount + ") " + pWeaponPartObjInfo.PartName + " " + !HasRecentCollision(pWeaponPartObjInfo, pOtherObjInfo)); //TO DO: REMOVE ME (This is here to test the collision order of different weapon parts)
            //If there is no interaction type coming from the weapon part, set it to the default for this weapon
            if (pInteractionType == InteractionType.None)
            {
                pInteractionType = m_interactionType;
            }

            if (pOtherObjInfo.IsType("DestructibleObjInfo"))
            {
                RegisterHit(pWeaponPartObjInfo, (DestructibleObjInfo)pOtherObjInfo, pDamageContainer, pKnockbackType, pCollisionNodeHit2D.Dir, pInteractionType);

                //Inform animator about collisions with this type
                m_charObjInfo.Animator.SetInteger("HitsRegisteredDestructible", m_charObjInfo.Animator.GetInteger("HitsRegisteredDestructible") + 1);

                if (pOtherObjInfo.IsType("CharObjInfo"))
                {
                    //Inform animator about collisions with this type
                    m_charObjInfo.Animator.SetInteger("HitsRegisteredCharacter", m_charObjInfo.Animator.GetInteger("HitsRegisteredCharacter") + 1);
                }
            }
            else if (pOtherObjInfo.IsType("ObstacleObjInfo"))
            {
                AudioClipManager.PlayAudioClip(pOtherObjInfo.GetInteractionAudioClip(pInteractionType), pCollisionNodeHit2D.Point);
                ParticleEffectManager.PlayParticleEffect(pOtherObjInfo.GetInteractionParticleEffect(pInteractionType), pCollisionNodeHit2D.Point, pCollisionNodeHit2D.Dir);

                //Inform animator about collisions with this type
                m_charObjInfo.Animator.SetInteger("HitsRegisteredObstacle", m_charObjInfo.Animator.GetInteger("HitsRegisteredObstacle") + 1);
            }

            //Currently attacking hazards with weapons is unsupported.. since the hazard expects an ObjInfo type, and WeaponInfos/WeaponPartObjInfos are not related to ObjInfos
            //I might make alternations to this to make it possible...
            /*
            else if(pOtherObjInfo.IsType("HazardObjInfo"))
            {
                //Inform animator about collisions with this type
                m_charObjInfo.Animator.SetInteger("HitsRegisteredHazard", m_charObjInfo.Animator.GetInteger("HitsRegisteredObstacle") + 1);
            }
            */

            //Inform the animator about some of the details of this collision
            int tempTotalBounce = pOtherObjInfo.BouncePower + pWeaponPartObjInfo.BounceModifier;
            if (m_charObjInfo.Animator.GetInteger("BouncePower") < tempTotalBounce)
            {
                m_charObjInfo.Animator.SetInteger("BouncePower", tempTotalBounce);
            }
            m_charObjInfo.Animator.SetInteger("HitsRegistered", m_charObjInfo.Animator.GetInteger("HitsRegistered") + 1);

            //Store the fact that this collision happened so that multi-collisions can be prevented
            if(!pOtherObjInfo.IsType("ObstacleObjInfo") || m_obstacleMultiHitDisabled) //Allow multi-hit for walls
            {
                AddRecentCollision(pWeaponPartObjInfo, pOtherObjInfo);
            }
        }
    }

    /// <summary>
    /// Builds a damage packet and passes it to the
    /// DestructibleObjInfo so that it can handle responding to it correctly
    /// (I'm most likely going to have characters delay responding to the
    /// damage packets to make it easy to allow players to kill each other
    /// or to come up with a good way to handle clashing (if I do that))
    /// </summary>
    /// <param name="pWeaponPartObjInfo"></param>
    /// <param name="pObjInfo"></param>
    /// <param name="pDamageContainer"></param>
    /// <param name="pKnockbackType"></param>
    /// <param name="pHitDir"></param>
    /// <param name="pInteractionType"></param>
    public virtual void RegisterHit(WeaponPartObjInfo pWeaponPartObjInfo, DestructibleObjInfo pObjInfo, DamageContainer pDamageContainer, KnockbackType pKnockbackType, Vector2 pHitDir, InteractionType pInteractionType = InteractionType.None)
    {
        pObjInfo.SufferDamagePacket(new DamagePacketStatusEffect(m_charObjInfo, pWeaponPartObjInfo, pObjInfo, pDamageContainer, 1, pWeaponPartObjInfo.HitDelay /*0.01f*/, pObjInfo.GetInteractionAudioClip(pInteractionType), pObjInfo.GetInteractionParticleEffect(pInteractionType), pKnockbackType, KnockbackType.ExternalDir, pHitDir));

        pWeaponPartObjInfo.GenerateMana(pObjInfo);
    }

    /// <summary>
    /// Checks if the collision is valid
    /// </summary>
    /// <param name="pWeaponPartObjInfo"></param>
    /// <param name="pOther"></param>
    /// <param name="pOtherObjInfo"></param>
    /// <returns></returns>
    public bool IsValid(WeaponPartObjInfo pWeaponPartObjInfo, Collider2D pOther, ObjInfo pOtherObjInfo)
    {
        if (pOther.gameObject == m_charObjInfo.gameObject)
        {
            return false;
        }

        if(HasRecentCollision(pWeaponPartObjInfo, pOtherObjInfo))
        {
            return false;
        }


        return true;
    }

    /// <summary>
    /// Adds an obj info to the dictionary of recent collisions
    /// </summary>
    /// <param name="pWeaponPartObjInfo"></param>
    /// <param name="pOtherObjInfo"></param>
    public virtual void AddRecentCollision(WeaponPartObjInfo pWeaponPartObjInfo, ObjInfo pOtherObjInfo)
    {
        m_recentCollisions[pWeaponPartObjInfo.RecentCollisionsGroup].Add(pOtherObjInfo); //Indicates that all colliders in this weapon that share a group with the passed WeaponpartObjInfo
                                                                                         //should be treated as if they already hit this ObjInfo
    }

    /// <summary>
    /// Accepts a csv string of keys to be cleared. If the string
    /// is empty, clears everything
    /// </summary>
    /// <param name="pRecentCollisionKeys"></param>
    public void ClearRecentCollision(string pRecentCollisionKeys)
    {
        if (pRecentCollisionKeys.Length > 0)
        {
            string[] keys = pRecentCollisionKeys.Split(',');

            foreach (string key in keys)
            {
                m_recentCollisions[key].Clear();
            }
        }
        else
        {
            ClearAllRecentCollisions();
        }
    }

    /// <summary>
    /// Clears all recent collisions
    /// </summary>
    public virtual void ClearAllRecentCollisions()
    {
        foreach (string key in m_recentCollisions.Keys)
        {
            m_recentCollisions[key].Clear();
        }
    }

    /// <summary>
    /// Checks if the passed ObjInfo is considered to have a recent
    /// collision
    /// </summary>
    /// <param name="pWeaponPartObjInfo"></param>
    /// <param name="pOtherObjInfo"></param>
    /// <returns></returns>
    public virtual bool HasRecentCollision(WeaponPartObjInfo pWeaponPartObjInfo, ObjInfo pOtherObjInfo)
    {
        return m_recentCollisions[pWeaponPartObjInfo.RecentCollisionsGroup].Contains(pOtherObjInfo);
    }

    /// <summary>
    /// Sets the current interaction type for the passed Part Name
    ///
    /// Note: If the part name is blank, it sets the type at the
    /// WeaponInfo level
    /// </summary>
    /// <param name="pWeaponPartName"></param>
    /// <param name="pInteractionType"></param>
    public void SetInteractionType(string pWeaponPartName, InteractionType pInteractionType)
    {
        if (pWeaponPartName == "")
        {
            m_interactionType = pInteractionType;
        }
        else
        {
            m_weaponPartObjInfos[pWeaponPartName].SetInteractionType(pInteractionType);
        }
    }

    /// <summary>
    /// Resets the current interaction type to the default for the
    /// passed Part Name
    ///
    /// Note: If the part name is blank, it resets the type at the
    /// WeaponInfo level.
    /// </summary>
    /// <param name="pWeaponPartName"></param>
    public void ResetInteractionType(string pWeaponPartName)
    {
        if (pWeaponPartName == "")
        {
            m_interactionType = m_defaultInteractionType;
        }
        else
        {
            m_weaponPartObjInfos[pWeaponPartName].ResetInteractionType();
        }
    }

    /// <summary>
    /// Resets all the current interaction types to their defaults.
    /// </summary>
    public void ResetAllInteractionTypes()
    {
        m_interactionType = m_defaultInteractionType;

        foreach (string key in m_weaponPartObjInfos.Keys)
        {
            m_weaponPartObjInfos[key].ResetInteractionType();
        }
    }

    #region Properties
    public Stance WeaponStance
    {
        get
        {
            return m_stance;
        }
    }
    #endregion
}
