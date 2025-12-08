using UnityEngine;
using XvXR.Engine;
using XvXR.SystemEvents;
using static XvXR.Foundation.XvSystemSetting;

namespace XvXR.Foundation
{
    /// <summary>
    /// This class provides methods for setting and retrieving system-related parameters. áThis class provides methods for setting and retrieving system-related parameters. 
    /// </summary>
    public sealed class XvSystemSettingManager : MonoBehaviour
    {
        private XvSystemSettingManager() { }

        private int level = 6;

        /// <summary>
        /// GetBrightnessLevel
        /// </summary>
        /// <returns></returns>
        public int GetBrightnessLevel() {
            return level;
        }
        /// <summary>
        /// SetBrightnessLevel
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
        /// SetIPD
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
        /// GetIPD
        /// </summary>
        /// <returns> float</returns>
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
        /// XSlamStartEventStream
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

        public int GetVolumeCurrent()
        {
#if UNITY_EDITOR
            return 0;
#endif
            return AndroidConnection.getVolumeCurr();
        }

        public int GetVolumeMax()
        {
#if UNITY_EDITOR
            return 0;
#endif
            return AndroidConnection.getVolumeMax();
        }
        /// <summary>
        /// AdjustVolume
        /// </summary>
        /// <param name="direction">AdjustVolume -1 or +1</param>
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
