//*****************************************************************************************
// File: MenuItems.cs
//
// Purpose: This defines all the MultiPivotTool menu items and other static feilds
//
// Written By: Eluem
//*****************************************************************************************

#if UNITY_EDITOR
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using MultiPivotTool;

namespace MultiPivotTool
{
    public static class MenuItems
    {
        #region Declarations
        public static Tool PrevToolSelected; //Stores the last tool the user had selected (before it was disabled)
        public static bool DisabledTool = false; //Stores the fact that this tool set the currently selected built in tool to none

        public static Pivot ActivePivot = null; //Stores the currently active pivot
        #endregion

        //***********************************************************************
        // Method: PivotToolMenuItem
        //
        // Purpose: Defines the button combination that the user presses to
        // attempt to active the Advanced Rotation Tool
        //***********************************************************************
        [MenuItem("Edit/Advanced Rotation Tool/Pivot Tool %E")]
        private static void PivotToolMenuItem()
        {
            InitiatePivotSystem();
        }

        //***********************************************************************
        // Method: InitiatePivotSystem
        //
        // Purpose: Takes all actions necessary to make sure that a pivot is
        // selected for the current game object. If one is selected already,
        // cycles to the next one.
        //***********************************************************************
        private static void InitiatePivotSystem()
        {
            //Get selected gameObject
            GameObject activeGameObject = Selection.activeGameObject;

            //If no gameObject is selected, cancel this action
            if (activeGameObject == null)
            {
                return;
            }

            //Get the pivotSystem attached to this gameOject
            PivotSystem tempPivotSystem = activeGameObject.GetComponent<PivotSystem>();

            //If no pivotSystem is attached, attach one
            if (tempPivotSystem == null)
            {
                tempPivotSystem = GeneratePivotSystem(activeGameObject);
            }

            //Tell the PivotSystem to cycle to the next Pivot
            PivotCycle(tempPivotSystem);
        }

        //***********************************************************************
        // Method: GeneratePivotSystem
        //
        // Purpose: Creates a pivot system and attaches it to the
        // passed gameObject, then returns it
        //***********************************************************************
        private static PivotSystem GeneratePivotSystem(GameObject pGameObject)
        {
            PivotSystem tempPivotSystem = pGameObject.AddComponent<PivotSystem>();
            //tempPivotSystem.Initialize();

            return tempPivotSystem;
        }

        //***********************************************************************
        // Method: PivotCycle
        //
        // Purpose: Editor method that will cause the system to cycle between
        // pivots
        //***********************************************************************
        private static void PivotCycle(PivotSystem pPivotSystem)
        {
            //Deactive the currently selected tool
            if (!DisabledTool)
            {
                PrevToolSelected = Tools.current;
                Tools.current = Tool.None;
                DisabledTool = true;
            }

            //If there is a currently active pivot that doesn't belong to the this system, set it to null and find a one from this system
            if (ActivePivot != null && ActivePivot.Target != pPivotSystem.transform)
            {
                ActivePivot = null;
            }

            //If no pivot is selected, select the first one
            if (ActivePivot == null)
            {
                //If no pivot exists, generate a fresh one
                if (pPivotSystem.Pivots.Count < 1)
                {
                    pPivotSystem.AddPivot(pPivotSystem.GeneratePivot());
                }

                ActivePivot = pPivotSystem.Pivots[0];
            }

            else
            {
                //If a pivot is selected, find where it is in the list of pivots, and move to the next one
                for (int i = 0; i < pPivotSystem.Pivots.Count; i++)
                {
                    if (pPivotSystem.Pivots[i] == ActivePivot)
                    {
                        if (i < pPivotSystem.Pivots.Count - 1)
                        {
                            ActivePivot = pPivotSystem.Pivots[i + 1]; //Move to the next pivot
                        }
                        else
                        {
                            ActivePivot = pPivotSystem.Pivots[0]; //Jump back to the first pivot
                        }
                        break;
                    }
                }
            }

            ActivePivot.isExpanded = true; //Force the newly selected pivot to be expanded

            EditorUtility.SetDirty(pPivotSystem); //Force the inspector to refresh
            SceneView.RepaintAll(); //Force the scene to refresh
        }


        /*
        //**************************************************************************************
        // Method: PositionHandle
        //
        // Purpose: Manually defined position handle
        //
        // Code obtained from this post:
        // http://answers.unity3d.com/questions/19354/handlesmatrix-seems-strange.html
        //**************************************************************************************
        public static Vector3 PositionHandle(Vector3 position, Quaternion rotation)
        {
            float moveSnapX = EditorPrefs.GetFloat("MoveSnapX");
            float moveSnapY = EditorPrefs.GetFloat("MoveSnapY");
            float moveSnapZ = EditorPrefs.GetFloat("MoveSnapZ");
            Vector3 moveSnap = new Vector3(moveSnapX, moveSnapY, moveSnapZ);

            float handleSize = HandleUtility.GetHandleSize(position);
            Color color = Handles.color;
            Handles.color = Handles.xAxisColor;
            position = Handles.Slider(position, rotation * Vector3.right, handleSize, new Handles.DrawCapFunction(Handles.ArrowCap), moveSnapX);
            Handles.color = Handles.yAxisColor;
            position = Handles.Slider(position, rotation * Vector3.up, handleSize, new Handles.DrawCapFunction(Handles.ArrowCap), moveSnapY);
            Handles.color = Handles.zAxisColor;
            position = Handles.Slider(position, rotation * Vector3.forward, handleSize, new Handles.DrawCapFunction(Handles.ArrowCap), moveSnapZ);
            Handles.color = Handles.centerColor;
            position = Handles.FreeMoveHandle(position, rotation, handleSize * 0.15f, moveSnap, new Handles.DrawCapFunction(Handles.RectangleCap));
            Handles.color = color;
            return position;
        }

        //**************************************************************************************
        // Method: RotationHandle
        //
        // Purpose: Manually defined rotation handle
        //
        // Code obtained from this post:
        // http://answers.unity3d.com/questions/19354/handlesmatrix-seems-strange.html
        //**************************************************************************************
        public static Quaternion RotationHandle(Quaternion rotation, Vector3 position)
        {
            float rotationSnap = EditorPrefs.GetFloat("RotationSnap"); 

            float handleSize = HandleUtility.GetHandleSize(position);
            Color color = Handles.color;
            Handles.color = Handles.xAxisColor;
            rotation = Handles.Disc(rotation, position, rotation * Vector3.right, handleSize, true, rotationSnap);
            Handles.color = Handles.yAxisColor;
            rotation = Handles.Disc(rotation, position, rotation * Vector3.up, handleSize, true, rotationSnap);
            Handles.color = Handles.zAxisColor;
            rotation = Handles.Disc(rotation, position, rotation * Vector3.forward, handleSize, true, rotationSnap);
            Handles.color = Handles.centerColor;
            rotation = Handles.Disc(rotation, position, Camera.current.transform.forward, handleSize * 1.1f, false, 0f);
            rotation = Handles.FreeRotateHandle(rotation, position, handleSize);
            Handles.color = color;
            return rotation;
        }
        */

        #region Properties
        #endregion
    }
}
#endif