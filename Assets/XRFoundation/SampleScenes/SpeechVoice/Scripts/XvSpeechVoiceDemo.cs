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
        [Tooltip("�Ƿ�ʹ����������")]

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
                modeText.text = isAvw ? "����������ʶ��ģʽ" : "���������ʶ��ģʽ";

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
        /// ֹͣ����ʶ��
        /// </summary>
        public void StopSpeechRecognition()
        {
            if (IsOn) {
                modeText.text = isAvw ? "���ر�������ʶ��ģʽ" : "�ر������ʶ��ģʽ";
                if (isAvw)
                {
                    //ֹͣ���Ѵʺ������ʶ��
                    xvSpeechVoiceManager.StopAVW();
                }
                else
                {
                    //ֹͣ�����ʶ��
                    xvSpeechVoiceManager.StopASR();
                }
                IsOn = false;
            }
        }

        /// <summary>
        /// ֹͣ����ʵ�ʶ������ǻ��Ѵ�ʶ��ģʽ������ֹͣ����ʵ�ʶ�𣬲�ֹͣ���Ѵ�ʶ��
        /// </summary>
        public void StopCurrentSpeechRecognition()
        {
            if (IsOn)
            {
                modeText.text = isAvw ? "���ر�������ʶ��ģʽ" : "�ر������ʶ��ģʽ";

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