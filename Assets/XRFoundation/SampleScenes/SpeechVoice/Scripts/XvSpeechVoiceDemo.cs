using UnityEngine;
using UnityEngine.UI;
using XvXR.Foundation;

namespace XvXR.Foundation.SampleScenes
{


    public class XvSpeechVoiceDemo : MonoBehaviour
    {
        public XvSpeechVoiceManager xvSpeechVoiceManager;
        private bool isRotate;
        public Transform obj;
        public AudioSource audioSource;
        public AudioClip voiceWake;
        public AudioClip voiceEnd;
        public GameObject StopCurrent;
        public Text modeText;

        [SerializeField]
        [Tooltip("是否使用语音唤醒")]

        private bool isAvw = true;

        private void Awake()
        {
            if (xvSpeechVoiceManager == null)
            {
                xvSpeechVoiceManager = GameObject.FindObjectOfType<XvSpeechVoiceManager>();
            }
          

            StopCurrent.SetActive(isAvw);
        }

        private void Update()
        {
            if (isRotate)
            {
                obj.rotation *= Quaternion.Euler(Vector3.up * Time.deltaTime * 50);
            }


        }
        public void StartRotate()
        {
            isRotate = true;
        }
        public void StopRotate()
        {
            isRotate = false;
        }
        public void ScaleUp()
        {

            obj.localScale *= 1.1f;
        }
        public void ScaleDown()
        {
            obj.localScale *= 0.9f;
        }

        private bool IsOn;
        public void StartSpeechRecognition()
        {
            if (!IsOn) {
                modeText.text = isAvw ? "开语音唤醒识别模式" : "开启命令词识别模式";

                if (isAvw)
                {

                    xvSpeechVoiceManager.StartAVW(5000, 500);
                }
                else
                {
                    xvSpeechVoiceManager.StartASR(5000, 500);
                }
                IsOn = true;
            }
           
        }

        /// <summary>
        /// 停止语音识别
        /// </summary>
        public void StopSpeechRecognition()
        {
            if (IsOn) {
                modeText.text = isAvw ? "开关闭音唤醒识别模式" : "关闭命令词识别模式";
                if (isAvw)
                {
                    //停止唤醒词和命令词识别
                    xvSpeechVoiceManager.StopAVW();
                }
                else
                {
                    //停止命令词识别
                    xvSpeechVoiceManager.StopASR();
                }
                IsOn = false;
            }
        }

        /// <summary>
        /// 停止命令词的识别，如果是唤醒词识别模式，仅仅停止命令词的识别，不停止唤醒词识别。
        /// </summary>
        public void StopCurrentSpeechRecognition()
        {
            if (IsOn)
            {
                modeText.text = isAvw ? "开关闭音唤醒识别模式" : "关闭命令词识别模式";

                xvSpeechVoiceManager.StopASR();
                IsOn = false;
            }
        }


        public void PlayVoiceWake()
        {
            if (audioSource != null && voiceWake != null)
            {
                audioSource.PlayOneShot(voiceWake);
            }
        }


      
        public void PlayVoiceEnd()
        {
            if (audioSource != null && voiceEnd != null)
            {
                audioSource.PlayOneShot(voiceEnd);
            }
        }


        public void OnRecognizedStatus(result result)
        {

            MyDebugTool.Log("OnRecognizedStatus:" + result.word);

        }


    }
}