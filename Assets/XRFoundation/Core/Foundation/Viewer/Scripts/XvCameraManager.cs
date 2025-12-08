
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Events;

using Quaternion = UnityEngine.Quaternion;

namespace XvXR.Foundation
{

    [DisallowMultipleComponent]
    public sealed class XvCameraManager : MonoBehaviour
    {

        private XvCameraManager() { }


        public XvARCameraParameter XvARCameraParameter = new XvARCameraParameter()
        {
            rgbResolution = RgbResolution.RGB_1280x720,
            fps = 30,

        };
        public XvWebCameraParameter XvWebCameraParameter = new XvWebCameraParameter()
        {
            width = 1280,
            height = 720,
            fps = 30,
        };

        public XvTofCameraParameter XvTofCameraParameter = new XvTofCameraParameter()
        {
            streamType = TofStreamType.DeapthStream,
            tofFramerate = TofFramerate.FPS_30,
            sonyTofLibMode = SonyTofLibMode.IQMIX_SF,
            tofResolution = TofResolution.QVGA,
            enableGamma = false,

        };


        private int requestedWidth = 1920;
        private int requestedHeight = 1080;

        public int Width
        {
            get
            {
                switch (XvARCameraParameter.rgbResolution)
                {
                    case RgbResolution.RGB_1920x1080:
                        requestedWidth = 1920;
                        requestedHeight = 1080;

                        break;
                    case RgbResolution.RGB_1280x720:
                        requestedWidth = 1280;
                        requestedHeight = 720;

                        break;
                    case RgbResolution.RGB_640x480:
                        requestedWidth = 640;
                        requestedHeight = 480;
                        break;
                    case RgbResolution.RGB_320x240:
                        requestedWidth = 320;
                        requestedHeight = 240;

                        break;
                    case RgbResolution.RGB_2560x1920:
                        requestedWidth = 2560;
                        requestedHeight = 1920;

                        break;
                    default:
                        break;
                }

                return requestedWidth;

            }


        }
        public int Height
        {
            get
            {
                switch (XvARCameraParameter.rgbResolution)
                {
                    case RgbResolution.RGB_1920x1080:
                        requestedWidth = 1920;
                        requestedHeight = 1080;

                        break;
                    case RgbResolution.RGB_1280x720:
                        requestedWidth = 1280;
                        requestedHeight = 720;

                        break;
                    case RgbResolution.RGB_640x480:
                        requestedWidth = 640;
                        requestedHeight = 480;
                        break;
                    case RgbResolution.RGB_320x240:
                        requestedWidth = 320;
                        requestedHeight = 240;

                        break;
                    case RgbResolution.RGB_2560x1920:
                        requestedWidth = 2560;
                        requestedHeight = 1920;
                        break;
                    default:
                        break;
                }

                return requestedHeight;

            }


        }

        public int Fps
        {
            get { return XvARCameraParameter.fps; }

        }

 
        public static UnityEvent<cameraData> onARCameraStreamFrameArrived = new UnityEvent<cameraData>();
        public static UnityEvent<cameraData> onLeftStereoStreamFrameArrived = new UnityEvent<cameraData>();
        public static UnityEvent<cameraData> onRightStereoStreamFrameArrived = new UnityEvent<cameraData>();
        public static UnityEvent<cameraData> onTofDepthCameraStreamFrameArrived = new UnityEvent<cameraData>();
        public static UnityEvent<cameraData> onTofIRCameraStreamFrameArrived = new UnityEvent<cameraData>();
        public static UnityEvent<cameraData> onWebCameraStreamFrameArrived = new UnityEvent<cameraData>();


        private void Awake()
        {

        }

        /// <summary>
        /// Open Camera
        /// </summary>
        /// <param name="cameraType"></param>
        public void StartCapture(XvCameraStreamType cameraType)
        {

#if PLATFORM_ANDROID && !UNITY_EDITOR

            while (!API.xslam_ready())
            {
                MyDebugTool.Log("xslam_ready==false");
            }
#endif




            switch (cameraType)
            {
                case XvCameraStreamType.WebCameraStream:


                    XvWebCameraManager.GetXvWebCameraManager().StartCapture(XvWebCameraParameter);
                    break;
                case XvCameraStreamType.ARCameraStream:
                    XvARCameraManager.GetXvARCameraManager().StartCapture(XvARCameraParameter);
                    break;
                case XvCameraStreamType.TofDepthCameraStream:

                    XvTofCameraParameter.streamType = TofStreamType.DeapthStream;

                    XvTofManager.GetXvTofManager().StartCapture(XvTofCameraParameter);
                    break;
                case XvCameraStreamType.TofIRCameraStream:

                    XvTofCameraParameter.streamType = TofStreamType.IRStream;

                    XvTofManager.GetXvTofManager().StartCapture(XvTofCameraParameter);
                    break;
                case XvCameraStreamType.LeftStereoCameraStream:

                    XvStereoCameraParameter XvLeftStereoCameraParameter = new XvStereoCameraParameter()
                    {
                        cameraIndex = StereoCameraIndex.LeftEye
                    };


                    XvStereoCameraManager.GetXvStereoCameraManager(true).StartCapture(XvLeftStereoCameraParameter);
                    break;
                case XvCameraStreamType.RightStereoCameraStream:
                    XvStereoCameraParameter XvRightStereoCameraParameter = new XvStereoCameraParameter()
                    {
                        cameraIndex = StereoCameraIndex.RightEye
                    };

                    XvStereoCameraManager.GetXvStereoCameraManager(false).StartCapture(XvRightStereoCameraParameter);

                    break;
                default:
                    break;
            }

        }
        /// <summary>
        /// Close Camera
        /// </summary>
        /// <param name="cameraType"></param>
        public void StopCapture(XvCameraStreamType cameraType)
        {
            switch (cameraType)
            {
                case XvCameraStreamType.WebCameraStream:
                    XvWebCameraManager.GetXvWebCameraManager().StopCapture();

                    break;
                case XvCameraStreamType.ARCameraStream:
                    XvARCameraManager.GetXvARCameraManager().StopCapture();
                    break;
                case XvCameraStreamType.TofDepthCameraStream:
                    XvTofManager.GetXvTofManager().StopCapture(TofStreamType.DeapthStream);
                    break;

                case XvCameraStreamType.TofIRCameraStream:
                    XvTofManager.GetXvTofManager().StopCapture(TofStreamType.IRStream);
                    break;

                case XvCameraStreamType.LeftStereoCameraStream:
                    XvStereoCameraManager.GetXvStereoCameraManager(true).StopCapture();

                    break;
                case XvCameraStreamType.RightStereoCameraStream:
                    XvStereoCameraManager.GetXvStereoCameraManager(false).StopCapture();
                    break;
                default:
                    break;
            }
        }

        public bool IsOn(XvCameraStreamType cameraType)
        {
            switch (cameraType)
            {
                case XvCameraStreamType.WebCameraStream:
                    return XvWebCameraManager.GetXvWebCameraManager().IsOn;

                case XvCameraStreamType.ARCameraStream:
                    return XvARCameraManager.GetXvARCameraManager().IsOn;

                case XvCameraStreamType.TofDepthCameraStream:

                    return XvTofManager.GetXvTofManager().IsOn(TofStreamType.DeapthStream);

                case XvCameraStreamType.TofIRCameraStream:

                    return XvTofManager.GetXvTofManager().IsOn(TofStreamType.IRStream);

                case XvCameraStreamType.LeftStereoCameraStream:
                    return XvStereoCameraManager.GetXvStereoCameraManager(true).IsOn;


                case XvCameraStreamType.RightStereoCameraStream:
                    return XvStereoCameraManager.GetXvStereoCameraManager(false).IsOn;

                default:
                    break;
            }

            return false;
        }



        private void Update()
        {

            if (XvStereoCameraManager.GetXvStereoCameraManager(true).IsOn)
            {
                XvStereoCameraManager.GetXvStereoCameraManager(true).Update();
            }

            if (XvStereoCameraManager.GetXvStereoCameraManager(false).IsOn)
            {
                XvStereoCameraManager.GetXvStereoCameraManager(false).Update();
            }
            XvTofManager.GetXvTofManager().Update();

            if (XvARCameraManager.GetXvARCameraManager().IsOn)
            {
                XvARCameraManager.GetXvARCameraManager().Update();
            }

            if (XvWebCameraManager.GetXvWebCameraManager().IsOn)
            {
                XvWebCameraManager.GetXvWebCameraManager().Update();
            }
        }

        private void OnDestroy()
        {

            StopCapture(XvCameraStreamType.WebCameraStream);
            StopCapture(XvCameraStreamType.ARCameraStream);
            StopCapture(XvCameraStreamType.TofDepthCameraStream);
            StopCapture(XvCameraStreamType.TofIRCameraStream);
            StopCapture(XvCameraStreamType.LeftStereoCameraStream);
            StopCapture(XvCameraStreamType.RightStereoCameraStream);

        }

        private void OnApplicationQuit()
        {
            StopCapture(XvCameraStreamType.WebCameraStream);
            StopCapture(XvCameraStreamType.ARCameraStream);
            StopCapture(XvCameraStreamType.TofDepthCameraStream);
            StopCapture(XvCameraStreamType.TofIRCameraStream);
            StopCapture(XvCameraStreamType.LeftStereoCameraStream);
            StopCapture(XvCameraStreamType.RightStereoCameraStream);
        }



        #region ToF Camera
        private int width;
        private int height;
        private bool isGetTofData;
        private Vector3[] vecGroup;


        private bool startPointCloud;

     

        /// <summary>
        /// Enable ToF Point Clound
        /// </summary>
        public void StartTofPointCloud()
        {

            startPointCloud = true;
            while (!API.xslam_ready())
            {
                MyDebugTool.Log("slam not start");
            }

            if (!IsOn(XvCameraStreamType.TofDepthCameraStream))
            {
                StopCapture(XvCameraStreamType.TofDepthCameraStream);
            }
            XvTofManager.GetXvTofManager().SetTofStreamMode((int)XvTofCameraParameter.tofStreamMode);

            XvTofManager.GetXvTofManager().StartTofStream(XvTofCameraParameter);

           //StartCapture(XvCameraStreamType.TofDepthCameraStream);
        }

        /// <summary>
        /// Get PointCloud
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public bool GetPointCloudData(out Vector3[] data)
        {
            if (!startPointCloud)
            {
                data = null;
                return false;
            }

            while (!API.xslam_ready())
            {
                MyDebugTool.Log("slam not start");
            }

            if (!isGetTofData)
            {
                width = API.xslam_get_tof_width();
                height = API.xslam_get_tof_height();

                //Debug.Log($"XVTof width:{API.xslam_get_tof_width()},height:{API.xslam_get_tof_height()}");
                if (width > 0)
                {
                    vecGroup = new Vector3[width * height];

                    isGetTofData = true;
                }
            }

            if (isGetTofData && vecGroup.Length > 0)
            {
                bool b = API.xslam_get_cloud_data_ex(vecGroup);
                data = vecGroup;

                return b;
            }
            data = null;

            return false;
        }






        /// <summary>
        /// Set ToF exp
        /// </summary>
        /// <param name="libmode"></param>
        /// <param name="resulution">resulution</param>
        /// <param name="fps">fps</param>
        /// <param name="exposureTimeMs">exposureTimeMs</param>
        public void SetTofExposure(int libmode, int resulution, int fps, float exposureTimeMs)
        {
            XvTofManager.GetXvTofManager().StopTofStream();

            bool v1 = API.xslam_start_sony_tof_stream(libmode, resulution, fps);
            bool v2 = API. xslam_tof_set_exposure(1, 0, exposureTimeMs);
        }

        public void StopTofPointCloud()
        {
            startPointCloud = false;
            XvTofManager.GetXvTofManager().StopTofStream();

        }

        #endregion


    }

    public enum XvCameraStreamType
    {

        WebCameraStream,
        ARCameraStream,
        TofDepthCameraStream,
        TofIRCameraStream,

        LeftStereoCameraStream,
        RightStereoCameraStream,
    }

    public class cameraData
    {
        public int texWidth;
        public int texHeight;
        public Texture tex;

        public CameraParameter parameter;
    }



    public struct CameraParameter
    {
        public Vector3 position;
        public Quaternion rotation;
        public double timeStamp;
        public float focal;
        public float fx;
        public float fy;
        public float cx;
        public float cy;
        public float width;
        public float height;
    }

    // RGB_1920x1080 = 0, ///< RGB 1080p
    // RGB_1280x720  = 1, ///< RGB 720p
    // RGB_640x480   = 2, ///< RGB 480p
    // RGB_320x240   = 3, ///< RGB QVGA
    // RGB_2560x1920 = 4, ///< RGB 5m
    public enum RgbResolution
    {
        RGB_1920x1080 = 0,
        RGB_1280x720 = 1,
        RGB_640x480 = 2,
        RGB_320x240 = 3,
        RGB_2560x1920 = 4
    }
}