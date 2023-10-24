//**************************************************************************************
// File: ObjectFactoryEditor.cs
//
// Purpose: 
//
// Written By: Eluem
//**************************************************************************************

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;


[CustomEditor(typeof(ObjectFactory))]
public class PivotSystemEditor : Editor
{
    #region Declarations
    #endregion

    //***********************************************************************
    // Method: OnInspectorGUI
    //
    // Purpose: Responds to events that occur when the inspector is in focus
    //***********************************************************************
    public override void OnInspectorGUI()
    {
        if (GUILayout.Button("Print Pool Data"))
        {
            ((ObjectFactory)target).PrintPoolData();
        }

        DrawDefaultInspector();
    }

    #region Properties
    #endregion
}