namespace XvXR.UI.Input
{
    using UnityEngine;
    using UnityEngine.EventSystems;

    public class CustomEventData : PointerEventData
    {
        #region  ���ⲻ��Ҫ���鷳��ò�Ҫֱ�ӷ���

        public bool pressPrecessed; // ��ǰ���̧��

        public float screenPositionDeltadelta; // �ƶ�λ������

        private XvInputControllerBase inputControllerBase;

        #endregion

        public CustomEventData(EventSystem eventSystem, XvInputControllerBase inputControllerBase)
            : base(eventSystem)
        {
            this.inputControllerBase = inputControllerBase;
            Debug.Log("[CustomEventData] ���죬�������������ʵ����" + (inputControllerBase != null));
        }

        public RaycastHit hover3DRaycastHit; // ѡ�е�3D����

        public static Vector3 ScreenCenterPoint
        {
            get
            {
                var point = new Vector2(Screen.width * 0.5f, Screen.height * 0.5f);
                //Debug.Log($"[CustomEventData] ScreenCenterPoint = {point}");
                return point;
            }
        }

        internal bool GetKeyPress()
        {
            bool pressed = inputControllerBase.GetKeyPress();
            Debug.Log($"[CustomEventData] GetKeyPress ���� {pressed}");
            return pressed;
        }
    }

    public abstract class XvInputControllerBase : MonoBehaviour
    {
        internal XvRaycaster customRaycaster { get; private set; }

        private CustomEventData customEventData;
        public CustomEventData CustomEventData
        {
            get
            {
                if (customEventData == null)
                {
                    customEventData = new CustomEventData(EventSystem.current, this);
                    Debug.Log("[XvInputControllerBase] ����CustomEventDataʵ��");
                }
                return customEventData;
            }
        }

        //�Ƿ���3D�ռ���������
        internal bool Is3DInput
        {
            get
            {
                bool is3D = customRaycaster != null;
                //Debug.Log($"[XvInputControllerBase] Is3DInput = {is3D}");
                return is3D;
            }
        }

        /// <summary>
        /// ��ȡ��ǰ�û��Զ�����Ļ�ϵĵ�
        /// </summary>
        internal Vector3 screenPosition { get; private set; }

        /// <summary>
        /// ��ȡ��֮֡ǰ���ƶ�λ��
        /// </summary>
        internal Vector3 PositionDeltadelta
        {
            get { return positionDeltadelta; }
            private set { positionDeltadelta = value; }
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
                Debug.Log("[XvInputControllerBase] GetInputPosition (3D): " + transform.forward);
                return transform.forward;
            }
            else
            {
                Debug.Log("[XvInputControllerBase] GetInputPosition (��Ļ): " + Input.mousePosition);
                return Input.mousePosition;
            }
        }

        public virtual Vector2 GetScrollDelta()
        {
            return Vector3.zero;
        }

        public virtual bool GetKeyPress()
        {
            bool pressed = Input.GetMouseButton(0);
            Debug.Log("[XvInputControllerBase] GetKeyPress (Ĭ��): " + pressed);
            return pressed;
        }

        protected virtual void Awake()
        {
            Debug.Log("[XvInputControllerBase] Awake");
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
            Debug.Log($"[XvInputControllerBase] Initialized, ��Ļ����: {screenPosition}");

            customRaycaster = GetComponent<XvRaycaster>();
            Debug.Log("[XvInputControllerBase] ����XvRaycaster���: " + (customRaycaster != null));

            if (EventSystem.current != null)
            {
                GameObject eventSysObj = EventSystem.current.gameObject;

                if (eventSysObj.GetComponent<XvXRInputModule>() == null)
                {
                    Debug.LogWarning("[XvInputControllerBase] �滻 StandaloneInputModule Ϊ XvXRInputModule");
                    var standMod = eventSysObj.GetComponent<StandaloneInputModule>();
                    if (standMod != null)
                    {
                        Destroy(standMod);
                    }
                    eventSysObj.AddComponent<XvXRInputModule>();
                }
            }
            else
            {
                Debug.LogError("[XvInputControllerBase] EventSystem.current Ϊ�գ�");
            }
        }

        /// <summary>
        /// �������������
        /// </summary>
        private void UpdateInputPosition()
        {
            if (Is3DInput)
            {
                Vector3 current = GetInputPosition();
                positionDeltadelta = (current - lastPosition) * 1000;
                lastPosition = current;
                Debug.Log($"[XvInputControllerBase] 3D PositionDeltadelta: {positionDeltadelta}");
            }
            else
            {
                screenPosition = GetInputPosition();
                positionDeltadelta = screenPosition - lastPosition;
                lastPosition = screenPosition;
                Debug.Log($"[XvInputControllerBase] ��Ļ PositionDeltadelta: {positionDeltadelta}, new screenPosition: {screenPosition}");
            }
        }
    }
}