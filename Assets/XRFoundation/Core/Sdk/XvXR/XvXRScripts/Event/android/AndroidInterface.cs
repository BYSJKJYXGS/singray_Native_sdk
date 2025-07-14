using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using XvXR.Engine;
using XvXR.utils;
namespace XvXR.SystemEvents
{

	internal class AndroidInterface : MonoBehaviour {


		private AndroidJavaObject interfaceObject;

		private AndroidJavaObject InterfaceObject{
			get{
				if(interfaceObject==null){
					AndroidJavaClass activityClass = new AndroidJavaClass("top.xv.xrlib.unity.XvMainActivity");
					AndroidJavaObject activityObject= activityClass.GetStatic<AndroidJavaObject>("currentActivity");
					if(activityObject!=null){
						interfaceObject=AndroidHelper.Create("top.xv.xrlib.unity.XvXRUnityInterface", new object[]{activityObject});
					}
				}
				return interfaceObject;
			}
		}


		private float[] lastQuaternion=new float[]{0,0,0,1f};

		private float[] lastPose = new float[] { 0, 0, 0, 1f, 0, 0, 0 };



		public float[] GetQuaternion(){
           
			float[] result=new float[4];
			bool q = AndroidHelper.CallObjectMethod<float[]> (ref result, InterfaceObject, "getQuaternion", new object[]{});
			if (!q) {
				return lastQuaternion;
			} else {
				lastQuaternion=result;
				return result;
			}
		}

		public float[] GetDevicePose()
        {
			float[] result = new float[7];
			bool q = AndroidHelper.CallObjectMethod<float[]>(ref result, InterfaceObject, "getDevicePose", new object[] { });
			if (!q)
			{
				return lastPose;
			}
			else
			{
				lastPose = result;
				return result;
			}
		}

        internal float[] GetXvXROpticalParameter()
        {
			float[] result = new float[14];
			bool q = AndroidHelper.CallObjectMethod<float[]>(ref result, InterfaceObject, "getXvXROpticalParameter", new object[] { });
			if (!q)
			{
				return null;
			}
			else
			{
				
				return result;
			}
			
        }

        internal int GetFd()
        {
			int result = -1;
			bool q = AndroidHelper.CallObjectMethod<int>(ref result, InterfaceObject, "getFd", new object[] { });
			if (!q)
			{
				return -1;
			}
			else
			{

				return result;
			}
			
        }

		internal void UpdateDevicePoseData(float qx, float qy, float qz, float qw, float px, float py, float pz)
        {
			AndroidHelper.CallObjectMethod(InterfaceObject, "updatePoseData", new object[] { qx, qy, qz, qw, px, py, pz });
		}


		/// <summary>
		/// 调用java库获取显示屏的一些参数类似分辨率:physicalWidth,physicalHeight, pixelWidth,pixelHeight，
		/// java库会获取android显示DisplayMetrics参数
		/// </summary>
		/// <returns></returns>
		public float [] GetUnityXvXRConfigInfo()
		{
			float[] result=new float[4];
            AndroidHelper.CallObjectMethod<float[]> (ref result, InterfaceObject, "getUnityXvXRConfigInfo", new object[]{});
			return result;
		}

		public void onSdkDeviceStatusChanged(string isConnect)
        {
			if ("true".Equals(isConnect))
			{
				XvDeviceManager.Manager.onDeviceConnectChanged(true);
			}
			else
			{
				XvDeviceManager.Manager.onDeviceConnectChanged(false);
			}
		}

		/// <summary>
		/// 在java sdk里,onDeviceAttach里会调用，设置vrmode是否使能
		/// </summary>
		/// <param name="isVrMode"></param>
		public void onSdkSwitchVrMode(string isVrMode)
		{
			if ("true".Equals(isVrMode))
			{
				XvXRManager.SDK.EnterVrMode();
			}
			else
			{
				XvXRManager.SDK.LeaveVrMode();
			}
		}

		public void onSdkReCenter(string nullstr)
		{
			XvXR.utils.XvXRLog.InternalXvXRLog("onSdkReCenter");
			XvXRManager.SDK.OnReCenterClick();
		}

		public void onSdkConfigParamterChange(string nullstr)
        {
			XvXR.utils.XvXRLog.InternalXvXRLog("onSdkConfigParamterChange");

			XvXRManager.SDK.onSdkConfigParamterChange();
		}

		/// <summary>
		/// 获取wifi连接状态的改变
		/// </summary>
		/// <param name="state"></param>
		public void onWifiConnectState(string state)
		{
			XvDeviceManager.Manager.onWifiConnectState(state);
		}
		
		/// <summary>
		/// 获取IP 相关信息
		/// </summary>
		/// <param name="ipInfo"></param>
		public void onIpInfo(string ipInfo)
		{
			//SetStaticIpControl.getIpInfo(ipInfo);
		}

		/// <summary>
		/// app 安装 删除 的状态回调
		/// </summary>
		/// <param name="state"></param>		
		public void onPkgChangeState(string state)
		{
			XvDeviceManager.Manager.onAppInstallState(state);
		}

		public void onWifiDisplayState(string state)
        {
			XvDeviceManager.Manager.onDisplayState(state);
		}

		public void onWifiApStateChange(string state)
		{
			XvDeviceManager.Manager.onWifiApStateChange(state);
		}

	}
}