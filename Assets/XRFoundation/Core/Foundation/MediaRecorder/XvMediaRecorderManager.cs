using NatSuite.Examples;
using UnityEngine;
using UnityEngine.Events;

namespace XvXR.Foundation
{
    /// <summary>
    /// This class is mainly responsible for local video recording and screenshot capture. ¼This class is mainly responsible for local video recording and screenshot capture. 
    /// </summary>
    [RequireComponent(typeof(ReplayCam))]
    [RequireComponent(typeof(JPG))]

    public sealed class XvMediaRecorderManager : MonoBehaviour
    {
        private XvMediaRecorderManager() { }
        private ReplayCam replayCam;
        private JPG jpg;

        [SerializeField]
        private XvMRVideoCaptureManager xvMRVideoCaptureManager;
        public XvMRVideoCaptureManager XvMRVideoCaptureManager {

            get {

                if (xvMRVideoCaptureManager == null)
                {
                    xvMRVideoCaptureManager = FindObjectOfType<XvMRVideoCaptureManager>();
                }

                if (xvMRVideoCaptureManager == null)
                {
                    GameObject newObj = Instantiate(Resources.Load<GameObject>("XvMRVideoCaptureManager"));
                    xvMRVideoCaptureManager = newObj.GetComponent<XvMRVideoCaptureManager>();
                    newObj.name = "XvMRVideoCaptureManager";
                }

                return xvMRVideoCaptureManager;
            }
        }
     
        [SerializeField]
        private int width=1920;
        public int Width { 
        get { return width; }
        }
        [SerializeField]
        private int height=1080;
        public int Height { 
        get { return height; }
        }

        private void Awake()
        {
            replayCam=GetComponent<ReplayCam>();
            jpg=GetComponent<JPG>();
            replayCam.videoWidth = width;
            replayCam.videoHeight = height;
            jpg.imageWidth=width; 
            jpg.imageHeight=height;
            jpg.cam = XvMRVideoCaptureManager.BgCamera;
            replayCam.cam = XvMRVideoCaptureManager.BgCamera;

        }


        /// <summary>
        /// StartCapture
        /// </summary>
        public void StartCapture() {
            XvMRVideoCaptureManager.StartCapture();
        }
        /// <summary>
        /// StopCapturecloseCamera==false
        /// </summary>
        /// <param name="closeCamera">true:false</param>
        public void StopCapture(bool closeCamera=false)
        {
            XvMRVideoCaptureManager.StopCapture(closeCamera);
        }

        /// <summary>
        /// StartCapture
        /// </summary>

        public void StartRecording()
        {
            StartCapture();
            replayCam.StartRecording();
        }

        /// <summary>
        ///StopRecording
        /// </summary>
        /// <param name="callback"></param>
        public void StopRecording(UnityAction<string> callback)
        {

            replayCam.StopRecording(callback);
        }

        public bool IsVideoRecording() {
            return replayCam.IsRecording;
        }

        /// <summary>
        /// SaveScreenshot
        /// </summary>
        /// <param name="callback"></param>
        public void SaveScreenshot(UnityAction<string> callback)
        {
            StartCapture();

            jpg.SaveScreenshot(callback);
        }

        public bool IsTakingScreenshots() {
            return jpg.IsRecording;


        }
    }
}
