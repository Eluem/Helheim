//**************************************************************************************
// File: ActorObjInfo.cs
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
public abstract class ActorObjInfo : ObjInfo
{
    #region Declarations
    #region Action Stats
    [Space]
    [Inspect, Group("Action Stats", 4)]
    public bool m_allowCollisions = true;
    [Inspect, Group("Action Stats")]
    public bool m_actorColliderEnabled = true;
    [Inspect, Group("Action Stats")]
    public bool m_actorInvisibleWallColliderEnabled = true;
    [Inspect, Group("Action Stats")]
    public bool m_actorAllColliderEnabled = true;
    [Inspect, Group("Action Stats")]
    public bool m_actorGroundColliderEnabled = true;
    [Inspect, Group("Action Stats")]
    public bool m_actorFloorColliderEnabled = true;
    [Inspect, Group("Action Stats")]
    public bool m_actorPitColliderEnabled = true;
    [Inspect, Group("Action Stats")]
    public bool m_actorHazardColliderEnabled = true;

    //I changed hazards to not be actors anymore.. if any special case Actor + Hazard hybrid needs to exist, I'll create a special case or another subclass
    //[Inspect, Group("Action Stats")]
    //public bool m_hazardColliderEnabled = true;
    #endregion

    #region Collision System
    [Inspect, Group("Collision System", 6)]
    protected Collider2D m_actorCollider;
    [Inspect, Group("Collision System")]
    protected Collider2D m_actorInvisibleWallCollider;
    [Inspect, Group("Collision System")]
    protected Collider2D m_actorAllCollider;
    [Inspect, Group("Collision System")]
    protected Collider2D m_actorGroundCollider;
    [Inspect, Group("Collision System")]
    protected Collider2D m_actorFloorCollider;
    [Inspect, Group("Collision System")]
    protected Collider2D m_actorPitCollider;
    [Inspect, Group("Collision System")]
    protected Collider2D m_actorHazardCollider;
    //[Inspect, Group("Collision System")]
    //protected Collider2D m_hazardCollider;

    [Inspect, Group("Collision System")]
    protected List<TerrainObjInfo> m_floorStack; //Stores a list of floors that are currently supporting this actor
    [Inspect, Group("Collision System")]
    protected List<TerrainObjInfo> m_pitStack; //Stores a list of pits that are currently consuming this actor
    #endregion
    #endregion

    /// <summary>
    /// This basically takes the place of a constructor
    /// </summary>
    public virtual void Initialize(bool pPooled, string pObjType = "ActorObjInfo", StatusEffectManager pStatusEffectManager = null)
    {
        base.Initialize(pPooled, pObjType);

        m_floorStack = new List<TerrainObjInfo>();
        m_pitStack = new List<TerrainObjInfo>();

        InitializeColliders();
    }

    /// <summary>
    /// Initializes all the colliders so that I don't need to set up all the pointers manually each time I change out the collider type
    /// TO DO: Replace this with a system that bakes the colliders in, instead of getting them on initialization
    /// </summary>
    protected virtual void InitializeColliders()
    {
        Component[] colliders = GetComponentsInChildren(typeof(Collider2D), true);

        foreach (Component collider in colliders)
        {
            if (GetObjInfoFromCollider((Collider2D)collider) != this)
            {
                //If the Collider2D is not part of the colliders for this ObjInfo, then move on
                continue;
            }

            switch (collider.name)
            {
                case "Actor":
                    m_actorCollider = (Collider2D)collider;
                    break;
                case "ActorInvisibleWall":
                    m_actorInvisibleWallCollider = (Collider2D)collider;
                    break;
                case "ActorAll":
                    m_actorAllCollider = (Collider2D)collider;
                    break;
                case "ActorGround":
                    m_actorGroundCollider = (Collider2D)collider;
                    break;
                case "ActorFloor":
                    m_actorFloorCollider = (Collider2D)collider;
                    break;
                case "ActorPit":
                    m_actorPitCollider = (Collider2D)collider;
                    break;
                case "ActorHazard":
                    m_actorHazardCollider = (Collider2D)collider;
                    break;
                    //case "Hazard":
                    //    m_hazardCollider = (Collider2D)collider;
                    //    break;
            }
        }
    }

    /// <summary>
    /// Destroys this object (or pretends to, if it's pooled)
    /// </summary>
    public override void DestroyMe()
    {
        base.DestroyMe();

        ClearFloorStack();
        ClearPitStack();
    }

    /// <summary>
    /// Accepts an object type and returns true if one of the types
    /// that it is matches the type passed.
    /// i.e. PlayerObjInfo is also a CharObjInfo and a DestructibleObjInfo
    /// </summary>
    /// <param name="pObjType"></param>
    /// <returns></returns>
    public override bool IsType(string pObjType)
    {
        if (pObjType == "ActorObjInfo" || pObjType == ObjType)
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
    /// LateUpdate is called once per frame after Update
    /// </summary>
    protected virtual void LateUpdate() //TO DO: Why LateUpdate again????
    {
        HandleColliders();
    }

    /// <summary>
    /// Handles verifying which colldiers should be enabled and disabled
    /// </summary>
    protected virtual void HandleColliders()
    {
        bool tempActorColliderEnabled;
        bool tempActorInvisibleWallColliderEnabled;
        bool tempActorAllColliderEnabled;
        bool tempActorGroundColliderEnabled;
        bool tempActorFloorColliderEnabled;
        bool tempActorPitColliderEnabled;
        bool tempActorHazardColliderEnabled;
        //bool tempHazardColliderEnabled;

        if (m_allowCollisions)
        {
            tempActorColliderEnabled = m_actorColliderEnabled;
            tempActorInvisibleWallColliderEnabled = m_actorInvisibleWallColliderEnabled;
            tempActorAllColliderEnabled = m_actorAllColliderEnabled;
            tempActorGroundColliderEnabled = m_actorGroundColliderEnabled;
            tempActorFloorColliderEnabled = m_actorFloorColliderEnabled;
            tempActorPitColliderEnabled = m_actorPitColliderEnabled;
            tempActorHazardColliderEnabled = m_actorHazardColliderEnabled;
            //tempHazardColliderEnabled = m_hazardColliderEnabled;
        }
        else
        {
            tempActorColliderEnabled = false;
            tempActorInvisibleWallColliderEnabled = false;
            tempActorAllColliderEnabled = false;
            tempActorGroundColliderEnabled = false;
            tempActorFloorColliderEnabled = false;
            tempActorPitColliderEnabled = false;
            tempActorHazardColliderEnabled = false;
            //tempHazardColliderEnabled = false;
        }

        //Check for all changes caused by the animaton variables and terrain
        if (tempActorFloorColliderEnabled)
        {
            //If the floor collider has just become enabled, force a pseudo collision against teh floor layer
            if (m_actorFloorCollider != null && !ActorFloorColliderEnabled)
            {
                ForcePseudoFloorLayerCollsionCheck();
            }

            if (m_floorStack.Count > 0)
            {
                //If the floor is enabled and the stack is greater than 0, disable the pit collider
                ClearPitStack();
                tempActorPitColliderEnabled = false;
            }
        }
        else
        {
            ClearFloorStack();
            //tempActorFloorColliderEnabled = false;
        }

        if (tempActorPitColliderEnabled)
        {
            //if (m_actorPitCollider != null && !ActorPitColliderEnabled)
            //{
            //    if (m_pitEnableDelayed)
            //    {
            //        m_pitEnableDelayed = false;
            //    }
            //    else
            //    {
            //        tempActorPitColliderEnabled = false;
            //        m_pitEnableDelayed = true;
            //    }
            //}

            if (m_pitStack.Count > 0)
            {
                tempActorColliderEnabled = false;
                tempActorGroundColliderEnabled = false;
                tempActorFloorColliderEnabled = false;
                //tempHazardColliderEnabled = false;
            }
        }
        else
        {
            ClearPitStack();
            //tempActorPitColliderEnabled = false;
            //m_pitEnableDelayed = false;
        }


        if (m_actorCollider != null)
        {
            ActorColliderEnabled = tempActorColliderEnabled;
        }

        if (m_actorInvisibleWallCollider != null)
        {
            ActorInvisibleWallColliderEnabled = tempActorInvisibleWallColliderEnabled;
        }

        if (m_actorAllCollider != null)
        {
            ActorAllColliderEnabled = tempActorAllColliderEnabled;
        }

        if (m_actorGroundCollider != null)
        {
            ActorGroundColliderEnabled = tempActorGroundColliderEnabled;
        }

        if (m_actorFloorCollider != null)
        {
            ActorFloorColliderEnabled = tempActorFloorColliderEnabled;
        }

        if (m_actorPitCollider != null)
        {
            ActorPitColliderEnabled = tempActorPitColliderEnabled;
        }

        if (m_actorHazardCollider != null)
        {
            ActorHazardColliderEnabled = tempActorHazardColliderEnabled;
        }

        //if (m_hazardCollider != null)
        //{
        //    HazardColliderEnabled = tempHazardColliderEnabled;
        //}
    }

    /// <summary>
    /// Increments the floor stack
    /// </summary>
    public void AddFloorStack(TerrainObjInfo pTerrainObjInfo)
    {
        m_floorStack.Add(pTerrainObjInfo);
    }

    /// <summary>
    /// Decrements the floor stack
    /// </summary>
    public void RemoveFloorStack(TerrainObjInfo pTerrainObjInfo)
    {
        m_floorStack.Remove(pTerrainObjInfo);
    }

    /// <summary>
    /// Returns true or false indicating whether the passed terrain is currently in the floorStack
    /// </summary>
    /// <param name="pTerrainObjInfo"></param>
    /// <returns></returns>
    public bool FloorStackContains(TerrainObjInfo pTerrainObjInfo)
    {
        return m_floorStack.Contains(pTerrainObjInfo);
    }

    /// <summary>
    /// Returns the FloorStack count
    /// </summary>
    /// <returns></returns>
    public int FloorStackCount()
    {
        return m_floorStack.Count;
    }

    /// <summary>
    /// Clears the FloorStack and informs all floors that they were removed from this Actor's FloorStack
    /// </summary>
    protected void ClearFloorStack()
    {
        foreach (TerrainObjInfo obj in m_floorStack)
        {
            obj.RemoveActor(this);
        }

        m_floorStack.Clear();
    }

    /// <summary>
    /// Increments the pit stack
    /// </summary>
    public void AddPitStack(TerrainObjInfo pTerrainObjInfo)
    {
        m_pitStack.Add(pTerrainObjInfo);
    }

    /// <summary>
    /// Decrements the pit stack
    /// </summary>
    public void RemovePitStack(TerrainObjInfo pTerrainObjInfo)
    {
        m_pitStack.Remove(pTerrainObjInfo);
    }

    /// <summary>
    /// Returns the PitStack count
    /// </summary>
    /// <returns></returns>
    public int PitStackCount()
    {
        return m_pitStack.Count;
    }

    /// <summary>
    /// Clears the PitStack and informs all pits that they were removed from this Actor's PitStack
    /// </summary>
    protected void ClearPitStack()
    {
        foreach(TerrainObjInfo obj in m_pitStack)
        {
            obj.RemoveActor(this);
        }

        m_pitStack.Clear();
    }

    /// <summary>
    /// Returns true or false indicating whether the passed terrain is currently in the pitStack
    /// </summary>
    /// <param name="pTerrainObjInfo"></param>
    /// <returns></returns>
    public bool PitStackContains(TerrainObjInfo pTerrainObjInfo)
    {
        return m_pitStack.Contains(pTerrainObjInfo);
    }

    /// <summary>
    /// Turns all the colliders off
    /// </summary>
    public virtual void DisableAllColliders()
    {
        m_allowCollisions = false;

        if (m_actorCollider != null)
        {
            ActorColliderEnabled = false;
        }

        if (m_actorInvisibleWallCollider != null)
        {
            ActorInvisibleWallColliderEnabled = false;
        }

        if (m_actorAllCollider != null)
        {
            ActorAllColliderEnabled = false;
        }

        if (m_actorGroundCollider != null)
        {
            ActorGroundColliderEnabled = false;
        }

        if (m_actorFloorCollider != null)
        {
            ActorFloorColliderEnabled = false;
        }

        if (m_actorPitCollider != null)
        {
            ActorPitColliderEnabled = false;
        }

        //if (m_hazardCollider != null)
        //{
        //    HazardColliderEnabled = false;
        //}
    }

    /// <summary>
    /// Turns all the colliders on
    /// </summary>
    public virtual void EnableAllColliders()
    {
        m_allowCollisions = true;

        if (m_actorCollider != null)
        {
            ActorColliderEnabled = true;
        }

        if (m_actorInvisibleWallCollider != null)
        {
            ActorInvisibleWallColliderEnabled = true;
        }

        if (m_actorAllCollider != null)
        {
            ActorAllColliderEnabled = true;
        }

        if (m_actorGroundCollider != null)
        {
            ActorGroundColliderEnabled = true;
        }

        if (m_actorFloorCollider != null)
        {
            ActorFloorColliderEnabled = true;
        }

        if (m_actorPitCollider != null)
        {
            ActorPitColliderEnabled = true;
        }

        //if (m_hazardCollider != null)
        //{
        //    HazardColliderEnabled = true;
        //}
    }


    /// <summary>
    /// Causes a collision check against all objects in the floor layer and runs the OnTriggerEnter2D event for any objects found. It also runs the OnCollisionEnter2D for this object against all of those objects.
    /// It runs the Floor object's event first, then the Actor's event for each collision found.
    /// </summary>
    public virtual void ForcePseudoFloorLayerCollsionCheck()
    {
        CircleCollider2D tempCollider2D = (CircleCollider2D)m_actorFloorCollider;

        HandleForcePseudoFloorLayerCollisions(Physics2D.OverlapCircleAll((Vector2)m_actorFloorCollider.gameObject.transform.position + tempCollider2D.offset, tempCollider2D.radius, FLOOR_LAYERMASK));
    }

    /// <summary>
    /// This actually handles the looping of the collisions that were found. This makes it easier to override the collision check logic.
    /// </summary>
    private void HandleForcePseudoFloorLayerCollisions(Collider2D[] pColliders)
    {
        for (int i = 0; i < pColliders.Length; i++)
        {
            ((TerrainObjInfo)pColliders[i].GetComponent(typeof(TerrainObjInfo))).HandleOnTriggerEnter2D(m_actorFloorCollider);
            OnTriggerEnter2D(pColliders[i]);
        }
    }

    /// <summary>
    /// When the trigger collider overlaps another collider, this
    /// function will run with that collider as the parameter.
    /// </summary>
    /// <param name="pOther">Other object's collider</param>
    protected virtual void OnTriggerEnter2D(Collider2D pOther)
    {

    }

    /// <summary>
    /// This function is called when the actor enters water
    /// </summary>
    public virtual void EnterWater()
    {

    }

    /// <summary>
    /// This function is called when the actor exits water
    /// </summary>
    public virtual void ExitWater()
    {

    }

    #region Properties
    public virtual bool ActorColliderEnabled
    {
        get
        {
            return m_actorCollider.enabled;
        }
        protected set
        {
            m_actorCollider.enabled = value;
        }
    }

    public virtual bool ActorInvisibleWallColliderEnabled
    {
        get
        {
            return m_actorInvisibleWallCollider.enabled;
        }
        protected set
        {
            m_actorInvisibleWallCollider.enabled = value;
        }
    }

    public virtual bool ActorAllColliderEnabled
    {
        get
        {
            return m_actorAllCollider.enabled;
        }
        protected set
        {
            m_actorAllCollider.enabled = value;
        }
    }

    public virtual bool ActorGroundColliderEnabled
    {
        get
        {
            return m_actorGroundCollider.enabled;
        }
        protected set
        {
            m_actorGroundCollider.enabled = value;
        }
    }

    public virtual bool ActorFloorColliderEnabled
    {
        get
        {
            return m_actorFloorCollider.enabled;
        }
        protected set
        {
            m_actorFloorCollider.enabled = value;
        }
    }

    public virtual bool ActorPitColliderEnabled
    {
        get
        {
            return m_actorPitCollider.enabled;
        }
        protected set
        {
            m_actorPitCollider.enabled = value;
        }
    }

    public virtual bool ActorHazardColliderEnabled
    {
        get
        {
            return m_actorHazardCollider.enabled;
        }
        protected set
        {
            m_actorHazardCollider.enabled = value;
        }
    }

    //public virtual bool HazardColliderEnabled
    //{
    //    get
    //    {
    //        return m_hazardCollider.enabled;
    //    }
    //    protected set
    //    {
    //        m_hazardCollider.enabled = value;
    //    }
    //}
    #endregion
}
