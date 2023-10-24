//**************************************************************************************
// File: XWeaponTrail.cs
//
// Purpose: Manages weapon trails (generally for melee attacks)
//
// Written By: shallway (from unity asset store)
//**************************************************************************************

using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

namespace Xft
{
    public class XWeaponTrail : MonoBehaviour
    {
        public class Element
        {
            #region Declarations
            public Vector3 PointStart;
            public Vector3 PointEnd;
            #endregion

            public Element(Vector3 start, Vector3 end)
            {
                PointStart = start;
                PointEnd = end;
            }

            public Element()
            {

            }

            #region Properties
            public Vector3 Pos
            {
                get
                {
                    return (PointStart + PointEnd) / 2f;
                }
            }
            #endregion
        }

        public class ElementPool
        {
            #region Declarations
            private readonly Stack<Element> _stack = new Stack<Element>();

            public int CountAll { get; private set; }
            public int CountActive { get { return CountAll - CountInactive; } }
            public int CountInactive { get { return _stack.Count; } }
            #endregion

            public ElementPool(int preCount)
            {
                for (int i = 0; i < preCount; i++)
                {
                    Element element = new Element();
                    _stack.Push(element);
                    CountAll++;
                }
            }

            public Element Get()
            {
                Element element;

                if (_stack.Count == 0)
                {
                    element = new Element();
                    CountAll++;
                }
                else
                {
                    element = _stack.Pop();
                }

                return element;
            }

            public void Release(Element element)
            {
                if (_stack.Count > 0 && ReferenceEquals(_stack.Peek(), element))
                {
                    Debug.LogError("Internal error. Trying to destroy object that is already released to pool.");
                }
                _stack.Push(element);
            }
        }

        #region Declarations
        #region Public Members
        public static string m_version = "1.1.0";

        public Transform m_pointStart;
        public Transform m_pointEnd;

        public int m_maxFrame = 14;
        public int m_granularity = 60;
        public float m_fps = 60f;

        public Color m_myColor = Color.white;
        public Material m_myMaterial;

        /// <summary>
        /// Toggle this off to escape out of Update and Late Update early. Toggling it back on will cause Activate to be called.
        /// </summary>
        public bool m_enabled = false;

        /// <summary>
        /// This is the length as a percentage (0-1) of the "expected" length. Reduce it during an animation to have the trail "fade out".
        /// </summary>
        public float m_trailLength = 1f;

        /// <summary>
        /// This is how long it will take to automatically fade the trail in the event that the trail wasn't faded out manually
        /// </summary>
        public float m_autoFadeTime = .3f;
        #endregion



        #region Protected Members
        protected float m_trailWidth = 0f;
        protected Element m_headElem = new Element();
        protected List<Element> m_snapshotList = new List<Element>();
        protected ElementPool m_elemPool;
        protected Spline m_spline = new Spline();


        protected bool m_isFading = false;
        protected float m_fadeValue = 1f;
        protected float m_fadeTime = 1f;
        protected float m_fadeElapsedime = 0f;
        protected float m_prevAutoFadeTime = .3f; //Used to be obtain the autofade time that was set in the last animation (if an animation transitions out and resets the value)
        protected float m_prevTrailLength = 1f; //Used to be able to indicate what the trail length was before the animation changed

        protected float m_elapsedTime = 0f;
        //protected GameObject m_meshObj;
        protected VertexPool m_vertexPool;
        protected VertexPool.VertexSegment m_vertexSegment;
        protected bool m_inited = false;
        protected bool m_currEnabled = false;
        #endregion
        #endregion

        #region API
        //you may pre-init the trail to save some performance.
        public void Init()
        {
            if (m_inited)
            {
                return;
            }


            m_elemPool = new ElementPool(m_maxFrame);

            m_trailWidth = (m_pointStart.position - m_pointEnd.position).magnitude;

            InitMeshObj();

            InitOriginalElements();

            InitSpline();

            m_inited = true;
        }

        public void Activate()
        {
            Init();

            gameObject.SetActive(true);

            m_currEnabled = true;

            m_isFading = false;
            m_fadeValue = 1f;
            m_fadeTime = 1f;
            m_fadeElapsedime = 0f;

            m_elapsedTime = 0f;

            //reset all elemts to head pos.
            for (int i = 0; i < m_snapshotList.Count; i++)
            {
                //m_snapshotList[i].PointStart = m_pointStart.position;
                //m_snapshotList[i].PointEnd = m_pointEnd.position;
                m_snapshotList[i].PointStart = PointStartPos;
                m_snapshotList[i].PointEnd = PointEndPos;

                //m_spline.ControlPoints[i].Position = m_snapshotList[i].Pos;
                //m_spline.ControlPoints[i].Normal = m_snapshotList[i].PointEnd - m_snapshotList[i].PointStart;
            }

            Vector3 tempPos = (PointStartPos + PointEndPos) / 2f;
            Vector3 tempNormal = (PointEndPos - PointStartPos);

            for (int i = 0; i < m_spline.ControlPoints.Count; i++)
            {
                m_spline.ControlPoints[i].Position = tempPos;
                m_spline.ControlPoints[i].Normal = tempNormal;
            }

            //reset vertex too.
            RefreshSpline();
            UpdateVertex();
        }

        public void Deactivate()
        {
            m_currEnabled = false;
            //gameObject.SetActive(false);
        }


        public void StopSmoothly(float fadeTime)
        {
            m_isFading = true;
            if(fadeTime == 0)
            {
                fadeTime = 0.000000001f;
            }
            m_fadeTime = fadeTime;
        }
        #endregion

        #region Unity Methods
        void Awake()
        {
            SceneManager.sceneLoaded += OnSceneLoaded; //Register callback to replace old OnLevelWasLoaded
        }

        void Update()
        {
            //Allows for a stop smoothly call to be initiated from an animation fairly easily
            if (!m_enabled && m_currEnabled && !m_isFading)
            {
                //If the manual trail was already reduced below 0, disable manually
                if (m_prevTrailLength < 0)
                {
                    Deactivate();
                }
                //If the manual trail length still had some fading to do, fade automatically for a fraction of the autoFadeTime (taking into consideration the fact that it may have been changed due to an animation statechange)
                else
                {
                    StopSmoothly(m_prevAutoFadeTime * m_prevTrailLength);
                }
            }
            else if (m_enabled && !m_currEnabled)
            {
                Activate();
            }

            //If not currently enabled, don't continue update
            if (!m_currEnabled)
            {
                return;
            }

            //If not initialized, don't continue update
            if (!m_inited)
            {
                return;
            }

            m_prevTrailLength = m_trailLength;
            m_prevAutoFadeTime = m_autoFadeTime;


            UpdateHeadElem();


            m_elapsedTime += Time.deltaTime;
            if (m_elapsedTime < UpdateInterval)
            {
                return;
            }
            m_elapsedTime -= UpdateInterval;



            RecordCurElem();

            RefreshSpline();

            UpdateFade();

            UpdateVertex();
        }

        void LateUpdate()
        {
            //If not enabled, don't continue late update TO DO: TEST REMOVING THIS CODE, IDK IF IT'S GOOD OR BAD!?
            if (!m_currEnabled)
            {
                return;
            }

            //If not initialized, don't continue late update
            if (!m_inited)
            {
                return;
            }


            m_vertexPool.LateUpdate();
        }

        /*
        THIS FUNCTION WAS DEPRECATED
        void OnLevelWasLoaded(int level)
        {
            m_inited = false;
        }
        */

        /// <summary>
        /// This exists to replace deprecated OnLevelWasLoaded function
        /// </summary>
        /// <param name="loadedScene"></param>
        /// <param name="mode"></param>
        void OnSceneLoaded(Scene loadedScene, LoadSceneMode mode)
        {
            m_inited = false;
        }

        void OnDestroy()
        {
            if (m_vertexPool != null)
            {
                m_vertexPool.Destroy();
            }

            SceneManager.sceneLoaded -= OnSceneLoaded; //Remove callback to prevent "accessing destroyed object" error
        }

        void Start()
        {
            m_inited = false;
            //Init();
        }

        void OnDrawGizmos()
        {
            if (m_pointEnd == null || m_pointStart == null)
            {
                return;
            }


            float dist = (m_pointStart.position - m_pointEnd.position).magnitude;

            if (dist < Mathf.Epsilon)
                return;


            Gizmos.color = Color.red;

            Gizmos.DrawSphere(m_pointStart.position, dist * 0.04f);


            Gizmos.color = Color.blue;
            Gizmos.DrawSphere(m_pointEnd.position, dist * 0.04f);
        }
        #endregion

        #region Local Methods
        void InitSpline()
        {
            m_spline.Granularity = m_granularity;

            m_spline.Clear();

            for (int i = 0; i < m_maxFrame; i++)
            {
                //m_spline.AddControlPoint(CurHeadPos, m_pointStart.position - m_pointEnd.position);
                m_spline.AddControlPoint(CurHeadPos, PointStartPos - PointEndPos);
            }
        }

        void RefreshSpline()
        {
            for (int i = 0; i < m_snapshotList.Count; i++)
            {
                m_spline.ControlPoints[i].Position = m_snapshotList[i].Pos;
                m_spline.ControlPoints[i].Normal = m_snapshotList[i].PointEnd - m_snapshotList[i].PointStart;
            }

            m_spline.RefreshSpline();
        }

        void UpdateVertex()
        {
            VertexPool pool = m_vertexSegment.Pool;


            for (int i = 0; i < m_granularity; i++)
            {
                int baseIdx = m_vertexSegment.VertStart + i * 3;

                float uvSegment = (float)i / m_granularity;


                float fadeT = uvSegment * TrailLength; //m_trailLength; //m_fadeT;

                Vector2 uvCoord = Vector2.zero;

                Vector3 pos = m_spline.InterpolateByLen(fadeT);

                //Debug.DrawRay(pos, Vector3.up, Color.red);

                Vector3 up = m_spline.InterpolateNormalByLen(fadeT);
                Vector3 pos0 = pos + (up.normalized * m_trailWidth * 0.5f);
                Vector3 pos1 = pos - (up.normalized * m_trailWidth * 0.5f);


                // pos0
                pool.Vertices[baseIdx] = pos0;
                pool.Colors[baseIdx] = m_myColor;
                uvCoord.x = 0f;
                uvCoord.y = uvSegment;
                pool.UVs[baseIdx] = uvCoord;

                //pos
                pool.Vertices[baseIdx + 1] = pos;
                pool.Colors[baseIdx + 1] = m_myColor;
                uvCoord.x = 0.5f;
                uvCoord.y = uvSegment;
                pool.UVs[baseIdx + 1] = uvCoord;

                //pos1
                pool.Vertices[baseIdx + 2] = pos1;
                pool.Colors[baseIdx + 2] = m_myColor;
                uvCoord.x = 1f;
                uvCoord.y = uvSegment;
                pool.UVs[baseIdx + 2] = uvCoord;
            }

            m_vertexSegment.Pool.UVChanged = true;
            m_vertexSegment.Pool.VertChanged = true;
            m_vertexSegment.Pool.ColorChanged = true;
        }

        void UpdateIndices()
        {
            VertexPool pool = m_vertexSegment.Pool;

            for (int i = 0; i < m_granularity - 1; i++)
            {
                int baseIdx = m_vertexSegment.VertStart + i * 3;
                int nextBaseIdx = m_vertexSegment.VertStart + (i + 1) * 3;

                int iidx = m_vertexSegment.IndexStart + i * 12;

                //triangle left
                pool.Indices[iidx + 0] = nextBaseIdx;
                pool.Indices[iidx + 1] = nextBaseIdx + 1;
                pool.Indices[iidx + 2] = baseIdx;
                pool.Indices[iidx + 3] = nextBaseIdx + 1;
                pool.Indices[iidx + 4] = baseIdx + 1;
                pool.Indices[iidx + 5] = baseIdx;


                //triangle right
                pool.Indices[iidx + 6] = nextBaseIdx + 1;
                pool.Indices[iidx + 7] = nextBaseIdx + 2;
                pool.Indices[iidx + 8] = baseIdx + 1;
                pool.Indices[iidx + 9] = nextBaseIdx + 2;
                pool.Indices[iidx + 10] = baseIdx + 2;
                pool.Indices[iidx + 11] = baseIdx + 1;

            }

            pool.IndiceChanged = true;
        }

        void UpdateHeadElem()
        {
            //m_snapshotList[0].PointStart = m_pointStart.position;
            //m_snapshotList[0].PointEnd = m_pointEnd.position;

            //I change the above code to the below code so that the mesh would, somewhere down the line, be generated with a 0 for the z axis so that the z-axis positioning can be set
            //directly during the Graphics.DrawMesh call using the Transform.position data from the GameObject this is attached to
            m_snapshotList[0].PointStart = PointStartPos; // new Vector3(m_pointStart.position.x, m_pointStart.position.y, 0);
            m_snapshotList[0].PointEnd = PointEndPos; // new Vector3(m_pointEnd.position.x, m_pointEnd.position.y, 0);
        }


        void UpdateFade()
        {
            if (!m_isFading)
            {
                return;
            }

            m_fadeElapsedime += Time.deltaTime;

            float t = m_fadeElapsedime / m_fadeTime;

            m_fadeValue = 1f - t;

            if (m_fadeValue < 0f)
            {
                Deactivate();
            }
        }

        void RecordCurElem()
        {
            //TODO: use element pool to avoid gc alloc.
            //Element elem = new Element(m_pointStart.position, m_pointEnd.position);

            Element elem = m_elemPool.Get();
            //elem.PointStart = m_pointStart.position;
            //elem.PointEnd = m_pointEnd.position;
            elem.PointStart = PointStartPos;
            elem.PointEnd = PointEndPos;

            if (m_snapshotList.Count < m_maxFrame)
            {
                m_snapshotList.Insert(1, elem);
            }
            else
            {
                m_elemPool.Release(m_snapshotList[m_snapshotList.Count - 1]);
                m_snapshotList.RemoveAt(m_snapshotList.Count - 1);
                m_snapshotList.Insert(1, elem);
            }

        }

        void InitOriginalElements()
        {
            m_snapshotList.Clear();
            //at least add 2 original elements
            //m_snapshotList.Add(new Element(m_pointStart.position, m_pointEnd.position));
            //m_snapshotList.Add(new Element(m_pointStart.position, m_pointEnd.position));
            m_snapshotList.Add(new Element(PointStartPos, PointEndPos));
            m_snapshotList.Add(new Element(PointStartPos, PointEndPos));

        }

        void InitMeshObj()
        {
            ////create a new mesh obj
            //m_meshObj = new GameObject("_XWeaponTrailMesh: " + gameObject.name);
            //m_meshObj.layer = gameObject.layer;
            //m_meshObj.SetActive(true);
            //MeshFilter mf = m_meshObj.AddComponent<MeshFilter>();
            //MeshRenderer mr = m_meshObj.AddComponent<MeshRenderer>();
            //mr.castShadows = false;
            //mr.receiveShadows = false;
            //mr.renderer.sharedMaterial = m_myMaterial;
            //mf.sharedMesh = new Mesh();

            //init vertexpool
            m_vertexPool = new VertexPool(m_myMaterial, this);
            m_vertexSegment = m_vertexPool.GetVertices(m_granularity * 3, (m_granularity - 1) * 12);
            UpdateIndices();
        }
        #endregion

        #region Properties
        public float UpdateInterval
        {
            get
            {
                return 1f / m_fps;
            }
        }

        public Vector3 CurHeadPos
        {
            get
            {
                //return (m_pointStart.position + m_pointEnd.position) / 2f;
                return (PointStartPos + PointEndPos) / 2f;
            }
        }

        public float TrailWidth
        {
            get
            {
                return m_trailWidth;
            }
        }

        protected float TrailLength
        {
            get
            {
                if (m_isFading)
                {
                    return m_fadeValue;
                }

                return m_trailLength;
            }
        }

        protected Vector3 PointStartPos
        {
            get
            {
                return (new Vector3(m_pointStart.position.x, m_pointStart.position.y, 0));
            }
        }

        protected Vector3 PointEndPos
        {
            get
            {
                return (new Vector3(m_pointEnd.position.x, m_pointEnd.position.y, 0));
            }
        }
        #endregion
    }

}


