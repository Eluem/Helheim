//**************************************************************************************
// File: RotateArounder.cs
//
// Purpose: This is a little script that I can attach to an object and tell it to
// rotate around another object using a function and the AvancedInspector to expose it
// as ane editor button. It's just a work around for the fact that unity has no
// capability to change pivot points during rotation
//
// Written By: Salvatore Hanusiewicz
//**************************************************************************************

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using AdvancedInspector;

[AdvancedInspector]
public class RotateArounder : MonoBehaviour
{
    public enum Axis
    {
        XAxis,
        YAxis,
        ZAxis
    }

    #region Declarations
    [Inspect]
    public Transform m_pivotObject;
    [Inspect]
    public float m_angle;
    [Inspect]
    public Axis m_axis = Axis.ZAxis;

    [Inspect] //TO DO: REMOVE INSPECT
    private List<RotateArounderHistoryInfo> m_history = new List<RotateArounderHistoryInfo>(); //Used to allow for undos
    
    [Inspect] //TO DO: REMOVE INSPECT
    private int m_historyIndex = -1; //Location we're currently at in the history
    #endregion

    //***********************************************************************
    // Method: Start
    //
    // Purpose: Use this for initialization
    //***********************************************************************
    void Start()
	{
        Debug.Log("Remove Me From: " + gameObject.name);
	}

    //***********************************************************************
    // Method: LogHistory
    //
    // Purpose: Creates a new log entry in the history, deleting all data
    // ahead of the history pointer
    //***********************************************************************
    public void LogHistory()
    {
        //Clear front loaded history
        if(m_historyIndex < m_history.Count - 1)
        {
            m_history.RemoveRange(m_historyIndex + 1, m_history.Count - m_historyIndex - 1);
        }

        m_history.Add(new RotateArounderHistoryInfo(transform.position, transform.rotation));

        m_historyIndex += 1;
    }

    //***********************************************************************
    // Method: RotateAroundPivot
    //
    // Purpose: Rotates this object around the pivot object
    //***********************************************************************
    [Inspect]
    public void RotateAroundPivot()
    {
        if(m_historyIndex == -1)
        {
            LogHistory();
        }

        Vector3 axis = Vector3.zero;

        switch (m_axis)
        {
            case Axis.XAxis:
                axis = Vector3.right;
                break;
            case Axis.YAxis:
                axis = Vector3.up;
                break;
            case Axis.ZAxis:
                axis = Vector3.forward;
                break;
        }

        transform.RotateAround(m_pivotObject.transform.position, axis, m_angle);

        LogHistory();
    }

    //***********************************************************************
    // Method: Undo
    //
    // Purpose: Sets the position and rotation to the previous values in the
    // history
    //***********************************************************************
    [Inspect]
    public void Undo()
    {
        if(m_historyIndex > 0)
        {
            m_historyIndex -= 1;
            m_history[m_historyIndex].ApplyHistoryInfo(transform);
        }
    }

    //***********************************************************************
    // Method: Redo
    //
    // Purpose: Sets the position and rotation to the next values in the
    // history
    //***********************************************************************
    [Inspect]
    public void Redo()
    {
        if (m_historyIndex < m_history.Count - 1)
        {
            m_historyIndex += 1;
            m_history[m_historyIndex].ApplyHistoryInfo(transform);
        }
    }

    #region Properties
    #endregion

    private class RotateArounderHistoryInfo
    {
        #region Declarations
        private Vector3 m_position;
        private Quaternion m_rotation;
        #endregion

        //***********************************************************************
        // Method: RotateArounderHistoryInfo
        //
        // Purpose: Constructor for class
        //***********************************************************************
        public RotateArounderHistoryInfo(Vector3 pPosition, Quaternion pRotation)
        {
            m_position = pPosition;
            m_rotation = pRotation;
        }

        //***********************************************************************
        // Method: ApplyHistoryInfo
        //
        // Purpose: Applies the history info to the passed transform
        //***********************************************************************
        public void ApplyHistoryInfo(Transform pTransform)
        {
            pTransform.position = m_position;
            pTransform.rotation = m_rotation;
        }

        #region Properties=
        #endregion
    }
}