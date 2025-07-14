using AOT;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Events;

namespace XvXR.Foundation
{
   /// <summary>
   /// 该类主要实现Cslam功能，可以对空间地图进行扫描生成地图文件，可以通过地图文件实现多人协同功能等
   /// </summary>
    public sealed class XvSpatialMapManager : MonoBehaviour
    {
        private XvSpatialMapManager() { }


       

        /// <summary>
        /// 第一个参数是map 保存状态
        /// 第二个参数是地图质量
        /// </summary>
        /// 
        public static UnityEvent<int,int> onMapSaveCompleteEvent=new UnityEvent<int, int>();
        /// <summary>
        /// 第一个参数是地图质量
        /// </summary>
        public static UnityEvent<int > onMapLoadCompleteEvent=new UnityEvent<int>();

        public static UnityEvent< float> onMapMatchingEvent = new UnityEvent< float>();

        


        /// <summary>
        /// 开始地图扫描
        /// </summary>
        public void StartSlamMap()
        {

            MyDebugTool.Log("扫描地图1");
#if UNITY_ANDROID && !UNITY_EDITOR
         API.xslam_reset_slam();
#endif
            MyDebugTool.Log("扫描地图2");

        }

        /// <summary>
        /// 保存扫描的地图
        /// </summary>
        /// <returns></returns>
        public string SaveSlamMap()
        {
            MyDebugTool.Log("保存地图1：" );

            string cslamName = GetNowStamp() + "_map.bin";
            string mapPath = Application.persistentDataPath + "/" + cslamName;
            API.xslam_save_map_and_switch_to_cslam(mapPath, OnCslamSaved, OnSaveLocalized);
            MyDebugTool.Log("保存地图2："+ mapPath);

            return mapPath;
        }

        /// <summary>
        /// 加载现有地图
        /// </summary>
        /// <param name="mapPath"></param>
        public void LoadSlamMap(string mapPath)
        {
            MyDebugTool.Log("加载地图1：" + mapPath);

            API.xslam_reset_slam();         
            API.xslam_load_map_and_switch_to_cslam(mapPath, OnCslamSwitched, OnLoadLocalized);
            MyDebugTool.Log("加载地图2：" + mapPath);

        }


        private bool showFeaturePoint;
        public bool ShowFeaturePoint { 
           get { return showFeaturePoint; } 
           
        }
        public void SwitchFeaturePointState() {

          
            if (!showFeaturePoint)
            {
                
                API.xslam_start_map();
                showFeaturePoint = true;

                MyDebugTool.Log("开启特征点");
            }
            else if(showFeaturePoint)
            {
                API.xslam_stop_map();
                showFeaturePoint = false;
                MyDebugTool.Log("关闭特征点");

            }
        }

      

      
        /// <summary>
        /// 建议不要频繁调用，一定时间间隔调用
        /// </summary>
        /// <returns>特征点空间坐标</returns>
        public List<Vector3> GetFeaturePoint()
        {
                if (!API.xslam_ready()|| !ShowFeaturePoint)
                {
                    return null;
                }
           
           
                int count = 0;
                IntPtr pt = API.xslam_get_slam_map(ref count);
                API.SlamMap[] objdata = new API.SlamMap[count];

                List<Vector3> pointList = new List<Vector3>();
                for (int i = 0; i < count; i++)
                {
                    IntPtr ptr = pt + i * Marshal.SizeOf(typeof(API.SlamMap));

                    objdata[i] = (API.SlamMap)Marshal.PtrToStructure(ptr, typeof(API.SlamMap));

                    Vector3 xyz = new Vector3(objdata[i].vertices[0], -objdata[i].vertices[1], objdata[i].vertices[2]);
                    pointList.Add(xyz);
                    Marshal.DestroyStructure(ptr, typeof(Vector3));
                }
                return pointList;

            

           

        }

        
        //private static float savePercent;
        //private static int save_map_quality;


        private static float similarity;
        private static int load_map_quality;

        //private static int status_of_saved_mapq;


      


        /// <summary>
        /// 保存地图的回调实现
        /// </summary>
        /// <param name="status_of_saved_map"></param>
        /// <param name="map_quality"></param>
        
        [MonoPInvokeCallback(typeof(API.detectCslamSaved_callback))]
        static void OnCslamSaved(int status_of_saved_map, int map_quality)
        {
          
            onMapSaveCompleteEvent?.Invoke(status_of_saved_map, map_quality);

            MyDebugTool.Log("保存完成：status_of_saved_map:"+ status_of_saved_map+ "   map_quality:" + map_quality);
        }

        /// <summary>
        /// 保存地图匹配度的回调实现
        /// </summary>
        /// <param name="percentc"></param>
        [MonoPInvokeCallback(typeof(API.detectLocalized_callback))]
        static void OnSaveLocalized(float percentc)
        {
            //savePercent = percentc;
            //MyDebugTool.Log("保存地图匹配度percentc:"+ percentc);
        }

        /// <summary>
        /// 加载地图的回调函数实现
        /// </summary>
        /// <param name="map_quality"></param>
        [MonoPInvokeCallback(typeof(API.detectSwitched_callback))]
        static void OnCslamSwitched(int map_quality)
        {
            MyDebugTool.Log("加载地图完成:" + map_quality);

            load_map_quality = map_quality;
            onMapLoadCompleteEvent?.Invoke(load_map_quality);
        }

        /// <summary>
        /// 加载地图的匹配度的回调实现
        /// </summary>
        /// <param name="percentc">0~1</param>
        [MonoPInvokeCallback(typeof(API.detectLocalized_callback))]
        static void OnLoadLocalized(float percentc)
        {
            similarity = percentc;
            onMapMatchingEvent?.Invoke( similarity);
        }


        /// <summary>
        /// 将DateTime转换成时间戳
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public long ConvertDateTimeTotTmeStamp(System.DateTime time)
        {
            System.DateTime startTime = TimeZoneInfo.ConvertTimeToUtc(new System.DateTime(1970, 1, 1, 0, 0, 0, 0), TimeZoneInfo.Local);
            long t = (time.Ticks - startTime.Ticks) / 10000;  //除10000调整为13位   
            return t;
        }

        /// <summary>
        /// 获取当前的时间戳
        /// </summary>
        /// <returns></returns>
        public long GetNowStamp()
        {
            return ConvertDateTimeTotTmeStamp(DateTime.Now);
        }
    }
}
