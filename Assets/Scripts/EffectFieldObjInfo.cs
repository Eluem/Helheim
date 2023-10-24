//**************************************************************************************
// File: EffectFieldObjInfo.cs
//
// Purpose: Defines the base class for all effectFields. Any typical effectField will
// simply customize the paramters of this class.
//
// Written By: Salvatore Hanusiewicz
//**************************************************************************************

using UnityEngine;
using System.Collections;
using AdvancedInspector;

public abstract class EffectFieldObjInfo : HazardObjInfo
{
    #region Declarations
    [Inspect, Group("Stats")]
    public float m_tickTime = 0;
    [Inspect, Group("Stats")]
    protected float m_currTickTime = 0;
    #endregion

    /// <summary>
    /// This basically takes the place of a constructor
    /// </summary>
    /// <param name="pPooled"></param>
    /// <param name="pObjType"></param>
    public override void Initialize(bool pPooled, string pObjType = "EffectFieldObjInfo")
    {
        base.Initialize(pPooled, pObjType);
    }

    /// <summary>
    /// This is the code that runs each time this object is spawned
    /// </summary>
    /// <param name="pOriginObjInfo"></param>
    /// <param name="pSourceObjInfo"></param>
    public override void Spawn(ObjInfo pOriginObjInfo, ObjInfo pSourceObjInfo)
    {
        base.Spawn(pOriginObjInfo, pSourceObjInfo);

        if (Pooled)
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


    /// <summary>
    /// Accepts an object type and returns true if one of the types
    /// that it is matches the type passed.
    /// i.e. PlayerObjInfo is also a CharObjInfo and a DestructibleObjInfo
    /// </summary>
    /// <param name="pObjType">Object Type string to check</param>
    /// <returns></returns>
    public override bool IsType(string pObjType)
    {
        if (pObjType == "HazardObjInfo" || pObjType == "EffectFieldObjInfo" || pObjType == ObjType)
        {
            return true;
        }

        return false;
    }

    /// <summary>
    /// Update is called once per frame
    /// </summary>
    protected override void Update()
    {
        base.Update();

        UpdateTick(Time.deltaTime); //TO DO: should this be called in FixedUpdate instead? (or should it be buffered up and then handled in fixed update if it's collision based?
    }

    /// <summary>
    /// Handles what happens when a hazard object is destroyed
    /// </summary>
    protected override void OnDeath()
    {
        base.OnDeath();

        //Clear main particle system but leave trails by default
        if (m_particleSystem != null)
        {
            //Prevent emitting more particles from the main emitter
            ParticleSystem.EmissionModule tempEmissionModule = m_particleSystem.emission;
            tempEmissionModule.enabled = false;

            //m_particleSystem.enableEmission = false; //Prevent emitting more particles from the main emitter
        }
    }

    /// <summary>
    /// Ticks down the
    /// </summary>
    protected virtual void UpdateTick(float pDeltaTime)
    {
        if(m_tickTime > 0)
        {
            m_currTickTime += pDeltaTime;

            if(m_currTickTime >= m_tickTime)
            {
                m_currTickTime = 0;
                Tick();
            }
        }
    }

    /// <summary>
    /// Dictates what happens each time the EffectField ticks
    /// </summary>
    protected abstract void Tick();

    #region Properties
    #endregion
}