
using TMPro;
using UnityEngine;


namespace XvXR.Foundation.SampleScenes
{
    public class SpatialMeshDemo : MonoBehaviour
    {
       // [SerializeField]
        private XvSpatialMeshManager xvSpatialMeshManager;
        //[SerializeField]
        private XvSpatialMeshVisualizer xvSpatialMeshVisualizer;

       private TextMeshPro meshText;
       private TextMeshPro colliderText;
       private TextMeshPro meshrenderText;

        private void Awake()
        {
            if (xvSpatialMeshManager == null)
            {
                xvSpatialMeshManager = FindObjectOfType<XvSpatialMeshManager>();

                if (xvSpatialMeshManager==null) {
                    GameObject newObj = Instantiate(Resources.Load<GameObject>("XvSpatialMeshManager"));

                    newObj.name = "XvSpatialMeshManager";
                    xvSpatialMeshManager = newObj.GetComponent<XvSpatialMeshManager>();
                }

               
            }

            if (xvSpatialMeshVisualizer == null)
            {
                xvSpatialMeshVisualizer = FindObjectOfType<XvSpatialMeshVisualizer>();
            }

            meshText = transform.Find("UI/Canvas/MeshDetection/IconAndText/TextMeshPro").GetComponent<TextMeshPro>();
            colliderText = transform.Find("UI/Canvas/MeshCollider/IconAndText/TextMeshPro").GetComponent<TextMeshPro>();
            meshrenderText = transform.Find("UI/Canvas/MeshRender/IconAndText/TextMeshPro").GetComponent<TextMeshPro>();

        }

        private void Start()
        {
            if (xvSpatialMeshManager.IsDetecting)
            {
                meshText.text = "StopDetction";
            }
            else
            {
                meshText.text = "StartDetction";
            }

            if (xvSpatialMeshVisualizer.EnableCollider)
            {

                colliderText.text = "DisableCollider";
            }
            else
            {
                colliderText.text = "EnableCollider";
            }

            if (xvSpatialMeshVisualizer.EnableRender)
            {

                meshrenderText.text = "DisableRender";
            }
            else
            {
                meshrenderText.text = "EnableRender";

            }
        }

        
        public void SetMeshDetection()
        {
            if (xvSpatialMeshManager.IsDetecting)
            {
                xvSpatialMeshManager.StopMeshDetection();

                meshText.text = "StartDetection";
            }
            else
            {
                xvSpatialMeshManager.StartMeshDetection();
                meshText.text = "StopDetection";

            }

        }



        public void SetCollider()
        {
            if (xvSpatialMeshVisualizer.EnableCollider)
            {

                xvSpatialMeshVisualizer.SetCollider(false);
                colliderText.text = "EnableCollider";
            }
            else
            {
                xvSpatialMeshVisualizer.SetCollider(true);
                colliderText.text = "DisableCollider";


            }
        }

        public void SetVisualizer()
        {
            if (xvSpatialMeshVisualizer.EnableRender)
            {

                xvSpatialMeshVisualizer.SetVisualizer(false);
                meshrenderText.text = "EnableRender";
            }
            else
            {
                xvSpatialMeshVisualizer.SetVisualizer(true);
                meshrenderText.text = "DisableRender";
            }

        }
    }
}
