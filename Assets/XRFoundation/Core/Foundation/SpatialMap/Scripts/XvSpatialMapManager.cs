using AOT;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Events;

namespace XvXR.Foundation
{

    public sealed class XvSpatialMapManager : MonoBehaviour
    {
        private XvSpatialMapManager() { }

        public static UnityEvent<int,int> onMapSaveCompleteEvent=new UnityEvent<int, int>();

        public static UnityEvent<int > onMapLoadCompleteEvent=new UnityEvent<int>();

        public static UnityEvent< float> onMapMatchingEvent = new UnityEvent< float>();

        public void StartSlamMap()
        {

#if UNITY_ANDROID && !UNITY_EDITOR
         API.xslam_reset_slam();
#endif

        }

        public string SaveSlamMap()
        {

            string cslamName = GetNowStamp() + "_map.bin";
            string mapPath = Application.persistentDataPath + "/" + cslamName;
            API.xslam_save_map_and_switch_to_cslam(mapPath, OnCslamSaved, OnSaveLocalized);

            return mapPath;
        }

      
        public void LoadSlamMap(string mapPath)
        {

            API.xslam_reset_slam();         
            API.xslam_load_map_and_switch_to_cslam(mapPath, OnCslamSwitched, OnLoadLocalized);

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

            }
            else if(showFeaturePoint)
            {
                API.xslam_stop_map();
                showFeaturePoint = false;

            }
        }

      

      
       
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


      


      
        [MonoPInvokeCallback(typeof(API.detectCslamSaved_callback))]
        static void OnCslamSaved(int status_of_saved_map, int map_quality)
        {
          
            onMapSaveCompleteEvent?.Invoke(status_of_saved_map, map_quality);

        }

      
        [MonoPInvokeCallback(typeof(API.detectLocalized_callback))]
        static void OnSaveLocalized(float percentc)
        {
            //savePercent = percentc;
        }

     
        [MonoPInvokeCallback(typeof(API.detectSwitched_callback))]
        static void OnCslamSwitched(int map_quality)
        {
            load_map_quality = map_quality;
            onMapLoadCompleteEvent?.Invoke(load_map_quality);
        }

       
        [MonoPInvokeCallback(typeof(API.detectLocalized_callback))]
        static void OnLoadLocalized(float percentc)
        {
            similarity = percentc;
            onMapMatchingEvent?.Invoke( similarity);
        }


       
        public long ConvertDateTimeTotTmeStamp(System.DateTime time)
        {
            System.DateTime startTime = TimeZoneInfo.ConvertTimeToUtc(new System.DateTime(1970, 1, 1, 0, 0, 0, 0), TimeZoneInfo.Local);
            long t = (time.Ticks - startTime.Ticks) / 10000;  
            return t;
        }

      
        public long GetNowStamp()
        {
            return ConvertDateTimeTotTmeStamp(DateTime.Now);
        }
    }
}
