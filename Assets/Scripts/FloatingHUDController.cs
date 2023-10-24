//**************************************************************************************
// File: FloatingHUDController.cs
//
// Purpose: This is the base class which will control all floating health/info HUDs
// for destructible objects.
//
// Written By: Salvatore Hanusiewicz
//**************************************************************************************

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using AdvancedInspector;


[AdvancedInspector]
public class FloatingHUDController : ObjInfo
{
    #region Declarations
    #region Pointers
    [Inspect, Group("Pointers")]
    public Canvas m_canvas;
    [Inspect, Group("Pointers")]
    public CanvasGroup m_canvasGroup;
    [Inspect, Group("Pointers")]
    public UnityEngine.UI.Image m_healthBarSlider;
    #endregion

    protected DestructibleObjInfo m_ownerObjInfo;

    protected Vector2 m_offset; //Offset from the owner's position

    protected float m_zAxisOrder; //Z axis transform position used to set render order

    protected List<ObjInfo> m_charOverlaps; //List of all characters this HUD overlaps
    protected List<ObjInfo> m_floatingHUDOverlaps; //List of all other floating HUDs overlapping with this one
    #endregion

    /// <summary>
    /// This basically takes the place of a constructor
    /// </summary>
    /// <param name="pOwnerObjInfo"></param>
    /// <param name="pZAxisOrder"></param>
    /// <param name="pOffset"></param>
    /// <param name="pObjType"></param>
    public virtual void Initialize(DestructibleObjInfo pOwnerObjInfo, float pZAxisOrder, Vector2 pOffset, string pObjType = "FloatingHUDController")
    {
        base.Initialize(false, pObjType);

        m_ownerObjInfo = pOwnerObjInfo;
        m_offset = pOffset;

        m_zAxisOrder = pZAxisOrder;

        m_charOverlaps = new List<ObjInfo>();
        m_floatingHUDOverlaps = new List<ObjInfo>();
    }


    /// <summary>
    /// Accepts an object type and returns true if one of the types that it is matches the type passed.
    /// i.e. PlayerObjInfo is also a CharObjInfo and a DestructibleObjInfo
    /// </summary>
    /// <param name="pObjType"></param>
    /// <returns></returns>
    public override bool IsType(string pObjType)
    {
        if (pObjType == "FloatingHUDController" || pObjType == ObjType)
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
        base.Start();
    }

    //***********************************************************************
    // Method: Update
    //
    // Purpose: Update is called once per frame
    //***********************************************************************
    protected override void Update()
    {
        HandleOwnerDestroyed();

        if (m_destroyed)
        {
            return;
        }

        TrackOwner();

        UpdateHealthBar();

        HandleOverlaps();
    }

    //***********************************************************************
    // Method: HandleOwnerDestroyed
    //
    // Purpose: Checks if the owner is destroyed, if so, destroy this
    //***********************************************************************
    protected void HandleOwnerDestroyed()
    {
        if (m_ownerObjInfo == null)
        {
            //TO DO: Use DestroyMe??????
            m_destroyed = true;

            Destroy(m_transform.gameObject);
        }
    }

    //***********************************************************************
    // Method: TrackOwner
    //
    // Purpose: Updates the position of this element to stay in the proper
    // relative position to the owner object
    //***********************************************************************
    protected void TrackOwner()
    {
        m_transform.position = new Vector3(m_ownerObjInfo.Transform.position.x + m_offset.x, m_ownerObjInfo.Transform.position.y + m_offset.y, m_zAxisOrder);
    }

    /// <summary>
    /// Handles when this Floating HUD is overlapping different ObjInfos
    /// </summary>
    protected void HandleOverlaps()
    {
        //Handle character overlaps
        if (m_charOverlaps.Count > 0)
        {
            m_canvasGroup.alpha = .3f;
        }
        else
        {
            m_canvasGroup.alpha = 1f;
        }

        //Handle HUD overlaps
        /*
        if (otherObjInfo.IsType("FloatingHUDController"))
        {

        }
        */
    }

    //***********************************************************************
    // Method: UpdateHealthBar
    //
    // Purpose: Updates the health bar
    //***********************************************************************
    protected void UpdateHealthBar()
    {
        m_healthBarSlider.fillAmount = m_ownerObjInfo.HealthFloat / m_ownerObjInfo.MaxHealth;
    }

    /// <summary>
    /// When the trigger collider overlaps another collider, this function will run with that collider as the parameter.
    /// </summary>
    /// <param name="pOther"></param>
    protected virtual void OnTriggerEnter2D(Collider2D pOther)
    {
        ObjInfo otherObjInfo = GetObjInfoFromCollider(pOther);

        if (otherObjInfo.IsType("CharObjInfo"))
        {
            if (!m_charOverlaps.Contains(otherObjInfo))
            {
                m_charOverlaps.Add(otherObjInfo);
            }
        }
        else if (otherObjInfo.IsType("FloatingHUDController"))
        {
            if (!m_floatingHUDOverlaps.Contains(otherObjInfo))
            {
                m_floatingHUDOverlaps.Add(otherObjInfo);
            }
        }
    }

    /// <summary>
    /// When the trigger collider stops overlapping another collider, this function will run with that collider as the parameter
    /// </summary>
    /// <param name="pOther"></param>
    protected virtual void OnTriggerExit2D(Collider2D pOther)
    {
        ObjInfo otherObjInfo = GetObjInfoFromCollider(pOther);

        if (otherObjInfo.IsType("CharObjInfo"))
        {
            m_charOverlaps.Remove(otherObjInfo);
        }
        else if (otherObjInfo.IsType("FloatingHUDController"))
        {
            m_floatingHUDOverlaps.Remove(otherObjInfo);
        }
    }

    #region Properties
    private DestructibleObjInfo OwnerObjInfo
    {
        get
        {
            return m_ownerObjInfo;
        }
    }
    #endregion
}