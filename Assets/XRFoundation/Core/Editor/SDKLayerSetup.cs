#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using System.IO;

public class SDKLayerSetup : AssetPostprocessor
{
    // ������Ҫ������Layer����
    private const string TARGET_LAYER = "XvBGVideo";

    static void OnPostprocessAllAssets(
        string[] importedAssets,
        string[] deletedAssets,
        string[] movedAssets,
        string[] movedFromAssetPaths)
    {
        // ����Ƿ���SDK�ؼ��ļ�������
        bool sdkImported = false;
        foreach (string asset in importedAssets)
        {
            if ( asset.Contains("XvXRFoundation"))
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

        // ���Layer�Ƿ��Ѵ���
        bool layerExists = false;
        for (int i = 6; i < layers.arraySize; i++) // 8-31Ϊ�û�����Layer
        {
            SerializedProperty layerProp = layers.GetArrayElementAtIndex(i);
            if (layerProp.stringValue == TARGET_LAYER)
            {
                layerExists = true;
                break;
            }
        }

        // ������Layer
        if (!layerExists)
        {
            for (int i = 6; i < layers.arraySize; i++)
            {
                SerializedProperty layerProp = layers.GetArrayElementAtIndex(i);
                if (string.IsNullOrEmpty(layerProp.stringValue))
                {
                    layerProp.stringValue = TARGET_LAYER;
                    tagManager.ApplyModifiedProperties();
                    Debug.Log($"[XvSDK] Created layer: {TARGET_LAYER} at index {i}");
                    return;
                }
            }
            Debug.LogError($"[XvSDK] Failed to create layer! All user layers are in use.");
        }
        else
        {
            Debug.Log($"[XvSDK] Layer {TARGET_LAYER} already exists");
        }
    }
}
#endif