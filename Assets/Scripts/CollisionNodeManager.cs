//**************************************************************************************
// File: CollisionNodeManager.cs
//
// Purpose: Manages all the collision nodes for a weapon part
//
// Written By: Salvatore Hanusiewicz
//**************************************************************************************

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using AdvancedInspector;

public class CollisionNodeManager : MonoBehaviour
{
    #region Declarations
    public Transform m_transform;
    public WeaponPartObjInfo m_owner;

    public int m_layerMask;

    public CollisionNode[] m_collisionNodes;

    protected List<CollisionNodeHit2D> m_collisions = new List<CollisionNodeHit2D>(); //List of RaycastHits for the current collision check
    protected List<Transform> m_ignoreList = new List<Transform>(); //List of game objects to ignore for hte current collision 
    #endregion

    /// <summary>
    /// This basically takes the place of a constructor
    /// </summary>
    public void Initialize()
    {

    }

    /// <summary>
    /// Use this for initialization
    /// </summary>
    void Start()
    {

    }

    /// <summary>
    /// Fires when this component becomes enabled
    /// </summary>
    void OnEnable()
    {
        //Debug.Log("OnEnable: " + Time.frameCount);
        UpdateNodePrevPositions();
    }

    /// <summary>
    /// Update is called once per frame
    /// </summary>
    void Update()
    {
        //if (name == "BladeLeft")
        //{
        //    Debug.Log("(" + Time.frameCount + ") Update: " + m_collisionNodes[0].m_transform.position);
        //}

        CheckCollisions();
        UpdateNodePrevPositions();
    }

    ///// <summary>
    ///// Physicsy Updates
    ///// </summary>
    //protected virtual void FixedUpdate()
    //{
    //    //if (name == "BladeLeft")
    //    //{
    //    //    Debug.Log("(" + Time.frameCount + ") FixedUpdate: " + m_collisionNodes[0].m_transform.position);
    //    //}

    //    //TO DO: Make sure the collision checks should happen in FixedUpdate and not Update... or possibly queued up in update and then processed in fixed???
    //    CheckCollisions();
    //    UpdateNodePrevPositions();
    //}

    /// <summary>
    /// Updates all the nodes' previous positions
    /// </summary>
    public void UpdateNodePrevPositions()
    {
        for (int i = 0; i < m_collisionNodes.Length; i++)
        {
            m_collisionNodes[i].UpdatePrevPos();
        }
    }

    /// <summary>
    /// Tells all nodes to check for collisions
    /// </summary>
    public void CheckCollisions()
    {
        RaycastHit2D[] tempRaycastHit2D;
        Vector2 tempOrigin;
        Vector2 tempDir;

        bool hitSuccess; //TO DO: DELETE ME/REMOVE ME

        for (int i = 0; i < m_collisionNodes.Length; i++)
        {
            tempRaycastHit2D = m_collisionNodes[i].CollisionCheck(m_layerMask, out tempOrigin, out tempDir);

            hitSuccess = false; //TODO DELETE ME/REMOVE ME

            for (int j = 0; j < tempRaycastHit2D.Length; j++)
            {
                if (tempRaycastHit2D[j].transform.root != m_transform.root && !m_ignoreList.Contains(tempRaycastHit2D[j].transform))
                {
                    m_ignoreList.Add(tempRaycastHit2D[j].transform);
                    m_collisions.Add(new CollisionNodeHit2D(tempRaycastHit2D[j], tempOrigin, tempDir));

                    hitSuccess = true;
                }
            }

            m_collisionNodes[i].DrawDebugRay(hitSuccess); //TO DO: DELETE ME/REMOVE ME
        }

        foreach (CollisionNodeHit2D hit in m_collisions)
        {
            m_owner.OnCollisionNodeEnter2D(hit);
        }

        m_collisions.Clear();
        m_ignoreList.Clear();
    }

    /// <summary>
    /// Debug function that populates all the fields automatically
    /// </summary>
    [Inspect]
    public void AutoConfigure()
    {
        //Set direct reference to transform
        m_transform = transform;

        //Set direct reference to WeaponPartObjInfo
        m_owner = gameObject.GetComponent<WeaponPartObjInfo>();

        //Auto-get layermask int
        string[] layerMaskArray = new string[] { "All", "Ground", "ActorHazard", "Hazard", "Weapon" };  //{ "Ground", "All" };

        LayerMask tempLayerMask = LayerMask.GetMask(layerMaskArray);

        string layerMasks = "[";

        foreach (string lm in layerMaskArray)
        {
            layerMasks += lm + "(" + LayerMask.NameToLayer(lm) + "), ";
        }

        layerMasks = layerMasks.Substring(0, layerMasks.Length - 2) + "]";

        Debug.Log(layerMasks + ": " + tempLayerMask.value);

        m_layerMask = tempLayerMask.value;


        //Auto-configure all the nodes
        List<Transform> tempNodeTransforms = new List<Transform>();
        foreach (Transform child in transform)
        {
            if (child.name.Contains("CollisionNode"))
            {
                tempNodeTransforms.Add(child);
            }
        }

        m_collisionNodes = new CollisionNode[tempNodeTransforms.Count];

        for (int i = 0; i < m_collisionNodes.Length; i++)
        {
            foreach (Transform node in tempNodeTransforms)
            {
                if (node.name.Contains(i.ToString()))
                {
                    m_collisionNodes[i] = new CollisionNode(node);
                }
            }
        }
    }

    #region Properties
    #endregion
}