//**************************************************************************************
// File: CollisionNodeHit2D.cs
//
// Purpose: CollisionNodeHit2D is used to add details to a typical RaycastHit2D
//
// Written By: Salvatore Hanusiewicz
//**************************************************************************************

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CollisionNodeHit2D
{
    #region Declarations
    protected RaycastHit2D m_raycastHit2D;
    protected Vector2 m_origin;
    protected Vector2 m_dir;
    #endregion
	
	/// <summary>
    /// Constructor for class
    /// </summary>
	public CollisionNodeHit2D(RaycastHit2D pRaycastHit2D, Vector2 pOrigin, Vector2 pDir)
	{
        m_raycastHit2D = pRaycastHit2D;
        m_origin = pOrigin;
        m_dir = pDir;
	}
	
	#region Properties
    /// <summary>
    /// Full Raycasthit2D object generated from the hit.
    /// </summary>
    public RaycastHit2D RaycastHit2D
    {
        get
        {
            return m_raycastHit2D;
        }
    }

    /// <summary>
    /// The origin of the raycast.
    /// </summary>
    public Vector2 Origin
    {
        get
        {
            return m_origin;
        }
    }

    /// <summary>
    /// The direction (including length) of the raycast.
    /// </summary>
    public Vector2 Dir
    {
        get
        {
            return m_dir;
        }
    }

    /// <summary>
    /// The centroid of the primitive used to perform the cast.
    /// </summary>
    public Vector2 Centroid
    {
        get
        {
            return m_raycastHit2D.centroid;
        }
    }

    /// <summary>
    /// The collider hit by the ray.
    /// </summary>
    public Collider2D Collider
    {
        get
        {
            return m_raycastHit2D.collider;
        }
    }

    /// <summary>
    /// The distance from the ray origin to the impact point.
    /// </summary>
    public float Distance
    {
        get
        {
            return m_raycastHit2D.distance;
        }
    }

    /// <summary>
    /// Fraction of the distance along the ray that the hit occurred.
    /// </summary>
    public float Fraction
    {
        get
        {
            return m_raycastHit2D.fraction;
        }
    }

    /// <summary>
    /// The normal vector to the surface hit by the ray.
    /// </summary>
    public Vector2 Normal
    {
        get
        {
            return m_raycastHit2D.normal;
        }
    }

    /// <summary>
    /// The point in world space where the ray hit the collider's surface.
    /// </summary>
    public Vector2 Point
    {
        get
        {
            return m_raycastHit2D.point;
        }
    }

    /// <summary>
    /// The Rigidbody2D attached to the object that was hit.
    /// </summary>
    public Rigidbody2D Rigidbody
    {
        get
        {
            return m_raycastHit2D.rigidbody;
        }
    }

    /// <summary>
    /// The Transform of the object that was hit.
    /// </summary>
    public Transform Transform
    {
        get
        {
            return m_raycastHit2D.transform;
        }
    }
    #endregion
}