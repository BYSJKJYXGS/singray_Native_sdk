
using UnityEngine;
using static API;

namespace XvXR.Foundation
{

    /// <summary>
    /// ������Ҫʵ��rgbd���ܣ�ͨ������������û��߹ر�rgbd���ܣ����Ը���rgb���������ȡ����ά�ռ�����
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
        /// ����RGBD����
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
        /// �ر�RGBD����
        /// </summary>
        public void StopRgbPose() {
#if  UNITY_EDITOR
            return;

#endif
            API.xv_stop_rgb_pixel_pose();
            CameraManager.StopCapture(XvCameraStreamType.ARCameraStream);
            XvCameraManager.onARCameraStreamFrameArrived.RemoveListener(onFrameArrived);


        }


        /// <summary>
        /// ͨ��rgb���������ȡ�ؼ���ά����
        /// </summary>
        /// <param name="rgbPoint">rgb ��������</param>
        /// <param name="spacePoint">�ռ���ά����</param>
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
        /// ����rgb���������б���ȡ��ά�ռ��������б�
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
