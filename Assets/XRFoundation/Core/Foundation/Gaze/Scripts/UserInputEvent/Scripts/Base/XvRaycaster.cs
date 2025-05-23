using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace XvXR.UI.Input
{
    public class XvRaycaster : BaseRaycaster
    {
        internal XvInputControllerBase controllerBase { get; private set; }

        /// <summary>
        /// ��¼��ǰ֡����ѡ�е�UI����
        /// </summary>
        private readonly List<RaycastResult> sortedRaycastResults = new List<RaycastResult>();//��ǰ����ѡ�񵽵Ķ���

        /// <summary>
        /// ���߹�����
        /// </summary>
        private Type RaycasterManager;

        private RaycastResult raycastResult3D;

        /// <summary>
        /// ÿ���Զ���������¼����
        /// </summary>
        private Camera fallbackCam;
        public override Camera eventCamera
        {
            get
            {
                if (fallbackCam == null)
                {
                    var go = new GameObject(name + " FallbackCamera");
                    Debug.Log("[XvRaycaster] ����FallbackCamera");
                    go.SetActive(false);

                    go.transform.SetParent(transform, false);
                    go.transform.localPosition = Vector3.zero;
                    go.transform.localRotation = Quaternion.identity;
                    go.transform.localScale = Vector3.one;

                    fallbackCam = go.AddComponent<Camera>();
                    fallbackCam.clearFlags = CameraClearFlags.Nothing;
                    fallbackCam.cullingMask = 0;
                    fallbackCam.orthographic = true;
                    fallbackCam.orthographicSize = 1;
                    fallbackCam.useOcclusionCulling = false;
#if !(UNITY_5_3 || UNITY_5_2 || UNITY_5_1 || UNITY_5_0)
                    fallbackCam.stereoTargetEye = StereoTargetEyeMask.None;
#endif
                    fallbackCam.nearClipPlane = nearDistance;
                    fallbackCam.farClipPlane = farDistance;
                }
                return fallbackCam;
            }
        }

        //�����ܽ������������
        [SerializeField]
        private float nearDistance = 0f;
        public float NearDistance
        {
            get { return nearDistance; }
            set
            {
                nearDistance = Mathf.Max(0f, value);
                if (eventCamera != null)
                    eventCamera.nearClipPlane = nearDistance;
                Debug.Log($"[XvRaycaster] ����NearDistance��{nearDistance}");
            }
        }

        /// <summary>
        /// ���ߵ���Զ����
        /// </summary>
        [SerializeField]
        private float farDistance = 20f;
        public float FarDistance
        {
            get { return farDistance; }
            set
            {
                farDistance = Mathf.Max(0f, nearDistance, value);
                if (eventCamera != null)
                    eventCamera.farClipPlane = farDistance;
                Debug.Log($"[XvRaycaster] ����FarDistance��{farDistance}");
            }
        }
        private List<BaseRaycaster> baseRaycasters;

        protected override void Awake()
        {
            controllerBase = GetComponent<XvInputControllerBase>();
            Debug.Log("[XvRaycaster] Awake, controllerBase����: " + (controllerBase != null)+controllerBase.gameObject.transform.name);
        }
        protected override void Start()
        {
            //ͨ�������ҵ�UGUI���߹�����
            RaycasterManager = EventSystem.current.GetType().Assembly.GetType("UnityEngine.EventSystems.RaycasterManager");
            Debug.Log("[XvRaycaster] Start, �����õ�RaycasterManager: " + (RaycasterManager != null));
            UpdateAllBaseRaycasters();
        }

        /// <summary>
        /// �������е�BaseRaycaster����,����Ƕ�̬������Canvas����Ҫ����һ��
        /// </summary>
        public void UpdateAllBaseRaycasters()
        {
            if (RaycasterManager == null)
            {
                Debug.LogError("[XvRaycaster] UpdateAllBaseRaycasters: RaycasterManagerΪ�գ�");
                return;
            }

            baseRaycasters = (List<BaseRaycaster>)RaycasterManager.InvokeMember(
                "GetRaycasters",
                BindingFlags.InvokeMethod | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic,
                null, null, null);

            Debug.Log($"[XvRaycaster] ��������BaseRaycasters������Ϊ: {baseRaycasters.Count}");
        }

        /// <summary>
        /// ��ȡ���ߵ�һ����������UI�������
        /// </summary>
        /// <returns></returns>
        public RaycastResult FirstRaycastResult()
        {
            Raycast();
            for (int i = 0; i < sortedRaycastResults.Count; ++i)
            {
                if (!sortedRaycastResults[i].isValid) { continue; }
                Debug.Log($"[XvRaycaster] First valid raycast result: {sortedRaycastResults[i].gameObject?.name}");
                return sortedRaycastResults[i];
            }
            Debug.Log("[XvRaycaster] û������������ЧUI����");
            return default(RaycastResult);

        }

        /// <summary>
        /// �����������λ�ã�ѭ���������߷ֱ��UIԪ�ؽ����Ľ��
        /// </summary>
        private void Raycast()
        {
            if (RaycasterManager != null)
            {
                sortedRaycastResults.Clear();
                Ray ray = new Ray(transform.position, transform.forward);
                RaycastHit raycastHit3D;
                if (Physics.Raycast(ray, out raycastHit3D, FarDistance))
                {
                    Debug.Log($"[XvRaycaster] ��������3D����: {raycastHit3D.transform.gameObject.name}, ����: {raycastHit3D.distance:F2}");
                    if (raycastHit3D.transform.GetComponent<Collider>() != null)
                    {
                        controllerBase.CustomEventData.hover3DRaycastHit = raycastHit3D;

                        raycastResult3D = new RaycastResult
                        {
                            gameObject = raycastHit3D.transform.gameObject,
                            module = this,
                            distance = raycastHit3D.distance,
                            worldPosition = raycastHit3D.point,
                            screenPosition = CustomEventData.ScreenCenterPoint,
                        };
                        sortedRaycastResults.Add(raycastResult3D);
                    }
                    else
                    {
                        Debug.Log("[XvRaycaster] ������û��Collider��3D���壡");
                        controllerBase.CustomEventData.hover3DRaycastHit = default(RaycastHit);
                    }
                }
                else
                {
                    Debug.Log("[XvRaycaster] û������3D���塣");
                    controllerBase.CustomEventData.hover3DRaycastHit = default(RaycastHit);
                }

                UpdateAllBaseRaycasters();
                foreach (var item in baseRaycasters)
                {
                    Canvas canvas = item.GetComponent<Canvas>();

                    if (canvas == null)
                    {
                        Debug.LogWarning($"[XvRaycaster] BaseRaycaster {item.name} û��Canvas�����������");
                        continue;
                    }

                    if (canvas.renderMode != RenderMode.WorldSpace)
                    {
                        Debug.Log($"[XvRaycaster] Canvas[{canvas.name}]����WorldSpace��������");
                        continue;
                    }

                    if (!RectTransformUtility.RectangleContainsScreenPoint(canvas.GetComponent<RectTransform>(), CustomEventData.ScreenCenterPoint, eventCamera))
                    {
                        Debug.Log($"[XvRaycaster] Canvas[{canvas.name}]���ĵ㲻����Ұ֮�ڣ�������");
                        continue;
                    }

                    Debug.Log($"[XvRaycaster] Canvas[{canvas.name}]�������߼�⡣");
                    Raycast(canvas, true, ray, FarDistance, sortedRaycastResults);
                }
                Debug.Log($"[XvRaycaster] ���߼��������ҵ�UI��������������{sortedRaycastResults.Count}");
            }
            else
            {
                Debug.LogError("[XvRaycaster] RaycasterManager == null ���ǲ�Ӧ�õģ�");
            }
        }

        /// <summary>
        /// �������ߺ�UIԪ�ؽ������
        /// </summary>
        /// <param name="canvas">��ǰ��Canvas</param>
        /// <param name="ignoreReversedGraphics">�Ƿ���Է������ϵ�ͼ��</param>
        /// <param name="ray">��ǰ����������</param>
        /// <param name="distance">�ܹ�����������</param>
        /// <param name="raycastResults">����һ�������</param>
        private void Raycast(Canvas canvas, bool ignoreReversedGraphics, Ray ray, float distance, List<RaycastResult> raycastResults)
        {
            if (canvas == null)
            {
                Debug.LogWarning("[XvRaycaster] Raycast(canvas): Canvas is null.");
                return;
            }

            var screenCenterPoint = CustomEventData.ScreenCenterPoint;
            var graphics = GraphicRegistry.GetGraphicsForCanvas(canvas);
            float rayCasterDis = FarDistance;

            Debug.DrawLine(ray.origin, ray.origin + ray.direction * 5, Color.green);

            for (int i = 0; i < graphics.Count; ++i)
            {
                var graphic = graphics[i];
                if (graphic.depth == -1 || !graphic.raycastTarget) { continue; }
                if (!RectTransformUtility.RectangleContainsScreenPoint(graphic.rectTransform, screenCenterPoint, eventCamera)) { continue; }
                if (ignoreReversedGraphics && Vector3.Dot(ray.direction, graphic.transform.forward) <= 0f) { continue; }
                if (!graphic.Raycast(screenCenterPoint, eventCamera)) { continue; }
                float dist = 10;
                new Plane(graphic.transform.forward, graphic.transform.position).Raycast(ray, out dist);
                if (dist > distance || dist > rayCasterDis)
                {
                    continue;
                }

                if (dist < rayCasterDis && sortedRaycastResults.Contains(raycastResult3D))
                {
                    Debug.Log("[XvRaycaster] ��⵽������UI���Ƴ�3D��������");
                    controllerBase.CustomEventData.hover3DRaycastHit = default(RaycastHit);
                    sortedRaycastResults.Remove(raycastResult3D);
                }

                Debug.Log($"[XvRaycaster] ����UIԪ��: {graphic.name}, ����: {dist:F2}");

                raycastResults.Add(new RaycastResult
                {
                    gameObject = graphic.gameObject,
                    module = this,
                    distance = dist,
                    worldPosition = ray.GetPoint(dist),
                    worldNormal = -graphic.transform.forward,
                    screenPosition = screenCenterPoint,
                    index = raycastResults.Count,
                    depth = graphic.depth,
                    sortingLayer = canvas.sortingLayerID,
                    sortingOrder = canvas.sortingOrder
                });
            }
            //���������Ƚ�����������
            raycastResults.Sort((g1, g2) => g2.depth.CompareTo(g1.depth));
        }

        private void LateUpdate()
        {
            controllerBase.CustomEventData.hover3DRaycastHit = default(RaycastHit);
        }

        public override void Raycast(PointerEventData eventData, List<RaycastResult> resultAppendList)
        {
            // ��ʵ�֣����ڼ���Unity��BaseRaycaster
        }
    }
}