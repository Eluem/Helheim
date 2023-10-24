//**************************************************************************************
// File: ISystemObject.cs
//
// Purpose: Defines a system object to unify all the objects under SystemObjects
//
// Written By: Salvatore Hanusiewicz
//**************************************************************************************

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public interface ISystemObject
{
    #region Declarations
    #endregion

    /// <summary>
    /// This function will be called any time a new map is loaded
    /// </summary>
    /// <param name="pSceneName">Name of the scene being loaded for the map</param>
    void Reload(string pSceneName);
	
	#region Properties
    #endregion
}