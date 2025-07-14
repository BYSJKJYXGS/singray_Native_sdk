using UnityEngine;
using XvXR.Engine;
using XvXR.SystemEvents;
using static XvXR.Foundation.XvSystemSetting;

namespace XvXR.Foundation
{
    /// <summary>
    /// 该类提供系统相关的参数设置、获取方法
    /// </summary>
    public sealed class XvSystemSettingManager : MonoBehaviour
    {
        private XvSystemSettingManager() { }

        private int level = 6;

        /// <summary>
        /// 获取当前亮度
        /// </summary>
        /// <returns></returns>
        public int GetBrightnessLevel() {
            return level;
        }
        /// <summary>
        /// 设置眼镜亮度
        /// </summary>
        /// <param name="level">1~9</param>
        public void SetBrightnessLevel(int level)
        {
            this.level = level;


#if UNITY_EDITOR
            return;
#endif
            XvSystemSetting.xslam_display_set_brightnesslevel(level);
        }

        /// <summary>
        /// 设置当前瞳距
        /// </summary>
        /// <param name="ipd">55mm~75mm</param>
        public void SetIPD(float ipd)
        {
#if UNITY_EDITOR

#endif

            float nowIpd = GetIPD();
            XvXRAndroidDevice.updateCalibra((2 * ipd - nowIpd) / 10);
            XvXRManager.SDK.GetDevice().setFedDis((2 * ipd - nowIpd) / 10);
            XvXREye.EDI = 0;
        }
        /// <summary>
        /// 获取当前IPD
        /// </summary>
        /// <returns> 返回值单位为mm（毫米）</returns>
        public float GetIPD()
        {
#if UNITY_EDITOR
            return 0;
#endif
            API.stereo_pdm_calibration fed = XvXRManager.SDK.GetDevice().GetFed();
            float nowIpd = (float)(fed.calibrations[1].extrinsic.translation[0] - fed.calibrations[0].extrinsic.translation[0]);
            nowIpd *= 1000;
            return nowIpd;
        }

        /// <summary>
        /// 监听眼镜按键事件、是否佩戴，感光事件等
        /// </summary>
        /// <param name="cb"></param>
        public void XSlamStartEventStream(device_stream_callback cb) {

#if UNITY_EDITOR
            return;
#endif
            xslam_start_event_stream(cb);
        }


        public void XSlamStopEventStream()
        {
            xslam_stop_event_stream();
        }

       //获取当前音量
        public int GetVolumeCurrent()
        {
#if UNITY_EDITOR
            return 0;
#endif
            return AndroidConnection.getVolumeCurr();
        }

        //获取最大音量
        public int GetVolumeMax()
        {
#if UNITY_EDITOR
            return 0;
#endif
            return AndroidConnection.getVolumeMax();
        }
        /// <summary>
        /// 调节音量
        /// </summary>
        /// <param name="direction">-1：减小音量  1:增加音量</param>
        /// <returns></returns>
        public int AdjustVolume(int direction)
        {
#if UNITY_EDITOR
            return 0;
#endif
            return AndroidConnection.adjustVolume(direction);

        }

    }
}
