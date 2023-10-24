//**************************************************************************************
// File: UltraGreatswordEarthquakeObjInfo.cs
//
// Purpose: Defines the object type and all it's information for the Ultragreatsword's
// fully charged overhead earthquake
//
// Written By: Salvatore Hanusiewicz
//**************************************************************************************

using UnityEngine;
using System.Collections;
using AdvancedInspector;

[AdvancedInspector]
public class UltraGreatswordEarthquakeObjInfo : ExplosionObjInfo
{
    #region Declarations
    [Inspect, Group("Pointers")]
    public ParticleSystem[] m_particleSystems;
    #endregion
	
	/// <summary>
    /// This basically takes the place of a constructor
    /// </summary>
    public void Initialize()
    {
        base.Initialize(false, "UltraGreatswordEarthquakeObjInfo");
    }

    /// <summary>
    /// This is the code that runs each time this object is spawned
    /// </summary>
    /// <param name="pOriginObjInfo"></param>
    /// <param name="pSourceObjInfo"></param>
    /// <param name="pDirection"></param>
    public void Spawn(ObjInfo pOriginObjInfo, ObjInfo pSourceObjInfo, Vector3 pDirection)
    {
        base.Spawn(pOriginObjInfo, pSourceObjInfo);

        m_transform.up = pDirection;
    }

    /// <summary>
    /// Triggers one of the explosion particle systems to play
    /// </summary>
    /// <param name="pIndex"></param>
    public void PlayParticleSystem(int pIndex)
    {
        m_particleSystems[pIndex].Play();
    }

    #region Properties
    #endregion
}