#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using System.IO;

public class SDKLayerSetup : AssetPostprocessor
{
    // 定义需要创建的Layer名称
    private const string TARGET_LAYER = "BGVideo";

    static void OnPostprocessAllAssets(
        string[] importedAssets,
        string[] deletedAssets,
        string[] movedAssets,
        string[] movedFromAssetPaths)
    {
        // 检查是否有SDK关键文件被导入
        bool sdkImported = false;
        foreach (string asset in importedAssets)
        {
            if ( asset.Contains("XRFoundation"))
            {
                sdkImported = true;
                break;
            }
        }

        if (sdkImported)
        {
            AddLayerIfMissing();
        }

     

    }


    public static void AddLayerIfMissing()
    {
        SerializedObject tagManager = new SerializedObject(
            AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset")[0]);
        SerializedProperty layers = tagManager.FindProperty("layers");

        // 检查Layer是否已存在
        bool layerExists = false;
        for (int i = 6; i < layers.arraySize; i++) 
        {
            SerializedProperty layerProp = layers.GetArrayElementAtIndex(i);
            if (layerProp.stringValue == TARGET_LAYER)
            {
                layerExists = true;
                break;
            }
        }

        if (!layerExists)
        {
            for (int i = 6; i < layers.arraySize; i++)
            {
                SerializedProperty layerProp = layers.GetArrayElementAtIndex(i);
                if (string.IsNullOrEmpty(layerProp.stringValue))
                {
                    layerProp.stringValue = TARGET_LAYER;
                    tagManager.ApplyModifiedProperties();
                    Debug.Log($"[SDK] Created layer: {TARGET_LAYER} at index {i}");
                    return;
                }
            }
            Debug.LogError($"[SDK] Failed to create layer! All user layers are in use.");
        }
        else
        {
            Debug.Log($"[SDK] Layer {TARGET_LAYER} already exists");
        }
    }
}
#endif