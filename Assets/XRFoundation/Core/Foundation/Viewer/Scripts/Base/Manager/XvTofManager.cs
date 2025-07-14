

using UnityEngine;
using XvXR.Engine;
using static XvXR.Foundation.XvCameraBase;

namespace XvXR.Foundation
{

    public enum TofStreamType { 
        Unknown = 0,
        DeapthStream,//深度图像
        IRStream,//IR图像
    
    }
    /// <summary>
    /// 深度图像
    /// </summary>
    public class XvTofDepth
    {
        private XvCameraBase frameBase;
       
        public bool IsOn { get; private set; }
        public void StartCapture(XvTofCameraParameter xvTofCameraParameter)
        {
#if UNITY_EDITOR
            return;
#endif
            if (IsOn)
            {
                return;
            }

           // StopCapture();

            if (frameBase==null) { 
            
            frameBase = new XvTofCamera(xvTofCameraParameter, FrameArrived);
            }
            IsOn = true;

            //if (XvTofManager.GetXvTofManager().modelSet == false)
            //{
            //    XvTofManager.GetXvTofManager().SetTofStreamMode(2);
            //}
            XvTofManager.GetXvTofManager().StartTofStream(xvTofCameraParameter);
            frameBase.StartCapture();
        }

        public void StopCapture()
        {
#if UNITY_EDITOR
            return;
#endif
            if (frameBase != null && frameBase.IsOpen)
            {
                frameBase.StopCapture();
                XvTofManager.GetXvTofManager().StopTofStream();
            }

            frameBase = null;
            // GC.Collect();

            IsOn = false;
        }

        private void FrameArrived(cameraData cameraData)
        {
            XvCameraManager.onTofDepthCameraStreamFrameArrived?.Invoke(cameraData);
        }

        public void Update() {
            frameBase?.Update();
        }

    }
    /// <summary>
    /// IR图像
    /// </summary>
    public class XvTofIR {


        private XvCameraBase frameBase;

        public bool IsOn
        {
            get;
            private set;
        }
        public void StartCapture(XvTofCameraParameter xvTofIRCameraParameter)
        {
            MyDebugTool.Log("StartIRCapture == 1:");
#if UNITY_EDITOR
            return;
#endif
            if (IsOn)
            {
                return;
            }
            MyDebugTool.Log("StartIRCapture");
           // StopCapture();

            if (frameBase==null) { 
            
            frameBase = new XvTofIRCamera(xvTofIRCameraParameter, FrameArrived);
            }
            IsOn = true;

            //if (XvTofManager.GetXvTofManager().modelSet == false)
            //{
            //    XvTofManager.GetXvTofManager().SetTofStreamMode(2);
            //}

            XvTofManager.GetXvTofManager().StartTofIRStream(xvTofIRCameraParameter);
            frameBase.StartCapture();
        }

        public void StopCapture()
        {
#if UNITY_EDITOR
            return;
#endif
            if (frameBase != null && frameBase.IsOpen)
            {
                frameBase.StopCapture();
                XvTofManager.GetXvTofManager(). StopTofStream();
            }

            frameBase = null;
            // GC.Collect();

            IsOn = false;
        }

        private void FrameArrived(cameraData cameraData)
        {
            XvCameraManager.onTofIRCameraStreamFrameArrived?.Invoke(cameraData);
        }


        public void Update()
        {
            
          

            frameBase?.Update();
        }
      


       

    }

  
    #region tof
    public sealed class XvTofManager
    {

        private static XvTofManager xvTofManager;

        public static XvTofManager GetXvTofManager()
        {
            if (xvTofManager == null)
            {
                xvTofManager = new XvTofManager();

            }
            return xvTofManager;
        }

        private XvTofManager() { }
        public bool modelSet = false;

        private XvTofDepth xvTofDepth = null;
        private XvTofIR xvTofIR = null;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="streamType">0: deapth  1:IR </param>
        public void StartCapture(XvTofCameraParameter xvTofCameraParameter)
        {
            MyDebugTool.Log("StartCapture:"+ xvTofCameraParameter.streamType);

            switch (xvTofCameraParameter.streamType)
            {
                case TofStreamType.Unknown:
                    break;
                case TofStreamType.DeapthStream:
                    if (xvTofDepth == null)
                    {
                        xvTofDepth = new XvTofDepth();
                    }

                    SetTofStreamMode((int)xvTofCameraParameter.tofStreamMode);
                    xvTofDepth.StartCapture(xvTofCameraParameter);
                    break;
                case TofStreamType.IRStream:
                    if (xvTofIR == null)
                    {
                        xvTofIR = new XvTofIR();
                    }
                    MyDebugTool.Log("tofImageType == 1:" + xvTofCameraParameter.streamType);
                    SetTofStreamMode((int)xvTofCameraParameter.tofStreamMode);

                    xvTofIR.StartCapture(xvTofCameraParameter);
                    break;
                default:
                    break;
            }
            
        }

        public void StopCapture(TofStreamType streamType) {
            switch (streamType)
            {
                case TofStreamType.Unknown:
                    break;
                case TofStreamType.DeapthStream:
                    if (xvTofDepth != null)
                    {
                        xvTofDepth.StopCapture();

                    }
                    break;
                case TofStreamType.IRStream:
                    if (xvTofIR != null)
                    {
                        xvTofIR.StopCapture();

                    }
                    break;
                default:
                    break;
            }
           
        }

        public bool IsOn(TofStreamType streamType) {
            switch (streamType)
            {
                case TofStreamType.Unknown:
                    break;
                case TofStreamType.DeapthStream:
                    if (xvTofDepth != null)
                    {
                        return xvTofDepth.IsOn;

                    }
                    break;
                case TofStreamType.IRStream:
                    if (xvTofIR != null)
                    {
                        return xvTofIR.IsOn;

                    }
                    break;
                default:
                    break;
            }
           
            return false;
        }

        /// <summary>
        /// 开启tof深度相机流
        /// </summary>
        public void StartTofStream(XvTofCameraParameter xvTofIRCameraParameter)
        {
            API.xslam_start_tof_stream();
            API.xslam_start_sony_tof_stream((int)xvTofIRCameraParameter.sonyTofLibMode, (int)xvTofIRCameraParameter.tofResolution, (int)xvTofIRCameraParameter.tofFramerate);

        }

        /// <summary>
        /// 开启tof IR相机流
        /// </summary>
        public void StartTofIRStream(XvTofCameraParameter xvTofIRCameraParameter)
        {
            API.xslam_start_tofir_stream((int)xvTofIRCameraParameter.sonyTofLibMode, (int)xvTofIRCameraParameter.tofResolution, (int)xvTofIRCameraParameter.tofFramerate);

            API.xslam_tof_enbale_ir_gramma(xvTofIRCameraParameter.enableGamma);
   
        }

        /// <summary>
        /// 设置当前tof 数据流模式
        /// </summary>
        /// <param name="mode"></param>
      public void SetTofStreamMode(int mode) {

            if (!modelSet)
            {
                API.xslam_stop_tof_stream();
                API.xslam_tof_set_steam_mode(mode);
            }
            modelSet = true;
        }
        /// <summary>
        /// 停止tof相机流
        /// </summary>

        public void StopTofStream()
        {
            API.xslam_stop_tof_stream();
            modelSet = false;
        }

        public void Update()
        {
            
#if UNITY_EDITOR
            return;
#endif
            if (xvTofDepth!=null)
            {
                xvTofDepth.Update();

            }
            if (xvTofIR!=null)
            {

                xvTofIR.Update();
            }

        }
    }

    #endregion
}