using UnityEngine;
using System.Collections;
using UnityEditor;
using Xft;

[CustomEditor(typeof(XWeaponTrail))]
[CanEditMultipleObjects]
public class XWeaponTrailEditor : Editor
{
    public override void OnInspectorGUI()
    {
        if (GUILayout.Button("Reset to Default", GUILayout.Width(200), GUILayout.Height(32)))
        {
            AutoSetDefaults();
        }

        EditorGUILayout.BeginHorizontal();

        EditorGUILayout.BeginVertical();
        EditorGUILayout.LabelField("Version: " + XWeaponTrail.m_version);
        EditorGUILayout.LabelField("Author: Shallway");
        EditorGUILayout.LabelField("Email: shallwaycn@gmail.com");
        //EditorGUILayout.LabelField("Web: http://phantomparticle.org");
        EditorGUILayout.EndVertical();

        EditorGUILayout.BeginVertical();
        if (GUILayout.Button("Forum", GUILayout.Width(120), GUILayout.Height(32)))
        {
            Application.OpenURL("http://phantomparticle.org/forums/forum/phantom-particle/");
        }

        if (GUILayout.Button("Get more effects!", GUILayout.Width(120), GUILayout.Height(32)))
        {
            Application.OpenURL("http://phantomparticle.org");
        }
        EditorGUILayout.EndVertical();

        EditorGUILayout.EndHorizontal();

        GUILayout.Space(10);
        DrawDefaultInspector();
    }

    /// <summary>
    /// An easy way to reset a WeaponTrail to the default values such as finding the child start and end point and setting Trail Length to 1
    /// </summary>
    public void AutoSetDefaults()
    {
        XWeaponTrail tempXWeaponTrail = (XWeaponTrail)target;

        tempXWeaponTrail.m_trailLength = 1;

        tempXWeaponTrail.m_myColor = new Color32(113, 113, 113, 20);
        

        foreach (Transform child in tempXWeaponTrail.transform)
        {
            if (child.name == "StartPoint")
            {
                tempXWeaponTrail.m_pointStart = child;
            }
            else if (child.name == "EndPoint")
            {
                tempXWeaponTrail.m_pointEnd = child;
            }
        }
    }
}

