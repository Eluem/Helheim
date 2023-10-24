//**************************************************************************************
// File: PivotSystem.cs
//
// Purpose: This defines the PivotSystem which will store and handle all manipulation
// methods to intract with a list of custom Pivots for the object this is attached to
//
// Written By: Eluem
//**************************************************************************************

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
//using UnityEditor: TO DO: Remove me from asset store version

namespace MultiPivotTool
{
    public enum PivotSimulationMode
    {
        /*
        /// <summary>
        /// Pivot and target can be moved and rotated completely separately
        /// </summary>
        Detached = -1,
        */

        /// <summary>
        /// Pivot will sit in world space. Rotating it will apply the change in rotation to the GameObject
        /// </summary>
        World = 0,

        /// <summary>
        /// Pivot's positioning will act as a child
        /// </summary>
        Child = 1,

        /// <summary>
        /// Pivot's position will be treated as if it is the child of the passed target, instead of the GameObject this is part of
        /// </summary>
        Track = 2

        /*
       /// <summary>
       /// Pivot will act as if it is the parent of the GameObject and the Child of the GameObject's Parent.
       /// Moving the GameObject in the editor will change it's LocalPosition in the Pivot.
       /// If you want to move it Locally in code, you need to use the Pivot.ChangeObjectLocally method
       /// </summary>
       Parent = 2
       */

        /*
        /// <summary>
        /// Pivot and GameObject will act as if they are both parents of each other. Moving/rotating one will affect the other.
        /// </summary>
        Attached = 3
        */
    }

    public class PivotSystem : MonoBehaviour
    {
        #region Declarations
        [SerializeField]
        private List<Pivot> m_pivots = new List<Pivot>();
        #endregion

        //***********************************************************************
        // Method: GeneratePivot
        //
        // Purpose: Generates a new pivot with an auto-generated name in the
        // simple format of "Pivot #" where # is the number of current pivots+1
        // and returns it
        //***********************************************************************
        /// <summary>
        /// Generate a new pivot
        /// </summary>
        /// <returns>Returns a new Pivot with an automatically generated name</returns>
        public Pivot GeneratePivot()
        {
            Pivot tempPivot = new Pivot(transform, "Pivot " + (m_pivots.Count + 1));
            return tempPivot;
        }

        //***********************************************************************
        // Method: AddPivot
        //
        // Purpose: Simply adds a new pivot to the m_pivots list
        //***********************************************************************
        /// <summary>
        /// Adds the passed Pivot to the system
        /// </summary>
        /// <param name="pPivot">Pivot to be added</param>
        public void AddPivot(Pivot pPivot)
        {
            m_pivots.Add(pPivot);
        }

        //***********************************************************************
        // Method: RemovePivot
        //
        // Purpose: Simply removes the passed pivot from the m_pivots list
        //***********************************************************************
        /// <summary>
        /// Removes the passed Pivot from the system
        /// </summary>
        /// <param name="pPivot">Pivot to be removed</param>
        public void RemovePivot(Pivot pPivot)
        {
            m_pivots.Remove(pPivot);
        }

        #region Properties
        /// <summary>
        /// List of all the Pivots in the system
        /// </summary>
        public List<Pivot> Pivots
        {
            get
            {
                return m_pivots;
            }
        }
        #endregion
    }

    //***********************************************************************
    // Class: Pivot
    //
    // Purpose: Stores data about an individual pivot
    //***********************************************************************
    [System.Serializable]
    public class Pivot
    {
        #region Declarations
        [SerializeField]
        private Transform m_target;
        [SerializeField]
        private string m_name;

        [SerializeField]
        private Vector3 m_position;

        [SerializeField]
        private PivotSimulationMode m_simulationMode;

        [SerializeField]
        private Transform m_trackTarget; //Indicates the target for the "Track" SimulationMode


#if UNITY_EDITOR
        /// <summary>
        /// Avoid interacting with this. This field is used by the PivotSystemEditor to allow keeping track of whether or not a Pivot's foldout has been expanded.
        /// </summary>
        public bool isExpanded; //Editor data that allows me to tell if a particular pivot has been expanded

        /// <summary>
        /// Avoid interacting with this. This field is used by the PivotSystemEditor to allow keeping track of whether or not a Pivot's position handle is supposed to follow it's target
        /// </summary>
        public bool posHandleFollowTarget = false; //Editor data that allows me to tell if a particular pivot is to have the position handle look at the object
#endif
        #endregion

        //***********************************************************************
        // Method: Pivot
        //
        // Purpose: Constructor for class
        //***********************************************************************
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pTarget">Transform to pivot</param>
        /// <param name="pName">Name of the Pivot</param>
        public Pivot(Transform pTarget, string pName)
        {
            m_target = pTarget;
            m_name = pName;

            m_position = m_target.position;

            m_simulationMode = PivotSimulationMode.World;
        }

        //***********************************************************************
        // Method: GetSimModePosition
        //
        // Purpose: Returns the world/local position depending on the
        // Simulation Mode
        //***********************************************************************
        /// <summary>
        /// Returns the world/local position depending on the Simulation Mode
        /// </summary>
        /// <returns></returns>
        public Vector3 GetSimModePosition()
        {
            switch (m_simulationMode)
            {
                case PivotSimulationMode.World:
                    return Position;
                case PivotSimulationMode.Child:
                    return LocalPosition;
                case PivotSimulationMode.Track:
                    return LocalPosition;
                default:
                    return Position;
            }
        }

        /*
        //***********************************************************************
        // Method: GetSimModeRotation
        //
        // Purpose: Returns the world/local rotation depending on the
        // Simulation Mode
        //***********************************************************************
        public Quaternion GetSimModeRotation()
        {
            return Rotation;
        }
        */

        //***********************************************************************
        // Method: Rotate
        //
        // Purpose: Rotates the object around the pivot by the passed value
        //***********************************************************************
        /// <summary>
        /// Rotates the object around the pivot by the passed value
        /// </summary>
        /// <param name="pRot"></param>
        public void Rotate(Quaternion pRot)
        {
            /*
            Quaternion tempPrevRot;
            tempPrevRot = Rotation;

            Quaternion differenceFromAtoB = pRot * Quaternion.Inverse(tempPrevRot);
            */

            Quaternion differenceFromAtoB = pRot;

            Vector3 tempAxis;
            float tempAngle;
            differenceFromAtoB.ToAngleAxis(out tempAngle, out tempAxis);

            m_target.RotateAround(Position, tempAxis, tempAngle);
        }

        //***********************************************************************
        // Method: SetPosition
        //
        // Purpose: Sets the position as if it were passed a value in world space
        //***********************************************************************
        /// <summary>
        /// Sets the world position to the passed position
        /// </summary>
        /// <param name="pPos"></param>
        public void SetPosition(Vector3 pPos)
        {
            switch (m_simulationMode)
            {
                case PivotSimulationMode.World:
                    m_position = pPos;
                    break;
                case PivotSimulationMode.Child:
                    m_position = ConvertToLocal(pPos);
                    break;
                case PivotSimulationMode.Track:
                    if (m_trackTarget == null)
                    {
                        m_position = Vector3.zero;
                    }
                    else
                    {
                        m_position = m_trackTarget.InverseTransformPoint(pPos);
                    }
                    break;
            }
        }

        //***********************************************************************
        // Method: SetLocalPosition
        //
        // Purpose: Sets the position as if it were passed a value in space
        // local to the target
        //***********************************************************************
        /// <summary>
        /// Sets the position as if the passed position were in local coordinates relative to the target transform
        /// </summary>
        /// <param name="pPos"></param>
        public void SetLocalPosition(Vector3 pPos)
        {
            switch (m_simulationMode)
            {
                case PivotSimulationMode.World:
                    m_position = m_target.TransformPoint(pPos);
                    break;
                case PivotSimulationMode.Child:
                    m_position = pPos;
                    break;
            }
        }

        //***********************************************************************
        // Method: ConvertToWorld
        //
        // Purpose: Converts the passed position to world position
        //***********************************************************************
        /// <summary>
        /// Convert passed position to world position
        /// </summary>
        /// <param name="pPos"></param>
        /// <returns></returns>
        public Vector3 ConvertToWorld(Vector3 pPos)
        {
            return Target.TransformPoint(pPos);
        }

        //***********************************************************************
        // Method: ConvertToWorld
        //
        // Purpose: Converts the passed rotation to world rotation
        //***********************************************************************
        /// <summary>
        /// Convert passed rotation to world rotation
        /// </summary>
        /// <param name="pRot"></param>
        /// <returns></returns>
        public Quaternion ConvertToWorld(Quaternion pRot)
        {
            return pRot * Target.rotation;
        }

        //***********************************************************************
        // Method: ConvertToLocal
        //
        // Purpose: Converts the passed position to local position
        //***********************************************************************
        /// <summary>
        /// Converts the passed position to local position
        /// </summary>
        /// <param name="pPos"></param>
        /// <returns></returns>
        public Vector3 ConvertToLocal(Vector3 pPos)
        {
            return Target.InverseTransformPoint(pPos);
        }

        //***********************************************************************
        // Method: ConvertToLocal
        //
        // Purpose: Converts the passed rotation to local rotation
        //***********************************************************************
        /// <summary>
        /// Converts the passed rotation to local rotation
        /// </summary>
        /// <param name="pRot"></param>
        /// <returns></returns>
        public Quaternion ConvertToLocal(Quaternion pRot)
        {
            return pRot * Quaternion.Inverse(m_target.rotation);
        }


        //***********************************************************************
        // Method: SetTrackTarget
        //
        // Purpose: Sets the TrackTarget to the passed target and configures
        // m_position accordingly
        //***********************************************************************
        /// <summary>
        /// Sets the TrackTarget for the Track simulation mode
        /// </summary>
        /// <param name="pTrackTarget"></param>
        public void SetTrackTarget(Transform pTrackTarget)
        {
            m_trackTarget = pTrackTarget;

            m_position = Vector3.zero;
        }

        #region Properties
        /// <summary>
        /// Transform of the object being pivoted
        /// </summary>
        public Transform Target
        {
            get
            {
                return m_target;
            }
        }

        /// <summary>
        /// Name of this particular Pivot
        /// </summary>
        public string Name
        {
            get
            {
                return m_name;
            }
            set
            {
                m_name = value;
            }
        }

        /// <summary>
        /// World position
        /// </summary>
        public Vector3 Position
        {
            get
            {
                switch (m_simulationMode)
                {
                    case PivotSimulationMode.World:
                        return m_position;
                    case PivotSimulationMode.Child:
                        return ConvertToWorld(m_position);
                    case PivotSimulationMode.Track:
                        if (m_trackTarget == null)
                        {
                            return Vector3.zero;
                        }
                        return m_trackTarget.TransformPoint(m_position);
                    default:
                        return m_position;
                }
            }
        }

        /// <summary>
        /// Local position
        /// </summary>
        public Vector3 LocalPosition
        {
            get
            {
                switch (m_simulationMode)
                {
                    case PivotSimulationMode.World:
                        return ConvertToLocal(m_position);
                    case PivotSimulationMode.Child:
                        return m_position;
                    case PivotSimulationMode.Track:
                        return m_position;
                    default:
                        return m_position;
                }
            }
        }

        /*
        /// <summary>
        /// World rotation
        /// </summary>
        public Quaternion Rotation
        {
            get
            {
                //return Quaternion.Euler(Vector3.RotateTowards(Vector3.forward, Target.position, 180, 0.0F));
                //return Quaternion.Euler(Vector3.RotateTowards(Vector3.right, Target.position, 180, 0.0F));
                //return Quaternion.Euler((Target.position - Position).normalized);
            }
        }
        */

        /// <summary>
        /// Indicates whether this Pivot will be treated as if it is a child of the parent transform or if it will act as if it exists as some external object in world space
        /// </summary>
        public PivotSimulationMode SimulationMode
        {
            get
            {
                return m_simulationMode;
            }
            set
            {
                //Convert the position and rotation to the new mode
                if (m_simulationMode == PivotSimulationMode.World)
                {
                    if (value == PivotSimulationMode.Child)
                    {
                        //Convert from World to Child
                        m_position = LocalPosition;
                    }
                    else if(value == PivotSimulationMode.Track)
                    {
                        m_position = Vector3.zero;
                    }
                }
                else if (m_simulationMode == PivotSimulationMode.Child)
                {
                    if (value == PivotSimulationMode.World)
                    {
                        //Convert from Child to World
                        m_position = Position;
                    }
                    else if (value == PivotSimulationMode.Track)
                    {
                        m_position = Vector3.zero;
                    }
                }
                else if (m_simulationMode == PivotSimulationMode.Track)
                {
                    if(value == PivotSimulationMode.Child)
                    {
                        m_trackTarget = null;
                        m_position = Vector3.zero;
                    }
                    else if (value == PivotSimulationMode.World)
                    {
                        m_trackTarget = null;
                        m_position = ConvertToWorld(Vector3.zero);
                    }
                }

                m_simulationMode = value;
            }
        }

        /// <summary>
        /// Returns the target for the track SimulationMode
        /// </summary>
        public Transform TrackTarget
        {
            get
            {
                return m_trackTarget;
            }
        }
        #endregion
    }
}