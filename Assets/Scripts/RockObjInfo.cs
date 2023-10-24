//**************************************************************************************
// File: RockObjInfo.cs
//
// Purpose: This class handles all information and interactions for rock objects
//
// Written By: Salvatore Hanusiewicz
//**************************************************************************************

using UnityEngine;
using System.Collections;

public class RockObjInfo : ObstacleObjInfo
{
    #region Declarations
    #endregion
	
	/// <summary>
    /// This basically takes the place of a constructor
    /// </summary>
    public void Initialize()
    {
        base.Initialize("RockObjInfo");
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
        if (pObjType == "ObstacleObjInfo" || pObjType == "RockObjInfo" || pObjType == ObjType)
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

    #region Properties
    #endregion
}