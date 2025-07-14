using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using XvXR.SystemEvents;

namespace XvXR.Foundation
{
    public enum Ble_Bond_Status
    {

        //扫描到的其他蓝牙设备状态
        BOND_NONE,//未连接
        BOND_BOND,//配对中
        BOND_BOND_COMPLETE,//已配对
        BOND_BOND_CONNECTED,//已连接


        //当前设备蓝牙状态
        STATE_OFF,//关闭
        STATE_TURNING_ON,//启动中
        STATE_ON,//已开启
        STATE_TURNING_OFF,//关闭中
    }
    public class BluetoothManager : MonoBehaviour
    {

        public class bleInfo
        {
            public string info;
            public Ble_Bond_Status status;
        }

        private AndroidJavaObject mAndroidBle;
        private BlePoseListener mBlePoseListener;

        private List<bleInfo> mBluetoolthInfo = new List<bleInfo>();
        public List<bleInfo> BluetoolthList { 
          get { return mBluetoolthInfo; }
        }

        private Ble_Bond_Status ble_Bond_Status = Ble_Bond_Status.STATE_OFF;
        public Ble_Bond_Status Ble_Bond_Status
        {
            get
            {
                return ble_Bond_Status;
            }
        }


        public UnityEvent onStateChange;
        public UnityEvent onScan;

        private void Start()
        {
#if UNITY_EDITOR
            return;
#endif
            if (AndroidConnection.IsTurnOnBluetooth())
            {
                ble_Bond_Status = Ble_Bond_Status.STATE_ON;
               
            }
            else
            {
                ble_Bond_Status = Ble_Bond_Status.STATE_OFF;

              
            }

        }


        /// <summary>
        /// 开启蓝牙设备监听
        /// </summary>
        public void StartBle()
        {

#if UNITY_EDITOR
            return;
#endif
            MyDebugTool.Log("StartBle");
            AndroidJavaObject unityActivity = GetUnityActivity();
            AndroidJavaClass clazz = new AndroidJavaClass("top.xv.xrlib.common.ble.BleController");
            mAndroidBle = clazz.CallStatic<AndroidJavaObject>("getInstance", unityActivity);

            mBlePoseListener = new BlePoseListener(this);

            // mAndroidBle.Call<bool>("grantPermission");
            mAndroidBle.Call<bool>("start", mBlePoseListener);


            MyDebugTool.Log("StartBle  finish");


        }



        /// <summary>
        /// 开启蓝牙
        /// </summary>
        public void openBluetooth()
        {
#if UNITY_EDITOR
            return;
#endif
            MyDebugTool.Log(" openBluetooth");
            AndroidJavaObject unityActivity = GetUnityActivity();
            //AndroidJavaClass clazz = new AndroidJavaClass("org.xv.xvsdk.ext.ble.BleController");
            AndroidJavaClass clazz = new AndroidJavaClass("top.xv.xrlib.common.ble.BleController");
            mAndroidBle = clazz.CallStatic<AndroidJavaObject>("getInstance", unityActivity);

            mBlePoseListener = new BlePoseListener(this);

            //  mAndroidBle.Call<bool>("grantPermission");

            mAndroidBle.Call<bool>("openBluetooth", mBlePoseListener);

            MyDebugTool.Log(" openBluetooth2");

        }

        /// <summary>
        /// 关闭蓝牙
        /// </summary>
        public void closeBluetooth()
        {
#if UNITY_EDITOR
            return;
#endif
            //关闭蓝牙
            MyDebugTool.Log(" closeBluetooth");

            mAndroidBle.Call<bool>("closeBluetooth");
        }

        /// <summary>
        /// 蓝牙连接
        /// </summary>
        /// <param name="bleInfo"></param>
        public void connectBle(string bleInfo)
        {
#if UNITY_EDITOR
            return;
#endif
            MyDebugTool.Log("connectBle:" + bleInfo);
            mAndroidBle.Call<bool>("connect", bleInfo);
        }

        /// <summary>
        /// 断开蓝牙连接
        /// </summary>
        /// <param name="bleInfo"></param>
        public void disconnect(string bleInfo)
        {
#if UNITY_EDITOR
            return;
#endif
            MyDebugTool.Log("disconnect:" + bleInfo);
            mAndroidBle.Call<bool>("disconnect", bleInfo);
        }

        /// <summary>
        /// 取消蓝牙配对
        /// </summary>
        /// <param name="bleInfo"></param>
        public void unpairDevice(string bleInfo)
        {
#if UNITY_EDITOR
            return;
#endif
            MyDebugTool.Log("unpairDevice:" + bleInfo);
            mAndroidBle.Call<bool>("unpairDevice", bleInfo);
        }


        /// <summary>
        /// 刷新蓝牙列表
        /// </summary>
        public void Scan()
        {
#if UNITY_EDITOR
            return;
#endif
            MyDebugTool.Log("Scan:");

            mAndroidBle.Call("scan");
        }



        // BLE send HID command
        //����Ĳ�������HIDָ����� �ֱ�slam��λ��cmd���� 021a9601
        public void writeHid(string cmd)
        {
#if UNITY_EDITOR
            return;
#endif
            MyDebugTool.Log("writeHid:" + cmd);
            mAndroidBle.Call<bool>("write", cmd);
        }


        AndroidJavaObject GetUnityActivity()
        {
            AndroidJavaClass unityPlayer = new AndroidJavaClass("top.xv.xrlib.unity.XvMainActivity");
            AndroidJavaObject unityActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
            return unityActivity;
        }
        class BlePoseListener : AndroidJavaProxy
        {
            public BlePoseListener(BluetoothManager blueToothManager) : base("top.xv.xrlib.common.ble.IPoseListener")
            {

                this.toothManager = blueToothManager;
            }
            private BluetoothManager toothManager;

            /// <summary>
            /// 扫描到设备回调
            /// </summary>
            /// <param name="bleInfo"></param>
            /// <param name="status"></param>
            /// <param name="isconnected"></param>
            public void onScan(string bleInfo, int status, bool isconnected)
            {
                WorkQueue.Instance.InvokeOnAppThread(() =>
                {
                    bleInfo info = null;

                    for (int i = 0; i < toothManager.mBluetoolthInfo.Count; i++)
                    {
                        if (toothManager.mBluetoolthInfo[i].info == bleInfo)
                        {
                            info = toothManager.mBluetoolthInfo[i];
                            break;
                        }
                    }


                    if (info == null)
                    {
                        info = new bleInfo();
                        toothManager.mBluetoolthInfo.Add(info);
                    }

                    info.info = bleInfo;

                    if (status == 10)
                    {
                        //未连接
                        info.status = Ble_Bond_Status.BOND_NONE;
                    }
                    else if (status == 11)
                    {
                        //连接中
                        info.status = Ble_Bond_Status.BOND_BOND;

                    }
                    else if (status == 12)
                    {
                        //已配对
                        info.status = Ble_Bond_Status.BOND_BOND_COMPLETE;

                    }

                    if (isconnected)
                    {
                        info.status = Ble_Bond_Status.BOND_BOND_CONNECTED;
                    }

                    toothManager.onScan?.Invoke();

                });

            }


            void onCallback(AndroidJavaObject obj)
            {

            }


            /// <summary>
            /// 连接状态改变回调
            /// </summary>
            /// <param name="status"></param>
            void onStateChange(int status)
            {

                WorkQueue.Instance.InvokeOnAppThread(() =>
                {
                    if (status == 10)
                    {
                        toothManager.ble_Bond_Status = Ble_Bond_Status.STATE_OFF;
                    }
                    else if (status == 11)
                    {

                        toothManager.ble_Bond_Status = Ble_Bond_Status.STATE_TURNING_ON;
                    }
                    else if (status == 12)
                    {
                        toothManager.ble_Bond_Status = Ble_Bond_Status.STATE_ON;
                    }
                    else if (status == 13)
                    {
                        toothManager.ble_Bond_Status = Ble_Bond_Status.STATE_TURNING_OFF;
                    }

                    toothManager.onStateChange?.Invoke();

                }
                    );
                MyDebugTool.Log("  onStateChange ： " + status + "    ");
            }
        }

     
    }
}
