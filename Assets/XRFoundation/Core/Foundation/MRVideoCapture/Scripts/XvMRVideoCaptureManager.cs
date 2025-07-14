using UnityEngine;
using UnityEngine.UI;

namespace XvXR.Foundation
{
    public enum CaptureType
    {
        OnlyCamera,
        OnlyUnityScene,
        MR,
    }
    /// <summary>
    /// ������Ҫ���ǻ����ʵ��Ƶ�Ĳ�׽��������XvCameraManager�࣬
    /// ���û����ʵ��׽֮ǰ��Ҫȷ������Ѿ���
    /// </summary>
    [DisallowMultipleComponent]
    public sealed class XvMRVideoCaptureManager : MonoBehaviour
    {
        private XvMRVideoCaptureManager() { }
        [SerializeField]
        private XvCameraManager cameraManager;


        public XvCameraManager CameraManager
        {
            get
            {

                if (cameraManager == null)
                {
                    cameraManager = FindObjectOfType<XvCameraManager>();
                }

                if (cameraManager == null)
                {
                    cameraManager = new GameObject("XvCameraManager").AddComponent<XvCameraManager>();
                }
                return cameraManager;

            }
        }
        [SerializeField]

        private RawImage rgbBackground;

        [SerializeField]
        private CaptureType captureType = CaptureType.MR;

        private int cullingMask;

        public CaptureType CaptureType
        {
            get { return captureType; }
            set
            {

                captureType = value;

                switch (captureType)
                {
                    case CaptureType.OnlyCamera:
                        rgbBackground.enabled = true;
                        BgCamera.cullingMask = (1 << rgbBackground.gameObject.layer);
                        break;
                    case CaptureType.OnlyUnityScene:
                        rgbBackground.enabled = false;
                        BgCamera.cullingMask = cullingMask;

                        BgCamera.cullingMask |= (1 << rgbBackground.gameObject.layer);
                        break;
                    case CaptureType.MR:
                        rgbBackground.enabled = true;
                        BgCamera.cullingMask = cullingMask;
                        break;
                    default:
                        break;
                }
            }
        }






        private Camera bgCamera;
        public Camera BgCamera
        {
            get
            {
                if (bgCamera == null)
                {
                    bgCamera = transform.Find("BgCamera").GetComponent<Camera>(); ;
                }


                return bgCamera;
            }

        }
        [SerializeField]
        private bool autoCapture = false;
        /// <summary>
        /// �����ʵ����
        /// </summary>

        private RenderTexture cameraRenderTexture = null;

        public RenderTexture CameraRenderTexture
        {
            get
            {
                if (cameraRenderTexture == null)
                {
                    cameraRenderTexture = new RenderTexture(CameraManager.Width, CameraManager.Height, 24, RenderTextureFormat.RGB565);
                }
                return cameraRenderTexture;
            }
        }


        private bool isOn = false;
        public bool IsOn
        {
            get { return isOn; }
        }

        // Start is called before the first frame update
       

        private void Awake()
        {

            //

          
            rgbBackground.gameObject.layer = LayerMask.NameToLayer("XvBGVideo");

            cullingMask = BgCamera.cullingMask;
            BgCamera.targetTexture = CameraRenderTexture;
            CaptureType = captureType;

            if (autoCapture)
            {
                StartCapture();

            }
        }

        private void Update()
        {
            //if (Input.GetKeyDown(KeyCode.LeftArrow))
            //{
            //    CaptureType = CaptureType.MR;
            //}
            //if (Input.GetKeyDown(KeyCode.RightArrow))
            //{
            //    CaptureType = CaptureType.OnlyCamera;
            //}

            //if (Input.GetKeyDown(KeyCode.UpArrow))
            //{
            //    CaptureType = CaptureType.OnlyUnityScene;
            //}
        }

        /// <summary>
        ///  ������׽��ʵ�����Ƶ��
        /// </summary>

        public void StartCapture()
        {
            //
            if (!CameraManager.IsOn(XvCameraStreamType.ARCameraStream))
            {
                MyDebugTool.Log("StartCapture XvCameraStreamType.ARCameraStream");
                CameraManager.StartCapture(XvCameraStreamType.ARCameraStream);
            }


            if (!isOn)
            {
                isOn = true;
                gameObject.SetActive(true);
                rgbBackground.gameObject.SetActive(true);
                BgCamera.gameObject.SetActive(true);
                XvCameraManager.onARCameraStreamFrameArrived.AddListener(onFrameArrived);
                MyDebugTool.Log("onARCameraStreamFrameArrived onFrameArrived");

            }

        }
        /// <summary>
        /// ֹͣ��׽��ʵ�����Ƶ������������ǹ��õģ��رյ�ʱ��ȷ����Ҫ�ж��Ƿ��������������Ӱ��
        /// </summary>
        /// <param name="closeCamera"> true:�ر����  false�����ر����</param>
        public void StopCapture(bool closeCamera = false)
        {
            if (isOn)
            {
                isOn = false;
                if (closeCamera && CameraManager.IsOn(XvCameraStreamType.ARCameraStream))
                {
                    CameraManager.StopCapture(XvCameraStreamType.ARCameraStream);
                }
                XvCameraManager.onARCameraStreamFrameArrived.RemoveListener(onFrameArrived);
                gameObject.SetActive(false);
            }
        }

        /// <summary>
        /// �������ͼ����䵽����ͼ��
        /// �����������
        /// </summary>
        /// <param name="cameraData"></param>
        private void onFrameArrived(cameraData cameraData)
        {

            if (rgbBackground != null)
            {
                rgbBackground.texture = cameraData.tex;
            }

            BgCamera.usePhysicalProperties = true;
            BgCamera.focalLength = cameraData.parameter.focal;
            BgCamera.sensorSize = new Vector2(cameraData.parameter.focal * cameraData.parameter.width / cameraData.parameter.fx,
                                          cameraData.parameter.focal * cameraData.parameter.height / cameraData.parameter.fy);

            BgCamera.lensShift = new Vector2(-(cameraData.parameter.cx - cameraData.parameter.width * 0.5f) / cameraData.parameter.width,
                                         (cameraData.parameter.cy - cameraData.parameter.height * 0.5f) / cameraData.parameter.height);

            BgCamera.gateFit = Camera.GateFitMode.Vertical;


            transform.localPosition = cameraData.parameter.position;
            transform.localRotation = cameraData.parameter.rotation;
        }

    }
}
