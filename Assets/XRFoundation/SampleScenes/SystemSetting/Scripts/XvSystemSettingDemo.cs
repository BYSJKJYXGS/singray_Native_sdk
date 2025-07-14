using AOT;
using System;
using UnityEngine;
using UnityEngine.UI;
using static XvXR.Foundation.XvSystemSetting;

namespace XvXR.Foundation.SampleScenes
{
    public class XvSystemSettingDemo : MonoBehaviour
    {
        public XvSystemSettingManager settingManager;
        public Text brightnessValue;
        public Text ipdValue;
        public Text battery;


        public static Text wearText;
        public static Text lightPerceptiontText;
        public static Text keyTxt;
        public static Text keyStateTxt;
        public  Text volumnText;
        public AudioSource audioSourceOfVolumn;


        private float ipd;

        private void Awake()
        {
            if (settingManager==null) {
                settingManager=FindObjectOfType<XvSystemSettingManager>();

                if (settingManager==null) {
                    settingManager = new GameObject("XvSystemSettingManager").AddComponent<XvSystemSettingManager>();
                }
            }
            brightnessValue = transform.Find("UI/Canvas/Brightness/brightnessValue").GetComponent<Text>();
            ipdValue = transform.Find("UI/Canvas/Ipd/IpdValue").GetComponent<Text>();

            wearText = transform.Find("UI/Canvas/Wear/Wear").GetComponent<Text>();
            lightPerceptiontText = transform.Find("UI/Canvas/LightPerceptiont/LightPerceptiont").GetComponent<Text>();
            keyTxt = transform.Find("UI/Canvas/Key/KeyTxt").GetComponent<Text>();
            keyStateTxt = transform.Find("UI/Canvas/Key/KeyStateTxt").GetComponent<Text>();
            battery = transform.Find("UI/Canvas/Battery/Value").GetComponent<Text>();


        }

        private void Start()
        {
            Invoke("Initialized", 2);
        }
        private void Update()
        {
            battery.text = SystemInfo.batteryLevel * 100 + "%";
        }


        private void Initialized()
        {

            ipd=  (float)Math.Round(settingManager.GetIPD(), 1);
            SetIpd(ipd);

            brightnessValue.text = string.Format("{0}", settingManager.GetBrightnessLevel());
            ipdValue.text = string.Format("{0}mm", ipd);

            settingManager.XSlamStartEventStream(OnDevice_stream_callback);
            volumnText.text=settingManager.GetVolumeCurrent().ToString();
        }


        public void BrightnessUp() {
           int level= settingManager.GetBrightnessLevel();
            level++;
            SetBrightness(level);
        }
        public void BrightnessDown()
        {
            int level = settingManager.GetBrightnessLevel();
            level--;
            SetBrightness(level);
        }
        public void SetBrightness(float value)
        {
            int level = (int)value;
            level = Mathf.Clamp(level, 0, 9);
           
            brightnessValue.text = string.Format("{0}", level);

           
            settingManager.SetBrightnessLevel(level);
        }
        public void IpdUp()
        {
            ipd += 1;
           
            SetIpd(ipd);
        }
        public void IpdDown()
        {
            ipd -= 1;
            SetIpd(ipd);
        }
        public void SetIpd(float value)
        {
            ipd = value;
          
            ipd = Mathf.Clamp(ipd, 55, 75);
            settingManager.SetIPD(ipd);
            ipdValue.text = string.Format("{0}mm", ipd);
          
            // ipdSlider.value = value;

        }


        private static bool isWear=false;
        [MonoPInvokeCallback(typeof(device_stream_callback))]
        public static void OnDevice_stream_callback(XvEvent xvEvent)
        {
            //key = 2 ,state = 0 ÑÛ¾µÕªµô×´Ì¬
            //key = 2 ,state = 1 ÑÛ¾µ´÷ÉÏ×´Ì¬

            //key = 6 ,state = 0 ¹â¸Ð

            //key = 14 ¡¢1 ¡¢13¡¢ 3 ,state = 254 Ñ¹ÏÂ 255 Ì§Æð 
            //key = 17 ¡¢18 ,state = 101 Ðý×ª+ 99 Ðý×ª-
            switch (xvEvent.type)
            {
                case 14:
                case 1:
                case 13:
                case 3:
                case 17:
                case 18:
                    keyTxt.text = xvEvent.type.ToString();
                    keyStateTxt.text = xvEvent.state.ToString();
                    break;
                case 2:

                   
                    if (xvEvent.state == 1)
                    {
                        wearText.text = "Åå´÷ÖÐ";


                    }
                    else
                    {

                        wearText.text = "Î´Åå´÷";

                    }
                    break;
                case 6:
                    lightPerceptiontText.text = xvEvent.state.ToString();
                    break;
            }
        }


        public void VolumnUp() {
            AdjustVolume(1);
        }

        public void VolumnDown() { 
            AdjustVolume(-1);

        }
        internal  void AdjustVolume(int direction)
        {
            if (audioSourceOfVolumn!=null) {
                if (audioSourceOfVolumn.isPlaying)
                {
                    audioSourceOfVolumn.Stop();
                }

                audioSourceOfVolumn.Play();
            }
            
            volumnText.text = settingManager.AdjustVolume(direction).ToString();
        }

    }
}
