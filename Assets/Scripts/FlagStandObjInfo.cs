//**************************************************************************************
// File: FlagStandObjInfo.cs
//
// Purpose: Defines all the logic for a flag stand. Flag stands hold your flag and
// act as the scoring location for capturing the enemy flag.
//
// Written By: Salvatore Hanusiewicz
//**************************************************************************************

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using AdvancedInspector;

[AdvancedInspector]
public class FlagStandObjInfo : ActorObjInfo
{
    #region Declarations
    #region Pointers
    [Inspect, Group("Pointers")]
    public FlagObjInfo m_flagObjInfo;
    [Inspect, Group("Pointers")]
    public Animator m_animator;
    #endregion

    #region Stats
    [Inspect, Group("Stats")]
    protected bool m_flagInStand = true;
    [Inspect, Group("Stats")]
    public float m_timeToCap = 3; //How long it takes to cap the flag
    #endregion


    protected List<FlagCapper> m_flagCappers;
    protected List<FlagCapper> m_flagCappersToRemove;
    #endregion

    /// <summary>
    /// This basically takes the place of a constructor
    /// </summary>
    public void Initialize()
    {
        base.Initialize(false, "FlagStandObjInfo");

        m_flagCappers = new List<FlagCapper>();
        m_flagCappersToRemove = new List<FlagCapper>();
    }

    /// <summary>
    /// Use this for initialization
    /// </summary>
    protected override void Start()
    {
        if (!m_initialized)
        {
            Initialize();
        }
    }

    /// <summary>
    /// Update is called once per frame
    /// </summary>
    protected override void Update()
    {
        base.Update();

        foreach (FlagCapper flagCapper in m_flagCappers)
        {
            flagCapper.Update(Time.deltaTime);
        }

        foreach (FlagCapper flagCapper in m_flagCappersToRemove)
        {
            m_flagCappers.Remove(flagCapper);
        }
        m_flagCappersToRemove.Clear();

        if(m_animator.GetBool("Capping") && m_flagCappers.Count == 0)
        {
            m_animator.SetBool("Capping", false);
        }
    }



    /// <summary>
    /// When the trigger collider overlaps another collider, this function will run with that collider as the parameter.
    /// </summary>
    /// <param name="pOther"></param>
    protected override void OnTriggerEnter2D(Collider2D pOther)
    {
        ObjInfo otherObjInfo = GetObjInfoFromCollider(pOther);

        if (otherObjInfo.IsType("CharObjInfo"))
        {
            if (otherObjInfo.Team == m_team && ((CharObjInfo)otherObjInfo).Loadout.IsCarrying("FlagObjInfo") && m_flagInStand)
            {
                AddFlagCapper((CharObjInfo)otherObjInfo);
            }
        }
    }

    /// <summary>
    /// When the trigger collider stops overlapping another collider, this
    /// function will run with that collider as the parameter.
    /// </summary>
    /// <param name="pOther">Other object's collider</param>
    protected virtual void OnTriggerExit2D(Collider2D pOther)
    {
        ObjInfo otherObjInfo = GetObjInfoFromCollider(pOther);

        foreach (FlagCapper flagCapper in m_flagCappers)
        {
            if (flagCapper.Capper == otherObjInfo)
            {
                RemoveFlagCapper(flagCapper);
            }
        }
    }

    /// <summary>
    /// Adds a flag capper to the flag cappers list
    /// </summary>
    /// <param name="pCharObjInfo">CharObjInfo to track as a capper</param>
    protected void AddFlagCapper(CharObjInfo pCharObjInfo)
    {
        m_animator.SetBool("Capping", true);
        m_animator.SetFloat("FillSpeedModifier", 1 / m_timeToCap);
        m_flagCappers.Add(new FlagCapper(this, (CharObjInfo)pCharObjInfo));
    }

    /// <summary>
    /// Queues up a flag capper to be removed
    /// </summary>
    /// <param name="pFlagCapper"></param>
    protected void RemoveFlagCapper(FlagCapper pFlagCapper)
    {
        m_flagCappersToRemove.Add(pFlagCapper);
    }

    /// <summary>
    /// Register a flag capture
    /// </summary>
    /// <param name="pFlagCapper">Flag capper who captured the flag</param>
    protected void CaptureFlag(FlagCapper pFlagCapper, FlagObjInfo pFlagObjInfo)
    {
        GameModeManager.IncrementTeamScore(m_team);
        RemoveFlagCapper(pFlagCapper);
        pFlagCapper.Capper.Loadout.DropOff(pFlagObjInfo);
        pFlagObjInfo.Reset();
    }

    #region Properties
    public bool FlagInStand
    {
        get
        {
            return m_flagInStand;
        }
        set
        {
            m_flagInStand = value;
        }
    }

    public float TimeToCap
    {
        get
        {
            return m_timeToCap;
        }
    }
    #endregion

    protected class FlagCapper
    {
        #region Declarations
        CharObjInfo m_capper;
        float m_capperTime;
        FlagStandObjInfo m_ownerFlagStand;
        #endregion

        /// <summary>
        /// Constructor for class
        /// </summary>
        /// <param name="pCapper"></param>
        public FlagCapper(FlagStandObjInfo pOwnerFlagStand, CharObjInfo pCapper)
        {
            m_ownerFlagStand = pOwnerFlagStand;
            m_capper = pCapper;
            m_capperTime = 0;
        }

        /// <summary>
        /// Update is called once per frame
        /// </summary>
        /// <param name="pDeltaTime"></param>
        public void Update(float pDeltaTime)
        {
            FlagObjInfo tempFlagObjInfo = (FlagObjInfo)m_capper.Loadout.GetCarriedObject("FlagObjInfo");

            if (!m_capper.ActorColliderEnabled || tempFlagObjInfo == null || !m_ownerFlagStand.FlagInStand)
            {
                m_ownerFlagStand.RemoveFlagCapper(this);
            }
            else
            {
                m_capperTime += pDeltaTime;

                if(m_capperTime >= m_ownerFlagStand.TimeToCap)
                {
                    m_ownerFlagStand.CaptureFlag(this, tempFlagObjInfo);
                }
            }
        }

        #region Properties
        public CharObjInfo Capper
        {
            get
            {
                return m_capper;
            }
        }
        #endregion
    }
}