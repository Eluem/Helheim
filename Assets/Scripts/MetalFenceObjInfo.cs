//**************************************************************************************
// File: MetalFenceObjInfo.cs
//
// Purpose: This class handles all information and interactions for metal fence objects
//
// Written By: Salvatore Hanusiewicz
//**************************************************************************************

using UnityEngine;
using System.Collections;

public class MetalFenceObjInfo : ObstacleObjInfo
{
    #region Declarations
    #endregion

    //***********************************************************************
    // Method: Initialize
    //
    // Purpose: This basically takes the place of a constructor
    //***********************************************************************
    public void Initialize()
    {
        base.Initialize("MetalFenceObjInfo");
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
        if (pObjType == "ObstacleObjInfo" || pObjType == "MetalFenceObjInfo" || pObjType == ObjType)
        {
            return true;
        }

        return false;
    }

    //***********************************************************************
    // Method: Start
    //
    // Purpose: Use this for initialization
    //***********************************************************************
    protected override void Start()
    {
        if (!m_initialized)
        {
            Initialize();
        }
    }

    //***********************************************************************
    // Method: Update
    //
    // Purpose: Update is called once per frame
    //***********************************************************************
    protected override void Update()
    {
        base.Update();
    }

    #region Properties
    #endregion
}