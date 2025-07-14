using UnityEngine;
using UnityEngine.Events;
namespace XvXR.Foundation
{

    /// <summary>
    /// �����ṩʶ��ɹ��Ժ�ص�����
    /// </summary>

    public class XvTagRecognizerBehavior : MonoBehaviour
    {

        [Tooltip("Apriltag ID")]

        public int id=-1;
        [Tooltip("QRCode �ı���ΪID")]
        public string qrcodeID ;

        public UnityEvent OnFoundEvent = new UnityEvent();
        public UnityEvent OnLostEvent = new UnityEvent();

        private XvTagRecognizerManager aprilTagManager;


        private TagDetection tagDetection;
        public TagDetection TagDetection { 
        get { return tagDetection; }
        }





        private void Awake()
        {
       
            aprilTagManager = FindAnyObjectByType<XvTagRecognizerManager>();

            if (id==-1) {
                int.TryParse(gameObject.name, out id);
            }
           // 
        }

        private void OnEnable()
        {
            if (aprilTagManager != null)
            {
                aprilTagManager.OnDetectedAprilTagEvent.AddListener(OnDetectedAprilTagEvent);
            }
        }

        private void OnDisable()
        {
            if (aprilTagManager != null)
            {
                aprilTagManager.OnDetectedAprilTagEvent.RemoveListener(OnDetectedAprilTagEvent);
            }
        }
        // Start is called before the first frame update
        void Start()
        {
            if (OnFoundEvent == null)
            {
                OnFoundEvent = new UnityEvent();
            }
            if (OnLostEvent == null)
            {
                OnLostEvent = new UnityEvent();
            }
            OnLostEvent?.Invoke();

        }



        private void OnDetectedAprilTagEvent(TagDetection[] tagDetections)
        {
            bool isFound = false;


            if (tagDetections == null)
            {
                isFound = false;
                MyDebugTool.Log("tagDetection == null");
            }
            else
            {
                //ʶ��ɹ�
                for (int i = 0; i < tagDetections.Length; i++)
                {
                    MyDebugTool.Log("detected��" + tagDetections[i].id + "   " + tagDetections[i].confidence + "  " + aprilTagManager.Confidence);

                    if (tagDetections[i].confidence >= aprilTagManager.Confidence)
                    {
                        if (aprilTagManager.TagGroupName == "36h11")
                        {

                            if (tagDetections[i].id == id)
                            {
                                isFound = true;
                                Vector3 position = tagDetections[i].translation;
                                Quaternion rotation = new Quaternion(tagDetections[i].quaternion[0], tagDetections[i].quaternion[1], tagDetections[i].quaternion[2], tagDetections[i].quaternion[3]);
                                transform.position = position;
                                transform.rotation = rotation;
                                tagDetection = tagDetections[i];
                                break;
                            }
                        }
                        else {
                           
                            string qrText = System.Text.Encoding.UTF8.GetString(tagDetections[i].qrcode).Trim();

                            MyDebugTool.Log("��ά�����ݣ�"+ tagDetections[i].qrcode+"  "+ qrText);
                          
                            if (qrText .Contains(qrcodeID) )
                            {
                                MyDebugTool.Log("qrText:" + qrText);
                                MyDebugTool.Log("qrTextID:" + qrcodeID);
                                isFound = true;
                                Vector3 position = tagDetections[i].translation;
                                Quaternion rotation = new Quaternion(tagDetections[i].quaternion[0], tagDetections[i].quaternion[1], tagDetections[i].quaternion[2], tagDetections[i].quaternion[3]);
                                transform.position = position;
                                transform.rotation = rotation;
                                tagDetection = tagDetections[i];
                                break;
                            }
                        }
                        
                        
                    }
                }
            }

            if (isFound)
            {
                MyDebugTool.Log("detected");

                transform.localScale = Vector3.one * (float)aprilTagManager.Size;
                OnFoundEvent?.Invoke();
            }
            else
            {
                //Debug.Log("��ʧ");

                OnLostEvent?.Invoke();

            }
        }

    }
}
