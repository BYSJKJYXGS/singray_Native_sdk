using System;
using UnityEngine;
using XvXR.Engine;
using static API;
using Quaternion = UnityEngine.Quaternion;

namespace XvXR.Foundation
{
    [Serializable]
    public class XvARCameraParameter: XvCameraParameterSetting {
        public RgbResolution rgbResolution;
     
        public int fps;
    }
    public  class XvARCamera : XvCameraBase
    {
        public XvARCamera(XvARCameraParameter cameraParameter, FrameArrived frameArrived) : base(cameraParameter, frameArrived)
        {
            this.cameraParameter = cameraParameter;
        }
        
        private Texture2D tex = null;
       
        private byte[] pixelBytes;

        private double rgbTimestamp = 0;
        private int lastWidth = 0;
        private int lastHeight = 0;
        private int countTime = 0;

        private XvARCameraParameter cameraParameter;


        public bool needOpenCamera;

        int count = 0;

        public override void StartCapture()
        {
            switch (cameraParameter.rgbResolution)
            {
                case RgbResolution.RGB_1920x1080:
                    API.xslam_set_rgb_resolution(0);

                    break;
                case RgbResolution.RGB_1280x720:
                    API.xslam_set_rgb_resolution(1);

                    break;
                case RgbResolution.RGB_640x480:
                    API.xslam_set_rgb_resolution(2);

                    break;
                case RgbResolution.RGB_320x240:
                    break;
                case RgbResolution.RGB_2560x1920:
                    break;
                default:
                    break;
            }

          

            needOpenCamera = true;

            //API.xslam_rgb_set_exposure(1, 0, 5);

        }
        public override void StopCapture()
        {

            if (IsOpen)
            {
                needOpenCamera = false;

                API.xslam_stop_rgb_stream();

                isOpen = false;

               
            }
            isOpen = false;


        }
        public override void Update()
        {

#if UNITY_EDITOR
            return;
#endif
            if (API.xslam_ready())
            {
                count++;
                if (count < 100)
                {
                    return;
                }


                if (needOpenCamera)
                {
                    if (!isOpen)
                    {
                        //// Stop streams due to firmware not stable
                        MyDebugTool.Log("XvisioDeviceManager stop streams");
                        API.xslam_stop_rgb_stream();

                        //if (!API.xslam_set_rgb_source(1))
                        //{
                        //    MyDebugTool.Log("XvisioDeviceManager set rgb source faild");
                        //}
                        //else
                        //{
                        //    MyDebugTool.Log("XvisioDeviceManager set rgb source success");
                        //}

                        //if (!API.xslam_set_rgb_resolution(0))
                        //{
                        //    MyDebugTool.Log("XvisioDeviceManager set rgb resolustion faild");
                        //}
                        //else
                        //{
                        //    MyDebugTool.Log("XvisioDeviceManager set rgb resolustion success");
                        //}
                        //// Start image streams
                        MyDebugTool.Log("XvisioDeviceManager start xslam_start_rgb_stream");
                        API.xslam_start_rgb_stream();

                        isOpen = true;
                    }
                }

            }
            else
            {
                if (XvXRSdkConfig.XvXR_PLATFORM == XvXRSdkConfig.PLATFORM.XvXR_UNITY_EDITOR)
                {
                    API.xslam_init();

                }
            }

            if (isOpen)
            {
                if (API.xslam_ready())
                {

                    if (!readRgbCalibrationFlag)
                    {
                        ReadRgbCalibration();
                    }

                    int width = API.xslam_get_rgb_width();
                    int height = API.xslam_get_rgb_height();

                    if (width > 0 && height > 0)
                    {

                        if (lastWidth != width || lastHeight != height)
                        {
                            try
                            {
                                double r = 1.0;
                                /*if (width < 1280 && height < 720) {
                                    r = 1.0;
                                }*/
                                int w = (int)(width * r);
                                int h = (int)(height * r);
                                MyDebugTool.Log("Create RGB texture " + w + "x" + h);
                                TextureFormat format = TextureFormat.RGBA32;
                                tex = new Texture2D(w, h, format, false);


                                cameraData.texWidth = w;
                                cameraData.texHeight = h;


                                //tex.filterMode = FilterMode.Point;
                                tex.Apply();


                                pixelBytes = new byte[w * h * 4];
                                
                            }
                            catch (Exception e)
                            {

                                return;
                            }

                            lastWidth = width;
                            lastHeight = height;
                        }

                        countTime++;

                        if (countTime == 2)
                        {
                            countTime = 0;
                            return;
                        }
                        try
                        {
                            if (API.xslam_get_rgb_image_RGBA_Byte(pixelBytes, tex.width, tex.height, ref rgbTimestamp))
                            {
                                MyDebugTool.Log("xslam_get_rgb_image_RGBA_Byte texture true");

                                tex.SetPixelData(pixelBytes,0,0);
                                tex.Apply();

                                cameraData.tex = tex;
                              
                                cameraData.parameter.timeStamp = rgbTimestamp;


                                if (rgbTimestamp > 0)
                                {
                                    if (API.xslam_get_pose_at(_poseData, rgbTimestamp))
                                    {
                                        cameraData.parameter.rotation = new Quaternion(-(float)_poseData[0], (float)_poseData[1], -(float)_poseData[2], (float)_poseData[3])*offsetRotation;
                                        cameraData.parameter.position = new Vector3((float)_poseData[4], -(float)_poseData[5], (float)_poseData[6])+ offsetPosition;

                                        if (XvXRSdkConfig.XvXR_PLATFORM == XvXRSdkConfig.PLATFORM.XvXR_UNITY_ANDROID && XvXRManager.SDK.IsUseUserPose)
                                        {
                                            Quaternion quaternion = cameraData.parameter.rotation;
                                            ((XvXRAndroidDevice)XvXRManager.SDK.GetDevice()).SetSrcQuaternionUnity(quaternion, (long)rgbTimestamp * 1000000);
                                            //Debug.Log("IsUseUserPose");
                                        }
                                    }
                                    else
                                    {
                                        MyDebugTool.Log("RGBRecord xslam_get_pose_at faild");
                                    }
                                }



                                frameArrived?.Invoke(cameraData);

                            }
                            else
                            {
                                MyDebugTool.Log("Invalid texture");
                            }
                        }
                        catch (Exception e)
                        {
                            MyDebugTool.LogError(e);
                            return;
                        }
                       // MyDebugTool.Log("====================================");
                    }
                }
            }
        }

        private bool readRgbCalibrationFlag = false;
        private double[] _R;
        private double[] _T;
        //左眼的欧拉角
        private double[] _EulerAngles;

        private double[] _poseData = new double[7];

        private Vector3 offsetPosition;
        private Quaternion offsetRotation;

        void ReadRgbCalibration()
        {
            API.rgb_calibration rgb_Calibration = default(API.rgb_calibration);
            if (API.readRGBCalibration(ref rgb_Calibration))
            {
                float near = 0.3f;
                float far = 1000f;
                MyDebugTool.Log("readRGBCalibration");
                pdm pdm = rgb_Calibration.intrinsic720;



                switch (cameraParameter.rgbResolution)
                {
                    case RgbResolution.RGB_1920x1080:
                        pdm = rgb_Calibration.intrinsic1080;

                        break;
                    case RgbResolution.RGB_1280x720:
                        pdm = rgb_Calibration.intrinsic720;

                        break;
                    case RgbResolution.RGB_640x480:
                        pdm = rgb_Calibration.intrinsic480;

                        break;
                    case RgbResolution.RGB_320x240:
                        break;
                    case RgbResolution.RGB_2560x1920:
                        break;
                    default:
                        break;
                }
                

                //intrinsic720
                //Matrix4x4 proj = XvXR.Engine.XvXRBaseDevice.PerspectiveOffCenter((float)pdm.K[0], (float)pdm.K[1],
                //     (float)pdm.K[2], (float)pdm.K[3], (float)pdm.K[9], (float)pdm.K[10], near, far);

                //camera.fieldOfView = 2 * Mathf.Atan(1 / proj[1, 1]) * Mathf.Rad2Deg;
                //camera.projectionMatrix = proj;

                _T = new double[3] { rgb_Calibration.extrinsic.translation[0], -rgb_Calibration.extrinsic.translation[1], rgb_Calibration.extrinsic.translation[2] };

                //左眼标定的旋转矩阵→欧拉角
                _R = new double[9] { rgb_Calibration.extrinsic.rotation[0], -rgb_Calibration.extrinsic.rotation[1], rgb_Calibration.extrinsic.rotation[2], -rgb_Calibration.extrinsic.rotation[3], rgb_Calibration.extrinsic.rotation[4],
                            -rgb_Calibration.extrinsic.rotation[5],rgb_Calibration.extrinsic.rotation[6],-rgb_Calibration.extrinsic.rotation[7],rgb_Calibration.extrinsic.rotation[8]};
                XvXR.Engine.XvXREye.RotationMatrixToEulerAngles(ref _EulerAngles, _R);



                offsetPosition = new Vector3((float)_T[0], (float)_T[1], (float)_T[2]);
                    Vector3 localEuler = new Vector3((float)_EulerAngles[0], (float)_EulerAngles[1], (float)_EulerAngles[2]);

                    offsetRotation = Quaternion.Euler(localEuler);

                    //设置Physical camera
                cameraData.parameter. focal = 3.519f;//RGB相机焦距（单位为毫米）
                cameraData.parameter.fx = (float)pdm.K[0];
                cameraData.parameter.fy = (float)pdm.K[1];
                cameraData.parameter.cx = (float)pdm.K[2];
                cameraData.parameter.cy = (float)pdm.K[3];

                cameraData.parameter.width = (float)pdm.K[9];
                cameraData.parameter.height = (float)pdm.K[10];

               

                MyDebugTool.Log("RGBRecord ReadRgbCalibration:" + rgb_Calibration);

                MyDebugTool.Log("RGBRecord ReadRgbCalibration camera Fx,Fy: " +
                    pdm.K[0] + "," + pdm.K[1]);

                readRgbCalibrationFlag = true;



            }
            else
            {
                MyDebugTool.Log("RGBRecord readStereoFisheyesCalibration faild");
            }
        }
    }
}