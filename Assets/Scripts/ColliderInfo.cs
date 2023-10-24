//**************************************************************************************
// File: ColliderInfo.cs
//
// Purpose: This is used to give information about a collider in an Objinfo. This makes
// it easier to allow multiple sub colliders in an object and still easily obtain their
// ObjInfo with out needing to do a lot of GetComponent calls or anything complex.
//
// Written By: Salvatore Hanusiewicz
//**************************************************************************************

using UnityEngine;
using System.Collections;
using AdvancedInspector;

[AdvancedInspector]
public class ColliderInfo : MonoBehaviour
{
    #region Declarations
    #region Pointers
    [Inspect, Group("Pointers", 1)]
    public ObjInfo m_objInfo; //Pointer to the objInfo that owns this collider
    #endregion

    protected bool m_initialized = false; //Indicates if this object has been initialized
    #endregion

    /// <summary>
    /// This basically takes the place of a constructor
    /// </summary>
    public void Initialize()
    {
        m_initialized = true;

        m_objInfo = (ObjInfo)GetComponentInParent(typeof(ObjInfo));
    }

    /// <summary>
    /// Use this for initialization
    /// </summary>
    protected virtual void Start()
    {
        if (!m_initialized)
        {
            Initialize();
        }
    }

    #region Properties
    public ObjInfo ObjInfo
    {
        get
        {
            return m_objInfo;
        }
    }
    #endregion
}