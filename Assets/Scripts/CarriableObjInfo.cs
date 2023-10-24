//**************************************************************************************
// File: CarriableObjInfo.cs
//
// Purpose: Defines the logic for any actor object which can be carried
//
// Written By: Salvatore Hanusiewicz
//**************************************************************************************

using UnityEngine;
using System.Collections;
using AdvancedInspector;

[AdvancedInspector]
public abstract class CarriableObjInfo : ActorObjInfo
{
    #region Declarations
    #region Pointers
    [Inspect, Group("Pointers")]
    protected CharObjInfo m_carrierObjInfo = null; //CharObjInfo which is currently carrying this object
    #endregion
    #endregion

    /// <summary>
    /// This basically takes the place of a constructor
    /// </summary>
    public virtual void Initialize(bool pPooled, string pObjType = "CarriableObjInfo")
    {
        base.Initialize(pPooled, pObjType);
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
        if (pObjType == "ActorObjInfo" || pObjType == "CarriableObjInfo" || pObjType == ObjType)
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
    /// Begin being carried by the passed CharObjInfo
    /// </summary>
    public virtual void CarryBegin(CharObjInfo pCarrierObjInfo)
    {
        m_carrierObjInfo = pCarrierObjInfo;
        m_transform.SetParent(m_carrierObjInfo.CarrierTransform);
    }

    /// <summary>
    /// Stop being carried
    /// </summary>
    public virtual void CarryEnd()
    {
        m_transform.SetParent(null);
    }

    #region Properties
    #endregion
}