using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
using static UnityEditor.PlayerSettings;
namespace XR.Foundation
{
    public class BuildSettingWidows : EditorWindow, IHasCustomMenu
    {

        [MenuItem("Singray XR/Tookkit/Project Settings", false, 99)]

        static void ConfigPlayerSetting()
        {
            ProjectSettings();

        }


        [MenuItem("GameObject/Singray XR/Project Settings", false, 99)]

        static void ConfigPlayerSettingA()
        {
            ProjectSettings();
        }


        private static void ProjectSettings() {
            Android.minSdkVersion = AndroidSdkVersions.AndroidApiLevel28;
            Android.targetSdkVersion = AndroidSdkVersions.AndroidApiLevel30;


            PlayerSettings.SetScriptingBackend(BuildTargetGroup.Android, ScriptingImplementation.IL2CPP);

            PlayerSettings.SetApiCompatibilityLevel(BuildTargetGroup.Android, ApiCompatibilityLevel.NET_Unity_4_8);


            PlayerSettings.SetUseDefaultGraphicsAPIs(BuildTarget.Android, false);

            PlayerSettings.SetGraphicsAPIs(BuildTarget.Android, new UnityEngine.Rendering.GraphicsDeviceType[] { GraphicsDeviceType.OpenGLES3 });

            PlayerSettings.Android.targetArchitectures = AndroidArchitecture.ARM64;

            PlayerSettings.defaultInterfaceOrientation = UIOrientation.LandscapeLeft;

            string XRProjectValidationSettingsPath = "Project/Player";


            SettingsService.OpenProjectSettings(XRProjectValidationSettingsPath);
        }


      
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