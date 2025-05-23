using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
using static Codice.Client.Common.DiffMergeToolConfig;
using static UnityEditor.PlayerSettings;
namespace XvXR.Foundation
{
    public class BuildSettingWidows : EditorWindow, IHasCustomMenu
    {

        [MenuItem("XvXR/Tookkit/Project Settings", false, 99)]

        static void ConfigPlayerSetting()
        {
            ProjectSettings();

        }


        [MenuItem("GameObject/XvXR/Project Settings", false, 99)]

        static void ConfigPlayerSettingA()
        {
            ProjectSettings();
        }


        private static void ProjectSettings() {
            //设置Android API版本
            Android.minSdkVersion = AndroidSdkVersions.AndroidApiLevel26;
            Android.targetSdkVersion = AndroidSdkVersions.AndroidApiLevel27;


            //设置构建方式为IL2CPP
            PlayerSettings.SetScriptingBackend(BuildTargetGroup.Android, ScriptingImplementation.IL2CPP);

            //设置.NetFramework
            PlayerSettings.SetApiCompatibilityLevel(BuildTargetGroup.Android, ApiCompatibilityLevel.NET_Unity_4_8);


            //禁用Auto Graphic API
            PlayerSettings.SetUseDefaultGraphicsAPIs(BuildTarget.Android, false);

            //设置Graphics API为OpenGLES3
            PlayerSettings.SetGraphicsAPIs(BuildTarget.Android, new UnityEngine.Rendering.GraphicsDeviceType[] { GraphicsDeviceType.OpenGLES3 });

            //设置ARM64             
            PlayerSettings.Android.targetArchitectures = AndroidArchitecture.ARM64;

            //设置屏幕旋转方向
            PlayerSettings.defaultInterfaceOrientation = UIOrientation.LandscapeLeft;

            string XRProjectValidationSettingsPath = "Project/Player";


            SettingsService.OpenProjectSettings(XRProjectValidationSettingsPath);
        }


       //static BuildSettingWidows windows;
       // private static void CreateWinbdows() {
       //     Rect rect = new Rect(0, 0, 800, 400);

       //      windows = (BuildSettingWidows)EditorWindow.GetWindowWithRect(typeof(BuildSettingWidows), rect);
       //     GUIContent gUIContent = new GUIContent("PlayerSetting");
       //     windows.titleContent = gUIContent;
       //     windows.Show();
       // }
       // private void OnGUI()
       // {

       //     EditorGUILayout.LabelField("Minimun API Level=26");
       //     EditorGUILayout.LabelField("Target API Level=27");
       //     EditorGUILayout.LabelField("Scripting Backend=IL2CPP");
       //     EditorGUILayout.LabelField("Api Compatibility Level*=.NET Framework");
       //     EditorGUILayout.LabelField("Target Architectures=ARM64");
       //     EditorGUILayout.LabelField("Auto Graphics  API");
       //     EditorGUILayout.LabelField("Graphics  API =OpenGLES3");
       //     EditorGUILayout.LabelField("Default Orientation=Landscape Left");

            




       //     if (GUILayout.Button("Apply"))
       //     {
       //         //设置Android API版本
       //         Android.minSdkVersion = AndroidSdkVersions.AndroidApiLevel26;
       //         Android.targetSdkVersion = AndroidSdkVersions.AndroidApiLevel27;


       //         //设置构建方式为IL2CPP
       //         PlayerSettings.SetScriptingBackend(BuildTargetGroup.Android, ScriptingImplementation.IL2CPP);

       //         //设置.NetFramework
       //         PlayerSettings.SetApiCompatibilityLevel(BuildTargetGroup.Android, ApiCompatibilityLevel.NET_Unity_4_8);


       //         //禁用Auto Graphic API
       //         PlayerSettings.SetUseDefaultGraphicsAPIs(BuildTarget.Android, false);

       //         //设置Graphics API为OpenGLES3
       //         PlayerSettings.SetGraphicsAPIs(BuildTarget.Android, new UnityEngine.Rendering.GraphicsDeviceType[] { GraphicsDeviceType.OpenGLES3 });

       //         //设置ARM64             
       //         PlayerSettings.Android.targetArchitectures = AndroidArchitecture.ARM64;

       //         //设置屏幕旋转方向
       //         PlayerSettings.defaultInterfaceOrientation = UIOrientation.LandscapeLeft;


       //         windows.Close();


       //     }







       // }

        private void OnDestroy()
        {

        }

        private void OnFocus()
        {

        }

        private void OnHierarchyChange()
        {

        }

        private void OnInspectorUpdate()
        {

        }

        private void OnLostFocus()
        {

        }
        private void OnProjectChange()
        {

        }

        private void OnSelectionChange()
        {

        }
        private void Update()
        {

        }

        public void AddItemsToMenu(GenericMenu menu)
        {
            menu.AddItem(new GUIContent("OK"), true, () =>
            {
                Debug.Log("ok");
            });
        }
    }

}