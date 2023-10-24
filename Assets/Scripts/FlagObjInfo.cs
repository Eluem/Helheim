//**************************************************************************************
// File: FlagObjInfo.cs
//
// Purpose: Defines a basic flag for the CTF game mode
//
// Written By: Salvatore Hanusiewicz
//**************************************************************************************

using UnityEngine;
using System.Collections;
using AdvancedInspector;

[AdvancedInspector]
public class FlagObjInfo : CarriableObjInfo
{
    #region Declarations
    #region Pointers
    [Inspect, Group("Pointers")]
    public FlagStandObjInfo m_flagStand;
    #endregion
    #endregion

    /// <summary>
    /// This basically takes the place of a constructor
    /// </summary>
    public void Initialize()
    {
        base.Initialize(false, "FlagObjInfo");
	}

	/// <summary>
    /// Use this for initialization
    /// </summary>
	protected override void Start()
	{
		if(!m_initialized)
        {
            Initialize();
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
            if(otherObjInfo.Team != m_team)
            {
                ((CharObjInfo)otherObjInfo).Loadout.PickUp(this);
            }
            else if(!m_flagStand.FlagInStand)
            {
                Reset();
            }
        }
    }

    /// <summary>
    /// Begin being carried by the passed CharObjInfo
    /// </summary>
    public override void CarryBegin(CharObjInfo pCarrierObjInfo)
    {
        base.CarryBegin(pCarrierObjInfo);

        m_transform.localRotation = Quaternion.Euler(0, 0, 180);
        m_transform.localScale = new Vector3(1, 0.26f, 1);
        m_transform.localPosition = new Vector3(0, -0.5f, -2);

        DisableAllColliders();

        m_flagStand.FlagInStand = false;
    }

    /// <summary>
    /// Stop being carried
    /// </summary>
    public override void CarryEnd()
    {
        base.CarryEnd();

        m_transform.localRotation = Quaternion.Euler(0, 0, 0);
        m_transform.localScale = new Vector3(1, 1, 1);

        m_transform.position = new Vector3(m_transform.position.x, m_transform.position.y + 1.524f, m_transform.position.z);

        EnableAllColliders();
    }

    /// <summary>
    /// Resets the flag to the stand's position
    /// </summary>
    public void Reset()
    {
        m_transform.position = new Vector3(m_flagStand.Transform.position.x, m_flagStand.Transform.position.y + 1.524f, -10);
        ResetSpriteSorting();

        m_flagStand.FlagInStand = true;
    }

    #region Properties
    #endregion
}