using System;
using System.Collections.Generic;
using UnityEngine;
using XvXR.utils;

namespace XvXR.SystemEvents
{
    internal class AndroidConnection
    {
        private const string className = "top.xv.xrlib.unity.XvXRUnityAndroidConnection";

        private static AndroidJavaClass androidClass;


        internal static void NativeJniEnvInit(int bufferMode)
        {
            if (activityObject == null)
            {
                InitActivityObject();
            }
            androidClass = AndroidHelper.GetClass(className);
            AndroidHelper.CallStaticMethod(androidClass, "onRenderJniEnvInit", new object[] { activityObject, bufferMode });
        }

        internal static void SetVrMode(bool isVrMode)
        {
            if (activityObject == null)
            {
                InitActivityObject();
            }

            AndroidHelper.CallObjectMethod(activityObject, "setVrMode", new object[] { isVrMode });
        }

        internal static bool GetVrMode()
        {
            if (activityObject == null)
            {
                InitActivityObject();
            }
            bool result = false;
            AndroidHelper.CallObjectMethod<bool>(ref result, activityObject, "getVrMode", new object[] { });
            return result;

        }

        internal static bool IsTurnOnBluetooth()
        {
            if (activityObject == null)
            {
                InitActivityObject();
            }
            bool result = false;
            AndroidHelper.CallObjectMethod<bool>(ref result, activityObject, "IsTurnOnBluetooth", new object[] { });
            return result;
        }

        internal static bool openBluetooth()
        {
            if (activityObject == null)
            {
                InitActivityObject();
            }

            bool result = false;
            AndroidHelper.CallObjectMethod<bool>(ref result, activityObject, "openBluetooth", new object[] { });
            return result;
        }

        internal static bool closeBluetooth()
        {
            if (activityObject == null)
            {
                InitActivityObject();
            }

            bool result = false;
            AndroidHelper.CallObjectMethod<bool>(ref result, activityObject, "closeBluetooth", new object[] { });
            return result;
        }

   
        internal static void startBackService()
        {
            if (activityObject == null)
            {
                InitActivityObject();
            }
            AndroidHelper.CallObjectMethod(activityObject, "startBackService", new object[] { });


        }


        internal static bool getWifiState()
        {
            if (activityObject == null)
            {
                InitActivityObject();
            }
            bool result = false;
            AndroidHelper.CallObjectMethod<bool>(ref result, activityObject, "getWifiState", new object[] { });
            return result;

        }

        internal static bool isWifiSupported()
        {
            if (activityObject == null)
            {
                InitActivityObject();
            }
            bool result = false;
            AndroidHelper.CallObjectMethod<bool>(ref result, activityObject, "isWifiSupported", new object[] { });
            return result;
        }

        internal static string getWifiList()
        {
            if (activityObject == null)
            {
                InitActivityObject();
            }
            string result = "";
            AndroidHelper.CallObjectMethod<string>(ref result, activityObject, "getWifiList", new object[] { });
            return result;

        }

        internal static void connectWifi(String SSID, String pwd, bool isHasPwd)
        {
            if (activityObject == null)
            {
                InitActivityObject();
            }
            AndroidHelper.CallObjectMethod(activityObject, "connectWifi", new object[] { SSID, pwd, isHasPwd });
        }



        internal static void openWifi()
        {
            if (activityObject == null)
            {
                InitActivityObject();
            }
            AndroidHelper.CallObjectMethod(activityObject, "openWifi", new object[] { });
        }

        internal static void closeWifi()
        {
            if (activityObject == null)
            {
                InitActivityObject();
            }
            AndroidHelper.CallObjectMethod(activityObject, "closeWifi", new object[] { });
        }
        internal static string getCurrWifiSsid()
        {
            if (activityObject == null)
            {
                InitActivityObject();
            }
            string result = "";
            AndroidHelper.CallObjectMethod<string>(ref result, activityObject, "getCurrWifiSsid", new object[] { });
            result = result.Substring(1, result.Length - 2);
            return result;
        }

        internal static string getWifiIpAddress()
        {
            if (activityObject == null)
            {
                InitActivityObject();
            }
            string result = "";
            AndroidHelper.CallObjectMethod<string>(ref result, activityObject, "getWifiIpAddress", new object[] { });
            return result;
        }

    
        public static string getConnectedApInfo()
        {
            if (activityObject == null)
            {
                InitActivityObject();
            }
            string result = "";
            AndroidHelper.CallObjectMethod<string>(ref result, activityObject, "getConnectedApInfo", new object[] { });
            return result;
        }



        internal static bool setStaticIp(string ip, string mask, string gatway, string dns)
        {
            if (activityObject == null)
            {
                InitActivityObject();
            }
            bool result = false;
            AndroidHelper.CallObjectMethod<bool>(ref result, activityObject, "setStaticIp", new object[] { ip, mask, gatway, dns });
            return result;
        }

        internal static void getIpInfo()
        {
            if (activityObject == null)
            {
                InitActivityObject();
            }
            //bool result = false;
            AndroidHelper.CallObjectMethod(activityObject, "getIpInfo", new object[] { });
            //return result;
        }


        internal static void startCustomPackage(String packageName)
        {
            if (activityObject == null)
            {
                InitActivityObject();
            }
            AndroidHelper.CallObjectMethod(activityObject, "startCustomerPackage", new object[] { packageName });
        }

        internal static void setBootStartPackage(String packageName)
        {
            if (activityObject == null)
            {
                InitActivityObject();
            }
            AndroidHelper.CallObjectMethod(activityObject, "setBootStartPackage", new object[] { packageName });
        }

        internal static String getBootStartPackage()
        {
            if (activityObject == null)
            {
                InitActivityObject();
            }
            string result = "";
            AndroidHelper.CallObjectMethod<string>(ref result, activityObject, "getBootStartPackage", new object[] { });
            return result;
        }

        internal static void UninstallApk(String appPackName)
        {
            if (activityObject == null)
            {
                InitActivityObject();
            }
            //string result = "";
            AndroidHelper.CallObjectMethod(activityObject, "UninstallApk", new object[] { appPackName });
            //return result;
        }

        internal static void resetPhone()
        {
            if (activityObject == null)
            {
                InitActivityObject();
            }
            AndroidHelper.CallObjectMethod(activityObject, "resetPhone", new object[] { });

        }

        internal static int getVolumeCurr()
        {
            if (activityObject == null)
            {
                InitActivityObject();
            }
            int result = 0;
            AndroidHelper.CallObjectMethod<int>(ref result, activityObject, "getVolumeCurr", new object[] { });
            return result;

        }

        internal static int getVolumeMax()
        {
            if (activityObject == null)
            {
                InitActivityObject();
            }
            int result = 0;
            AndroidHelper.CallObjectMethod<int>(ref result, activityObject, "getVolumeMax", new object[] { });
            return result;
        }

        internal static int adjustVolume(int direction)
        {
            if (activityObject == null)
            {
                InitActivityObject();
            }
            int result = 0;
            AndroidHelper.CallObjectMethod<int>(ref result, activityObject, "adjustVolume", new object[] { direction });
            return result;
        }

     
        internal static int GetWIFISignalStrength()
        {
            if (activityObject == null)
            {
                InitActivityObject();
            }
            int result = 0;
            AndroidHelper.CallObjectMethod<int>(ref result, activityObject, "GetWIFISignalStrength", new object[] { });
            return result;

        }

        internal static int GetTeleSignalStrength()
        {
            if (activityObject == null)
            {
                InitActivityObject();
            }
            int result = 0;
            AndroidHelper.CallObjectMethod<int>(ref result, activityObject, "GetTeleSignalStrength", new object[] { });
            return result;

        }

        internal static String getTotalMemory()
        {
            if (activityObject == null)
            {
                InitActivityObject();
            }
            string result = "";
            AndroidHelper.CallObjectMethod<string>(ref result, activityObject, "getTotalMemory", new object[] { });
            return result;
        }

        internal static String getAvailMemory()
        {
            if (activityObject == null)
            {
                InitActivityObject();
            }
            string result = "";
            AndroidHelper.CallObjectMethod<string>(ref result, activityObject, "getAvailMemory", new object[] { });
            return result;
        }

        internal static String getCpuInfo()
        {
            if (activityObject == null)
            {
                InitActivityObject();
            }
            string result = "";
            AndroidHelper.CallObjectMethod<string>(ref result, activityObject, "getCpuInfo", new object[] { });
            return result;
        }

        internal static String getVersion()
        {
            if (activityObject == null)
            {
                InitActivityObject();
            }
            string result = "";
            AndroidHelper.CallObjectMethod<string>(ref result, activityObject, "getVersion", new object[] { });
            return result;
        }

        internal static String getProductName()
        {
            if (activityObject == null)
            {
                InitActivityObject();
            }
            string result = "";
            AndroidHelper.CallObjectMethod<string>(ref result, activityObject, "getProductName", new object[] { });
            return result;
        }

        internal static String getTimeZonesdata(int timeZone)
        {
            if (activityObject == null)
            {
                InitActivityObject();
            }
            string result = "";
            AndroidHelper.CallObjectMethod<string>(ref result, activityObject, "getTimeZonesdata", new object[] { timeZone });
            return result;
        }


        internal static String getDefaultTimeZone(int lang)
        {
            if (activityObject == null)
            {
                InitActivityObject();
            }
            string result = "";
            AndroidHelper.CallObjectMethod<string>(ref result, activityObject, "getDefaultTimeZone", new object[] { lang });
            return result;
        }

        internal static void setTimeZone(String timeZone)
        {
            if (activityObject == null)
            {
                InitActivityObject();
            }
            AndroidHelper.CallObjectMethod(activityObject, "setTimeZone", new object[] { timeZone });
        }

        internal static void setSysTime(int hour, int minute)
        {
            if (activityObject == null)
            {
                InitActivityObject();
            }
            AndroidHelper.CallObjectMethod(activityObject, "setSysTime", new object[] { hour, minute });
        }

        internal static void setSysDate(int year, int month, int day)
        {
            if (activityObject == null)
            {
                InitActivityObject();
            }
            AndroidHelper.CallObjectMethod(activityObject, "setSysDate", new object[] { year, month, day });
        }

        internal static void setAutoDateTime(int checkednum)
        {
            if (activityObject == null)
            {
                InitActivityObject();
            }
            AndroidHelper.CallObjectMethod(activityObject, "setAutoDateTime", new object[] { checkednum });
        }

        internal static bool isDateTimeAuto()
        {
            if (activityObject == null)
            {
                InitActivityObject();
            }
            bool result = false;
            AndroidHelper.CallObjectMethod<bool>(ref result, activityObject, "isDateTimeAuto", new object[] { });
            return result;
        }

        internal static void setAutoTimeZone(int checkednum)
        {
            if (activityObject == null)
            {
                InitActivityObject();
            }
            AndroidHelper.CallObjectMethod(activityObject, "setAutoTimeZone", new object[] { checkednum });
        }

        internal static bool isTimeZoneAuto()
        {
            if (activityObject == null)
            {
                InitActivityObject();
            }
            bool result = false;
            AndroidHelper.CallObjectMethod<bool>(ref result, activityObject, "isTimeZoneAuto", new object[] { });
            return result;
        }

        internal static void setHourFormat(int hour)
        {
            if (activityObject == null)
            {
                InitActivityObject();
            }
            AndroidHelper.CallObjectMethod(activityObject, "setHourFormat", new object[] { hour });
        }

        internal static bool isAutoEntryLauncher()
        {
            if (activityObject == null)
            {
                InitActivityObject();
            }
            bool result = false;
            AndroidHelper.CallObjectMethod<bool>(ref result, activityObject, "isAutoEntryLauncher", new object[] { });
            return result;
        }

        internal static bool getPhoneRestartPreference()
        {
            if (activityObject == null)
            {
                InitActivityObject();
            }
            bool result = false;
            AndroidHelper.CallObjectMethod<bool>(ref result, activityObject, "getPhoneRestartPreference", new object[] { });
            return result;
        }

        internal static void resetPhoneRestartPreference()
        {
            if (activityObject == null)
            {
                InitActivityObject();
            }
            AndroidHelper.CallObjectMethod(activityObject, "resetPhoneRestartPreference", new object[] { });
        }

        internal static bool isSystemFirstStart()
        {
            if (activityObject == null)
            {
                InitActivityObject();
            }
            bool result = false;
            AndroidHelper.CallObjectMethod(ref result, activityObject, "isSystemFirstStart", new object[] { });
            return result;
        }



        internal static bool is24Hour()
        {
            if (activityObject == null)
            {
                InitActivityObject();
            }
            bool result = false;
            AndroidHelper.CallObjectMethod<bool>(ref result, activityObject, "is24Hour", new object[] { });
            return result;
        }

        internal static bool GetAppsInfo()
        {
            if (activityObject == null)
            {
                InitActivityObject();
            }
            int flag = new AndroidJavaClass("android.content.pm.PackageManager").GetStatic<int>("GET_META_DATA");
            AndroidJavaObject pm = activityObject.Call<AndroidJavaObject>("getPackageManager");
            AndroidJavaObject packages = pm.Call<AndroidJavaObject>("getInstalledApplications", flag);
            int count = packages.Call<int>("size");
            string[] names = new string[count];
            string[] pknames = new string[count];
            List<Sprite> sprites = new List<Sprite>();

            for (int i = 0; i < count; i++)
            {
                AndroidJavaObject currentObject = packages.Call<AndroidJavaObject>("get", i);
                try
                {
                    bool result = false;
                    AndroidHelper.CallObjectMethod<bool>(ref result, activityObject, "isXvApp", new object[] { currentObject });
                    if (result)
                    {
                        names[i] = pm.Call<string>("getApplicationLabel", currentObject);
                        pknames[i] = currentObject.Get<string>("packageName");
                        byte[] decodedBytes = null;
                        AndroidHelper.CallObjectMethod<byte[]>(ref decodedBytes, activityObject, "getIcon", new object[] { pm, currentObject });

                        Texture2D text = new Texture2D(1, 1, TextureFormat.ARGB32, false);
                        text.LoadImage(decodedBytes);
                        Sprite sprite = Sprite.Create(text, new Rect(0, 0, text.width, text.height), new Vector2(.5f, .5f));
                        sprites.Add(sprite);
                    }
                }
                catch (Exception e)
                {

                }

            }
            return true;
        }
        internal static void VrShowRecenter()
        {
            if (activityObject == null)
            {
                InitActivityObject();
            }

            AndroidHelper.CallObjectMethod(activityObject, "vrShowRecenter", new object[] { });
        }

     
        internal static bool installApkPackage(String path)
        {
            if (activityObject == null)
            {
                InitActivityObject();
            }
            bool result = false;
            AndroidHelper.CallObjectMethod<bool>(ref result, activityObject, "installApkPackage", new object[] { path });
            return result;
        }

        internal static void insertLoginUser(String userName)
        {
            if (activityObject == null)
            {
                InitActivityObject();
            }
            AndroidHelper.CallObjectMethod(activityObject, "insertLoginUser", new object[] { userName });
        }

        internal static string getLoginUser()
        {
            if (activityObject == null)
            {
                InitActivityObject();
            }
            string result = "";
            AndroidHelper.CallObjectMethod<string>(ref result, activityObject, "getLoginUser", new object[] { });
            return result;
        }

        internal static void rebootDevice()
        {
            if (activityObject == null)
            {
                InitActivityObject();
            }

            AndroidHelper.CallObjectMethod(activityObject, "rebootDevice", new object[] { });
        }

        internal static void shutdownDevice()
        {
            if (activityObject == null)
            {
                InitActivityObject();
            }

            AndroidHelper.CallObjectMethod(activityObject, "shutdownDevice", new object[] { });
        }


        internal static void startWifiDisplay(bool isOpen)
        {
            if (activityObject == null)
            {
                InitActivityObject();
            }
            Debug.LogError("wayland = startWifiDisplay == " + isOpen);
            AndroidHelper.CallObjectMethod(activityObject, "startWifiDisplay", new object[] { isOpen });
        }

        internal static void openWifiDisplay(bool isOpen)
        {
            if (activityObject == null)
            {
                InitActivityObject();
            }
            Debug.LogError("wayland = openWifiDisplay == " + isOpen);
            AndroidHelper.CallObjectMethod(activityObject, "openWifiDisplay", new object[] { isOpen });
        }



        internal static void connectWifiDisplay(String displayName)
        {
            if (activityObject == null)
            {
                InitActivityObject();
            }
            AndroidHelper.CallObjectMethod(activityObject, "connectWifiDisplay", new object[] { displayName });
        }


        internal static int getWifiDisplayStatus()
        {
            if (activityObject == null)
            {
                InitActivityObject();
            }
            int result = 0;
            AndroidHelper.CallObjectMethod<int>(ref result, activityObject, "getWifiDisplayStatus", new object[] { });
            return result;
        }



        internal static void startTether()
        {
            if (activityObject == null)
            {
                InitActivityObject();
            }
            AndroidHelper.CallObjectMethod(activityObject, "startTether", new object[] { });
        }

    
        internal static string getWifiApStatus()
        {
            if (activityObject == null)
            {
                InitActivityObject();
            }

            string result = "";
            AndroidHelper.CallObjectMethod<string>(ref result, activityObject, "getWifiApStatus", new object[] { });

            return result;
        }

     
        internal static void stopTether()
        {
            if (activityObject == null)
            {
                InitActivityObject();
            }
            AndroidHelper.CallObjectMethod(activityObject, "stopTether", new object[] { });
        }

      
        internal static void onTetherConfigUpdated(int type, String ssid, String passwd)
        {
            if (activityObject == null)
            {
                InitActivityObject();
            }
            AndroidHelper.CallObjectMethod(activityObject, "onTetherConfigUpdated", new object[] { type, ssid, passwd });
        }


     
        internal static void setNormalPerformance()
        {
            if (activityObject == null)
            {
                InitActivityObject();
            }
            AndroidHelper.CallObjectMethod(activityObject, "setNormalPerformance");
        }

     
        internal static void setHighPerformance(int level)
        {
            if (activityObject == null)
            {
                InitActivityObject();
            }
            AndroidHelper.CallObjectMethod(activityObject, "setHighPerformance", new object[] { level });
        }


       

      
        internal static void setHomeKeyEnable(bool enable)
        {

#if UNITY_EDITOR
            return;
#endif
            if (activityObject == null)
            {
                InitActivityObject();
            }
            AndroidHelper.CallObjectMethod(activityObject, "setHomeKeyEnable", new object[] { enable });
        }


     
        internal static string getBoxName()
        {
            if (activityObject == null)
            {
                InitActivityObject();
            }
            string result = "";
            AndroidHelper.CallObjectMethod<string>(ref result, activityObject, "getBoxName", new object[] { });

            return result;
        }


        internal static int getGpuLoad()
        {
            if (activityObject == null)
            {
                InitActivityObject();
            }

            int result = 0;
            AndroidHelper.CallObjectMethod<int>(ref result, activityObject, "getGpuLoad", new object[] { });

            return result;
        }



        internal static void UnityFinish()
        {
            if (activityObject == null)
            {
                InitActivityObject();
            }

            AndroidHelper.CallObjectMethod(activityObject, "unityFinish", new object[] { });
        }

        private static void InitActivityObject()
        {
            AndroidJavaClass activityClass = new AndroidJavaClass("top.xv.xrlib.unity.XvMainActivity");
            activityObject = activityClass.GetStatic<AndroidJavaObject>("currentActivity");
        }



        private static AndroidJavaObject activityObject = null;
    }
}
