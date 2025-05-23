using UnityEngine;
using XvXR.Engine;


namespace XvXR.Foundation
{
    /// <summary>
    /// �����ṩϵͳ��صĲ������á���ȡ����
    /// </summary>
    public sealed class XvSystemSettingManager : MonoBehaviour
    {
        private XvSystemSettingManager() { }

        private int level = 6;

        /// <summary>
        /// ��ȡ��ǰ����
        /// </summary>
        /// <returns></returns>
        public int GetBrightnessLevel() {
            return level;
        }
        /// <summary>
        /// �����۾�����
        /// </summary>
        /// <param name="level">1~9</param>
        public void SetBrightnessLevel(int level)
        {
            this.level = level;

            XvSystemSetting.xslam_display_set_brightnesslevel(level);
#if !UNITY_EDITOR
         
            XvSystemSetting.xslam_display_set_brightnesslevel(level);
#endif
        }

        /// <summary>
        /// ���õ�ǰͫ��
        /// </summary>
        /// <param name="ipd">55mm~75mm</param>
        public void SetIPD(float ipd)
        {
#if !UNITY_EDITOR
           float nowIpd = GetIPD();
            XvXRAndroidDevice.updateCalibra((2 * ipd - nowIpd) / 10);
            XvXRManager.SDK.GetDevice().setFedDis((2 * ipd - nowIpd) / 10);
            XvXREye.EDI = 0;
#endif

        }
        /// <summary>
        /// ��ȡ��ǰIPD
        /// </summary>
        /// <returns> ����ֵ��λΪmm�����ף�</returns>
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

    }
}
