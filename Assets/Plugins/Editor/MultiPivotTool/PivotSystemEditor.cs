//**************************************************************************************
// File: PivotSystemEditor.cs
//
// Purpose: This is the Editor for the PivotSystem. It handles all Unity Editor GUI
// interactions with the PivotSystem.
//
// Written By: Eluem
//**************************************************************************************

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

namespace MultiPivotTool
{
    [CustomEditor(typeof(PivotSystem))]
    public class PivotSystemEditor : Editor
    {
        #region Declarations
        private PivotSystem m_pivotSystem; //Conversion of target for ease of use

        //private List<Pivot> m_pivotAddList = new List<Pivot>(); //List of pivot data to be added
        private List<Pivot> m_pivotDelList = new List<Pivot>(); //List of pivot data to be deleted

        private Quaternion m_tempRot;
        private Quaternion m_prevTempRot;

        //private Quaternion m_lockedInPosHandleRot; //Locked in position of the rotation handle when moving it so it doesn't act weird by continually looking at the object while moving
        //private bool m_lockedInPosHandleRotSet; //Inidicates if the lock in value was set for this move

        #region GUI Info
        private Vector3 m_rotateBy; //Stores the rotation value for the tool that lets you rotate the pivot in the inspector



        private bool m_GUIStylesInitialized = false;

        GUIStyle headerStyle;
        GUIStyle buttonStyle;
        GUIStyle foldOutStyle;
        GUIStyle pivotStyle;

        GUIStyle toggleButtonUpStyle;
        GUIStyle toggleButtonDownStyle;

        GUIStyle labelStyle;
        #endregion
        #endregion

        //***********************************************************************
        // Method: OnEnable
        //
        // Purpose: Callled when the editor's target is selected
        //***********************************************************************
        public void OnEnable()
        {
            m_pivotSystem = (PivotSystem)target;

            m_tempRot = Quaternion.identity;
            m_prevTempRot = m_tempRot;

            //m_lockedInPosHandleRot = Quaternion.identity;
            //m_lockedInPosHandleRotSet = false;
        }

        //***********************************************************************
        // Method: OnDisable
        //
        // Purpose: Called when the editor's target is deselected
        //***********************************************************************
        public void OnDisable()
        {
            if (target != null && Selection.activeGameObject != m_pivotSystem.gameObject)
            {
                if (MenuItems.DisabledTool)
                {
                    Tools.current = MenuItems.PrevToolSelected;
                    MenuItems.DisabledTool = false;
                }

                MenuItems.ActivePivot = null;
            }
        }

        //***********************************************************************
        // Method: OnInspectorGUI
        //
        // Purpose: Responds to events that occur when the inspector is in focus
        //***********************************************************************
        public override void OnInspectorGUI()
        {
            //Check if the tool has been changed by user, if so.. turn off pivot editing
            if (Tools.current != Tool.None)
            {
                MenuItems.ActivePivot = null;
                MenuItems.DisabledTool = false;
            }

            InitializeGUIStyles();

            //Bounding box
            EditorGUILayout.BeginVertical("box");

            //List header/add button
            GUILayout.BeginHorizontal(headerStyle);
            GUILayout.Label("Pivot Points");

            AddButton();
            GUILayout.EndHorizontal();

            //Fill list with pivots
            foreach (Pivot pivot in m_pivotSystem.Pivots)
            {
                GUILayout.BeginVertical("box");
                GUILayout.BeginHorizontal(EditorStyles.toolbar);

                PivotFoldOut(pivot);

                DeleteButton(pivot);

                GUILayout.EndHorizontal();

                if (pivot.isExpanded)
                {
                    OnPivotContentGUI(pivot);
                }

                GUILayout.EndVertical();
            }

            EditorGUILayout.EndVertical();

            //Apply the deleted pivot(s)
            foreach (Pivot pivot in m_pivotDelList)
            {
                m_pivotSystem.RemovePivot(pivot);

                if (pivot == MenuItems.ActivePivot)
                {
                    MenuItems.ActivePivot = null;
                }
            }
            m_pivotDelList.Clear();
        }

        //***********************************************************************
        // Method: OnSceneGUI
        //
        // Purpose: Responds to events that occur while the scene is in focus
        //***********************************************************************
        private void OnSceneGUI()
        {
            if (MenuItems.ActivePivot != null && (MenuItems.ActivePivot.SimulationMode != PivotSimulationMode.Track || MenuItems.ActivePivot.TrackTarget != null))
            {
                Vector3 tempPos;
                Quaternion tempPosHandleRot;

                /*
                float quaterionMagnitude = MenuItems.ActivePivot.Rotation.w * MenuItems.ActivePivot.Rotation.w + MenuItems.ActivePivot.Rotation.x * MenuItems.ActivePivot.Rotation.x + MenuItems.ActivePivot.Rotation.y * MenuItems.ActivePivot.Rotation.y + MenuItems.ActivePivot.Rotation.z * MenuItems.ActivePivot.Rotation.z;

                if (quaterionMagnitude != 1.000009)
                {
                    Debug.Log(quaterionMagnitude);
                }
                */

                EditorGUI.BeginChangeCheck();
                //TO DO: Get position handle to allow raycast positioning
                //TO DO: mess with position (and rotation) handle sizes?


                /*
                if (MenuItems.ActivePivot.posHandleFollowTarget)
                {
                    if (GUIUtility.hotControl == 0) // || (GUI.GetNameOfFocusedControl() != "xAxis" && GUI.GetNameOfFocusedControl() != "yAxis" && GUI.GetNameOfFocusedControl() != "zAxis")) //I don't technically need to check for this since these controls only show when no other control would normally be able to show. Only extreme edge cases would require me to handle it that way
                    {
                        tempPosHandleRot = GetLookAtRotation();

                        m_lockedInPosHandleRotSet = false;
                    }
                    else
                    {
                        if(!m_lockedInPosHandleRotSet)
                        {
                            m_lockedInPosHandleRot = GetLookAtRotation();
                            m_lockedInPosHandleRotSet = true;
                        }
                        tempPosHandleRot = m_lockedInPosHandleRot;
                    }
                }
                else
                {
                    //Leave position handle unrotated
                    tempPosHandleRot = Quaternion.identity;

                    m_lockedInPosHandleRotSet = false;
                }
                */

                //If no control is currently being held, reset the rotation
                if (GUIUtility.hotControl == 0)
                {
                    m_tempRot = GetLookAtRotation();
                }

                if (MenuItems.ActivePivot.posHandleFollowTarget)
                {
                    tempPosHandleRot = m_tempRot;
                }
                else
                {
                    tempPosHandleRot = Quaternion.identity;
                }

                //tempPos = PositionHandle(MenuItems.ActivePivot.Position, tempPosHandleRot, "PivotPositionHandle");
                tempPos = Handles.PositionHandle(MenuItems.ActivePivot.Position, tempPosHandleRot);
                HandlePosition(MenuItems.ActivePivot, tempPos);

                EditorGUI.BeginChangeCheck();
                //tempRot = Handles.RotationHandle(MenuItems.ActivePivot.Rotation, MenuItems.ActivePivot.Position);
                m_tempRot = Handles.RotationHandle(m_tempRot, MenuItems.ActivePivot.Position);
                HandleRotation(MenuItems.ActivePivot, m_tempRot * Quaternion.Inverse(m_prevTempRot));

                m_prevTempRot = m_tempRot;
            }
        }

        //***********************************************************************
        // Method: InitializeGUIStyles
        //
        // Purpose: Initializes all the GUI styles
        //***********************************************************************
        public void InitializeGUIStyles()
        {
            if (m_GUIStylesInitialized)
            {
                return;
            }

            headerStyle = new GUIStyle(EditorStyles.toolbar);

            buttonStyle = new GUIStyle(EditorStyles.toolbarButton);
            buttonStyle.fixedWidth = 20;

            foldOutStyle = new GUIStyle(EditorStyles.foldout);
            foldOutStyle.margin.left = 20;

            pivotStyle = new GUIStyle();
            pivotStyle.margin.left = 20;

            toggleButtonUpStyle = new GUIStyle(EditorStyles.miniButton);
            toggleButtonUpStyle.fixedWidth = 40;
            toggleButtonUpStyle.fixedHeight = 25;
            toggleButtonUpStyle.alignment = TextAnchor.MiddleCenter;

            toggleButtonDownStyle = new GUIStyle(toggleButtonUpStyle);
            toggleButtonDownStyle.normal.background = toggleButtonDownStyle.active.background;

            labelStyle = new GUIStyle(EditorStyles.label);
            labelStyle.padding.top = 5;

            m_GUIStylesInitialized = true;
        }


        //***********************************************************************
        // Method: PivotFoldOut
        //
        // Purpose: Handles displaying and responding to the GUI for the
        // Pivot foldout
        //***********************************************************************
        private void PivotFoldOut(Pivot pPivot)
        {
            bool tempPrevIsExpanded = pPivot.isExpanded;
            pPivot.isExpanded = EditorGUILayout.Foldout(pPivot.isExpanded, pPivot.Name, foldOutStyle);

            if (pPivot.isExpanded && !tempPrevIsExpanded)
            {
                if (!MenuItems.DisabledTool)
                {
                    MenuItems.PrevToolSelected = Tools.current;
                    Tools.current = Tool.None;
                    MenuItems.DisabledTool = true;
                }

                if (MenuItems.ActivePivot != pPivot && MenuItems.ActivePivot == null)
                {
                    MenuItems.ActivePivot = pPivot;
                    SceneView.RepaintAll();
                }
            }
            else if (!pPivot.isExpanded && pPivot == MenuItems.ActivePivot)
            {
                if (MenuItems.DisabledTool)
                {
                    Tools.current = MenuItems.PrevToolSelected;
                    MenuItems.DisabledTool = false;
                }

                if (MenuItems.ActivePivot == pPivot)
                {
                    MenuItems.ActivePivot = null;
                    SceneView.RepaintAll();
                }
            }
        }

        //***********************************************************************
        // Method: AddButton
        //
        // Purpose: Adds a new pivot element.
        // Fires when the "+" button is pressed.
        //***********************************************************************
        public void AddButton()
        {
            EditorGUI.BeginChangeCheck();
            if (GUILayout.Button("+", buttonStyle))
            {
                if (EditorGUI.EndChangeCheck())
                {
                    Undo.RecordObject(target, "Add Pivot");
                    //Undo.FlushUndoRecordObjects(); //This is to prevent registering multiple commands as one
                    //TO DO: Look into fixing the issue where if you hit the button too fast, it groups them into one undo

                    m_pivotSystem.AddPivot(m_pivotSystem.GeneratePivot());

                    if (GUI.changed)
                    {
                        EditorUtility.SetDirty(target);
                    }
                }
            }
        }

        //***********************************************************************
        // Method: DeleteButton
        //
        // Purpose: Removes the corresponding pivot element.
        // Fires when the "-" button is pressed.
        //***********************************************************************
        public void DeleteButton(Pivot pPivot)
        {
            EditorGUI.BeginChangeCheck();
            if (GUILayout.Button("-", buttonStyle))
            {
                Undo.RecordObject(target, "Delete Pivot");
                //Undo.FlushUndoRecordObjects(); //This is to prevent registering multiple commands as one
                //TO DO: Look into fixing the issue where if you hit the button too fast, it groups them into one undo

                m_pivotDelList.Add(pPivot);

                if (GUI.changed)
                {
                    EditorUtility.SetDirty(target);
                }
            }
        }

        //***********************************************************************
        // Method: PivotButton
        //
        // Purpose: Enable/disable editing the passed pivot
        //***********************************************************************
        private void PivotButton(Pivot pPivot)
        {
            bool toggledOn = (pPivot == MenuItems.ActivePivot);

            Color tempColor = GUI.color;

            GUILayout.BeginHorizontal();

            GUILayout.FlexibleSpace();

            //TO DO: Give the "Edit Pivot" toggle button a better graphical look and have it line up with the other controls like the Edit Collider button
            //GUILayout.BeginHorizontal(GUILayout.MaxWidth(EditorGUIUtility.currentViewWidth * 0.52f));
            GUILayout.BeginHorizontal();

            GUIStyle tempButtonStyle;
            if (toggledOn)
            {
                tempButtonStyle = toggleButtonDownStyle;
                GUI.color = new Color(0.7f, 0.7f, 0.7f);
            }
            else
            {
                tempButtonStyle = toggleButtonUpStyle;
            }

            if (GUILayout.Button("Pivot", tempButtonStyle))
            {
                if (toggledOn)
                {
                    MenuItems.ActivePivot = null;
                    toggledOn = false;
                }
                else
                {
                    MenuItems.ActivePivot = pPivot;
                    toggledOn = true;
                }


                if (toggledOn && !MenuItems.DisabledTool)
                {
                    MenuItems.PrevToolSelected = Tools.current;
                    Tools.current = Tool.None;
                    MenuItems.DisabledTool = true;
                }

                if (!toggledOn && MenuItems.DisabledTool)
                {
                    Tools.current = MenuItems.PrevToolSelected;
                    MenuItems.DisabledTool = false;
                }

                SceneView.RepaintAll();
            }

            GUI.color = tempColor;

            GUILayout.Label("Edit Pivot", labelStyle);

            GUILayout.EndHorizontal();

            GUILayout.EndHorizontal();
        }

        //***********************************************************************
        // Method: OnPivotContentGUI
        //
        // Purpose: Draws and handles all the content for the currently expanded
        // pivot
        //***********************************************************************
        public void OnPivotContentGUI(Pivot pPivot)
        {
            //Fields used for processing modifications
            Vector3 tempPos;
            string tempName;
            PivotSimulationMode tempSimulationMode;

            GUILayout.BeginVertical(pivotStyle);

            PivotButton(pPivot);

            EditorGUI.BeginChangeCheck();
            tempName = EditorGUILayout.TextField("Name", pPivot.Name);
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(target, "Rename Pivot");

                pPivot.Name = tempName;

                if (GUI.changed)
                {
                    EditorUtility.SetDirty(target);
                }
            }
            if (pPivot.SimulationMode == PivotSimulationMode.Track && pPivot.TrackTarget == null)
            {
                EditorGUI.BeginDisabledGroup(true);
                EditorGUI.BeginChangeCheck();
                EditorGUILayout.Vector3Field("Position", Vector3.zero);
                EditorGUI.EndDisabledGroup();
            }
            else
            {
                EditorGUI.BeginChangeCheck();
                tempPos = EditorGUILayout.Vector3Field("Position", pPivot.GetSimModePosition());
                if (pPivot.SimulationMode == PivotSimulationMode.Child)
                {
                    tempPos = pPivot.ConvertToWorld(tempPos);
                }
                else if (pPivot.SimulationMode == PivotSimulationMode.Track)
                {
                    tempPos = pPivot.TrackTarget.TransformPoint(tempPos);
                }
                HandlePosition(pPivot, tempPos);
            }

            EditorGUI.BeginChangeCheck();
            string tempModeTip = "\n\n";
            switch (pPivot.SimulationMode)
            {
                case PivotSimulationMode.World:
                    tempModeTip += "World: Pivot sits in world space";
                    break;

                case PivotSimulationMode.Child:
                    tempModeTip += "Child: Pivot's position is treated as local to the object";
                    break;

                case PivotSimulationMode.Track:
                    tempModeTip += "Track: Pivot's position is treated as local to the chosen TrackTarget";
                    break;
            }
            tempSimulationMode = (PivotSimulationMode)EditorGUILayout.EnumPopup(new GUIContent("Simulation Mode", "Controls how the pivot points position acts with respect to the object" + tempModeTip), pPivot.SimulationMode);
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(target, "Pivot Simulation Mode Change");

                pPivot.SimulationMode = tempSimulationMode;

                if (GUI.changed)
                {
                    EditorUtility.SetDirty(target);
                }
            }

            if (pPivot.SimulationMode == PivotSimulationMode.Track)
            {
                Transform tempTrackTarget;

                EditorGUI.BeginChangeCheck();
                tempTrackTarget = (Transform)EditorGUILayout.ObjectField("Track Target", pPivot.TrackTarget, typeof(Transform), true);
                if (EditorGUI.EndChangeCheck())
                {
                    Undo.RecordObject(target, "Pivot Track Target Change");

                    pPivot.SetTrackTarget(tempTrackTarget);

                    if (GUI.changed)
                    {
                        EditorUtility.SetDirty(target);
                    }
                }
            }

            EditorGUI.BeginChangeCheck();
            bool tempPosHandleFollowTarget;
            tempPosHandleFollowTarget = EditorGUILayout.Toggle(new GUIContent("Follow Object", "If enabled, the position handle will 'look at' the object"), pPivot.posHandleFollowTarget);
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(target, "Pivot Follow Object Change");

                pPivot.posHandleFollowTarget = tempPosHandleFollowTarget;

                if (GUI.changed)
                {
                    EditorUtility.SetDirty(target);
                }
            }

            EditorGUILayout.Space();
            EditorGUILayout.Space();
            GUILayout.BeginHorizontal();
            m_rotateBy = EditorGUILayout.Vector3Field("Rotate By", m_rotateBy);

            EditorGUI.BeginChangeCheck();
            if (GUILayout.Button("Rotate"))
            {
                HandleRotation(pPivot, Quaternion.Euler(m_rotateBy));
            }
            GUILayout.EndHorizontal();

            GUILayout.EndVertical();
        }

        //***********************************************************************
        // Method: HandlePosition
        //
        // Purpose: Handles changes in the position of the pivot
        //***********************************************************************
        private void HandlePosition(Pivot pPivot, Vector3 pPos)
        {
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(target, "Move Pivot");
                pPivot.SetPosition(pPos);

                if (GUI.changed)
                {
                    EditorUtility.SetDirty(target);
                }
            }
        }

        //***********************************************************************
        // Method: HandleRotation
        //
        // Purpose: Handles changes in the rotation of the pivot
        //***********************************************************************
        private void HandleRotation(Pivot pPivot, Quaternion pRot)
        {
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObjects(new UnityEngine.Object[] { target, m_pivotSystem.transform }, "Rotate Pivot");

                pPivot.Rotate(pRot);

                if (GUI.changed)
                {
                    EditorUtility.SetDirty(target);
                }
            }
        }

        //************************************************************************************************************
        // Method: isSceneView2D
        //
        // Purpose: Checks if the passed camera's sceneView is 2d
        //
        // Code from: http://answers.unity3d.com/questions/736515/detecting-if-the-2d-mode-button-was-pressed.html
        //************************************************************************************************************
        private bool isSceneView2D(Camera cam)
        {
            for (int i = 0, n = UnityEditor.SceneView.sceneViews.Count; i < n; ++i)
            {
                UnityEditor.SceneView sv = UnityEditor.SceneView.sceneViews[i] as UnityEditor.SceneView;
                if (sv.camera == cam)
                {
                    return sv.in2DMode;
                }
            }
            return false;
        }

        //************************************************************************************************************
        // Method: GetLookAtRotation
        //
        // Purpose: Gets the rotation that will cause the position handle to look at the target object
        //************************************************************************************************************
        private Quaternion GetLookAtRotation()
        {
            if (isSceneView2D(Camera.current))
            {
                //Point the position handle's x a axis at the target and normalize z-axis (for 2d)
                return Quaternion.FromToRotation(Vector3.right, (new Vector3(MenuItems.ActivePivot.Target.position.x, MenuItems.ActivePivot.Target.position.y, MenuItems.ActivePivot.Position.z) - MenuItems.ActivePivot.Position));

                //return Quaternion.FromToRotation(Vector3.right, (MenuItems.ActivePivot.Target.position - MenuItems.ActivePivot.Position));
            }
            else
            {
                //Point the position handle's z axis at the target (for 3d)
                return Quaternion.FromToRotation(Vector3.forward, (MenuItems.ActivePivot.Target.position - MenuItems.ActivePivot.Position));
            }
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
        private Vector3 PositionHandle(Vector3 pPosition, Quaternion pRotation, string pName)
        {
            float moveSnapX = EditorPrefs.GetFloat("MoveSnapX");
            float moveSnapY = EditorPrefs.GetFloat("MoveSnapY");
            float moveSnapZ = EditorPrefs.GetFloat("MoveSnapZ");
            Vector3 moveSnap = new Vector3(moveSnapX, moveSnapY, moveSnapZ);

            float handleSize = HandleUtility.GetHandleSize(pPosition);
            Color color = Handles.color;

            Handles.color = Handles.xAxisColor;
            GUI.SetNextControlName(pName + "." + "xAxis");
            pPosition = Handles.Slider(pPosition, pRotation * Vector3.right, handleSize, new Handles.DrawCapFunction(Handles.ArrowCap), moveSnapX);

            Handles.color = Handles.yAxisColor;
            GUI.SetNextControlName(pName + "." + "yAxis");
            pPosition = Handles.Slider(pPosition, pRotation * Vector3.up, handleSize, new Handles.DrawCapFunction(Handles.ArrowCap), moveSnapY);

            Handles.color = Handles.zAxisColor;
            GUI.SetNextControlName(pName + "." + "zAxis");
            pPosition = Handles.Slider(pPosition, pRotation * Vector3.forward, handleSize, new Handles.DrawCapFunction(Handles.ArrowCap), moveSnapZ);


            if (Event.current.shift)
            {
                Handles.color = Handles.centerColor;
                GUI.SetNextControlName(pName + "." + "FreeMoveAxis");
                pPosition = Handles.FreeMoveHandle(pPosition, pRotation, handleSize * 0.15f, moveSnap, new Handles.DrawCapFunction(Handles.RectangleCap));
            }
            else
            {
                Vector3 tempPos;
                Handles.color = Handles.yAxisColor;
                GUI.SetNextControlName(pName + "." + "xzPlane");
                tempPos = Handles.FreeMoveHandle(pPosition, pRotation, handleSize * 0.3f, moveSnap, new Handles.DrawCapFunction(xzPlaneCap));
                pPosition = new Vector3(tempPos.x, pPosition.y, tempPos.z);
            }

            Handles.color = color;
            return pPosition;
        }

        private void xzPlaneCap(int pControlID, Vector3 pPosition, Quaternion pRotation, float pSize)
        {
            Vector3[] verts = new Vector3[4];

            verts[0] = pPosition;
            verts[1] = (pPosition + (pRotation * new Vector3(1, 0, 0) * pSize));
            verts[2] = (pPosition + (pRotation * new Vector3(1, 0, 1) * pSize));
            verts[3] = (pPosition + (pRotation * new Vector3(0, 0, 1) * pSize));

            Color faceColor = Handles.color;
            faceColor.a = .3f;

            Color outlineColor = Handles.color;

            Handles.DrawSolidRectangleWithOutline(verts, faceColor, outlineColor);
            //Handles.RectangleCap(controlID, position, rotation, size);
        }
        */

        #region Properties
        #endregion
    }
}