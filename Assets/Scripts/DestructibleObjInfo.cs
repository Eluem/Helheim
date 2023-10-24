//**************************************************************************************
// File: DestructibleObjInfo.cs
//
// Purpose: This is the base class for all objects with health that can be destroyed
//
// Written By: Salvatore Hanusiewicz
//**************************************************************************************

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using AdvancedInspector;

[AdvancedInspector]
public abstract class DestructibleObjInfo : ActorObjInfo
{
    #region Declarations
    //Pointers to different classes
    #region Pointers
    [Inspect, Group("Pointers")]
    public Animator m_animator;
    [Inspect, Group("Pointers")]
    public Rigidbody2D m_rigidbody;

    protected StatusEffectManager m_statusEffectManager;
    #endregion

    #region Stats
    [Inspect(0), Group("Stats")]
    protected float m_health; //Current health
    [Inspect(0), Group("Stats")]
    protected int m_maxHealth; //Maximum health
    [Inspect(0), Group("Stats")]
    protected float m_healthRegenTimer; //Time left before health can regenerate
    [Inspect(0), Group("Stats")]
    protected float m_healthRegenDelayTime; //Delay time after losing health before it regenerates
    [Inspect(0), Group("Stats")]
    protected int m_healthRegenRate; //How much health regenerates per second
    protected List<int> m_healthThresholds; //How much health each sub bar has
    protected int m_currentHealthThreshold; //Stores the current health threshold to be observed NOTE: This needs to be maintained anytime health or health thresholds change for some reason
    #endregion


    //I decided that some objects need to have their sleep state manually managed (taking the animator into consideration)
    [Inspect(0)]
    public bool m_manualSleep = false; //Indicates whether or not the object's sleep mode should be managed manually

    private float m_idleTime; //Time spent in a state that will trigger sleep

    protected FloatingHUDController m_floatingHUDController; //Pointer to the floatingHUDController for this DestructibleObjInfo
    #endregion

    /// <summary>
    /// This basically takes the place of a constructor
    /// </summary>
    /// <param name="pPooled"></param>
    /// <param name="pObjType"></param>
    /// <param name="pStatusEffectManager"></param>
    public override void Initialize(bool pPooled, string pObjType = "DestructibleObjInfo", StatusEffectManager pStatusEffectManager = null)
    {
        base.Initialize(pPooled, pObjType);

        m_maxHealth = 10;
        HealthFloat = m_maxHealth;
        m_healthRegenDelayTime = 5f;
        m_healthRegenTimer = 0;
        m_healthRegenRate = 1;

        m_healthThresholds = new List<int>();
        m_healthThresholds.Add(4);

        m_currentHealthThreshold = GetCurrentHealthThreshold();

        if (pStatusEffectManager == null)
        {
            m_statusEffectManager = new StatusEffectManager(this);
        }
        else
        {
            m_statusEffectManager = pStatusEffectManager;
        }

        //If there is no animator, simply let the rigid body handle sleeping the object
        if(m_manualSleep)
        {
            if(m_animator == null)
            {
                Debug.LogError("You need an animator to use the manual sleep system");
            }
            
            if(m_rigidbody.sleepMode != RigidbodySleepMode2D.NeverSleep)
            {
                Debug.LogError("You need to set the rigidbody's sleep mode to 'Never Sleep' to use the manual sleep system");
            }
        }

        //TO DO: REMOVE ME IF WE CONTINUE WITH HUDLESSHEIM
        //Spawns an object to represent the "over head" floating hud for this DestructibleObjInfo
        //CreateFloatingHUD();
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
        if (pObjType == "ActorObjInfo" || pObjType == "DestructibleObjInfo" || pObjType == ObjType)
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
            Initialize(false);
        }
    }

    /// <summary>
    /// Update is called once per frame
    /// </summary>
    protected override void Update()
    {
        base.Update();

        m_statusEffectManager.Update(Time.deltaTime);
        HandleHealthRegen(Time.deltaTime);
    }

    /// <summary>
    /// Physicsy Updates
    /// </summary>
    protected virtual void FixedUpdate()
    {
        //Handle checking if this object needs to be put to sleep
        if (m_manualSleep)
        {
            if ((m_animator.GetCurrentAnimatorStateInfo(0).IsName("Idle") || m_animator.GetCurrentAnimatorStateInfo(0).IsTag("Idle")) && m_rigidbody.velocity.magnitude <= Physics2D.linearSleepTolerance && m_rigidbody.angularVelocity <= Physics2D.angularSleepTolerance)
            {
                m_idleTime += Time.fixedDeltaTime;

                if(!m_rigidbody.IsSleeping() && m_idleTime >= Physics2D.timeToSleep)
                {
                    m_rigidbody.Sleep();
                }
            }
            else
            {
                Shake();
            }
        }
    }

    /// <summary>
    /// If this object is currently sleeping, wakes it up
    /// </summary>
    public override void Shake()
    {
        if(m_destroyed)
        {
            return;
        }

        m_idleTime = 0;

        if (m_rigidbody.IsSleeping())
        {
            m_rigidbody.WakeUp();
        }
    }

    /// <summary>
    /// Spawns a floating hud for this destructible obj info
    /// </summary>
    protected virtual void CreateFloatingHUD()
    {
        
    }

    /// <summary>
    /// Handles what happens when a destructible object runs out of health
    /// </summary>
    protected abstract void OnDeath();

    /// <summary>
    /// This is used so that OnDeath can trigger an animation which
    /// can trigger OnDeathFinalAnim which will call OnDeathFinal
    /// </summary>
    protected abstract void OnDeathFinal();

    /// <summary>
    /// Handles regenerating health
    /// </summary>
    /// <param name="pDeltaTime"></param>
    protected virtual void HandleHealthRegen(float pDeltaTime)
    {
        if (m_healthRegenTimer > 0)
        {
            m_healthRegenTimer -= pDeltaTime;
        }
        else if (HealthFloat < m_currentHealthThreshold)
        {
            GainHealthToCurrentThreshold(m_healthRegenRate * pDeltaTime);
        }
    }

    /// <summary>
    /// Accepts a number for the amount of health healed and applies
    /// it to the current health while obeying current threshold constraint
    /// </summary>
    /// <param name="pHealing"></param>
    public virtual void GainHealthToCurrentThreshold(float pHealing)
    {
        HealthFloat += pHealing;

        if (HealthFloat > m_currentHealthThreshold)
        {
            HealthFloat = m_currentHealthThreshold;
        }

        m_currentHealthThreshold = GetCurrentHealthThreshold();
    }

    /// <summary>
    /// Accepts a number for the amount of health healed and applies
    /// it to the current health while obeying max health constraint
    /// </summary>
    /// <param name="pHealing"></param>
    public virtual void GainHealth(int pHealing)
    {
        HealthFloat += pHealing;

        if (HealthFloat > m_maxHealth)
        {
            HealthFloat = m_maxHealth;
        }
    }

    /// <summary>
    /// Returns the value of the current health threshold
    /// </summary>
    /// <returns></returns>
    protected int GetCurrentHealthThreshold()
    {
        foreach (int threshold in m_healthThresholds)
        {
            if (HealthFloat <= threshold)
            {
                return threshold;
            }
        }

        //return m_maxHealth; Old system always regenerated to top of current threshold
        
        return m_healthThresholds[m_healthThresholds.Count - 1]; //Prevent regeneration when above the highest threshold
    }

    #region Suffering Effects
    /// <summary>
    /// Accepts a damage packet and registers it
    /// </summary>
    /// <param name="pDamagePacket"></param>
    public virtual void SufferDamagePacket(DamagePacketStatusEffect pDamagePacket)
    {
        m_statusEffectManager.AddImmediate(pDamagePacket);
    }

    /// <summary>
    /// Accepts a number for damage dealt and applies it
    /// </summary>
    /// <param name="pDamage"></param>
    public virtual void SufferDamage(int pDamage)
    {
        HealthFloat -= pDamage;

        m_healthRegenTimer = m_healthRegenDelayTime;

        m_currentHealthThreshold = GetCurrentHealthThreshold();

        if (HealthFloat <= 0)
        {
            OnDeath();
        }
    }

    /// <summary>
    /// Accepts a number for poise damage dealt and applies it
    /// </summary>
    /// <param name="pPoiseDamage"></param>
    public virtual void SufferPoiseDamage(int pPoiseDamage)
    {
    }

    /// <summary>
    /// Accepts a vector for knockback dealt and applies it
    /// </summary>
    /// <param name="pKnockback"></param>
    public virtual void SufferKnockback(Vector2 pKnockback)
    {
        m_rigidbody.velocity = Vector3.zero; //Eliminate original velocity

        m_rigidbody.AddForce(pKnockback, ForceMode2D.Impulse); //Apply the knockback force
    }

    /// <summary>
    /// Accepts a vector for launch power and a float for hangTime
    /// </summary>
    /// <param name="pOriginObj"></param>
    /// <param name="pSourceObj"></param>
    /// <param name="pLaunchForce"></param>
    /// <param name="pLaunchHangTime"></param>
    public virtual void SufferLaunch(ObjInfo pOriginObj, ObjInfo pSourceObj, Vector2 pLaunchForce, float pLaunchHangTime)
    {
        Debug.Log(pLaunchHangTime);

        m_rigidbody.velocity = Vector3.zero; //Eliminate original velocity

        //If there is no animator, simply apply the force
        if (m_animator == null)
        {
            m_rigidbody.AddForce(pLaunchForce, ForceMode2D.Impulse);
            return;
        }

        //Remove 0s to prevent divide by 0 (and negatives since they make no sense)
        if (pLaunchHangTime <= 0)
        {
            pLaunchHangTime = 0.001f;
        }

        //Set the modifier to slow down the launch animation so that it looks like it hangs longer
        m_animator.SetFloat("LaunchAnimSpeedModifier", 1 / pLaunchHangTime);

        //Set the launch trigger
        m_animator.SetTrigger("Launch");

        //Apply launch force
        m_rigidbody.AddForce(pLaunchForce, ForceMode2D.Impulse);
    }

    /// <summary>
    /// Accepts a vector for knockDown power and a float for knockDownTime
    /// </summary>
    /// <param name="pOriginObj"></param>
    /// <param name="pSourceObj"></param>
    /// <param name="pKnockDownForce"></param>
    /// <param name="pKnockDownTime"></param>
    public virtual void SufferKnockDown(ObjInfo pOriginObj, ObjInfo pSourceObj, Vector2 pKnockDownForce, float pKnockDownTime)
    {
        m_rigidbody.velocity = Vector3.zero; //Eliminate original velocity

        //If there is no animator, simply apply the force
        if (m_animator == null)
        {
            m_rigidbody.AddForce(pKnockDownForce, ForceMode2D.Impulse);
            return;
        }

        //Remove 0s to prevent divide by 0 (and negatives since they make no sense)
        if (pKnockDownTime <= 0)
        {
            pKnockDownTime = 0.001f;
        }

        //Set the modifier to slow down the knockdown animation so that the player stays downed longer
        m_animator.SetFloat("KnockDownAnimSpeedModifier", 1 / pKnockDownTime);

        //Set the knockDown trigger
        m_animator.SetTrigger("KnockDown");

        //Apply knockdown force
        m_rigidbody.AddForce(pKnockDownForce, ForceMode2D.Impulse);
    }

    /// <summary>
    /// Accepts a number for poison damage dealt and applies it
    /// </summary>
    /// <param name="pPoison"></param>
    public virtual void SufferPoison(int pPoison)
    {
    }

    /// <summary>
    /// Triggers the destructible object to enter a pitfall effect and subsequently die
    /// </summary>
    public virtual void SufferPitFallBottomless()
    {
        m_animator.SetTrigger("PitFallBottomless");
    }

    /// <summary>
    /// Triggers the destructible object to enter a pitlava effect and subsequently die
    /// </summary>
    public virtual void SufferPitFallLava()
    {
        m_animator.SetTrigger("PitFallLava");
    }

    /// <summary>
    /// Triggers the destructible object to begin drowning
    /// </summary>
    public virtual void SufferDrowning()
    {
        m_animator.SetTrigger("SufferDrowning");
    }

    /// <summary>
    /// Triggers the destructible object to stop drowning
    /// </summary>
    public virtual void RelieveDrowning()
    {
        m_animator.SetTrigger("RelieveDrowning");
    }
    #endregion

    //This is where I expose different mechanics (i.e. lunging, dashing, ect) to the animator
    #region Mechanics
    /// <summary>
    /// Forces the first instance of the status effect with the
    /// passed StatusEffectType
    ///
    /// Note: Used by animations
    /// </summary>
    /// <param name="pStatusEffectType"></param>
    public virtual void ForceTick(StatusEffectType pStatusEffectType)
    {
        m_statusEffectManager.ForceTick(pStatusEffectType);
    }

    /// <summary>
    /// Triggers the object to be destroyed by the effect of a
    /// bottomless pitfall.
    ///
    /// Note: This exists as a work around due to the fact that the unity
    /// animation system doesn't directly support overridden functions
    /// for some reason
    /// </summary>
    public virtual void SufferPitFall_BottomlessDeathAnim()
    {
        SufferPitFall_BottomlessDeath();
    }

    /// <summary>
    /// Triggers the object to be destroyed by the effect of a
    /// lava pitfall.
    ///
    /// Note: This exists as a work around due to the fact that the unity
    /// animation system doesn't directly support overridden functions
    /// for some reason
    /// </summary>
    public virtual void SufferPitFall_LavaDeathAnim()
    {
        SufferPitFall_LavaDeath();
    }

    /// <summary>
    /// Calls the final part of OnDeath for a destructibleObjInfo
    ///
    /// Note: This exists as a work around due to the fact that the unity
    /// animation system doesn't directly support overridden functions
    /// for some reason
    /// </summary>
    public virtual void OnDeathAnim()
    {
        OnDeathFinal();
    }
    #endregion

    //These are other parts of the mechanics that are potentially hidden
    #region Hidden Mechanics
    /// <summary>
    /// Triggers the object to be destroyed by the effect of a bottomless pitfall.
    /// </summary>
    public virtual void SufferPitFall_BottomlessDeath()
    {
        OnDeathFinal();
    }

    /// <summary>
    /// Triggers the object to be destroyed by the effect of a lava pitfall.
    /// </summary>
    public virtual void SufferPitFall_LavaDeath()
    {
        OnDeathFinal();
    }
    #endregion

    #region Properties
    public int Health
    {
        get
        {
            return (int)m_health;
        }
    }

    public float HealthFloat
    {
        get
        {
            return m_health;
        }

        set
        {
            if (m_destroyed)
            {
                return;
            }

            m_health = Mathf.Clamp(value, 0, m_maxHealth); //m_stamina = Mathf.Clamp(value, 0, m_maxStamina);

            m_animator.SetFloat("Health", m_health);
        }
    }

    public int MaxHealth
    {
        get
        {
            return m_maxHealth;
        }
    }

    public Animator Animator
    {
        get
        {
            return m_animator;
        }
    }

    public override Rigidbody2D RigidBody2D
    {
        get
        {
            return m_rigidbody;
        }
    }
    #endregion
}
