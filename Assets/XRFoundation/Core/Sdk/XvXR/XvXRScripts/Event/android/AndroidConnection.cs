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

        //蓝牙是否打开了
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

   
        //设置wifi连接走service
        internal static void startBackService()
        {
            if (activityObject == null)
            {
                InitActivityObject();
            }
            AndroidHelper.CallObjectMethod(activityObject, "startBackService", new object[] { });


        }


        //wifi是否打开
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

        //是否支持wifi
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

        //获取 wifi List
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

        // 连接wifi
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

        /// <summary>
        /// 当本地设备开启热点功能的时候，通过该接口获取到连接当前热点的设备IP
        /// </summary>
        /// <returns>格式如："{name=192.168.1.45, address=16:5F:3A:8C:9D:F1}{name=192.168.1.187, address=38:2A:7E:54:BF:D2}"</returns>
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

        //获取当前音量
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

        //获取最大音量
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

        //获取wifi信号强度
        //得到的值是一个0到-100的区间值，是一个int型数据，
        //其中0到-50表示信号最好，
        //-50到-70表示信号偏差，
        //小于-70表示最差，有可能连接不上或者掉线。
        //这个函数是返回5个级别！！！！！！
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

        //sim卡信号强度
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

        /** 获得手机系统总内存 */
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

        /** 获取android当前可用内存大小 */
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

        /*获取cpu 信息 占用率|温度|内存占用*/
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

        /*获取系统版本号*/
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

        /*获取产品名称*/
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

        //一次次的从xml文件时区 ;分割
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


        //获取系统当前的时区 0是中文 1是英文
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

        //设置系统时区
        internal static void setTimeZone(String timeZone)
        {
            if (activityObject == null)
            {
                InitActivityObject();
            }
            AndroidHelper.CallObjectMethod(activityObject, "setTimeZone", new object[] { timeZone });
        }

        //设置系统时间
        internal static void setSysTime(int hour, int minute)
        {
            if (activityObject == null)
            {
                InitActivityObject();
            }
            AndroidHelper.CallObjectMethod(activityObject, "setSysTime", new object[] { hour, minute });
        }

        //设置系统日期
        internal static void setSysDate(int year, int month, int day)
        {
            if (activityObject == null)
            {
                InitActivityObject();
            }
            AndroidHelper.CallObjectMethod(activityObject, "setSysDate", new object[] { year, month, day });
        }

        //设置系统的时间是否需要自动获取
        internal static void setAutoDateTime(int checkednum)
        {
            if (activityObject == null)
            {
                InitActivityObject();
            }
            AndroidHelper.CallObjectMethod(activityObject, "setAutoDateTime", new object[] { checkednum });
        }

        //判断系统的时间是否自动获取的
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

        //设置系统的时区是否自动获取
        internal static void setAutoTimeZone(int checkednum)
        {
            if (activityObject == null)
            {
                InitActivityObject();
            }
            AndroidHelper.CallObjectMethod(activityObject, "setAutoTimeZone", new object[] { checkednum });
        }

        //判断系统的时区是否是自动获取的
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

        //设置时间格式12 / 24小时制
        internal static void setHourFormat(int hour)
        {
            if (activityObject == null)
            {
                InitActivityObject();
            }
            AndroidHelper.CallObjectMethod(activityObject, "setHourFormat", new object[] { hour });
        }

        //判断是否插入眼镜后自启动app
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



        //是否是24小时制
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
                    // Debug.LogError(e, this);

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

        /// <summary>
        /// apk安装
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
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

        /// <summary>
        /// wifi 投屏 开关
        /// </summary>
        /// <param name="isOpen"></param>
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



        /// <summary>
        /// 连接电视
        /// </summary>
        /// <param name="displayName"></param>
        internal static void connectWifiDisplay(String displayName)
        {
            if (activityObject == null)
            {
                InitActivityObject();
            }
            AndroidHelper.CallObjectMethod(activityObject, "connectWifiDisplay", new object[] { displayName });
        }

        /// <summary>
        /// 连接电视 状态
        /// 0 1 不可用
        /// 2   已关闭
        /// 3   已打开
        /// 
        /// </summary>
        /// <param name="displayName"></param>
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


        /// <summary>
        /// 打开热点
        /// </summary>
        internal static void startTether()
        {
            if (activityObject == null)
            {
                InitActivityObject();
            }
            AndroidHelper.CallObjectMethod(activityObject, "startTether", new object[] { });
        }

        /// <summary>
        //已关闭 11
        //关闭中 10
        //已打开 13
        //打开中 12
        //打开失败 14
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// 关闭热点
        /// </summary>
        internal static void stopTether()
        {
            if (activityObject == null)
            {
                InitActivityObject();
            }
            AndroidHelper.CallObjectMethod(activityObject, "stopTether", new object[] { });
        }

        /// <summary>
        /// 设置热点
        /// </summary>
        /// <param name="type">默认 1</param>
        /// <param name="ssid"></param>
        /// <param name="passwd"></param>
        internal static void onTetherConfigUpdated(int type, String ssid, String passwd)
        {
            if (activityObject == null)
            {
                InitActivityObject();
            }
            AndroidHelper.CallObjectMethod(activityObject, "onTetherConfigUpdated", new object[] { type, ssid, passwd });
        }


        /// <summary>
        /// /// <summary>
        /// 设置GPU CPU频率为系统自动调节模式，必须使用系统签名
        /// </summary>
        /// 
        /// </summary>
        internal static void setNormalPerformance()
        {
            if (activityObject == null)
            {
                InitActivityObject();
            }
            AndroidHelper.CallObjectMethod(activityObject, "setNormalPerformance");
        }

        /// <summary>
        /// 设置GPU CPU频率，必须使用系统签名
        /// </summary>
        /// <param name="level">3 2 1  3最大 1最小</param>
        internal static void setHighPerformance(int level)
        {
            if (activityObject == null)
            {
                InitActivityObject();
            }
            AndroidHelper.CallObjectMethod(activityObject, "setHighPerformance", new object[] { level });
        }


       

        /// <summary>
        /// 设置应用是否可以通过Back和Home返回主界面
        /// </summary>
        /// <param name="enable">true 返回： false 不返回</param>
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


        /// <summary>
        /// 获取当前设备名称，判断G2还是3588平台
        /// </summary>
        /// <returns>包含Kalama:G2    包含SeerPad:3588</returns>
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
