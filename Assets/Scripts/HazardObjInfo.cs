//**************************************************************************************
// File: HazardObjInfo.cs
//
// Purpose: Base class for all information about a hazard object
//
// Written By: Salvatore Hanusiewicz
//**************************************************************************************

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using AdvancedInspector;

[AdvancedInspector]
public abstract class HazardObjInfo : ObjInfo
{
    #region Declarations
    //Pointers to different classes
    #region Pointers
    [Inspect, Group("Pointers")]
    public Rigidbody2D m_rigidbody;
    [Inspect, Group("Pointers")]
    public Collider2D m_collider;
    [Inspect, Group("Pointers")]
    public ParticleSystem m_particleSystem;
    [Inspect, Group("Pointers")]
    public Animator m_animator;


    protected ObjInfo m_originObjInfo; //Stores the objInfo that was the root source
                                       //(i.e. the player shoots a fireball that explodes, they are the source and origin of the fireball, but only the origin of the explosion, not the source)

    protected ObjInfo m_sourceObjInfo; //Stores the objInfo that is the source of this hazard (i.e. the player that shot the fireball)
    #endregion

    #region Stats
    [Inspect(0), Group("HitStats", 3)]
    public float m_hitDelay = 0; //Delay that this damage packet has before it applies the damage after hitting a destructible object
    [Inspect(0), Group("HitStats")]
    public float m_damage = 0; //Damage this hazard will deal
    [Inspect, Group("HitStats")]
    public float m_poiseDamage = 0; //Poise damage this hazard will deal
    [Inspect, Group("HitStats")]
    public float m_knockback = 0; //Knockback this hazard wil apply
    [Inspect, Group("HitStats")]
    public float m_poison = 0; //Posion effect this hazard will apply
    [Inspect, Group("HitStats")]
    public float m_burn = 0; //Burn effect this hazard will apply
    [Inspect, Group("HitStats")]
    public float m_bleed = 0; //Bleed effect this hazard will apply
    [Inspect, Group("HitStats")]
    public float m_blind = 0; //Blind effect damage this hazard will apply
    [Inspect, Group("HitStats")]
    public float m_launchPower = 0; //Launch power this hazard will apply
    [Inspect, Group("HitStats")]
    public float m_launchHangTime = 1; //Launch hang time (in 10ths of a second) this hazard will apply
    [Inspect, Group("HitStats")]
    public float m_knockDownPower = 0; //KnockDown power this hazard will apply
    [Inspect, Group("HitStats")]
    public float m_knockDownTime = 1; //KnockDown time (in 10ths of a second) this hazard will apply
    [Inspect, Group("HitStats")]
    public float m_manaGeneration = 0; //This indicates how much mana this hazard will generate when hitting an object (Objects will allow you to generate a % of this mana upon hitting them)
    #endregion

    #region 
    [Inspect, Group("Stats")]
    public bool m_decays = true; //Does this hazard eventually decay and get deleted
    [Inspect, Group("Stats")]
    public float m_decayTime = 3; //Time this hazard lasts before it decays
    [Inspect, Group("Stats")]
    protected float m_aliveTime = 0; //Time this hazard has been alive

    [Inspect, Group("Stats")]
    public float m_lingerTime = 1; //Time this hazard lasts after it is destroyed (to allow trails to catch up and such..... kind of a meh cludge :/)

    [Inspect, Group("Stats")]
    public bool m_ignoreOrigin = false; //Indicates that this ignores collisions with the origin object
    [Inspect, Group("Stats")]
    public bool m_ignoreOriginEnds = false; //Indicates that this will eventually stop ignoring the origin object
    [Inspect, Group("Stats")]
    public float m_ignoreOriginEndTime = 0.1f; //Time remaining before this stops ignoring collisions with the origin object (if m_ignoreOriginEnds is true)

    [Inspect, Group("Stats")]
    public bool m_ignoreSource = false; //Indicates that this ignores collisions with the source object
    [Inspect, Group("Stats")]
    public bool m_ignoreSourceEnds = false; //Indicates that this will eventually stop ignoring the source object
    [Inspect, Group("Stats")]
    public float m_ignoreSourceEndTime = 0.1f; //Time remaining before this stops ignoring collisions with the source object (if m_ignoreSourceEnds is true)

    [Inspect, Group("Stats")]
    public bool m_isDestroyedOnObstacleHit = false; //Does this object get destroyed when it collides with an obstacle?
    [Inspect, Group("Stats")]
    public bool m_isDestroyedOnDestructibleHit = false; //Does this object get destroyed when it collides with a destructible?
    [Inspect, Group("Stats")]
    public bool m_isDestroyedOnHazardHit = false; //Does this object get destroyed when it collides with a hazard?
    #endregion

    #region Info
    [Inspect, Group("Info", 4)]
    public InteractionType m_interactionType = InteractionType.None;
    [Inspect, Group("Info")]
    public List<string> m_ignoreObjTypeCollision = new List<string>() { "WeaponPartObjInfo" }; //This is a list of object types that will be ignored during collisions for this hazard
    #endregion


    protected List<ObjInfo> m_recentCollisions; //List that stores which objInfos should be ignored until it is reset 
    #endregion

    /// <summary>
    /// This basically takes the place of a constructor
    /// </summary>
    /// <param name="pOriginObjInfo">ObjInfo for the original orgin of this hazard</param>
    /// <param name="pSourceObjInfo">ObjInfo for the most recent source of this hazard</param>
    /// <param name="pObjType">Type of hazard </param>
    public virtual void Initialize(bool pPooled, string pObjType = "HazardObjInfo")
    {
        base.Initialize(pPooled, pObjType);

        m_recentCollisions = new List<ObjInfo>();
    }

    /// <summary>
    /// This is the code that runs each time this object is spawned
    /// </summary>
    public virtual void Spawn(ObjInfo pOriginObjInfo, ObjInfo pSourceObjInfo)
    {
        base.Spawn(pSourceObjInfo.Team);

        m_originObjInfo = pOriginObjInfo;
        m_sourceObjInfo = pSourceObjInfo;

        if (Pooled)
        {
            m_collider.enabled = true; //Re-enable collision

            if (m_rigidbody != null)
            {
                m_rigidbody.simulated = true; //Reenable the physics
            }

            if (m_animator != null)
            {
                m_animator.SetBool("OnDeath", false);
            }

            m_aliveTime = 0;

            ClearRecentCollision();
        }
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
        if (pObjType == "HazardObjInfo" || pObjType == ObjType)
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
        if (!m_initialized)
        {
            Initialize(false); //If not initialzied properly, define itself as the source? Or I could define some "environmental world source" as the default source? TO DO: Look into this more
            Spawn(this, this);
        }
    }

    /// <summary>
    /// Update is called once per frame
    /// </summary>
    protected override void Update()
    {
        base.Update();

        m_aliveTime += Time.deltaTime;

        //Update if origin obj should continue being ignored
        if (m_ignoreOrigin && m_ignoreOriginEnds)
        {
            m_ignoreOriginEndTime -= Time.deltaTime;
            if (m_ignoreOriginEndTime <= 0)
            {
                m_ignoreOrigin = false;
            }
        }

        //Update if source obj should continue being ignored
        if (m_ignoreSource && m_ignoreSourceEnds)
        {
            m_ignoreSourceEndTime -= Time.deltaTime;
            if (m_ignoreSourceEndTime <= 0)
            {
                m_ignoreSource = false;
            }
        }

        //If this object decays, check if it's lived long enough to trigger OnDecay
        if (!m_destroyed && m_decays)
        {
            if (m_aliveTime >= m_decayTime)
            {
                OnDecay();
            }
        }

        //If this object is "destroyed" and lingering, process the linger time until it can be completely removed (this is mostly for visual purposes)
        if (m_destroyed)
        {
            //m_lingerTime -= Time.deltaTime;
            if (m_aliveTime >= m_decayTime + m_lingerTime) //if (m_lingerTime <= 0)
            {
                OnLingerExit();
            }
        }
    }

    /// <summary>
    /// Generates a damage container containing all the damage info in this hazards
    /// </summary>
    /// <returns></returns>
    protected DamageContainer GenerateDamageContainer()
    {
        return new DamageContainer((int)m_damage, (int)m_poiseDamage, (int)m_knockback, (int)m_poison, (int)m_burn, (int)m_bleed, (int)m_blind, (int)m_launchPower, m_launchHangTime, (int)m_knockDownPower, m_knockDownTime);
    }

    /// <summary>
    /// When the trigger collider overlaps another collider, this
    /// function will run with that collider as the parameter.
    /// </summary>
    /// <param name="pOther">Other object's collider</param>
    protected virtual void OnTriggerEnter2D(Collider2D pOther)
    {
        ObjInfo otherObjInfo = GetObjInfoFromCollider(pOther); //((ColliderInfo)pOther.GetComponent(typeof(ColliderInfo))).ObjInfo;

        if (IsValid(pOther, otherObjInfo))
        {
            HitDetected(pOther, otherObjInfo);
        }
    }

    /// <summary>
    /// Checks if this collision is valid
    /// </summary>
    /// <param name="pOther">Other object's collider</param>
    /// <param name="pOtherObjInfo">Other ObjInfo</param>
    /// <returns></returns>
    protected virtual bool IsValid(Collider2D pOther, ObjInfo pOtherObjInfo)
    {
        //Handle ignoring the origin/source
        if ((m_ignoreOrigin && pOtherObjInfo == m_originObjInfo) || (m_ignoreSource && pOtherObjInfo == m_sourceObjInfo))
        {
            return false;
        }

        //Check if there was already a recent collision with this object
        if (HasRecentCollision(pOtherObjInfo))
        {
            return false;
        }

        //Check if it's an ignored collision
        if (IsCollisionIgnored(pOtherObjInfo))
        {
            return false;
        }

        //Check if the collision is blocked
        if(IsCollisionBlocked(pOther, pOtherObjInfo))
        {
            return false;
        }

        return true;
    }

    /// <summary>
    /// Checks if the collision to the passed object is blocked by something in the way
    /// </summary>
    /// <param name="pOther">Object to check against</param>
    /// <returns></returns>
    protected virtual bool IsCollisionBlocked(Collider2D pOther, ObjInfo pOtherObjInfo)
    {
        return false;
    }

    /// <summary>
    /// Checks if the passed object is of a type that is ignored
    /// </summary>
    /// <param name="pOtherObjInfo">Object to check</param>
    /// <returns></returns>
    protected bool IsCollisionIgnored(ObjInfo pOtherObjInfo)
    {
        foreach (string objType in m_ignoreObjTypeCollision)
        {
            if (pOtherObjInfo.IsType(objType))
            {
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// Handles detecting a valid hit
    /// </summary>
    /// <param name="pOther">Other object's collider</param>
    /// <param name="pOtherObjInfo">Other object's ObjInfo</param>
    protected virtual void HitDetected(Collider2D pOther, ObjInfo pOtherObjInfo)
    {
        if (pOtherObjInfo.IsType("DestructibleObjInfo"))
        {
            RegisterHit((DestructibleObjInfo)pOtherObjInfo, GenerateDamageContainer(), m_interactionType);

            if (m_isDestroyedOnDestructibleHit)
            {
                OnDeath();
            }

            AddRecentCollision(pOtherObjInfo); //Add recent collision for this destructible so that it can't be hit again
        }
        else if (pOtherObjInfo.IsType("ObstacleObjInfo"))
        {
            AudioClipManager.PlayAudioClip(pOtherObjInfo.GetInteractionAudioClip(m_interactionType), m_transform.position);
            ParticleEffectManager.PlayParticleEffect(pOtherObjInfo.GetInteractionParticleEffect(m_interactionType), m_transform.position);

            if (m_isDestroyedOnObstacleHit)
            {
                OnDeath();
            }

            //AddRecentCollision(pOtherObjInfo); Don't add recent collision for obstacles.. this allows them to be hit multiple times.. override to change this
        }
        else if (pOtherObjInfo.IsType("HazardObjInfo"))
        {
            if (m_isDestroyedOnHazardHit)
            {
                OnDeath();
            }

            AddRecentCollision(pOtherObjInfo); //Add recent collision for other hazards to prevent hitting them multiple times

            /*
            //TO DO: Possibly refactor this to be configurable (which things can register hits multiple times/how many times/timers until reset per object/timers until reset all?
            //(possibly configure all this per base type [i.e. effect fields might act on a pulse timer so they clear every x seconds, explosions that pulse multiple times and reset their collider each pulse?..
            //Though this can be done using multiple explosions spawning out of each other perhaps?
            //Projectiles might hit something X times before ignoring? usually they would just be destroyed though after hitting some number of times...]
            */
        }

        //AddRecentCollision(pWeaponPartObjInfo, pOtherObjInfo);
    }

    /// <summary>
    /// Handles what happens when this hazard decays
    /// </summary>
    protected virtual void OnDecay()
    {
        OnDeath();
    }

    /// <summary>
    /// Handles what happens when this hazard finishes lingering
    /// </summary>
    protected virtual void OnLingerExit()
    {
        OnFinalDeath();
    }

    /// <summary>
    /// Builds a damage packet and passes it to the
    /// DestructibleObjInfo so that it can handle responding to it correctly
    /// (I'm most likely going to have characters delay responding to the
    /// damage packets to make it easy to allow players to kill each other
    /// or to come up with a good way to handle clashing (if I do that))
    /// </summary>
    /// <param name="pObjInfo">Destructible object to receive damage</param>
    /// <param name="pDamageContainer">Damage container which will define the damage data</param>
    /// <param name="pInteractionType">Type of interaction to occur when damage is registered (causing audio and particle effects)</param>
    protected virtual void RegisterHit(DestructibleObjInfo pObjInfo, DamageContainer pDamageContainer, InteractionType pInteractionType = InteractionType.None)
    {
        //Send the damage to the target object
        pObjInfo.SufferDamagePacket(new DamagePacketStatusEffect(m_sourceObjInfo, this, pObjInfo, pDamageContainer, 1, m_hitDelay, pObjInfo.GetInteractionAudioClip(pInteractionType), pObjInfo.GetInteractionParticleEffect(pInteractionType), KnockbackType.SourceRadial, KnockbackType.SourceRadial));

        //Generate mana depending on the object hit
        GenerateMana(pObjInfo);
    }

    /// <summary>
    /// Handles what happens when a hazard object is destroyed
    /// </summary>
    protected virtual void OnDeath()
    {
        if (m_lingerTime > 0)
        {
            m_destroyed = true;

            m_collider.enabled = false; //Disable the collider (so that it won't hit anything while the linger effect is taking place)

            if (m_rigidbody != null)
            {
                m_rigidbody.velocity = Vector2.zero; //Set velocity to zero
                m_rigidbody.simulated = false; //Disable physics (so that it won't keep moving while the linger effect is taking place)
            }

            if (m_animator != null)
            {
                m_animator.SetTrigger("OnDeath");
            }
        }
        else
        {
            OnFinalDeath();
        }
    }

    /// <summary>
    /// Handles what happens when a hazard object is truely destroyed
    /// </summary>
    protected virtual void OnFinalDeath()
    {
        DestroyMe();
    }

    /// <summary>
    /// Calls on death but is able to be exposed to the animation
    /// system as an animation event. This is a work around for the fact
    /// that animation events can't see overridden functions for some reason
    /// </summary>
    protected void OnDeathAnim()
    {
        OnDeath();
    }

    /// <summary>
    /// Gives the owner of this hazard mana based on the object this hazard hit
    /// </summary>
    /// <param name="pHitObject"></param>
    public void GenerateMana(ObjInfo pHitObject)
    {
        if (m_originObjInfo.IsType("CharObjInfo"))
        {
            ((CharObjInfo)m_originObjInfo).GainMana((int)(m_manaGeneration * pHitObject.ManaGenerationFactor));
        }
    }

    /// <summary>
    /// Adds an objInfo to the list of recent collisions
    /// </summary>
    /// <param name="pOtherObjInfo">Object to be added to recent collisions</param>
    public virtual void AddRecentCollision(ObjInfo pOtherObjInfo)
    {
        m_recentCollisions.Add(pOtherObjInfo);
    }

    /// <summary>
    /// Clears all the recent collisions so that they can be detected again
    /// </summary>
    public void ClearRecentCollision()
    {
        m_recentCollisions.Clear();
    }

    /// <summary>
    /// Checks if the passed ObjInfo is considered to have a recent collision
    /// </summary>
    /// <param name="pOtherObjInfo">Object to test for recent collisions</param>
    /// <returns></returns>
    public virtual bool HasRecentCollision(ObjInfo pOtherObjInfo)
    {
        return m_recentCollisions.Contains(pOtherObjInfo);
    }

    #region Properties
    public float HitDelay
    {
        get
        {
            return m_hitDelay;
        }
    }

    public Rigidbody2D Rigidbody2D
    {
        get
        {
            return m_rigidbody;
        }
    }

    public float AliveTime
    {
        get
        {
            return m_aliveTime;
        }
        set
        {
            m_aliveTime = value;
        }
    }
    #endregion
}