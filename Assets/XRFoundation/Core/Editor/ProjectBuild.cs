using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System;
using System.IO;
namespace XR.Foundation
{
    class ProjectBuild : Editor
    {
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


     
        [MenuItem("Singray XR/Tookkit/Build Scenes/Wifi", false, 0)]
        static void Wifi()
        {
            string[] scenes = { "Assets/XRFoundation/SampleScenes/Wifi/Scenes/Wifi.unity" };
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

        [MenuItem("Singray XR/Tookkit/Build Scenes/Bluetooth", false, 0)]
        static void Bluetooth()
        {
            string[] scenes = { "Assets/XRFoundation/SampleScenes/Bluetooth/Scenes/Bluetooth.unity" };
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


        [MenuItem("Singray XR/Tookkit/Build Scenes/Viewer", false, 0)]
        static void XvViewer()
        {
            string[] scenes = { "Assets/XRFoundation/SampleScenes/Viewer/Scenes/Viewer.unity" };
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





        [MenuItem("Singray XR/Tookkit/Build Scenes/TagRecognizer", false, 0)]
        static void XvTagRecognizer()
        {
            string[] scenes = { "Assets/XRFoundation/SampleScenes/TagRecognizer/Scenes/TagRecognizer.unity" };
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
        [MenuItem("Singray XR/Tookkit/Build Scenes/PlaneDetection", false, 0)]
        static void XvPlaneDetection()
        {
            string[] scenes = { "Assets/XRFoundation/SampleScenes/PlaneDetection/Scenes/PlaneDetection.unity" };
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

        [MenuItem("Singray XR/Tookkit/Build Scenes/SpatialMesh", false, 0)]
        static void SpatialMesh()
        {
            string[] scenes = { "Assets/XRFoundation/SampleScenes/SpatialMesh/Scenes/SpatialMesh.unity" };
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


        [MenuItem("Singray XR/Tookkit/Build Scenes/SpatialMap", false, 0)]
        static void SpatialMap()
        {
            string[] scenes = { "Assets/XRFoundation/SampleScenes/SpatialMap/Scenes/SpatialMap.unity" };
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

        [MenuItem("Singray XR/Tookkit/Build Scenes/SpeechVoice", false, 0)]
        static void SpeechVoice()
        {
            string[] scenes = { "Assets/XRFoundation/SampleScenes/SpeechVoice/Scenes/SpeechVoice.unity" };
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
        [MenuItem("Singray XR/Tookkit/Build Scenes/MRVideoCapture", false, 0)]
        static void MRVideoCapture()
        {
            string[] scenes = { "Assets/XRFoundation/SampleScenes/MRVideoCapture/Scenes/MRVideoCapture.unity" };
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


        [MenuItem("Singray XR/Tookkit/Build Scenes/RTSPStreamer", false, 0)]
        static void RTSPStreamer()
        {
            string[] scenes = { "Assets/XRFoundation/SampleScenes/RTSPStreamer/Scenes/RTSPStreamer.unity" };
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


        [MenuItem("Singray XR/Tookkit/Build Scenes/Rgbd", false, 0)]
        static void Rgbd()
        {
            string[] scenes = { "Assets/XRFoundation/SampleScenes/Rgbd/Scenes/Rgbd.unity" };
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

        [MenuItem("Singray XR/Tookkit/Build Scenes/MediaRecorder", false, 0)]
        static void MediaRecorder()
        {
            string[] scenes = { "Assets/XRFoundation/SampleScenes/MediaRecorder/Scenes/MediaRecorder.unity" };
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

        [MenuItem("Singray XR/Tookkit/Build Scenes/SystemSetting", false, 0)]
        static void SystemSetting()
        {
            string[] scenes = { "Assets/XRFoundation/SampleScenes/SystemSetting/Scenes/SystemSetting.unity" };
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
        [MenuItem("Singray XR/Tookkit/Build Scenes/MRTK2", false, 0)]
        static void MRTK2()
        {
            string[] scenes = { "Assets/XRFoundation/SampleScenes/MRTK2/Scenes/MRTK2.unity" };
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
        [MenuItem("Singray XR/Tookkit/Build Scenes/EyeTracking", false, 0)]
        static void EyeTracking()
        {
            string[] scenes = { "Assets/XRFoundation/SampleScenes/EyeTracking/Scenes/EyeTracking.unity" };
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
        [MenuItem("Singray XR/Tookkit/Build Scenes/EyeCalibration", false, 0)]
        static void EyeCalibration()
        {
            string[] scenes = { "Assets/XRFoundation/SampleScenes/EyeTracking/Scenes/EyeCalibration.unity" };
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

        [MenuItem("Singray XR/Tookkit/Build Scenes/EyeImage", false, 0)]
        static void EyeImage()
        {
            string[] scenes = { "Assets/XRFoundation/SampleScenes/EyeTracking/Scenes/EyeImage.unity" };
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

        [MenuItem("Singray XR/Tookkit/Build Scenes/Gaze", false, 0)]
        static void Gaze()
        {
            string[] scenes = { "Assets/XRFoundation/SampleScenes/Gaze/Scenes/Gaze.unity" };
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

        [MenuItem("Singray XR/Tookkit/Build Scenes/Keyboard", false, 0)]
        static void Keyboard()
        {
            string[] scenes = { "Assets/XRFoundation/SampleScenes/Keyboard/Scenes/Keyboard.unity" };
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


        [MenuItem("Singray XR/Tookkit/Build Scenes/StaticGesture", false, 0)]
        static void StaticGesture()
        {
            string[] scenes = { "Assets/XRFoundation/SampleScenes/StaticGesture/Scenes/StaticGesture.unity" };
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

        [MenuItem("Singray XR/Tookkit/Build Scenes/Joystick", false, 0)]
        static void Joystick()
        {
            string[] scenes = { "Assets/XRFoundation/SampleScenes/Joystick/Scenes/Joystick.unity" };
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

        [MenuItem("Singray XR/Tookkit/Build Scenes/TofPointCloud", false, 0)]
        static void TofPointCloud()
        {
            string[] scenes = { "Assets/XRFoundation/SampleScenes/TofPointCloud/Scenes/TofPointCloud.unity" };
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

        [MenuItem("Singray XR/Tookkit/Build Scenes/IRToWorld", false, 0)]
        static void VideoPlay()
        {
            string[] scenes = { "Assets/XRFoundation/SampleScenes/IRToWorld/Scenes/IRToWorld.unity" };
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



        [MenuItem("Singray XR/Tookkit/Build Scenes/BaseScene", false, 0)]
        static void BaseScene()
        {

            string[] scenes = {

                "Assets/XRFoundation/SampleScenes/SDKSamples/Scenes/BaseScene.unity",
                "Assets/XRFoundation/SampleScenes/SDKSamples/Scenes/Viewer.unity",
                "Assets/XRFoundation/SampleScenes/SDKSamples/Scenes/MediaRecorder.unity",
                "Assets/XRFoundation/SampleScenes/SDKSamples/Scenes/MRTK2.unity",
                "Assets/XRFoundation/SampleScenes/SDKSamples/Scenes/MRVideoCapture.unity",
                "Assets/XRFoundation/SampleScenes/SDKSamples/Scenes/PlaneDetection.unity",
                "Assets/XRFoundation/SampleScenes/SDKSamples/Scenes/Rgbd.unity",
                "Assets/XRFoundation/SampleScenes/SDKSamples/Scenes/RTSPStreamer.unity",
                "Assets/XRFoundation/SampleScenes/SDKSamples/Scenes/SpatialMap.unity",
                "Assets/XRFoundation/SampleScenes/SDKSamples/Scenes/SpatialMesh.unity",
                "Assets/XRFoundation/SampleScenes/SDKSamples/Scenes/SpeechVoice.unity",
                "Assets/XRFoundation/SampleScenes/SDKSamples/Scenes/SystemSetting.unity",
                "Assets/XRFoundation/SampleScenes/SDKSamples/Scenes/TagRecognizer.unity",
                "Assets/XRFoundation/SampleScenes/SDKSamples/Scenes/EyeCalibration.unity",
                "Assets/XRFoundation/SampleScenes/SDKSamples/Scenes/EyeTracking.unity",
                "Assets/XRFoundation/SampleScenes/SDKSamples/Scenes/EyeImage.unity",
                "Assets/XRFoundation/SampleScenes/SDKSamples/Scenes/Keyboard.unity",
                "Assets/XRFoundation/SampleScenes/SDKSamples/Scenes/Gaze.unity",
                "Assets/XRFoundation/SampleScenes/SDKSamples/Scenes/StaticGesture.unity",
                "Assets/XRFoundation/SampleScenes/SDKSamples/Scenes/Joystick.unity",
                "Assets/XRFoundation/SampleScenes/SDKSamples/Scenes/TofPointCloud.unity",
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

        [MenuItem("Singray XR/Tookkit/Build Scenes/SDKSamples", false, 100)]
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