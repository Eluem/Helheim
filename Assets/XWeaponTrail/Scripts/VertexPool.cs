//----------------------------------------------
//            Xffect Editor
// Copyright © 2012- Shallway Studio
// http://shallway.net
//----------------------------------------------
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Xft;

namespace Xft
{

    public class VertexPool
    {
        public class VertexSegment
        {
            public int VertStart;
            public int IndexStart;
            public int VertCount;
            public int IndexCount;
            public VertexPool Pool;

            public VertexSegment(int start, int count, int istart, int icount, VertexPool pool)
            {
                VertStart = start;
                VertCount = count;
                IndexCount = icount;
                IndexStart = istart;
                Pool = pool;
            }


            public void ClearIndices()
            {
                for (int i = IndexStart; i < IndexStart + IndexCount; i++)
                {
                    Pool.Indices[i] = 0;
                }

                Pool.IndiceChanged = true;
            }

        }

        public Vector3[] Vertices;
        public int[] Indices;
        public Vector2[] UVs;
        public Color[] Colors;

        public bool IndiceChanged;
        public bool ColorChanged;
        public bool UVChanged;
        public bool VertChanged;
        public bool UV2Changed;



        public Mesh _mesh;
        public Material Material;

        protected int VertexTotal;
        protected int VertexUsed;
        protected int IndexTotal = 0;
        protected int IndexUsed = 0;
        public bool FirstUpdate = true;

        protected bool VertCountChanged;


        public const int BlockSize = 108;

        public float BoundsScheduleTime = 1f;
        public float ElapsedTime = 0f;


        protected List<VertexSegment> SegmentList = new List<VertexSegment>();

        protected XWeaponTrail _owner;
        protected Transform m_ownerTransform;

        public void RecalculateBounds()
        {
            _mesh.RecalculateBounds();
        }

        public VertexPool(Material material, XWeaponTrail owner)
        {
            VertexTotal = VertexUsed = 0;
            VertCountChanged = false;
            _mesh = new Mesh();
            Material = material;
            InitArrays();
            _owner = owner;
            m_ownerTransform = _owner.transform;

            IndiceChanged = ColorChanged = UVChanged = UV2Changed = VertChanged = true;
        }


        public Material GetMaterial()
        {
            return Material;
        }



        public VertexSegment GetVertices(int vcount, int icount)
        {
            int vertNeed = 0;
            int indexNeed = 0;
            if (VertexUsed + vcount >= VertexTotal)
            {
                vertNeed = (vcount / BlockSize + 1) * BlockSize;
            }
            if (IndexUsed + icount >= IndexTotal)
            {
                indexNeed = (icount / BlockSize + 1) * BlockSize;
            }
            VertexUsed += vcount;
            IndexUsed += icount;
            if (vertNeed != 0 || indexNeed != 0)
            {
                EnlargeArrays(vertNeed, indexNeed);
                VertexTotal += vertNeed;
                IndexTotal += indexNeed;
            }

            VertexSegment ret = new VertexSegment(VertexUsed - vcount, vcount, IndexUsed - icount, icount, this);

            return ret;
        }


        protected void InitArrays()
        {
            Vertices = new Vector3[4];
            UVs = new Vector2[4];
            Colors = new Color[4];
            Indices = new int[6];
            VertexTotal = 4;
            IndexTotal = 6;
        }



        public void EnlargeArrays(int count, int icount)
        {
            Vector3[] tempVerts = Vertices;
            Vertices = new Vector3[Vertices.Length + count];
            tempVerts.CopyTo(Vertices, 0);

            Vector2[] tempUVs = UVs;
            UVs = new Vector2[UVs.Length + count];
            tempUVs.CopyTo(UVs, 0);

            Color[] tempColors = Colors;
            Colors = new Color[Colors.Length + count];
            tempColors.CopyTo(Colors, 0);

            int[] tempTris = Indices;
            Indices = new int[Indices.Length + icount];
            tempTris.CopyTo(Indices, 0);

            VertCountChanged = true;
            IndiceChanged = true;
            ColorChanged = true;
            UVChanged = true;
            VertChanged = true;
            UV2Changed = true;
        }


        public void Destroy()
        {
            if (_mesh != null)
            {
                Mesh.DestroyImmediate(_mesh);
            }
        }

        public void LateUpdate()
        {
            if (VertCountChanged)
            {
                _mesh.Clear();
            }

            // we assume the vertices are always changed.
            _mesh.vertices = Vertices;
            if (UVChanged)
            {
                _mesh.uv = UVs;
            }

            if (ColorChanged)
            {
                _mesh.colors = Colors;
            }

            if (IndiceChanged)
            {
                _mesh.triangles = Indices;
            }

            ElapsedTime += Time.deltaTime;
            if (ElapsedTime > BoundsScheduleTime || FirstUpdate)
            {
                RecalculateBounds();
                ElapsedTime = 0f;
            }

            if (ElapsedTime > BoundsScheduleTime)
                FirstUpdate = false;

            VertCountChanged = false;
            IndiceChanged = false;
            ColorChanged = false;
            UVChanged = false;
            UV2Changed = false;
            VertChanged = false;


            //Matrix4x4 matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, Vector3.one);

            //Graphics.DrawMesh(_mesh, Matrix4x4.identity, Material, _owner.gameObject.layer, null, 0, null, false, false);

            //Replaced the above code with the below code so that I can force the actual position to use the owner object's z position.
            //Inside teh XWeaponTrail.cs file I changed the "snapshot" code in UpdateHeadElem() to leave 0 in the z axis.
            //I believe this will allow me to get the mesh to be properly rendered with z-axis ordering. Without actually having a position, unity does some weird rendering.. the mesh itself
            //seems to be treated as though it has no position in terms of render order.. idk.. -sigh-
            Graphics.DrawMesh(_mesh, new Vector3(0, 0, m_ownerTransform.position.z), Quaternion.identity, Material, _owner.gameObject.layer, null, 0, null, false, false);
        }
    }
}