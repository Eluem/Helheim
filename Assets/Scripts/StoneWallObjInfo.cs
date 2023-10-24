//**************************************************************************************
// File: StoneWallObjInfo.cs
//
// Purpose: This class handles all information and interactions for stone wall objects
//
// Written By: Salvatore Hanusiewicz
//**************************************************************************************

using UnityEngine;
using System.Collections;

public class StoneWallObjInfo : ObstacleObjInfo
{
    #region Declarations
    #endregion

    /// <summary>
    /// This basically takes the place of a constructor
    /// </summary>
    public void Initialize()
    {
        base.Initialize("StoneWallObjInfo");
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
        if (pObjType == "ObstacleObjInfo" || pObjType == "StoneWallObjInfo" || pObjType == ObjType)
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