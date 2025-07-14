using UnityEngine;
using System.Runtime.InteropServices;
using System;
namespace XvXR.Foundation
{
    public enum TofFramerate
    {
        FPS_5,
        FPS_10,
        FPS_15,
        FPS_20,
        FPS_25,
        FPS_30
    }
    public enum SonyTofLibMode
    {
        IQMIX_DF,
        IQMIX_SF,
        LABELIZE_DF,
        LABELIZE_SF,
        M2MIX_DF,
        M2MIX_SF
    }
    public enum TofResolution
    {
        Unknown = -1,
        VGA,
        QVGA,
        HQVGA,

    }

    public enum TofStreamMode
    { 
    
    DepthOnly=0,//0:
    CloudOnly=1,//1:
    DepthAndCloud=2,//2:
    None=3,//3
    CloudOnLeftHandSlam=4,//4:
    }
    [Serializable]
    public class XvTofCameraParameter : XvCameraParameterSetting
    {


        [HideInInspector]
        public TofStreamType streamType;
        public TofFramerate tofFramerate;
        public SonyTofLibMode sonyTofLibMode;
        public TofResolution tofResolution;
        public TofStreamMode tofStreamMode;



        public bool enableGamma;//IRÍ¼Ïñ¿ÉÓÃ
    }
    public class XvTofCamera : XvCameraBase
    {

        public XvTofCamera(XvTofCameraParameter cameraParameter, FrameArrived frameArrived) : base(cameraParameter, frameArrived)
        {
            this.cameraParameter = cameraParameter;

        }

        private Texture2D tex = null;


        private Color32[] pixel32;
        private GCHandle pixelHandle;
        private IntPtr pixelPtr;
        private XvTofCameraParameter cameraParameter;

        public override void StartCapture()
        {
            if (!isOpen) {

                isOpen = true;
            }
        }

        public override void StopCapture()
        {
            if (isOpen) {
                pixelHandle.Free();
                tex = null;
                
            }
            isOpen = false;

        }
        public override void Update()
        {
            if (isOpen&&API.xslam_ready() )
            {
                int width = API.xslam_get_tof_width();
                int height = API.xslam_get_tof_height();

                if (width > 0 && height > 0)
                {

                    if (!tex)
                    {
                        MyDebugTool.Log("Create TOF texture " + width + "x" + height);
                        TextureFormat format = TextureFormat.RGBA32;
                        tex = new Texture2D(width, height, format, false);


                        pixel32 = tex.GetPixels32();
                        pixelHandle = GCHandle.Alloc(pixel32, GCHandleType.Pinned);
                        pixelPtr = pixelHandle.AddrOfPinnedObject();


                    }


                    if (API.xslam_get_tof_image(pixelPtr, tex.width, tex.height))
                    {
                        //Update the Texture2D with array updated in C++
                        tex.SetPixels32(pixel32);
                        tex.Apply();
                        cameraData.tex = tex;
                        cameraData.texWidth = tex.width;

                        cameraData.texHeight = tex.height;
                        

                        frameArrived?.Invoke(cameraData);
                    }
                    else
                    {
                        MyDebugTool.Log("Invalid TOF texture");
                    }



                }
            }
        }
      
       
    }
}
