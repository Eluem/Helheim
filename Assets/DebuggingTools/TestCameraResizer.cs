//**************************************************************************************
// File: TestCameraResizer.cs
//
// Purpose: This is a test system meant to allow me to resize the camera during
// run time using the keyboard to test out how it works in unity
//
// TO DO: Rename.. this does more than resize now..
//
// Written By: Salvatore Hanusiewicz
//**************************************************************************************

using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using AdvancedInspector;

[AdvancedInspector]
public class TestCameraResizer : MonoBehaviour
{
    #region Declarations
    [Inspect]
    public Camera m_camera;
    [Inspect]
    public Text m_text;


    [Inspect, Group("Line Drawer")]
    public Vector2 m_origin;
    [Inspect, Group("Line Drawer")]
    public Vector2 m_direction;
    [Inspect, Group("Line Drawer")]
    public float m_distance;
    [Inspect, Group("Line Drawer")]
    public float m_duration;
    [Inspect, Group("Line Drawer")]
    public Color m_color;

    [Inspect, Group("Animation Recorder")]
    public bool m_recording = false; //Indicates that the camera should be "recording
    [Inspect, Group("Animation Recorder")]
    public List<GameObject> m_visibleObjects = new List<GameObject>(); //List of objects to be visible in the "recording"

    [Inspect, Group("Draw Mesh")]
    public Mesh m_mesh;
    [Inspect, Group("Draw Mesh")]
    public Material m_material;
    [Inspect, Group("Draw Mesh")]
    public int m_layer;
    [Inspect, Group("Draw Mesh")]
    public Vector3 m_position = Vector3.zero;
    #endregion

    /// <summary>
    /// Use this for initialization
    /// </summary>
    void Start()
    {
    }

    /// <summary>
    /// Update is called once per frame
    /// </summary>
    void Update()
    {
        HandleCameraResizeInput();

        Record();

        m_text.text = m_camera.orthographicSize.ToString();
    }


    /// <summary>
    /// Handles input that will cause the camera to resize
    /// </summary>
    void HandleCameraResizeInput()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            m_camera.orthographicSize += 0.1f;
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            m_camera.orthographicSize -= 0.1f;
        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            m_camera.orthographicSize += 0.05f;
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            m_camera.orthographicSize -= 0.05f;
        }

        if (Input.GetKeyDown(KeyCode.RightBracket))
        {
            m_camera.orthographicSize += 0.01f;
        }
        if (Input.GetKeyDown(KeyCode.LeftBracket))
        {
            m_camera.orthographicSize -= 0.01f;
        }

        if (Input.GetKeyDown(KeyCode.PageUp))
        {
            m_camera.orthographicSize += 1f;
        }
        if (Input.GetKeyDown(KeyCode.PageDown))
        {
            m_camera.orthographicSize -= 1f;
        }
    }

    /// <summary>
    /// Takes a screenshot with only the objects in the m_visibleObjects list in it, if m_recording is set true
    /// </summary>
    void Record()
    {
        if (m_recording)
        {
            SaveScreenshotToFile("Screenshots\\test " + Time.frameCount + ".png");
        }
    }

    private Texture2D Screenshot()
    {
        Camera camera = m_camera;


        int resWidth = camera.pixelWidth;
        int resHeight = camera.pixelHeight;

        RenderTexture rt = new RenderTexture(resWidth, resHeight, 32);
        Texture2D screenShot = new Texture2D(resWidth, resHeight, TextureFormat.ARGB32, false);

        camera.targetTexture = rt;
        camera.Render();
        RenderTexture.active = rt;

        screenShot.ReadPixels(new Rect(0, 0, resWidth, resHeight), 0, 0);
        screenShot.Apply();

        camera.targetTexture = null;
        RenderTexture.active = null; // JC: added to avoid errors

        Destroy(rt);

        return screenShot;
    }

    private Texture2D SaveScreenshotToFile(string fileName)
    {
        Texture2D screenShot = Screenshot();
        byte[] bytes = screenShot.EncodeToPNG();
        System.IO.File.WriteAllBytes(fileName, bytes);
        return screenShot;
    }


    /// <summary>
    /// This function is revealed to the inspector to allow easily drawing lines based on some of the values in this class
    /// </summary>
    [Inspect, Group("Line Drawer")]
    public void DrawLine()
    {
        Debug.DrawLine(m_origin, m_origin + m_direction * m_distance, m_color, m_duration, false);
    }

    /// <summary>
    /// Test function that draws a mesh based on the some of the values in this class
    /// </summary>
    [Inspect, Group("Draw Mesh")]
    public void DrawTestMesh()
    {
        Graphics.DrawMesh(m_mesh, m_position, Quaternion.identity, m_material, m_layer, null, 0, null, false, false);
        Debug.Break();
    }

    /// <summary>
    /// Debug function to print out LayerMask as int
    /// </summary>
    [Inspect]
    public void PrintLayerMask()
    {
        string[] layerMaskArray = new string[] { "Floor" }; //{ "Pit", "Floor" }; //{ "ActorHazard", "Hazard"}; //{ "Hazard" };  //{ "Ground", "All" };

        LayerMask tempLayerMask = LayerMask.GetMask(layerMaskArray);

        string layerMasks = "[";

        foreach (string lm in layerMaskArray)
        {
            layerMasks += lm + "(" + LayerMask.NameToLayer(lm) + "), ";
        }

        layerMasks = layerMasks.Substring(0, layerMasks.Length - 2) + "]";

        Debug.Log(layerMasks + ": " + tempLayerMask.value);
    }

    #region Properties
    #endregion
}