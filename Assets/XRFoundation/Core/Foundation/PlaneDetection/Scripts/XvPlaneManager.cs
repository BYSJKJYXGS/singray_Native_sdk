using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

namespace XvXR.Foundation
{
    /// <summary>
    /// ������Ҫ�����������ƽ��ʹ����ʶ��
    /// </summary>

    public sealed class XvPlaneManager : MonoBehaviour
    {
        private XvPlaneManager() { }
        public event Action<plane[]> planesChanged;

        private bool isDetecting;
        public bool IsDetecting
        {
            get { return isDetecting; }
        }


        [SerializeField]
        private XvCameraManager cameraManager;


        public XvCameraManager CameraManager
        {
            get
            {

                if (cameraManager == null)
                {
                    cameraManager = FindObjectOfType<XvCameraManager>();
                }

                if (cameraManager == null)
                {
                    cameraManager = new GameObject("XvCameraManager").AddComponent<XvCameraManager>();
                }
                return cameraManager;

            }
        }

        private float gapTime;

        /// <summary>
        /// 
        /// </summary>
        public void StartPlaneDetction()
        {
            if (!isDetecting)
            {
                isDetecting = true;
#if !PLATFORM_ANDROID || UNITY_EDITOR
                return;
#endif

                while (!API.xslam_ready())
                {
                    MyDebugTool.Log("xslam_ready==false");
                }

                // CameraManager.StopCapture(XvCameraStreamType.TofDepthCameraStream);

                MyDebugTool.Log("����ƽ����1");
                API.xslam_tof_set_framerate(5);
                API.xslam_start_detect_plane_from_tof_nosurface();
                MyDebugTool.Log("����ƽ����2");

            }

        }

        public void StopPlaneDetection()
        {
            if (isDetecting)
            {
                isDetecting = false;
#if !PLATFORM_ANDROID || UNITY_EDITOR
                return;
#endif          
                MyDebugTool.Log("�ر�ƽ����1");

                API.xslam_stop_detect_plane_from_tof();
                MyDebugTool.Log("�ر�ƽ����2");

            }
        }

        private void Update()
        {
#if !PLATFORM_ANDROID || UNITY_EDITOR
            return;
#endif   
            if (isDetecting)
            {
                gapTime += Time.deltaTime;

                if (gapTime >= 1)
                {
                    plane[] planes = GetPlane();
                    if (planes != null)
                    {
                        planesChanged?.Invoke(planes);
                    }

                    gapTime = 0;
                }

            }
        }





        private plane[] GetPlane()
        {
            int len = 1024 * 64;
            byte[] rdata = new byte[len];
            GCHandle rh = GCHandle.Alloc(rdata, GCHandleType.Pinned);
            bool ret = API.xslam_get_plane_from_tof(rh.AddrOfPinnedObject(), ref len);
            rh.Free();
            if (ret)
                return ParsePlane(rdata, len);
            else
                return null;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="rdata">ƽ������</param>
        /// <param name="len">�����ܳ���</param>
        /// <returns></returns>
        private plane[] ParsePlane(byte[] rdata, int len)
        {
            plane[] planes = new plane[0];
            if (len < 4)
                return null;

            int pos = 0;

            int nPlane = BitConverter.ToInt32(rdata, pos);
            pos += 4;
            if (nPlane > 0 && len > pos)
            {
                planes = new plane[nPlane];
                for (int i = 0; i < nPlane; i++)
                {
                    if (len <= pos)
                        break;

                    planes[i] = new plane();

                    int nPoint = BitConverter.ToInt32(rdata, pos); pos += 4;
                    planes[i].points = new List<Vector3D>();
                    for (int j = 0; j < nPoint; j++)
                    {
                        Vector3D point;
                        point.x = BitConverter.ToDouble(rdata, pos); pos += 8;
                        point.y = BitConverter.ToDouble(rdata, pos); pos += 8;
                        point.z = BitConverter.ToDouble(rdata, pos); pos += 8;
                        planes[i].points.Add(point);
                    }
                    planes[i].normal.x = BitConverter.ToDouble(rdata, pos); pos += 8;
                    planes[i].normal.y = BitConverter.ToDouble(rdata, pos); pos += 8;
                    planes[i].normal.z = BitConverter.ToDouble(rdata, pos); pos += 8;
                    planes[i].d = BitConverter.ToDouble(rdata, pos); pos += 8;
                    int idLen = BitConverter.ToInt32(rdata, pos); pos += 4;
                    planes[i].id = BitConverter.ToString(rdata, pos, idLen); pos += idLen;
                }
                return planes;
            }
            return null;
        }

        private void OnDestroy()
        {
            StopPlaneDetection();
        }

        private void OnApplicationQuit()
        {
            StopPlaneDetection();
        }
    }
    public struct Vector3D
    {
        public double x;
        public double y;
        public double z;
    };


    public class plane
    {
        public List<Vector3D> points;//ƽ�涥������
        public Vector3D normal;//ƽ�淨��
        public double d;
        public string id;//ƽ��ID
    };
}
