//**************************************************************************************
// File: CollisionNode.cs
//
// Purpose: Defines the logic for a collision node. Collision nodes are used for
// melee weapons to grant increased collision detection accuracy by firing raycasts
// between their previous and current positions.
//
// Written By: Salvatore Hanusiewicz
//**************************************************************************************

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class CollisionNode
{
    #region Declarations
    public Transform m_transform;
    protected Vector3 m_prevPosition;
    #endregion
	
	/// <summary>
    /// Constructor for class
    /// </summary>
	public CollisionNode()
	{

	}

    /// <summary>
    /// Constructor for class
    /// </summary>
	public CollisionNode(Transform pTransform)
    {
        m_transform = pTransform;
    }

    /// <summary>
    /// called each time the nodes should update their previous position
    /// </summary>
    public void UpdatePrevPos()
    {
        m_prevPosition = m_transform.position;
    }

    /// <summary>
    /// Returns all collisions found
    /// </summary>
    /// <returns></returns>
    public RaycastHit2D[] CollisionCheck(int pLayerMask, out Vector2 pOrigin, out Vector2 pDir)
    {
        Vector2 dir = m_transform.position - m_prevPosition;

        pOrigin = m_transform.position;
        pDir = dir;

        return Physics2D.RaycastAll(m_prevPosition, dir, dir.magnitude, pLayerMask);
    }

    /// <summary>
    /// Draws a debug ray
    /// </summary>
    /// <param name="hitSuccess"></param>
    public void DrawDebugRay(bool hitSuccess)
    {
        Vector2 dir = m_transform.position - m_prevPosition;
        if (hitSuccess)
        {
            //Debug.Log("green");
            Debug.DrawRay(m_prevPosition, dir, Color.green, 10, false);
        }
        else
        {
            //Debug.Log("red");
            Debug.DrawRay(m_prevPosition, dir, Color.red, 10, false);
        }
    }
	
	#region Properties
    #endregion
}