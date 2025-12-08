using AOT;
using Microsoft.MixedReality.Toolkit.Utilities;
using System;
using System.IO;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Events;
using XvXR.Engine;
namespace XvXR.Foundation
{
    using static XvEyeTracking;

  
    public sealed class XvEyeTrackingManager : MonoBehaviour
    {

        private XvEyeTrackingManager() { }
        private static string config_path = "/data/misc/xr/";
      

        Matrix4x4 MatrixHead = Matrix4x4.identity;
        Matrix4x4 MatrixMiddleOfEyes = Matrix4x4.identity;
        private Matrix4x4 middleOfEyeToHeadMatrix;

        public Matrix4x4 MiddleOfEyeToHeadMatrix
        {
            get { return middleOfEyeToHeadMatrix; }
        }

     
        private static bool tracking;
        public bool Tracking
        {
            get
            {
                return tracking;
            }
        }

        public XV_ET_EYE_DATA_EX EyeData
        {
            get
            {
                return eyeData;
            }
        }

      
        public Vector3 GazeOrigin
        {
            get
            {

                return GetGazePoint(eyeData.recomGaze.gazeOrigin);
            }
        }

       
        public Vector3 GazeDirection
        {
            get
            {
                return GetDirection(eyeData.recomGaze.gazeOrigin, eyeData.recomGaze.gazeDirection);
            }
        }



       
        public Vector3 LeftGazeOrigin
        {
            get
            {
                return GetGazePoint(eyeData.leftGaze.gazeOrigin);
            }
        }

   
        public Vector3 LeftGazeDirection
        {
            get
            {
                return GetDirection(eyeData.leftGaze.gazeOrigin, eyeData.leftGaze.gazeDirection); ;
            }
        }



        public Vector3 RightGazeOrigin
        {
            get
            {
                return GetGazePoint(eyeData.rightGaze.gazeOrigin);

            }
        }


        public Vector3 RightGazeDirection
        {
            get
            {
                return GetDirection(eyeData.rightGaze.gazeOrigin, eyeData.rightGaze.gazeDirection); ;
            }
        }

        public float Ipd {
            get
            {
                return eyeData.ipd;
            }

        }
        




        public void StartGaze()
        {

#if UNITY_EDITOR
            return;
#endif
            if (!tracking)
            {
                 
                xslam_gaze_set_config_path(config_path);
                MyDebugTool.Log($"XVETinit config_path:{config_path}");

                bool b_start_gaze = xslam_start_gaze();
                MyDebugTool.Log("XVETinit b_start_gaze:" + b_start_gaze);

                string path = "/data/misc/xr/XVETcaliData_" + XvXR.SystemEvents.AndroidConnection.getLoginUser() + ".dat";
                //string path = "/sdcard/XVETcaliData.dat";
                int apply = xslam_gaze_calibration_apply(path);
                MyDebugTool.Log($"XVETinit xslam_gaze_calibration_apply:{apply},path:{path}");



                bool b_set_exposure = xslam_set_exposure(50, 7, 50, 7);
                MyDebugTool.Log("XVETinit b_set_exposure:" + b_set_exposure);
                //bool b_set_bright = xslam_set_bright(2, 8, 27);
                //MyDebugTool.Log("XVETinit b_set_bright:" + b_set_bright);

                int b_set_gaze_callback = xslam_set_gaze_callback(OnStartSkeletonCallback);
                MyDebugTool.Log("XVETinit b_set_gaze_callback:" + b_set_gaze_callback);


            }
        }


    
        public void StopGaze()
        {
#if UNITY_EDITOR
            return;
#endif

            if (Tracking)
            {

              

                bool unset = xslam_unset_gaze_callback();
                Debug.Log($" xslam_unset_gaze_callback:{unset}");
                bool b = xslam_stop_gaze();
                Debug.Log($" xslam_stop_gaze:{b}");

                tracking = false;
            }
        }

        public static XV_ET_EYE_DATA_EX eyeData = new XV_ET_EYE_DATA_EX();

        [MonoPInvokeCallback(typeof(fn_gaze_callback))]
        private static void OnStartSkeletonCallback(XV_ET_EYE_DATA_EX gazedata)
        {
            tracking = true;
            MyDebugTool.Log($"OnStartSkeletonCallback");
            eyeData = gazedata;

            MyDebugTool.Log($"XVETmanager ipd:{gazedata.ipd}");
            MyDebugTool.Log($"XVETmanager eyeData leftEyeMove:{eyeData.leftEyeMove}");
            MyDebugTool.Log($"XVETmanager eyeData rightEyeMove:{eyeData.rightEyeMove}");
        }


       

        private void UpdateMatrix()
        {
            MatrixHead.SetTRS(Camera.main.transform.position, Camera.main.transform.rotation, Vector3.one);

            API.stereo_pdm_calibration fed = XvXRManager.SDK.GetDevice().GetFed();

            Vector3 LeftdisplayPos = new Vector3((float)fed.calibrations[0].extrinsic.translation[0], -(float)fed.calibrations[0].extrinsic.translation[1], (float)fed.calibrations[0].extrinsic.translation[2]);
            Vector3 RightdisplayPos = new Vector3((float)fed.calibrations[1].extrinsic.translation[0], -(float)fed.calibrations[1].extrinsic.translation[1], (float)fed.calibrations[1].extrinsic.translation[2]);

            Vector3 normal = (RightdisplayPos - LeftdisplayPos).normalized;
            float distance = Vector3.Distance(LeftdisplayPos, RightdisplayPos);
            Vector3 middleOfEyes_pos = normal * (distance * 0.5f) + LeftdisplayPos;


            DebugLog($"middleOfEyes_pos:" + middleOfEyes_pos);

            Quaternion middleQua = RotationMatrixToQuaternion(fed.calibrations[0].extrinsic.rotation);

            MatrixMiddleOfEyes.SetTRS(middleOfEyes_pos, new Quaternion(-middleQua.x, middleQua.y, -middleQua.z, middleQua.w), Vector3.one);

            middleOfEyeToHeadMatrix = MatrixHead * MatrixMiddleOfEyes;
        }

        private Vector3 GetGazePoint(XV_ETPoint3D oGazeOrigin)
        {
            Vector3 gazeOrigin = new Vector3(oGazeOrigin.x, -oGazeOrigin.y, oGazeOrigin.z) / 1000;



            Matrix4x4 Matrix_gazeOrigin = Matrix4x4.identity;
            Matrix4x4 Matrix_XVgazeOrigin = Matrix4x4.identity;
            Matrix_gazeOrigin.SetTRS(gazeOrigin, Quaternion.identity, Vector3.one);
            Matrix_XVgazeOrigin = middleOfEyeToHeadMatrix * Matrix_gazeOrigin;

            gazeOrigin = Matrix_XVgazeOrigin.GetColumn(3);

            return gazeOrigin;

        }
        private Vector3 GetDirection(XV_ETPoint3D oGazeOrigin, XV_ETPoint3D oGazeDirection)
        {
            Vector3 gazeOrigin = new Vector3(oGazeOrigin.x, -oGazeOrigin.y, oGazeOrigin.z) / 1000;
            Vector3 gazeDirection = gazeOrigin + new Vector3(oGazeDirection.x, -oGazeDirection.y, oGazeDirection.z) * 15;
            Matrix4x4 Matrix_target = Matrix4x4.identity;
            Matrix4x4 Matrix_XVtarget = Matrix4x4.identity;

            Matrix_target.SetTRS(gazeDirection, Quaternion.identity, Vector3.one);

            Matrix_XVtarget = middleOfEyeToHeadMatrix * Matrix_target;
            gazeDirection = (Vector3)Matrix_XVtarget.GetColumn(3) - GetGazePoint(oGazeOrigin);

            return gazeDirection;

        }

      

        private Quaternion RotationMatrixToQuaternion(double[] rm)
        {
            double w, x, y, z;
            //if (1 + rm[0] + rm[4] + rm[8]>0&& (Math.Sqrt(1 + rm[0] + rm[4] + rm[8]) / 2)!=0)
            {
                w = Math.Sqrt(1 + rm[0] + rm[4] + rm[8]) / 2;
                x = (rm[7] - rm[5]) / (4 * w);
                y = (rm[2] - rm[6]) / (4 * w);
                z = (rm[3] - rm[1]) / (4 * w);
            }
            Quaternion qua = new Quaternion((float)x, (float)y, (float)z, (float)w);
            return qua;
        }

        private void DebugLog(object msg)
        {
             MyDebugTool.Log(msg);
        }



        #region GazeCalibration


        public void GazeCalibrationEnter()
        {
            XvEyeTracking.xslam_gaze_calibration_enter();
        }

       
        public int GazeCalibrationCollect(Vector3 point, int index)
        {
            return XvEyeTracking.xslam_gaze_calibration_collect(point.x, point.y, point.z, index);
        }

        
        public bool UnsetGazeCallback()
        {
            return XvEyeTracking.xslam_unset_gaze_callback();
        }
       
        public int CalibrationComplete()
        {
            int setup = xslam_gaze_calibration_setup();
            MyDebugTool.Log($" xslam_gaze_calibration_setup:{setup}");

            int compute_apply = xslam_gaze_calibration_compute_apply();
            MyDebugTool.Log($" xslam_gaze_calibration_compute_apply:{compute_apply}");

            int leave = xslam_gaze_calibration_leave();
            MyDebugTool.Log($" xslam_gaze_calibration_leave:{leave}");

            string path = "/data/misc/xr/XVETcaliData_" + XvXR.SystemEvents.AndroidConnection.getLoginUser() + ".dat";

            if (File.Exists(path))
            {
                File.Delete(path);
            }

            int retrieve = xslam_gaze_calibration_retrieve(path);
            MyDebugTool.Log($" xslam_gaze_calibration_retrieve:{retrieve}");

            sys_chmod(path, _0755);
            return retrieve;

        }

        // user permissions
        const int S_IRUSR = 0x100;
        const int S_IWUSR = 0x80;
        const int S_IXUSR = 0x40;

        // group permission
        const int S_IRGRP = 0x20;
        const int S_IWGRP = 0x10;
        const int S_IXGRP = 0x8;

        // other permissions
        const int S_IROTH = 0x4;
        const int S_IWOTH = 0x2;
        const int S_IXOTH = 0x1;

        const int _0755 = S_IRUSR | S_IXUSR | S_IWUSR | S_IRGRP | S_IXGRP | S_IROTH | S_IXOTH;
        [System.Runtime.InteropServices.DllImport("libc", EntryPoint = "chmod", SetLastError = true)]
        private static extern int sys_chmod(string path, int mode);
        #endregion

        #region 1

        private Texture2D lefttex = null;
        private Texture2D righttex = null;
        private Color32[] pixel32_Left;
        private Color32[] pixel32_Right;
        private GCHandle pixelHandle_Left;
        private GCHandle pixelHandle_Right;
        private IntPtr pixelPtr_Left;
        private IntPtr pixelPtr_Right;
        private  bool isGetEyeImage = false;
        int width;
        int height;
        private EyeCameraData eyeCameraData = new EyeCameraData();

        public static UnityEvent<EyeCameraData> onEyeCameraStreamFrameArrived = new UnityEvent<EyeCameraData>();

        public class EyeCameraData
        {
            public int texWidth;
            public int texHeight;
            public Texture leftTex;
            public Texture rightTex;
            public CameraParameter parameter;

        }


        public   bool StartCapture() {

            if (isGetEyeImage==false) { 
            isGetEyeImage = true;
             return xv_eyetracking_start();
            }

            return true;

        }


        public  bool StopCapture() {

            if (isGetEyeImage) { 
            
              isGetEyeImage = false;
                return xv_eyetracking_stop();
            }
            return true;

        }


        private  void GetEyetrackingRGBA() {
            if (!isGetEyeImage) {
                return;
            }

            if ( API.xslam_ready())
            {
                Debug.Log($"width:{width},height:{height}");

                if (xv_eyetracking_get_rgba(pixelPtr_Left, pixelPtr_Right, ref width, ref height))
                {

                    if (width != 0 && height != 0)
                    {
                        TextureFormat format = TextureFormat.RGBA32;

                        if (!lefttex)
                        {
                            lefttex = new Texture2D(width, height, format, false);
                            pixel32_Left = lefttex.GetPixels32();
                            pixelHandle_Left = GCHandle.Alloc(pixel32_Left, GCHandleType.Pinned);
                            pixelPtr_Left = pixelHandle_Left.AddrOfPinnedObject();
                        }
                        if (!righttex)
                        {
                            righttex = new Texture2D(width, height, format, false);
                            pixel32_Right = righttex.GetPixels32();
                            pixelHandle_Right = GCHandle.Alloc(pixel32_Right, GCHandleType.Pinned);
                            pixelPtr_Right = pixelHandle_Right.AddrOfPinnedObject();
                        }

                        DebugUtilities.Log($"xv_eyetracking_get_rgba");
                        lefttex.SetPixels32(pixel32_Left);
                        lefttex.Apply();
                        righttex.SetPixels32(pixel32_Right);
                        righttex.Apply();

                        eyeCameraData.leftTex = lefttex;
                        eyeCameraData.rightTex = righttex;
                        eyeCameraData.texWidth = width;
                        eyeCameraData.texHeight = height;

                        onEyeCameraStreamFrameArrived?.Invoke(eyeCameraData);

                    }
                    else
                    {
                        DebugUtilities.Log("GetEyeImage Invalid texture");
                    }



                }
                else
                {

                    DebugUtilities.Log("GetEyeImage xv_eyetracking_get_rgba Invalid texture");
                }

            }
            
        }

        #endregion

# region Unity

        private void Update()
        {

            if (!Tracking)
            {
                return;
            }

            UpdateMatrix();

            GetEyetrackingRGBA();

        }
        private void OnDestroy()
        {
            StopGaze();
        }
       

        private void OnApplicationPause(bool isPause)
        {
            if (isPause)
            {

                if (Tracking) { 
                
                StopGaze();
                }

                if (isGetEyeImage) { 
                  StopCapture();
                }
            }
        }

        #endregion
    }
}