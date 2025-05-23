using System.Collections.Generic;
using UnityEngine;
namespace XvXR.Foundation
{
    /// <summary>
    /// ������Ҫ����ƽ�����ݵĻ���
    /// </summary>
    public sealed class XvPlaneMeshVisualizer : MonoBehaviour
    {

        private XvPlaneMeshVisualizer() { }

        private List<GameObject> planeList = new List<GameObject>();

        [SerializeField]
        private XvPlaneManager xvPlaneManager;
        [SerializeField]
        private Material planeMat;

        [SerializeField]
        private bool enableCollider=true;

        public bool EnableCollider { 
        get { return enableCollider; } 
        }

        [SerializeField]
        private bool enableRender = true;

        public bool EnableRender { 
        get { return enableRender; }
        }

        [SerializeField]
        private bool autoDetection=true;


        public bool AutoDetection
        {
            get { return autoDetection; }
        }
        private void Awake()
        {
            if (xvPlaneManager == null)
            {
                xvPlaneManager = FindAnyObjectByType<XvPlaneManager>();
            }

            if (xvPlaneManager == null)
            {
                xvPlaneManager = new GameObject("XvPlaneManager").AddComponent<XvPlaneManager>();

            }
        }

        private void OnEnable()
        {
            if (xvPlaneManager != null)
            {

                xvPlaneManager.planesChanged += PlanesChanged;

                if (autoDetection) { 
                
                xvPlaneManager.StartPlaneDetction();
                }

            }
        }

        private void OnDisable()
        {
            if (xvPlaneManager != null)
            {
                xvPlaneManager.planesChanged -= PlanesChanged;
                if (autoDetection)
                {
                    xvPlaneManager.StopPlaneDetection();
                }
            }
        }

        private void PlanesChanged(plane[] planes)
        {
            for (int i = 0; i < planeList.Count; i++)
            {
                Destroy(planeList[i]);
            }
            planeList = new List<GameObject>();

            //Debug.LogError("planes === " + planes.Length);

            for (int i = 0; i < planes.Length; i++)
            {
                Vector3[] v = new Vector3[planes[i].points.Count];

                for (int f = 0; f < planes[i].points.Count; f++)
                {
                    v[f] = new Vector3((float)planes[i].points[f].x, -(float)planes[i].points[f].y, (float)planes[i].points[f].z);
                }

                DoCreatPloygonMesh(v);
            }
        }

        /// <summary>
        /// �����Զ������η���
        /// </summary>
        /// <param name="s_Vertives">�Զ���Ķ�������</param>
        private void DoCreatPloygonMesh(Vector3[] s_Vertives)
        {
            //�½�һ����������н��л����Զ�������
            GameObject tPolygon = new GameObject("tPolygon");

            //������������������
            tPolygon.AddComponent<MeshFilter>();
            tPolygon.AddComponent<MeshRenderer>();

            //������һ��Mesh����
            Mesh tMesh = new Mesh();

            //�洢���еĶ���
            Vector3[] tVertices = s_Vertives;

            //�洢�����������εĵ�����
            List<int> tTriangles = new List<int>();

            //�������ж�����������
            for (int i = 0; i < tVertices.Length - 1; i++)
            {
                tTriangles.Add(i);
                tTriangles.Add(i + 1);
                tTriangles.Add(tVertices.Length - i - 1);
            }

            //��ֵ����ζ���
            tMesh.vertices = tVertices;

            //��ֵ�����ε�����
            tMesh.triangles = tTriangles.ToArray();

            //��������UV������
            tMesh.RecalculateBounds();
            tMesh.RecalculateNormals();

            //�����ƺõ�Mesh��ֵ
            tPolygon.GetComponent<MeshFilter>().mesh = tMesh;
            tPolygon.GetComponent<Renderer>().material = planeMat;
            Collider collider= tPolygon.AddComponent<MeshCollider>();
            collider.enabled = enableCollider;

            tPolygon.GetComponent<MeshRenderer>().enabled = enableRender;
            planeList.Add(tPolygon);
        }


        public void SetCollider(bool enable)
        {
            enableCollider = enable;
            foreach (var item in planeList)
            {
                item.GetComponent<Collider>().enabled = enableCollider;
            }
        }

        public void SetVisualizer(bool enable)
        {
            this.enableRender = enable;
            foreach (var item in planeList)
            {
                item.GetComponent<MeshRenderer>().enabled = enableRender;
            }
        }

        private void OnDestroy()
        {
            for (int i = 0; i < planeList.Count; i++)
            {
                Destroy(planeList[i]);
            }
        }
    }
}