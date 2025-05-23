namespace XvXR.UI.Input
{
    using UnityEngine;
    using UnityEngine.EventSystems;

    public class CustomEventData : PointerEventData
    {
        #region  避免不必要的麻烦最好不要直接访问

        public bool pressPrecessed; // 标记按下抬起

        public float screenPositionDeltadelta; // 移动位置增量

        private XvInputControllerBase inputControllerBase;

        #endregion

        public CustomEventData(EventSystem eventSystem, XvInputControllerBase inputControllerBase)
            : base(eventSystem)
        {
            this.inputControllerBase = inputControllerBase;
            Debug.Log("[CustomEventData] 构造，关联输入控制器实例：" + (inputControllerBase != null));
        }

        public RaycastHit hover3DRaycastHit; // 选中的3D物体

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
            Debug.Log($"[CustomEventData] GetKeyPress 返回 {pressed}");
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
                    Debug.Log("[XvInputControllerBase] 创建CustomEventData实例");
                }
                return customEventData;
            }
        }

        //是否是3D空间射线输入
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
        /// 获取当前用户自定的屏幕上的点
        /// </summary>
        internal Vector3 screenPosition { get; private set; }

        /// <summary>
        /// 获取两帧之前的移动位置
        /// </summary>
        internal Vector3 PositionDeltadelta
        {
            get { return positionDeltadelta; }
            private set { positionDeltadelta = value; }
        }

        private Vector3 lastPosition;
        private Vector3 positionDeltadelta;

        /// <summary>
        /// 获取屏幕输入的坐标
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
                Debug.Log("[XvInputControllerBase] GetInputPosition (屏幕): " + Input.mousePosition);
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
            Debug.Log("[XvInputControllerBase] GetKeyPress (默认): " + pressed);
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
        /// 初始化自定义输入模块
        /// </summary>
        private void Initialized()
        {
            screenPosition = new Vector2(Screen.width / 2, Screen.height / 2);
            Debug.Log($"[XvInputControllerBase] Initialized, 屏幕中心: {screenPosition}");

            customRaycaster = GetComponent<XvRaycaster>();
            Debug.Log("[XvInputControllerBase] 查找XvRaycaster组件: " + (customRaycaster != null));

            if (EventSystem.current != null)
            {
                GameObject eventSysObj = EventSystem.current.gameObject;

                if (eventSysObj.GetComponent<XvXRInputModule>() == null)
                {
                    Debug.LogWarning("[XvInputControllerBase] 替换 StandaloneInputModule 为 XvXRInputModule");
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
                Debug.LogError("[XvInputControllerBase] EventSystem.current 为空！");
            }
        }

        /// <summary>
        /// 更新输入坐标点
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
                Debug.Log($"[XvInputControllerBase] 屏幕 PositionDeltadelta: {positionDeltadelta}, new screenPosition: {screenPosition}");
            }
        }
    }
}