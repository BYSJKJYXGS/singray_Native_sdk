using UnityEngine;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using XvXR.SystemEvents;
using System;
using XvXR.utils;
using Assets.XvXRScripts.Engine;

namespace XvXR.Engine
{
    public abstract class XvXRMobileDevice:XvXRBaseDevice
    {

        protected const int jniEnvInitEventId = 0x66660;//call java init the jni evn for android

        protected const int renderEventId = 0x666666;//render draw event 

        protected const int initRenderEventId = 0x666667;//init render

        protected const int changeRenderDataEventId = 0x666668;//change render data

        protected const int copyRederEventId = 0x7666666;

        protected int lastLeftId = 0;

        protected int lastRightId = 0;


        public abstract void  ReadConfigInfo();
 
        public override void UpdateScreenData()
        {
            XvXRLog.InternalXvXRLog("mobile test UpdateScreenData");
            ReadConfigInfo();
            ComputeEyesFromProfile();
            ChangeRenderData();


        }

        public override void SetStereoScreen(RenderTexture leftRenderTexture, RenderTexture rightRenderTexture)
        {


            lastLeftId = leftRenderTexture != null ? (int)leftRenderTexture.GetNativeTexturePtr() : 0;
            lastRightId = rightRenderTexture != null ? (int)rightRenderTexture.GetNativeTexturePtr() : 0;


            SetTextureIdMobile();
        }

        protected void CheckTextureId()//has the problem 
        {
            if (XvXRManager.SDK.StereoScreen != null)
            {
                if (lastLeftId != (int)XvXRManager.SDK.StereoScreen[0].GetNativeTexturePtr() || lastRightId != (int)XvXRManager.SDK.StereoScreen[1].GetNativeTexturePtr())
                {
                    SetStereoScreen(XvXRManager.SDK.StereoScreen[0], XvXRManager.SDK.StereoScreen[1]);
                }
            }
        }
        public override void SetDistortionCorrectionEnabled(bool enabled)
        {

        }

        public override void Recenter()
        {

        }


        public void ChangeRenderData()
        {
            XvXRLog.InternalXvXRLog("ChangeRenderData");
            SetRenderDataMobile();
            GL.IssuePluginEvent(RenderEventFunc(), changeRenderDataEventId);

        }

        public override void PostRender()
        {
            GL.IssuePluginEvent(RenderEventFunc(), renderEventId);
        }

        public void InitRenderEvent()
        {
            InitRenderMobile(1);//0: backbuffer ; 1 singlebuffer
            GL.IssuePluginEvent(RenderEventFunc(), initRenderEventId);
        }


        internal abstract void InitRenderMobile(int bufferMode);

        internal abstract void SetRenderDataMobile();

        internal abstract void SetTextureIdMobile();

        internal abstract IntPtr RenderEventFunc();




    }
}
