using System.Collections.Generic;
using UnityEngine;
namespace XvXR.Foundation.SampleScenes
{
    public class XvParticlesCloudPoint : MonoBehaviour
    {
        public Transform centerCube;
        public ParticleSystem ps;
        ParticleSystem.Particle[] allParticles;

        List<Vector3> FilterVec = new List<Vector3>();

        Matrix4x4 P_tofpoint_world = Matrix4x4.identity;

        Matrix4x4 P_tofpoint_tofcam = Matrix4x4.identity;//Original TOF point cloud
        Matrix4x4 P_tofcam_glassImu = Matrix4x4.identity;//TOF external parameters
        Matrix4x4 P_glassImu_world = Matrix4x4.identity;//Glasses 6DOF

        public Material[] Mat_alpha;
        public List<GameObject> planeList = new List<GameObject>();
        bool isCreateMesh;
        float totaltime;
        void Start()
        {

        }

        void Update()
        {
            /*totaltime += Time.deltaTime;

           if (totaltime>=1)
           {
               isCreateMesh = true;
               totaltime = 0;
           }*/
        }

        public void StartDraw(Vector3[] vs)
        {
            FilterVec.Clear();

         

            for (int i = 0; i < vs.Length; i++)
            {
                int randomIndex = UnityEngine.Random.Range(0, 30);
                if (randomIndex > 28)
                {
                    FilterVec.Add(vs[i]);
                }
            }


          
            var main = ps.main;

            var pointCount = FilterVec.Count;
            allParticles = new ParticleSystem.Particle[pointCount];
            main.maxParticles = pointCount;
            //ps.maxParticles = pointCount;
            ps.Emit(pointCount);
            ps.GetParticles(allParticles);
            for (int i = 0; i < pointCount; i++)
            {
                allParticles[i].position = (Vector3)FilterVec[i];    // Set the position of each point
                allParticles[i].startColor = Color.blue;    // Set the RGB of each point
                allParticles[i].startSize = 0.025f;
            }


            ps.SetParticles(allParticles, pointCount);      // Load the point cloud into the particle system
        
        }

        void CreateMesh(List<Vector3> FilterVec)
        {
            for (int i = 0; i < planeList.Count; i++)
            {
                Destroy(planeList[i]);
            }
            planeList = new List<GameObject>();

            Vector3[] Verts = new Vector3[FilterVec.Count];
            for (int i = 0; i < FilterVec.Count; i++)
            {
                Verts[i] = FilterVec[i];
            }
            DoCreatPloygonMesh(Verts);
        }

        /// <summary>
        /// Method to generate custom polygons
        /// </summary>
        /// <param name="s_Vertives">Custom vertex array</param>
        public void DoCreatPloygonMesh(Vector3[] s_Vertives)
        {
            //Create a new empty object to draw a custom polygon
            GameObject tPolygon = new GameObject("tPolygon");

            //The two components necessary for drawing
            tPolygon.AddComponent<MeshFilter>();
            tPolygon.AddComponent<MeshRenderer>();

            //Apply for a new Mesh network
            Mesh tMesh = new Mesh();

            //Store all vertices
            Vector3[] tVertices = s_Vertives;

            //Store the sorted points of all triangles in the drawing
            List<int> tTriangles = new List<int>();

            //Sort the fill points based on all vertices
            for (int i = 0; i < tVertices.Length - 1; i++)
            {
                tTriangles.Add(i);
                tTriangles.Add(i + 1);
                tTriangles.Add(tVertices.Length - i - 1);
            }

            //Assign polygon vertices
            tMesh.vertices = tVertices;

            //Assign triangle point sorting
            tMesh.triangles = tTriangles.ToArray();

            //Reset UVs and normals
            tMesh.RecalculateBounds();
            tMesh.RecalculateNormals();

            //Assign the drawn Mesh
            tPolygon.GetComponent<MeshFilter>().mesh = tMesh;
            tPolygon.GetComponent<Renderer>().materials = Mat_alpha;
            tPolygon.AddComponent<MeshCollider>();

            planeList.Add(tPolygon);
        }
    }
}