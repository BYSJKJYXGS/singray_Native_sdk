using System;
using UnityEngine;
using UnityEngine.Events;
namespace XvXR.Foundation
{
   /// <summary>
   /// �����ṩApriltag��QRCode��ʶ����
   /// </summary>
    public sealed class XvTagRecognizerManager : MonoBehaviour
    {
        private XvTagRecognizerManager() { }
        [SerializeField]
        private CameraType cameraType = 0;

        public CameraType CameraType
        {
            get
            {
                return cameraType;
            }

            set
            {
                cameraType = value;
            }
        }

        /// <summary>
        /// ʹ��ʶ�������� apritag="36h11"   qrcode="qrcode"
        /// </summary>
        [SerializeField]

        private string tagGroupName = "36h11";

        public string TagGroupName
        {
            get
            {
                return tagGroupName;
            }

            set
            {
                tagGroupName = value;
            }
        }


        [SerializeField]
        [Tooltip("ʶ���������ߴ�")]
        private double size = 0.16f;

        public double Size
        {
            get
            {
                return size;
            }

            set
            {
                size = value;
            }

        }


        [SerializeField]
        [Tooltip("���Ŷ���ֵ")]
        private float confidence = 0;
        public float Confidence
        {
            get
            {
                return confidence;
            }

            set
            {
                confidence = value;
            }

        }

        [SerializeField]
        private bool isDetect = true;
        public bool IsDetect
        {
            get
            {
                return isDetect;
            }

            set
            {
                isDetect = value;
            }

        }


        private UnityEvent<TagDetection[]> onDetectedAprilTagEvent;
        public UnityEvent<TagDetection[]> OnDetectedAprilTagEvent
        {
            get
            {
                if (onDetectedAprilTagEvent == null)
                {
                    onDetectedAprilTagEvent = new UnityEvent<TagDetection[]>();
                }

                return onDetectedAprilTagEvent;
            }

        }
        private TagDetection[] tagDetection;

        private void Awake()
        {
            if (OnDetectedAprilTagEvent == null)
            {

                Debug.LogError("Awake.OnDetectedAprilTagEvent==null");
            }
        }
        void StartDetect()
        {
#if !PLATFORM_ANDROID || UNITY_EDITOR
            return;
#endif

            if (cameraType == CameraType.Rgb)
            {
                tagDetection = XvAprilTag.StartRgbDetector(tagGroupName, size);//���� ģʽ
            }
            else if (cameraType == CameraType.FishEye)
            {
                tagDetection = XvAprilTag.StartFishEyeDetector(tagGroupName, size);//���� ģʽ
            }

            if (tagDetection == null || tagDetection.Length == 0)
            {
                MyDebugTool.Log($"XVtagDetection.Length:tagDetection == null || tagDetection.Length == 0");

                OnDetectedAprilTagEvent?.Invoke(null);
                return;
            }

            MyDebugTool.Log($"AprilTagDemo##tagDetection.Length:{tagDetection.Length}");
            for (int i = 0; i < tagDetection.Length; i++)
            {
                TagDetection tag = tagDetection[i];
                MyDebugTool.Log("AprilTagDemo##StartDetect translation:" + string.Format($"{i}=id:{tag.id},translation:{tag.translation.ToString()}"));
                MyDebugTool.Log("AprilTagDemo##StartDetect rotation:" + string.Format($"{i}=id:{tag.id},{tag.rotation.ToString()}"));
                MyDebugTool.Log("AprilTagDemo##StartDetect quaternion:" + string.Format($"{i}=id:{tag.id},{tag.quaternion.ToString()}"));
            }

            // Debug.LogError("tagDetection ==== " + tagDetection);

            try
            {
                if (tagDetection.Length > 0)
                {

                    Debug.LogError("tagDetection.Length ==== " + tagDetection.Length);
                    if (OnDetectedAprilTagEvent == null)
                    {

                        Debug.LogError("StartDetect.OnDetectedAprilTagEvent==null");
                    }
                    OnDetectedAprilTagEvent?.Invoke(tagDetection);
                }
                else
                {

                }
            }
            catch (Exception e)
            {
                //�����쳣
                MyDebugTool.Log("tagDetection is Null!!!" + e.ToString());
            }
        }


        /// <summary>
        /// �л����״̬
        /// </summary>
        /// <param name="isDetect"></param>
        public void SetDetectStatus(bool isDetect)
        {
#if !PLATFORM_ANDROID || UNITY_EDITOR
            return;
#endif

            if (this.isDetect)
            {
                if (!isDetect) {
                    if (cameraType == CameraType.Rgb)
                    {
                        XvAprilTag.StopRgbDetector();//���� ģʽ
                    }
                    else if (cameraType == CameraType.FishEye)
                    {
                        XvAprilTag.StopFishEyeDetector();//���� ģʽ
                    }
                }
            
            }

            this.isDetect = isDetect;
        }




        // Update is called once per frame
        void Update()
        {
            if (!isDetect)
            {
                return;
            }

            StartDetect();


        }

        private void OnDestroy()
        {
            SetDetectStatus(false);
        }
    }
    public enum CameraType
    {
        Rgb = 0,//rgb���
        FishEye = 1,//�������
    }
}

