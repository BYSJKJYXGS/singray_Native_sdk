using UnityEngine;
using Microsoft.MixedReality.Toolkit.Input;
using Microsoft.MixedReality.Toolkit;

namespace XvXR.UI.Input
{
    [DisallowMultipleComponent]
    public sealed class XvHeadGazeInputController : XvInputControllerBase, IMixedRealityInputActionHandler
    {
        private bool handSelectPressed = false;

        private XvHeadGazeInputController() { }
        public static XvHeadGazeInputController Instance;
        public KeyCode keyCode = KeyCode.Q;

        public Transform pointer;

        private Vector3 targetPosition = Vector3.forward * 3;
        public float scaleFactor = 1;

        private Transform cameraTran;
        private bool enable = true;

        protected override void Awake()
        {
            Instance = this;
            base.Awake();
            cameraTran = Camera.main != null ? Camera.main.transform : null;
            Debug.Log("[XvHeadGazeInputController] Awake: Instance set, cameraTran initialized");
        }

      void OnEnable()
        {
            if (CoreServices.InputSystem != null)
            {
                CoreServices.InputSystem.RegisterHandler<IMixedRealityInputActionHandler>(this);
                Debug.Log("[XvHeadGazeInputController] Registered MRTK Input Handler");
            }
            else
            {
                Debug.LogWarning("[XvHeadGazeInputController] CoreServices.InputSystem is null in OnEnable!");
            }
        }

       void OnDisable()
        {
            if (CoreServices.InputSystem != null)
            {
                CoreServices.InputSystem.UnregisterHandler<IMixedRealityInputActionHandler>(this);
                Debug.Log("[XvHeadGazeInputController] Unregistered MRTK Input Handler");
            }
        }

        public void ShowOrHidePointer(bool isShow)
        {
            enable = isShow;
            if (pointer != null)
            {
                pointer.gameObject.SetActive(enable);
                Debug.Log($"[XvHeadGazeInputController] Pointer SetActive({enable})");
            }
            else
            {
                Debug.LogWarning("[XvHeadGazeInputController] Pointer is null in ShowOrHidePointer!");
            }
        }

        public Vector3 GetPointer3DPoint()
        {
            if (pointer != null)
            {
                return pointer.position;
            }
            Debug.LogWarning("[XvHeadGazeInputController] Pointer is null in GetPointer3DPoint!");
            return Vector3.zero; // fallback
        }
        protected override void Update()
        {
            base.Update();
            Control();
        }

        private void Control()
        {
            if (cameraTran != null)
            {
                transform.position = cameraTran.position;
                transform.rotation = cameraTran.rotation;
            }
            else
            {
                Debug.LogWarning("[XvHeadGazeInputController] cameraTran is null in Control!");
            }

            if (pointer != null)
            {
                if (CustomEventData.hover3DRaycastHit.transform?.gameObject != null && CustomEventData.hover3DRaycastHit.distance < 5)
                {
                    targetPosition.z = CustomEventData.hover3DRaycastHit.distance;
                    pointer.localScale = Vector3.one * CustomEventData.hover3DRaycastHit.distance * scaleFactor;
                    pointer.localPosition = targetPosition;

                    Debug.Log("选中3D物体: " + CustomEventData.hover3DRaycastHit.transform?.gameObject.name);
                }
                else
                {
                    if (CustomEventData.pointerCurrentRaycast.gameObject != null)
                    {
                        targetPosition.z = CustomEventData.pointerCurrentRaycast.distance;
                        pointer.localScale = Vector3.one * CustomEventData.pointerCurrentRaycast.distance * scaleFactor;
                        pointer.localPosition = targetPosition;
                        Debug.Log("选中UI物体: " + CustomEventData.pointerCurrentRaycast.gameObject.name);
                    }
                    else
                    {
                        pointer.localScale = Vector3.one * 3 * scaleFactor;
                        pointer.transform.localPosition = Vector3.forward * 3;
                        Debug.Log("没有选中任何物体");
                    }
                }
            }
            else
            {
                Debug.LogWarning("[XvHeadGazeInputController] Pointer is null in Control!");
            }

            if (!Is3DInput)
            {
                return;
            }

#if UNITY_EDITOR
            if (UnityEngine.Input.GetMouseButton(1))
            {
                float x = UnityEngine.Input.GetAxis("Mouse X");
                float y = UnityEngine.Input.GetAxis("Mouse Y");
                transform.localRotation *= Quaternion.Euler(new Vector3(-y, x, 0));
            }
#endif
        }

        public override Vector3 GetInputPosition()
        {
            return transform.forward;
        }

        public override bool GetKeyPress()
        {
            bool keyPress = UnityEngine.Input.GetKey(keyCode);
            bool OK = UnityEngine.Input.GetKey(KeyCode.Return) || UnityEngine.Input.GetMouseButton(0);
            bool result = (keyPress || OK || handSelectPressed) && enable;

            if (result)
            {
                Debug.Log($"[XvHeadGazeInputController] GetKeyPress triggered! keyPress:{keyPress}, OK:{OK}, handSelectPressed:{handSelectPressed}, enable:{enable}");
            }

            if (handSelectPressed) handSelectPressed = false; // 单帧响应
            return result;
        }

        public void OnActionStarted(BaseInputEventData eventData)
        {
            Debug.Log($"[XvHeadGazeInputController] OnActionStarted: {eventData.MixedRealityInputAction.Description}");
            if (eventData.MixedRealityInputAction.Description == "Select")
            {
                handSelectPressed = true; // 手势按下
                Debug.Log("[XvHeadGazeInputController] Select gesture detected (Pressed)");
            }
        }

        public void OnActionEnded(BaseInputEventData eventData)
        {
            Debug.Log($"[XvHeadGazeInputController] OnActionEnded: {eventData.MixedRealityInputAction.Description}");
            if (eventData.MixedRealityInputAction.Description == "Select")
            {
                handSelectPressed = false; // 手势松开
                Debug.Log("[XvHeadGazeInputController] Select gesture detected (Released)");
            }
        }
    }
}