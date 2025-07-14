using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System;
using System.IO;
namespace XvXR.Foundation
{
    class ProjectBuild : Editor
    {
        //在这里找出你当前工程所有的场景文件，假设你只想把部分的scene文件打包 那么这里可以写你的条件判断 总之返回一个字符串数组。
        static string[] GetBuildScenes()
        {
            List<string> names = new List<string>();
            foreach (EditorBuildSettingsScene e in EditorBuildSettings.scenes)
            {
                if (e == null)
                    continue;
                if (e.enabled)
                    names.Add(e.path);
            }
            return names.ToArray();
        }


        //[MenuItem("Make Page/Test", false, 0)]
        //static void Test()
        //{
        //    double aa = 77;
        //    aa -= 1;
        //    Debug.LogError(aa / 10);
        //    Debug.LogError(aa);
        //}
        [MenuItem("XvXR/Tookkit/Build Scenes/Wifi", false, 0)]
        static void Wifi()
        {
            string[] scenes = { "Assets/XvXRFoundation/SampleScenes/Wifi/Scenes/Wifi.unity" };
            string identifierName = "com.xv.Wifi";
            string AppName = "Wifi";

            // init("Gesture");
            PlayerSettings.productName = AppName;
            PlayerSettings.applicationIdentifier = identifierName;

            string path = Application.dataPath + "/apk/" + AppName + "_" + getDate() + ".apk";
            path = path.Replace("Assets/", "");
            Debug.LogError(path);

            PlayerSettings.Android.useCustomKeystore = true;
            PlayerSettings.Android.keystorePass = "android";
            PlayerSettings.Android.keyaliasName = "platform";
            PlayerSettings.Android.keyaliasPass = "android";

            BuildPipeline.BuildPlayer(scenes, path, BuildTarget.Android, BuildOptions.None);
        }

        [MenuItem("XvXR/Tookkit/Build Scenes/Bluetooth", false, 0)]
        static void Bluetooth()
        {
            string[] scenes = { "Assets/XvXRFoundation/SampleScenes/Bluetooth/Scenes/Bluetooth.unity" };
            string identifierName = "com.xv.Bluetooth";
            string AppName = "Bluetooth";

            // init("Gesture");
            PlayerSettings.productName = AppName;
            PlayerSettings.applicationIdentifier = identifierName;
           // PlayerSettings.Android.useCustomKeystore = false;

            string path = Application.dataPath + "/apk/" + AppName + "_" + getDate() + ".apk";
            path = path.Replace("Assets/", "");
            Debug.LogError(path);

            //PlayerSettings.Android.useCustomKeystore = true;
            //PlayerSettings.Android.keystorePass = "android";
            //PlayerSettings.Android.keyaliasName = "platform";
            //PlayerSettings.Android.keyaliasPass = "android";

            BuildPipeline.BuildPlayer(scenes, path, BuildTarget.Android, BuildOptions.None);
        }


        [MenuItem("XvXR/Tookkit/Build Scenes/Viewer", false, 0)]
        static void XvViewer()
        {
            string[] scenes = { "Assets/XvXRFoundation/SampleScenes/Viewer/Scenes/Viewer.unity" };
            string identifierName = "com.xv.Viewer";
            string AppName = "Viewer";

            // init("Gesture");
            PlayerSettings.productName = AppName;
            PlayerSettings.applicationIdentifier = identifierName;
           // PlayerSettings.Android.useCustomKeystore = false;
         

            string path = Application.dataPath + "/apk/" + AppName + "_" + getDate() + ".apk";
            path = path.Replace("Assets/", "");
            Debug.LogError(path);
            BuildPipeline.BuildPlayer(scenes, path, BuildTarget.Android, BuildOptions.None);
        }





        [MenuItem("XvXR/Tookkit/Build Scenes/TagRecognizer", false, 0)]
        static void XvTagRecognizer()
        {
            string[] scenes = { "Assets/XvXRFoundation/SampleScenes/TagRecognizer/Scenes/TagRecognizer.unity" };
            string identifierName = "com.xv.TagRecognizer";
            string AppName = "TagRecognizer";

            // init("Gesture");
            PlayerSettings.productName = AppName;
            PlayerSettings.applicationIdentifier = identifierName;

            string path = Application.dataPath + "/apk/" + AppName + "_" + getDate() + ".apk";
            path = path.Replace("Assets/", "");
            Debug.LogError(path);
           // PlayerSettings.Android.useCustomKeystore = false;
            BuildPipeline.BuildPlayer(scenes, path, BuildTarget.Android, BuildOptions.None);
        }
        [MenuItem("XvXR/Tookkit/Build Scenes/PlaneDetection", false, 0)]
        static void XvPlaneDetection()
        {
            string[] scenes = { "Assets/XvXRFoundation/SampleScenes/PlaneDetection/Scenes/PlaneDetection.unity" };
            string identifierName = "com.xv.PlaneDetection";
            string AppName = "PlaneDetection";

            // init("Gesture");
            PlayerSettings.productName = AppName;
            PlayerSettings.applicationIdentifier = identifierName;

            string path = Application.dataPath + "/apk/" + AppName + "_" + getDate() + ".apk";
            path = path.Replace("Assets/", "");
            Debug.LogError(path);
           // PlayerSettings.Android.useCustomKeystore = false;
            BuildPipeline.BuildPlayer(scenes, path, BuildTarget.Android, BuildOptions.None);
        }

        [MenuItem("XvXR/Tookkit/Build Scenes/SpatialMesh", false, 0)]
        static void SpatialMesh()
        {
            string[] scenes = { "Assets/XvXRFoundation/SampleScenes/SpatialMesh/Scenes/SpatialMesh.unity" };
            string identifierName = "com.xv.SpatialMesh";
            string AppName = "SpatialMesh";

            // init("Gesture");
            PlayerSettings.productName = AppName;
            PlayerSettings.applicationIdentifier = identifierName;

            string path = Application.dataPath + "/apk/" + AppName + "_" + getDate() + ".apk";
            path = path.Replace("Assets/", "");
            Debug.LogError(path);
           // PlayerSettings.Android.useCustomKeystore = false;
            BuildPipeline.BuildPlayer(scenes, path, BuildTarget.Android, BuildOptions.None);
        }


        [MenuItem("XvXR/Tookkit/Build Scenes/SpatialMap", false, 0)]
        static void SpatialMap()
        {
            string[] scenes = { "Assets/XvXRFoundation/SampleScenes/SpatialMap/Scenes/SpatialMap.unity" };
            string identifierName = "com.xv.SpatialMap";
            string AppName = "SpatialMap";

            // init("Gesture");
            PlayerSettings.productName = AppName;
            PlayerSettings.applicationIdentifier = identifierName;

            string path = Application.dataPath + "/apk/" + AppName + "_" + getDate() + ".apk";
            path = path.Replace("Assets/", "");
            Debug.LogError(path);
          //  PlayerSettings.Android.useCustomKeystore = false;
            BuildPipeline.BuildPlayer(scenes, path, BuildTarget.Android, BuildOptions.None);
        }

        [MenuItem("XvXR/Tookkit/Build Scenes/SpeechVoice", false, 0)]
        static void SpeechVoice()
        {
            string[] scenes = { "Assets/XvXRFoundation/SampleScenes/SpeechVoice/Scenes/SpeechVoice.unity" };
            string identifierName = "com.xv.SpeechVoice";
            string AppName = "SpeechVoice";

            // init("Gesture");
            PlayerSettings.productName = AppName;
            PlayerSettings.applicationIdentifier = identifierName;

            string path = Application.dataPath + "/apk/" + AppName + "_" + getDate() + ".apk";
            path = path.Replace("Assets/", "");
            Debug.LogError(path);
           // PlayerSettings.Android.useCustomKeystore = false;
            BuildPipeline.BuildPlayer(scenes, path, BuildTarget.Android, BuildOptions.None);
        }
        [MenuItem("XvXR/Tookkit/Build Scenes/MRVideoCapture", false, 0)]
        static void MRVideoCapture()
        {
            string[] scenes = { "Assets/XvXRFoundation/SampleScenes/MRVideoCapture/Scenes/MRVideoCapture.unity" };
            string identifierName = "com.xv.MRVideoCapture";
            string AppName = "MRVideoCapture";

            // init("Gesture");
            PlayerSettings.productName = AppName;
            PlayerSettings.applicationIdentifier = identifierName;

            string path = Application.dataPath + "/apk/" + AppName + "_" + getDate() + ".apk";
            path = path.Replace("Assets/", "");
            Debug.LogError(path);
           // PlayerSettings.Android.useCustomKeystore = false;
            BuildPipeline.BuildPlayer(scenes, path, BuildTarget.Android, BuildOptions.None);
        }


        [MenuItem("XvXR/Tookkit/Build Scenes/RTSPStreamer", false, 0)]
        static void RTSPStreamer()
        {
            string[] scenes = { "Assets/XvXRFoundation/SampleScenes/RTSPStreamer/Scenes/RTSPStreamer.unity" };
            string identifierName = "com.xv.RTSPStreamer";
            string AppName = "RTSPStreamer";

            // init("Gesture");
            PlayerSettings.productName = AppName;
            PlayerSettings.applicationIdentifier = identifierName;

            string path = Application.dataPath + "/apk/" + AppName + "_" + getDate() + ".apk";
            path = path.Replace("Assets/", "");
            Debug.LogError(path);
           // PlayerSettings.Android.useCustomKeystore = false;
            BuildPipeline.BuildPlayer(scenes, path, BuildTarget.Android, BuildOptions.None);
        }


        [MenuItem("XvXR/Tookkit/Build Scenes/Rgbd", false, 0)]
        static void Rgbd()
        {
            string[] scenes = { "Assets/XvXRFoundation/SampleScenes/Rgbd/Scenes/Rgbd.unity" };
            string identifierName = "com.xv.Rgbd";
            string AppName = "Rgbd";

            // init("Gesture");
            PlayerSettings.productName = AppName;
            PlayerSettings.applicationIdentifier = identifierName;

            string path = Application.dataPath + "/apk/" + AppName + "_" + getDate() + ".apk";
            path = path.Replace("Assets/", "");
            Debug.LogError(path);
          //  PlayerSettings.Android.useCustomKeystore = false;
            BuildPipeline.BuildPlayer(scenes, path, BuildTarget.Android, BuildOptions.None);
        }

        [MenuItem("XvXR/Tookkit/Build Scenes/MediaRecorder", false, 0)]
        static void MediaRecorder()
        {
            string[] scenes = { "Assets/XvXRFoundation/SampleScenes/MediaRecorder/Scenes/MediaRecorder.unity" };
            string identifierName = "com.xv.MediaRecorder";
            string AppName = "MediaRecorder";

            // init("Gesture");
            PlayerSettings.productName = AppName;
            PlayerSettings.applicationIdentifier = identifierName;

            string path = Application.dataPath + "/apk/" + AppName + "_" + getDate() + ".apk";
            path = path.Replace("Assets/", "");
            Debug.LogError(path);
           // PlayerSettings.Android.useCustomKeystore = false;
            BuildPipeline.BuildPlayer(scenes, path, BuildTarget.Android, BuildOptions.None);
        }

        [MenuItem("XvXR/Tookkit/Build Scenes/SystemSetting", false, 0)]
        static void SystemSetting()
        {
            string[] scenes = { "Assets/XvXRFoundation/SampleScenes/SystemSetting/Scenes/SystemSetting.unity" };
            string identifierName = "com.xv.SystemSetting";
            string AppName = "SystemSetting";

            // init("Gesture");
            PlayerSettings.productName = AppName;
            PlayerSettings.applicationIdentifier = identifierName;

            string path = Application.dataPath + "/apk/" + AppName + "_" + getDate() + ".apk";
            path = path.Replace("Assets/", "");
            Debug.LogError(path);
           // PlayerSettings.Android.useCustomKeystore = false;
            BuildPipeline.BuildPlayer(scenes, path, BuildTarget.Android, BuildOptions.None);
        }
        [MenuItem("XvXR/Tookkit/Build Scenes/MRTK2", false, 0)]
        static void MRTK2()
        {
            string[] scenes = { "Assets/XvXRFoundation/SampleScenes/MRTK2/Scenes/MRTK2.unity" };
            string identifierName = "com.xv.MRTK2";
            string AppName = "MRTK2" +
                "";

            // init("Gesture");
            PlayerSettings.productName = AppName;
            PlayerSettings.applicationIdentifier = identifierName;

            string path = Application.dataPath + "/apk/" + AppName + "_" + getDate() + ".apk";
            path = path.Replace("Assets/", "");
            Debug.LogError(path);
           // PlayerSettings.Android.useCustomKeystore = false;
            BuildPipeline.BuildPlayer(scenes, path, BuildTarget.Android, BuildOptions.None);
        }
        [MenuItem("XvXR/Tookkit/Build Scenes/EyeTracking", false, 0)]
        static void EyeTracking()
        {
            string[] scenes = { "Assets/XvXRFoundation/SampleScenes/EyeTracking/Scenes/EyeTracking.unity" };
            string identifierName = "com.xv.EyeTracking";
            string AppName = "EyeTracking" +
                "";

            // init("Gesture");
            PlayerSettings.productName = AppName;
            PlayerSettings.applicationIdentifier = identifierName;

            string path = Application.dataPath + "/apk/" + AppName + "_" + getDate() + ".apk";
            path = path.Replace("Assets/", "");
            Debug.LogError(path);
           // PlayerSettings.Android.useCustomKeystore = false;
            BuildPipeline.BuildPlayer(scenes, path, BuildTarget.Android, BuildOptions.None);
        }
        [MenuItem("XvXR/Tookkit/Build Scenes/EyeCalibration", false, 0)]
        static void EyeCalibration()
        {
            string[] scenes = { "Assets/XvXRFoundation/SampleScenes/EyeTracking/Scenes/EyeCalibration.unity" };
            string identifierName = "com.xv.EyeCalibration";
            string AppName = "EyeCalibration" +
                "";

            // init("Gesture");
            PlayerSettings.productName = AppName;
            PlayerSettings.applicationIdentifier = identifierName;

            string path = Application.dataPath + "/apk/" + AppName + "_" + getDate() + ".apk";
            path = path.Replace("Assets/", "");
            Debug.LogError(path);
           // PlayerSettings.Android.useCustomKeystore = false;
            BuildPipeline.BuildPlayer(scenes, path, BuildTarget.Android, BuildOptions.None);
        }

        [MenuItem("XvXR/Tookkit/Build Scenes/EyeImage", false, 0)]
        static void EyeImage()
        {
            string[] scenes = { "Assets/XvXRFoundation/SampleScenes/EyeTracking/Scenes/EyeImage.unity" };
            string identifierName = "com.xv.EyeImage";
            string AppName = "EyeImage";

            // init("Gesture");
            PlayerSettings.productName = AppName;
            PlayerSettings.applicationIdentifier = identifierName;

            string path = Application.dataPath + "/apk/" + AppName + "_" + getDate() + ".apk";
            path = path.Replace("Assets/", "");
            Debug.LogError(path);
           // PlayerSettings.Android.useCustomKeystore = false;
            BuildPipeline.BuildPlayer(scenes, path, BuildTarget.Android, BuildOptions.None);
        }

        [MenuItem("XvXR/Tookkit/Build Scenes/Gaze", false, 0)]
        static void Gaze()
        {
            string[] scenes = { "Assets/XvXRFoundation/SampleScenes/Gaze/Scenes/Gaze.unity" };
            string identifierName = "com.xv.Gaze";
            string AppName = "Gaze" +
                "";

            // init("Gesture");
            PlayerSettings.productName = AppName;
            PlayerSettings.applicationIdentifier = identifierName;

            string path = Application.dataPath + "/apk/" + AppName + "_" + getDate() + ".apk";
            path = path.Replace("Assets/", "");
            Debug.LogError(path);
           // PlayerSettings.Android.useCustomKeystore = false;
            BuildPipeline.BuildPlayer(scenes, path, BuildTarget.Android, BuildOptions.None);
        }

        [MenuItem("XvXR/Tookkit/Build Scenes/Keyboard", false, 0)]
        static void Keyboard()
        {
            string[] scenes = { "Assets/XvXRFoundation/SampleScenes/Keyboard/Scenes/Keyboard.unity" };
            string identifierName = "com.xv.Keyboard";
            string AppName = "Keyboard";

            // init("Gesture");
            PlayerSettings.productName = AppName;
            PlayerSettings.applicationIdentifier = identifierName;

            string path = Application.dataPath + "/apk/" + AppName + "_" + getDate() + ".apk";
            path = path.Replace("Assets/", "");
            Debug.LogError(path);
            //PlayerSettings.Android.useCustomKeystore = false;
            BuildPipeline.BuildPlayer(scenes, path, BuildTarget.Android, BuildOptions.None);
        }


        [MenuItem("XvXR/Tookkit/Build Scenes/StaticGesture", false, 0)]
        static void StaticGesture()
        {
            string[] scenes = { "Assets/XvXRFoundation/SampleScenes/StaticGesture/Scenes/StaticGesture.unity" };
            string identifierName = "com.xv.StaticGesture";
            string AppName = "StaticGesture";

            // init("Gesture");
            PlayerSettings.productName = AppName;
            PlayerSettings.applicationIdentifier = identifierName;

            string path = Application.dataPath + "/apk/" + AppName + "_" + getDate() + ".apk";
            path = path.Replace("Assets/", "");
            Debug.LogError(path);
            //PlayerSettings.Android.useCustomKeystore = false;
            BuildPipeline.BuildPlayer(scenes, path, BuildTarget.Android, BuildOptions.None);
        }

        [MenuItem("XvXR/Tookkit/Build Scenes/Joystick", false, 0)]
        static void Joystick()
        {
            string[] scenes = { "Assets/XvXRFoundation/SampleScenes/Joystick/Scenes/Joystick.unity" };
            string identifierName = "com.xv.Joystick";
            string AppName = "Joystick";

            // init("Gesture");
            PlayerSettings.productName = AppName;
            PlayerSettings.applicationIdentifier = identifierName;

            string path = Application.dataPath + "/apk/" + AppName + "_" + getDate() + ".apk";
            path = path.Replace("Assets/", "");
            Debug.LogError(path);
           // PlayerSettings.Android.useCustomKeystore = false;
            BuildPipeline.BuildPlayer(scenes, path, BuildTarget.Android, BuildOptions.None);
        }

        [MenuItem("XvXR/Tookkit/Build Scenes/TofPointCloud", false, 0)]
        static void TofPointCloud()
        {
            string[] scenes = { "Assets/XvXRFoundation/SampleScenes/TofPointCloud/Scenes/TofPointCloud.unity" };
            string identifierName = "com.xv.TofPointCloud";
            string AppName = "TofPointCloud";

            // init("Gesture");
            PlayerSettings.productName = AppName;
            PlayerSettings.applicationIdentifier = identifierName;

            string path = Application.dataPath + "/apk/" + AppName + "_" + getDate() + ".apk";
            path = path.Replace("Assets/", "");
            Debug.LogError(path);
           // PlayerSettings.Android.useCustomKeystore = false;
            BuildPipeline.BuildPlayer(scenes, path, BuildTarget.Android, BuildOptions.None);
        }

        [MenuItem("XvXR/Tookkit/Build Scenes/IRToWorld", false, 0)]
        static void VideoPlay()
        {
            string[] scenes = { "Assets/XvXRFoundation/SampleScenes/IRToWorld/Scenes/IRToWorld.unity" };
            string identifierName = "com.xv.IRToWorld";
            string AppName = "IRToWorld";

            // init("Gesture");
            PlayerSettings.productName = AppName;
            PlayerSettings.applicationIdentifier = identifierName;

            string path = Application.dataPath + "/apk/" + AppName + "_" + getDate() + ".apk";
            path = path.Replace("Assets/", "");
            Debug.LogError(path);
            //PlayerSettings.Android.useCustomKeystore = false;
            BuildPipeline.BuildPlayer(scenes, path, BuildTarget.Android, BuildOptions.None);
        }



        [MenuItem("XvXR/Tookkit/Build Scenes/BaseScene", false, 0)]
        static void BaseScene()
        {

            string[] scenes = {

                "Assets/XvXRFoundation/SampleScenes/SDKSamples/Scenes/BaseScene.unity",
                "Assets/XvXRFoundation/SampleScenes/SDKSamples/Scenes/Viewer.unity",
                "Assets/XvXRFoundation/SampleScenes/SDKSamples/Scenes/MediaRecorder.unity",
                "Assets/XvXRFoundation/SampleScenes/SDKSamples/Scenes/MRTK2.unity",
                "Assets/XvXRFoundation/SampleScenes/SDKSamples/Scenes/MRVideoCapture.unity",
                "Assets/XvXRFoundation/SampleScenes/SDKSamples/Scenes/PlaneDetection.unity",
                "Assets/XvXRFoundation/SampleScenes/SDKSamples/Scenes/Rgbd.unity",
                "Assets/XvXRFoundation/SampleScenes/SDKSamples/Scenes/RTSPStreamer.unity",
                "Assets/XvXRFoundation/SampleScenes/SDKSamples/Scenes/SpatialMap.unity",
                "Assets/XvXRFoundation/SampleScenes/SDKSamples/Scenes/SpatialMesh.unity",
                "Assets/XvXRFoundation/SampleScenes/SDKSamples/Scenes/SpeechVoice.unity",
                "Assets/XvXRFoundation/SampleScenes/SDKSamples/Scenes/SystemSetting.unity",
                "Assets/XvXRFoundation/SampleScenes/SDKSamples/Scenes/TagRecognizer.unity",
                "Assets/XvXRFoundation/SampleScenes/SDKSamples/Scenes/EyeCalibration.unity",
                "Assets/XvXRFoundation/SampleScenes/SDKSamples/Scenes/EyeTracking.unity",
                "Assets/XvXRFoundation/SampleScenes/SDKSamples/Scenes/EyeImage.unity",
                "Assets/XvXRFoundation/SampleScenes/SDKSamples/Scenes/Keyboard.unity",
                "Assets/XvXRFoundation/SampleScenes/SDKSamples/Scenes/Gaze.unity",
                "Assets/XvXRFoundation/SampleScenes/SDKSamples/Scenes/StaticGesture.unity",
                "Assets/XvXRFoundation/SampleScenes/SDKSamples/Scenes/Joystick.unity",
                "Assets/XvXRFoundation/SampleScenes/SDKSamples/Scenes/TofPointCloud.unity",
            };
            string identifierName = "com.xv.BaseScene";
            string AppName = "BaseScene";

            // init("Gesture");
            PlayerSettings.productName = AppName;
            PlayerSettings.applicationIdentifier = identifierName;

            string path = Application.dataPath + "/apk/" + AppName + "_" + getDate() + ".apk";
            path = path.Replace("Assets/", "");
            Debug.LogError(path);
           // PlayerSettings.Android.useCustomKeystore = false;
            BuildPipeline.BuildPlayer(scenes, path, BuildTarget.Android, BuildOptions.None);
        }

        [MenuItem("XvXR/Tookkit/Build Scenes/SDKSamples", false, 100)]
        static void SDKSamples()
        {
            Keyboard();
            EyeCalibration();
            EyeTracking();
            EyeImage();
            Gaze();           
            MediaRecorder();
            MRTK2();
            MRVideoCapture();
            XvPlaneDetection();
            Rgbd();
            RTSPStreamer();
            SpatialMap();
            SpatialMesh();
            SpeechVoice();
            StaticGesture();
            SystemSetting();
            XvTagRecognizer();
            XvViewer();
            BaseScene();
            Joystick();
            Wifi();
            Bluetooth();
        }
      
        static void init(string app = "Default")
        {

            SetDefaultIcon(app);

            //AssetDatabase.Refresh();
        }

        public static void SetDefaultIcon(string iconName)
        {
            Texture2D texture = AssetDatabase.LoadAssetAtPath(string.Format("Assets/Images/Icon/{0}.png", iconName),
                typeof(Texture2D)) as Texture2D;

            int[] iconSize = PlayerSettings.GetIconSizesForTargetGroup(BuildTargetGroup.Android);
            Texture2D[] textureArray = new Texture2D[iconSize.Length];
            for (int i = 0; i < textureArray.Length; i++)
            {
                textureArray[i] = texture;
            }
            textureArray[0] = texture;
            PlayerSettings.SetIconsForTargetGroup(BuildTargetGroup.Android, textureArray);
            AssetDatabase.SaveAssets();
        }

        private static string getDate()
        {
            string str = DateTime.Now.Year.ToString();
            if (DateTime.Now.Month.ToString().Length == 1)
            {
                str += "0" + DateTime.Now.Month;
            }
            else
            {
                str += DateTime.Now.Month;
            }
            if (DateTime.Now.Day.ToString().Length == 1)
            {
                str += "0" + DateTime.Now.Day;
            }
            else
            {
                str += DateTime.Now.Day;
            }
            if (DateTime.Now.Hour.ToString().Length == 1)
            {
                str += "0" + DateTime.Now.Hour;
            }
            else
            {
                str += DateTime.Now.Hour;
            }
            if (DateTime.Now.Minute.ToString().Length == 1)
            {
                str += "0" + DateTime.Now.Minute;
            }
            else
            {
                str += DateTime.Now.Minute;
            }
            return str;
        }


        private static void AddBuildToolsToPath()
        {
            string sdkRoot = EditorPrefs.GetString("AndroidSdkRoot");
            string[] levels = Directory.GetDirectories(Path.Combine(sdkRoot, "build-tools"));
            // 优先使用高版本 SDK
            System.Array.Reverse(levels);

#if UNITY_EDITOR_WIN
            string delimiter = ";";
#else
    string delimiter = ":";
#endif

            var name = "PATH";
            string PATH = System.Environment.GetEnvironmentVariable(name);
            var value = PATH + delimiter + string.Join(delimiter, levels);
            var target = System.EnvironmentVariableTarget.Process;
            System.Environment.SetEnvironmentVariable(name, value, target);
        }


    }
}