//**************************************************************************************
// File: WeaponPartObjInfo.cs
//
// Purpose: This class is meant to detect collisions and pass the information up to
// the WeaponInfo for the weapon so that the collision can be handled.
//
// Written By: Salvatore Hanusiewicz
//**************************************************************************************

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using AdvancedInspector;

[AdvancedInspector]
public class WeaponPartObjInfo : HazardObjInfo
{
    #region Declarations
    #region Pointers
    [Inspect, Group("Pointers")]
    public CharObjInfo m_charObjInfo; //Stores a pointer to the charObjInfo for the character using the weapon this is part of.
    [Inspect, Group("Pointers")]
    public WeaponInfo m_weaponInfo; //Stores a pointer to the WeaponInfo this weapon belongs to)
    #endregion

    #region Info
    [Inspect, Group("Info")]
    public string m_partName; //Stores the name of this part
    [Inspect, Group("Info")]
    public string m_recentCollisionsGroup = "Default";
    [Inspect, Group("Info")]
    public InteractionType m_defaultInteractionType; //Stores the default type of interaction this weapon part will be considered to be making when colliding with an object
    [Inspect, Group("Info")]
    public Vector2 m_colliderSize; //Stores the width (x) and length (y) of the weapon part (assuming a rectangle collider)
                                   //Note: for any any edge collider, just use length (y). For circle colliders, set both to the radius.
    #endregion

    #region Stats
    [Inspect(1), Group("HitStats")]
    public float m_bounceModifier = 0; //This will be added to the bouncePower from any object that is hit to allow variable sensitivity based on weapon part
    [Inspect(1), Group("HitStats")]
    public KnockbackType m_knockbackType = KnockbackType.OriginFacing; //This stores the weapon part info's knockback type
    #endregion
    #endregion

    /// <summary>
    /// This will search the passed game object for any collide
    /// handlers with the passed weapon info as the parent
    /// </summary>
    /// <param name="pGameObject"></param>
    /// <param name="pWeaponInfo"></param>
    /// <returns></returns>
    public static Dictionary<string, WeaponPartObjInfo> GetWeaponPartObjInfos(GameObject pGameObject, WeaponInfo pWeaponInfo)
    {
        Dictionary<string, WeaponPartObjInfo> tempWeaponPartObjInfos = new Dictionary<string, WeaponPartObjInfo>();

        WeaponPartObjInfo[] tempWeaponPartObjInfosArray = pGameObject.GetComponentsInChildren<WeaponPartObjInfo>(true);

        foreach (WeaponPartObjInfo wcd in tempWeaponPartObjInfosArray)
        {
            if (wcd.m_weaponInfo == pWeaponInfo)
            {
                tempWeaponPartObjInfos.Add(wcd.m_partName, wcd);
            }
        }

        return tempWeaponPartObjInfos;
    }

    /// <summary>
    /// This basically takes the place of a constructor
    /// </summary>
    /// <param name="pObjType">Object Type</param>
    public void Initialize(string pObjType = "WeaponPartObjInfo")
    {
        base.Initialize(false, pObjType);

        //Sets the origin and source of this "hazard" to the charObjInfo wielding it upon initialization
        m_originObjInfo = m_charObjInfo;
        m_sourceObjInfo = m_charObjInfo;

        ResetInteractionType();

        m_weaponInfo.RegisterWeaponPartObjInfo(this);
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
        if (pObjType == "WeaponPartObjInfo" || pObjType == ObjType)
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
    }


    //TO DO: Clean this up? This should be replaced by the CollisionNode system
    /// <summary>
    /// When the trigger collider overlaps another collider, this
    /// function will run with that collider as the parameter.
    /// </summary>
    /// <param name="pOther">Other object's collider</param>
    protected override void OnTriggerEnter2D(Collider2D pOther)
    {
        //TO DO: Implement GetPointOfContact into full system for DamagePacketStatusEffect....
        //TO DO: Implement system for finding angles other than transform.up (?)

        /*
        Vector2 tempCollisionPoint2 = pOther.transform.position;
        GetPointOfContact(pOther, ref tempCollisionPoint2);
        ParticleEffectManager.PlayParticleEffect(ParticleEffectEnum.Blood, tempCollisionPoint2, m_charObjInfo.gameObject.transform.up);
        */

        //ObjInfo tempObjInfo = pOther.GetComponent<ObjInfo>();
        //ObjInfo tempObjInfo = GetObjInfoFromCollider(pOther); //((ColliderInfo)pOther.GetComponent(typeof(ColliderInfo))).ObjInfo;

        //if (IsCollisionIgnored(tempObjInfo))
        //{
        //    return;
        //}

        //Vector2 tempCollisionPoint;
        //if (tempObjInfo.PreciseCollision)
        //{
        //    if(!GetPointOfContact(pOther, out tempCollisionPoint))
        //    {
        //        tempCollisionPoint = m_transform.position;
        //    }
        //}
        //else
        //{
        //    tempCollisionPoint = m_transform.position;
        //}

        //m_weaponInfo.HandleCollision(this, pOther, tempObjInfo, tempCollisionPoint, m_partName, GenerateDamageContainer(), m_knockbackType, m_interactionType);
    }

    /// <summary>
    /// Handles a collision registered by a CollisionNode for this WeaponPart
    /// </summary>
    /// <param name="pRaycastHit2D"></param>
    /// <param name="pRayOrigin"></param>
    /// <param name="pRayDir"></param>
    public virtual void OnCollisionNodeEnter2D(CollisionNodeHit2D pCollisionNodeHit2D)
    {
        ObjInfo tempObjInfo = GetObjInfoFromCollider(pCollisionNodeHit2D.Collider); //((ColliderInfo)pOther.GetComponent(typeof(ColliderInfo))).ObjInfo;

        if (IsCollisionIgnored(tempObjInfo))
        {
            return;
        }

        //Vector2 tempCollisionPoint;
        //if (tempObjInfo.PreciseCollision)
        //{
        //    if (!GetPointOfContact(pRaycastHit2D.collider, out tempCollisionPoint))
        //    {
        //        tempCollisionPoint = m_transform.position;
        //    }
        //}
        //else
        //{
        //    tempCollisionPoint = m_transform.position;
        //}

        m_weaponInfo.HandleCollision(this, pCollisionNodeHit2D, tempObjInfo, m_partName, GenerateDamageContainer(), m_knockbackType, m_interactionType);
    }

    /// <summary>
    /// Fires a RaycastAll and checks if any of the detected objects
    /// are the object that the Trigger detected. If it's found (it should be)
    /// overrides the pCollisionPoint (which is defaulted to the center of
    /// this weapon part)
    /// </summary>
    /// <param name="pCollider2D"></param>
    /// <param name="pCollisionPoint"></param>
    /// <returns></returns>
    private bool GetPointOfContact(Collider2D pCollider2D, out Vector2 pCollisionPoint)
    {
        Vector2 tempOrigin;
        Vector2 tempDirection;
        float tempDistance;

        Vector2 tempRight2D = (new Vector2(transform.right.x, transform.right.y)).normalized;

        if (m_transform.parent.localScale.y < 0)
        {
            tempDirection = transform.up * -1;
        }
        else
        {
            tempDirection = transform.up;
        }

        tempDistance = m_colliderSize.y;


        //Check the center of the object
        //NOTE: I assume the collider used was the ground collider, since this is only really used for hitting walls right now, I might need to come up with a way to robustify this and determine WHICH collider hit
        //TO DO: Possibly convert entire OnTriggerEnter2D system for *ALL* objects to make full use of the "ColliderInfo" system.. and instead of registering the events up here, register them down there
        //and pass the hit back up... along with the collider that hit
        //****ANOTHER OPTION**** I can use the layermask of the collider that this is hitting (pCollider2D) to figure out which one of my colliders it hit, since it can only be so many...
        //I could actually also just use the name of the collider to decide if it matches with which of my colliders
        tempOrigin = new Vector2(transform.position.x, transform.position.y - (m_colliderSize.y / 2)); //- m_actorGroundCollider.offset.y)); This system handled y offsets in the past, but I've decided that it's somewhat pointless to do so
        if (RaycastCollisionCheck(pCollider2D, tempOrigin, tempDirection, tempDistance, out pCollisionPoint))
        {
            return true;
            //Debug.Log("Center"); //TO DO: Remove me
        }

        //Check the leading edge
        tempOrigin -= (m_colliderSize.x / 2) * tempRight2D;
        if (RaycastCollisionCheck(pCollider2D, tempOrigin, tempDirection, tempDistance, out pCollisionPoint))
        {
            return true;
            //Debug.Log("Leading"); //TO DO: Remove me
        }

        //Check the back edge
        tempOrigin += m_colliderSize.x * tempRight2D;
        if (RaycastCollisionCheck(pCollider2D, tempOrigin, tempDirection, tempDistance, out pCollisionPoint))
        {
            return true;
            //Debug.Log("Back"); //TO DO: Remove me
        }


        //Debug.Log("(" + name + ") " + "Error: GetPointOfContact failed to find hit object.");

        //PrintInfo(); //TO DO: remove me
        //Debug.Break(); //TO DO: remove me

        return false;
    }

    /// <summary>
    /// Attempts a 2d raycast based on the passed parameters and returns whether or not the Collider2D passed was hit as well as returning the point of collision via an out parameter
    /// </summary>
    /// <param name="pCollidePoint">Returns point of collision via this variable</param>
    /// <param name="pCollider2D">Collider to check against</param>
    /// <param name="pOrigin">Origin for raycast</param>
    /// <param name="pDirection">Direction for raycast</param>
    /// <param name="pDistance">Length of raycast</param>
    /// <returns></returns>
    private bool RaycastCollisionCheck(Collider2D pCollider2D, Vector2 pOrigin, Vector2 pDirection, float pDistance, out Vector2 pCollidePoint)
    {
        //Project a ray from pOrigin, toward pDirection, with a length of pDistance and return all colliders that this ray passes through
        RaycastHit2D[] tempRaycastHit2D = Physics2D.RaycastAll(pOrigin, pDirection, pDistance);

        //Debug.Log(pOrigin.ToString("F8") + ", " + pDirection.ToString("F8") + ", " + pDistance.ToString("F8") + ", " + tempRaycastHit2D.Length); //TO DO: REMOVE ME

        foreach(RaycastHit2D raycastHit2D in tempRaycastHit2D)
        {
            if(raycastHit2D.collider == pCollider2D)
            {
                pCollidePoint = raycastHit2D.point;
                return true;
            }
        }

        pCollidePoint = Vector2.zero;
        return false;
    }

    /// <summary>
    /// Sets the current interaction type
    /// </summary>
    /// <param name="pInteractionType"></param>
    public void SetInteractionType(InteractionType pInteractionType)
    {
        m_interactionType = pInteractionType;
    }

    /// <summary>
    /// Resets the current interaction type to the default
    /// </summary>
    public void ResetInteractionType()
    {
        m_interactionType = m_defaultInteractionType;
    }

        #region Properties
    public string PartName
    {
        get
        {
            return m_partName;
        }
    }

    public int BounceModifier
    {
        get
        {
            return (int)m_bounceModifier;
        }
    }

    public KnockbackType KnockbackType
    {
        get
        {
            return m_knockbackType;
        }
    }

    public string RecentCollisionsGroup
    {
        get
        {
            return m_recentCollisionsGroup;
        }
    }
    #endregion
}
