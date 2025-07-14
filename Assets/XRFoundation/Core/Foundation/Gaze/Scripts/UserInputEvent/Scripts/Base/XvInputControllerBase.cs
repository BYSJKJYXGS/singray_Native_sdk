

namespace XvXR.UI.Input
{
    using UnityEngine;
    using UnityEngine.EventSystems;
    public class CustomEventData : PointerEventData
    {
        #region  ���ⲻ��Ҫ���鷳��ò�Ҫֱ�ӷ���

        public bool pressPrecessed;//��ǰ���̧��

        public float screenPositionDeltadelta;//�ƶ�λ������

        private XvInputControllerBase inputControllerBase;



        #endregion

        public CustomEventData(EventSystem eventSystem, XvInputControllerBase inputControllerBase) : base(eventSystem)
        {
            this.inputControllerBase = inputControllerBase;
        }



        public RaycastHit hover3DRaycastHit;//ѡ�е�3D����
        public static Vector3 ScreenCenterPoint { get { return new Vector2(Screen.width * 0.5f, Screen.height * 0.5f); } }

        internal bool GetKeyPress()
        {
            return inputControllerBase.GetKeyPress();

        }
    }

  
    

    public abstract class XvInputControllerBase : MonoBehaviour
    {
      
        internal XvRaycaster customRaycaster
        {
            get;
            private set;
        }
        private CustomEventData customEventData;
        public CustomEventData CustomEventData
        {
            get
            {

                if (customEventData == null)
                {
                    customEventData = new CustomEventData(EventSystem.current, this);
                }
                return customEventData;
            }
        }



        //�Ƿ���3D�ռ���������
        internal bool Is3DInput
        {
            get
            {
                return customRaycaster != null;
            }
        }

        /// <summary>
        /// ��ȡ��ǰ�û��Զ�����Ļ�ϵĵ�
        /// </summary>
        internal Vector3 screenPosition
        {
            get;
            private set;
        }

        /// <summary>
        /// ��ȡ��֮֡ǰ���ƶ�λ��
        /// </summary>
        internal Vector3 PositionDeltadelta
        {
            get
            {
                return positionDeltadelta;
            }
            private set
            {
                positionDeltadelta = value;
            }
        }

        private Vector3 lastPosition;

        private Vector3 positionDeltadelta;


        /// <summary>
        /// ��ȡ��Ļ���������
        /// </summary>
        /// <returns></returns>
        public virtual Vector3 GetInputPosition()
        {

            if (Is3DInput)
            {
                return transform.forward;
            }
            else
            {
                return Input.mousePosition;
            }

        }

        public virtual Vector2 GetScrollDelta()
        {
            return Vector3.zero;
        }

        public virtual bool GetKeyPress()
        {
           
            return Input.GetMouseButton(0);
        }
        protected virtual void Awake()
        {
            Initialized();
        }

        protected virtual void Update()
        {
            UpdateInputPosition();
        }

        /// <summary>
        /// ��ʼ���Զ�������ģ��
        /// </summary>
        private void Initialized()
        {
            screenPosition = new Vector2(Screen.width / 2, Screen.height / 2);

            customRaycaster = GetComponent<XvRaycaster>();
            if (EventSystem.current.gameObject.GetComponent<XvXRInputModule>() == null)
            {
                Debug.LogError("ִ�д���");
                Destroy(EventSystem.current.gameObject.GetComponent<StandaloneInputModule>());
                EventSystem.current.gameObject.AddComponent<XvXRInputModule>();
            }
        }

        /// <summary>
        /// �������������
        /// </summary>
        private void UpdateInputPosition()
        {
            if (Is3DInput)
            {
                positionDeltadelta = (GetInputPosition() - lastPosition) * 1000;
                lastPosition = GetInputPosition();
            }
            else
            {
                screenPosition = GetInputPosition();
                positionDeltadelta = screenPosition - lastPosition;
                lastPosition = screenPosition;
            }
        }
    }
}
