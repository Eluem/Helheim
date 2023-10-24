//**************************************************************************************
// File: ProjectileObjInfo.cs
//
// Purpose: Defines the base class for all projectiles. Any typical projectile will
// simply customize the paramters of this class.
//
// Written By: Salvatore Hanusiewicz
//**************************************************************************************

using UnityEngine;
using System.Collections;
using AdvancedInspector;

[AdvancedInspector]
public abstract class ProjectileObjInfo : HazardObjInfo
{
    #region Declarations
    [Inspect, Group("Stats")]
    public bool m_forceAlignWithVelocity = true;

    protected Vector3 m_trajectoryDirection;
    protected float m_trajectoryPower;
    #endregion

    /// <summary>
    /// This basically takes the place of a constructor
    /// </summary>
    /// <param name="pObjType">Object Type</param>
    public override void Initialize(bool pPooled, string pObjType = "ProjectileObjInfo")
    {
        base.Initialize(pPooled, pObjType);
    }

    /// <summary>
    /// This is the code that runs each time this object is spawned
    /// </summary>
    /// <param name="pOriginObjInfo"></param>
    /// <param name="pSourceObjInfo"></param>
    /// <param name="pDirection"></param>
    public virtual void Spawn(ObjInfo pOriginObjInfo, ObjInfo pSourceObjInfo, Vector3 pDirection, float pPower)
    {
        base.Spawn(pOriginObjInfo, pSourceObjInfo);

        m_transform.up = pDirection;

        m_trajectoryDirection = pDirection;
        m_trajectoryPower = pPower;

        if(Pooled)
        {
            //Reenable main partlce system
            if (m_particleSystem != null)
            {
                //Reneable emitting more particles from the main emitter
                ParticleSystem.EmissionModule tempEmissionModule = m_particleSystem.emission;
                tempEmissionModule.enabled = true;
            }
        }
    }

    //***********************************************************************
    // Method: IsType
    //
    // Purpose: Accepts an object type and returns true if one of the types
    // that it is matches the type passed.
    // i.e. PlayerObjInfo is also a CharObjInfo and a DestructibleObjInfo
    //***********************************************************************
    public override bool IsType(string pObjType)
    {
        if (pObjType == "HazardObjInfo" || pObjType == "ProjectileObjInfo" || pObjType == ObjType)
        {
            return true;
        }

        return false;
    }

    //***********************************************************************
    // Method: FixedUpdate
    //
    // Purpose: Physicsy Updates
    //***********************************************************************
    protected virtual void FixedUpdate()
    {
        if(m_forceAlignWithVelocity)
        {
            m_transform.up = m_rigidbody.velocity;
        }

        if(m_animator != null)
        {
            m_animator.SetFloat("Speed", m_rigidbody.velocity.magnitude / m_trajectoryPower);
        }
    }

    //***********************************************************************
    // Method: OnDeath
    //
    // Purpose: Handles what happens when a hazard object is destroyed
    //***********************************************************************
    protected override void OnDeath()
    {
        base.OnDeath();

        //Clear main particle system but leave trails by default
        if (m_particleSystem != null)
        {
            m_particleSystem.Clear(false); //Clear all the particles from the main particle emitter, but leave children so that trails can fade naturally
            
            
            //Prevent emitting more particles from the main emitter
            ParticleSystem.EmissionModule tempEmissionModule = m_particleSystem.emission;
            tempEmissionModule.enabled = false;

            //m_particleSystem.enableEmission = false; //Prevent emitting more particles from the main emitter
        }
    }

    //***********************************************************************
    // Method: LaunchProjectile
    //
    // Purpose: Launches the projectile in the trajectory direction with the
    // trajectory power
    //***********************************************************************
    protected virtual void LaunchProjectile()
    {
        Vector2 force = m_trajectoryDirection * m_trajectoryPower;
        m_rigidbody.AddForce(force, ForceMode2D.Impulse);
    }

    #region Properties
    #endregion
}