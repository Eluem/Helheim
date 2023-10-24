//**************************************************************************************
// File: TerrainObjInfo.cs
//
// Purpose: Defines the base class for all terrain that an object can interact with
//
// Written By: Salvatore Hanusiewicz
//**************************************************************************************

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using AdvancedInspector;

public enum TerrainType
{
    None = 0,
    Floor = 1,
    Pit = 2
}

[AdvancedInspector]
public abstract class TerrainObjInfo : ObjInfo
{
    #region Declarations
    [Inspect, Group("Stats")]
    public TerrainType m_terrainType;

    protected List<ActorObjInfo> m_containedActorObjInfos; //List of ActorObjInfos that are currently contained within this TerrainObjInfo
    #endregion

    /// <summary>
    /// This basically takes the place of a constructor
    /// </summary>
    /// <param name="pObjType">Type of object</param>
    public virtual void Initialize(string pObjType = "TerrainObjInfo")
    {
        base.Initialize(false, pObjType);

        m_containedActorObjInfos = new List<ActorObjInfo>();
    }

    /// <summary>
    /// Destroys this object (or pretends to, if it's pooled)
    /// </summary>
    public override void DestroyMe()
    {
        base.DestroyMe();

        ClearActors();
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
        if (pObjType == "TerrainObjInfo" || pObjType == ObjType)
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
            Initialize();
        }
    }

    /// <summary>
    /// Update is called once per frame
    /// </summary>
    protected override void Update()
    {
        base.Update();
    }

    /// <summary>
    /// When the trigger collider overlaps another collider, this
    /// function will run with that collider as the parameter.
    /// </summary>
    /// <param name="pOther">Other object's collider</param>
    protected virtual void OnTriggerEnter2D(Collider2D pOther)
    {
        HandleOnTriggerEnter2D(pOther);
    }

    /// <summary>
    /// Exposes the event so that it can be forced to fire by the ActorObjInfo
    /// </summary>
    /// <param name="pOther"></param>
    public virtual void HandleOnTriggerEnter2D(Collider2D pOther)
    {
        ObjInfo otherObjInfo = GetObjInfoFromCollider(pOther);

        if (otherObjInfo.IsType("ActorObjInfo"))
        {
            ActorObjInfo otherActorObjInfo = ((ActorObjInfo)otherObjInfo);
            AddActor(otherActorObjInfo);

            switch (m_terrainType)
            {
                case TerrainType.Floor:
                    if (!otherActorObjInfo.FloorStackContains(this))
                    {
                        otherActorObjInfo.AddFloorStack(this);
                        OnTriggerEnter2D_DupePruned(pOther);
                    }
                    break;
                case TerrainType.Pit:
                    //This code was intended to allow teleporting onto a floor that was over a pit and detecting the floor first... however.. since the effectors and possibly other things can trigger before this
                    //event, it's not really meaningful.... for example, with the bridge over the water, the floor was detected first, and then the first part of the if statement would be detected which would prevent
                    //the player from falling into the water, but the player would still slide a little from the push of the water........ There's other ways to work around this that would make this work but I think I'll just
                    //see if I can prevent ever teleporting anything without turning off the colliders first and then turning them on in the new location, either simultaneously or floor first... similar to how being launched works
                    //if(otherActorObjInfo.FloorStackCount() > 0)
                    //{
                    //    Debug.Log("0!!!!");
                    //    return;
                    //}
                    //else
                    //{
                    //    Debug.Log("A!!!!");
                    //    otherActorObjInfo.ForcePseudoFloorLayerCollsionCheck();
                    //    Debug.Log("B!!!!");
                    //    if (otherActorObjInfo.FloorStackCount() > 0)
                    //    {
                    //        Debug.Log("hello!!!!");
                    //        return;
                    //    }
                    //}

                    if (!otherActorObjInfo.PitStackContains(this))
                    {
                        otherActorObjInfo.AddPitStack(this);
                        OnTriggerEnter2D_DupePruned(pOther);
                    }
                    break;
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

        if (otherObjInfo.IsType("ActorObjInfo"))
        {
            ActorObjInfo otherActorObjInfo = ((ActorObjInfo)otherObjInfo);
            RemoveActor(otherActorObjInfo);

            switch (m_terrainType)
            {
                case TerrainType.Floor:
                    if (otherActorObjInfo.FloorStackContains(this))
                    {
                        otherActorObjInfo.RemoveFloorStack(this);
                        OnTriggerExit2D_DupePruned(pOther);
                    }
                    break;
                case TerrainType.Pit:
                    if (otherActorObjInfo.PitStackContains(this))
                    {
                        otherActorObjInfo.RemovePitStack(this);
                        OnTriggerExit2D_DupePruned(pOther);
                    }
                    break;
            }
        }
    }

    /// <summary>
    /// The TerrainObjInfo base class handles checking if an ActorObjInfo has this TerrainObjInfo in one of its lists. If it does, then this isn't called
    /// </summary>
    /// <param name="pOther">Other object's collider</param>
    protected virtual void OnTriggerEnter2D_DupePruned(Collider2D pOther)
    {
    }

    /// <summary>
    /// The TerrainObjInfo base class handles checking if an ActorObjInfo has this TerrainObjInfo in one of its lists. If it doesn't, then this isn't called
    /// </summary>
    /// <param name="pOther">Other object's collider</param>
    protected virtual void OnTriggerExit2D_DupePruned(Collider2D pOther)
    {
    }

    /// <summary>
    /// Called when this behavior becomes disabled
    /// </summary>
    protected void OnDisable()
    {
        ClearActors();
    }

    /// <summary>
    /// Adds an actor to the list of contained actors (if it's not already in the list)
    /// </summary>
    /// <param name="pActorObjInfo"></param>
    protected void AddActor(ActorObjInfo pActorObjInfo)
    {
        if (!m_containedActorObjInfos.Contains(pActorObjInfo))
        {
            m_containedActorObjInfos.Add(pActorObjInfo);
        }
    }

    /// <summary>
    /// Removes an ActorObjInfo from the list of contained ActorObjInfos
    /// </summary>
    /// <param name="pActorObjInfo"></param>
    public void RemoveActor(ActorObjInfo pActorObjInfo)
    {
        m_containedActorObjInfos.Remove(pActorObjInfo);
    }

    /// <summary>
    /// Clears out contained actors and informs them that they have stopped being contained
    /// </summary>
    protected void ClearActors()
    {
        switch (m_terrainType)
        {
            case TerrainType.Floor:
                foreach (ActorObjInfo obj in m_containedActorObjInfos)
                {
                    obj.RemoveFloorStack(this);
                    obj.Shake();
                }
                break;
            case TerrainType.Pit:
                foreach (ActorObjInfo obj in m_containedActorObjInfos)
                {
                    obj.RemovePitStack(this);
                    obj.Shake();
                }
                break;
        }

        m_containedActorObjInfos.Clear();
    }

    #region Properties
    #endregion
}