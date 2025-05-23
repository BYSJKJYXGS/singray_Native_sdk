﻿using UnityEngine;
using System.Collections;
namespace XvXR.Engine
{
    public class XvXRSdkConfig {

	    public enum PLATFORM{
		    XvXR_UNITY_EDITOR,
		    XvXR_UNITY_ANDROID,
            XvXR_UNITY_IOS,
		    XvXR_UNKOWN,
	    }

        public enum SDK_MODE
        {
            XvXR_UNITY_SRC_MODE,
            XvXR_UNITY_CLIENT_MODE
        }


        public static SDK_MODE sdkUseMode = SDK_MODE.XvXR_UNITY_CLIENT_MODE;

        public static bool isTurnOnZForReCenter = true;


        public static PLATFORM XvXR_PLATFORM {
		    get;
		    set;
	    }

        public static int isAberration=0;
        public static int isReverse = 0;
        public static int vignette = 1;
        public static int useAtw = 0;

        public static  RenderTextureFormat  textureFormat = RenderTextureFormat.RGB565;

        public static int textureDepth = 24;

        public static int MaxWidthChoose = 1280;
        //public static int MaxWidthChoose = 1920;

        public static int MinWidthChoose = 1280;

    }

}
