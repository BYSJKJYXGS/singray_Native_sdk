using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


namespace XvXR.UI.Input
{

   // [RequireComponent(typeof(HandInputController))]
    public class XvRaycaster : BaseRaycaster
    {
        internal XvInputControllerBase controllerBase
        {
            get;
            private set;
        }
        /// <summary>
        /// Records the UI object selected by the ray in the current frame.
        /// </summary>
        private readonly List<RaycastResult> sortedRaycastResults = new List<RaycastResult>();

        /// <summary>
        /// RaycasterManager
        /// </summary>
        private Type RaycasterManager;

        private RaycastResult raycastResult3D;

        /// <summary>
        /// Each event camera for custom input
        /// </summary>
        private Camera fallbackCam;
        public override Camera eventCamera
        {
            get
            {
                if (fallbackCam == null)
                {
                    var go = new GameObject(name + " FallbackCamera");
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



    
        [SerializeField]
        private float nearDistance = 0f;
        public float NearDistance
        {
            get { return nearDistance; }
            set
            {
                nearDistance = Mathf.Max(0f, value);
                if (eventCamera != null)
                {
                    eventCamera.nearClipPlane = nearDistance;
                }
            }
        }


        /// <summary>
        /// farDistance max distance
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
                {
                    eventCamera.farClipPlane = farDistance;
                }
            }
        }
        private  List<BaseRaycaster> baseRaycasters;

        protected override void Awake()
        {
            controllerBase = GetComponent<XvInputControllerBase>();
        }
        protected override void OnEnable()
        {
          
        }

        protected override void Start()
        {
            RaycasterManager = EventSystem.current.GetType().Assembly.GetType("UnityEngine.EventSystems.RaycasterManager");
            UpdateAllBaseRaycasters();
        }

        protected override void OnDisable()
        {
            

        }
        /// <summary>
        /// Updates input for all BaseRaycasters. This method must be called for dynamically constructed Canvases.
        /// </summary>
        public void UpdateAllBaseRaycasters()
        {
            baseRaycasters = (List<BaseRaycaster>)RaycasterManager.InvokeMember("GetRaycasters", BindingFlags.InvokeMethod | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic, null, null, null);
        }



        /// <summary>
        /// Gets the first UI interactive component hit by the ray.
        /// </summary>
        /// <returns></returns>
        public RaycastResult FirstRaycastResult()
        {
            Raycast();
            for (int i = 0; i < sortedRaycastResults.Count; ++i)
            {
                if (!sortedRaycastResults[i].isValid) { continue; }
                return sortedRaycastResults[i];
            }
            return default(RaycastResult);

        }
        /// <summary>
        /// Sets the camera's position and processes the interaction results between the ray and each UI element in a loop.
        /// </summary>
        private void Raycast()
        {
            if (RaycasterManager != null)
            {
                sortedRaycastResults.Clear();
                Ray ray = new Ray(transform.position, transform.forward);
                if (Physics.Raycast(ray, out RaycastHit raycastHit3D, FarDistance))
                {
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
                    else {
                        controllerBase.CustomEventData.hover3DRaycastHit = default(RaycastHit);
                    }
                   
                }
                else {
                    controllerBase.CustomEventData.hover3DRaycastHit = default(RaycastHit);
                }


                UpdateAllBaseRaycasters();
                foreach (var item in baseRaycasters)
                {

                    Canvas canvas = item.GetComponent<Canvas>();

                    if (canvas.renderMode != RenderMode.WorldSpace)
                    {
                        continue;

                    }
                    //  Debug.LogError(12);
                  
                    if (!RectTransformUtility.RectangleContainsScreenPoint(canvas.GetComponent<RectTransform>(), CustomEventData.ScreenCenterPoint, eventCamera))
                    {
                        continue;
                    }
                  
                    Raycast(canvas, true, ray, FarDistance, sortedRaycastResults);
                }
            }
            else
            {
                Debug.LogError("RaycasterManager == null");
            }


        }

      
        private void Raycast(Canvas canvas, bool ignoreReversedGraphics, Ray ray, float distance, List<RaycastResult> raycastResults)
        {
            if (canvas == null) { return; }

           

            var screenCenterPoint = CustomEventData.ScreenCenterPoint;
            var graphics = GraphicRegistry.GetGraphicsForCanvas(canvas);
            float rayCasterDis = FarDistance;

            Debug.DrawLine(ray.origin, ray.origin+ray.direction*5,Color.green);
           
          
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
                    controllerBase.CustomEventData.hover3DRaycastHit = default(RaycastHit);
                    sortedRaycastResults.Remove(raycastResult3D);
                }
                //Debug.LogError(graphic.name);
               // controllerBase.CustomEventData.hover3DRaycastHit = default(RaycastHit);

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
            raycastResults.Sort((g1, g2) => g2.depth.CompareTo(g1.depth));
        }
        private void LateUpdate()
        {
            controllerBase.CustomEventData.hover3DRaycastHit = default(RaycastHit);
        }
        public override void Raycast(PointerEventData eventData, List<RaycastResult> resultAppendList)
        {

        }
    }
}