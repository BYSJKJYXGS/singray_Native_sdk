
using UnityEngine;
using static API;

namespace XvXR.Foundation
{

    /// <summary>
    /// This class mainly implements RGBD functionality. Through this class, you can enable or disable the RGBD feature, 
    /// and obtain 3D spatial coordinates based on RGB pixel coordinates.
    /// </summary>
    public sealed class XvRgbdManager : MonoBehaviour
    {
        private XvRgbdManager() { }


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


        private double hostTimestamp;


        /// <summary>
        /// Start RGB
        /// </summary>
        public void StartRgbPose() {
           
#if  UNITY_EDITOR
            return;

#endif
            while (!API.xslam_ready())
            {
                MyDebugTool.Log("xslam_ready==false");
            }
            MyDebugTool.Log("xslam_ready==true");
                API.xv_start_rgb_pixel_pose();
            CameraManager.StartCapture(XvCameraStreamType.ARCameraStream);
            XvCameraManager.onARCameraStreamFrameArrived.AddListener(onFrameArrived);

            //API.xv_start_rgb_pixel_pose();
            //CameraManager.StartCapture(XvCameraStreamType.ARCameraStream);
            //XvCameraManager.onARCameraStreamFrameArrived.AddListener(onFrameArrived);
        }
        /// <summary>
        /// StopRgbPose
        /// </summary>
        public void StopRgbPose() {
#if  UNITY_EDITOR
            return;

#endif
            API.xv_stop_rgb_pixel_pose();
            CameraManager.StopCapture(XvCameraStreamType.ARCameraStream);
            XvCameraManager.onARCameraStreamFrameArrived.RemoveListener(onFrameArrived);


        }


        /// <summary>/// Gets the 3D coordinate of the control via the RGB pixel coordinate.
        /// /// </summary>/// <param name="rgbPoint">RGB pixel coordinate</param>/// 
        /// <param name="spacePoint">Spatial 3D coordinate</param>///
        /// <returns></returns>
        public bool GetRgbPixel3DPose(Vector2 rgbPoint,ref Vector3 spacePoint)
        {


#if PLATFORM_ANDROID && !UNITY_EDITOR
  API.Vector2F rgbPixelPoint = default(API.Vector2F);
            rgbPixelPoint.x = rgbPoint.x;
            rgbPixelPoint.y = rgbPoint.y;

            API.Vector3F pointerPose = default(API.Vector3F);
            if (API.xv_get_rgb_pixel_pose(ref pointerPose, ref rgbPixelPoint, hostTimestamp, 30))
            {
                spacePoint.x = pointerPose.x;
                spacePoint.y = -pointerPose.y;
                spacePoint.z = pointerPose.z;
                return true;
            }
#endif

            return false;
        }
        /// <summary>
        /// Gets the 3D spatial coordinate list based on the RGB pixel coordinate list.
        /// </summary>
        /// <param name="rgbPoint"></param>
        /// <param name="spacePose"></param>
        /// <returns></returns>
        public bool GetRgbPixelPoseList( Vector2[] rgbPoint,ref pointer_3dpose[] spacePose)
        {


#if PLATFORM_ANDROID && !UNITY_EDITOR
  int size = rgbPoint.Length;
            if (API.xslam_start_get_rgb_pixel_buff3d_pose(spacePose, rgbPoint, size, hostTimestamp, 30))
            {

                for (int i = 0; i < spacePose.Length; i++)
                {
                    spacePose[i].pointerPose.y *= -1;
                }
                return true;
            }

          
#endif
            return false;

        }

        private void onFrameArrived(cameraData cameraData)
        {
            hostTimestamp = cameraData.parameter.timeStamp;
        }

    }
}
