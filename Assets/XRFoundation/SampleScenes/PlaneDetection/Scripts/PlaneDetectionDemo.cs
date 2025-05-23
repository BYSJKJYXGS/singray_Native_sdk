
using TMPro;
using UnityEngine;

namespace XvXR.Foundation.SampleScenes
{
    public class PlaneDetectionDemo : MonoBehaviour
    {
        //[SerializeField]
        private XvPlaneManager xvPlaneManager;
        //[SerializeField]
        private XvPlaneMeshVisualizer xvPlaneMeshVisualizer;

        private TextMeshPro meshText;
        private TextMeshPro colliderText;
        private TextMeshPro meshrenderText;



        private void Awake()
        {
            if (xvPlaneManager == null)
            {
                xvPlaneManager = FindObjectOfType<XvPlaneManager>();

                if (xvPlaneManager==null) {
                    GameObject newObj = Instantiate(Resources.Load<GameObject>("XvPlaneManager"));

                    newObj.name = "XvPlaneManager";
                    xvPlaneManager = newObj.GetComponent<XvPlaneManager>();
                }
            }

            if (xvPlaneMeshVisualizer == null)
            {
                xvPlaneMeshVisualizer = FindObjectOfType<XvPlaneMeshVisualizer>();
            }

            meshText = transform.Find("UI/Canvas/PlaneDetection/IconAndText/TextMeshPro").GetComponent<TextMeshPro>();
            colliderText = transform.Find("UI/Canvas/MeshCollider/IconAndText/TextMeshPro").GetComponent<TextMeshPro>();
            meshrenderText = transform.Find("UI/Canvas/MeshRender/IconAndText/TextMeshPro").GetComponent<TextMeshPro>();

        }

        private void Start()
        {
            if (xvPlaneManager.IsDetecting)
            {
                meshText.text = "StopDetction";
            }
            else
            {
                meshText.text = "StartDetction";
            }

            if (xvPlaneMeshVisualizer.EnableCollider)
            {

                colliderText.text = "DisableCollider";
            }
            else
            {
                colliderText.text = "EnableCollider";
            }

            if (xvPlaneMeshVisualizer.EnableRender)
            {

                meshrenderText.text = "DisableRender";
            }
            else
            {
                meshrenderText.text = "EnableRender";

            }
        }


        public void SetPlaneDetection() {
            if (xvPlaneManager.IsDetecting)
            {
                xvPlaneManager.StopPlaneDetection();

                meshText.text = "StartDetction";
            }
            else { 
            xvPlaneManager.StartPlaneDetction();
                meshText.text = "StopDetction";

            }

        }
       

       
        public void SetCollider()
        {
            if (xvPlaneMeshVisualizer.EnableCollider)
            {

                xvPlaneMeshVisualizer.SetCollider(false);
                colliderText.text = "EnableCollider";
            }
            else { 
                xvPlaneMeshVisualizer.SetCollider(true);
                colliderText.text = "DisableCollider";


            }
        }

        public void SetVisualizer()
        {
            if (xvPlaneMeshVisualizer.EnableRender)
            {

                xvPlaneMeshVisualizer.SetVisualizer(false);
                meshrenderText.text = "EnableRender";
            }
            else
            {
                xvPlaneMeshVisualizer.SetVisualizer(true);
                meshrenderText.text = "DisableRender";
            }
          
        }
    }
}


