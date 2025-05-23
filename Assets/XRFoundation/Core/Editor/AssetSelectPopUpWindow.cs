using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using System.IO;


public class AssetSelectPopUpWindow : EditorWindow
{
    private Vector2 scrollPosition;
    private List<string> items = null;
    //�Ƿ񵼳��ű�
    public static bool exportWithScript = false;
    private bool[] selectionStates;


    #region editor�˵����


    [MenuItem("Assets/Tools/����Unity��Դ��")]
    public static void ExportWithoutScript()
    {
        exportWithScript = false;
        ShowWindow();
    }


    [MenuItem("Assets/Tools/����Unity��Դ��(�����ű�)")]
    public static void ExportWithScript()
    {
        exportWithScript = true;
        ShowWindow();
    }

    public static void ShowWindow()
    {
        AssetSelectPopUpWindow wnd = GetWindow<AssetSelectPopUpWindow>();
        wnd.titleContent = new GUIContent("��Դ����");
        wnd.minSize = new Vector2(450, 200);
        wnd.maxSize = new Vector2(1920, 720);
        wnd.Show();
    }
    #endregion




    public void GetAllFiles(bool withScript)
    {
        //��ȡ���ѡ�е������ļ�
        Object[] selectedObjects = Selection.GetFiltered<Object>(SelectionMode.Assets);
        List<string> assetPathNames = new List<string>();
        for (int i = 0; i < selectedObjects.Length; i++)
        {
            string directoryPath = AssetDatabase.GetAssetPath(selectedObjects[i]);
            if (directoryPath != null)
            {
                //������ļ��У��ͱ����ļ����µ�������Դ
                if (Directory.Exists(directoryPath))
                {
                    string[] folders = Directory.GetFiles(directoryPath);
                    for (int j = 0; j < folders.Length; j++)
                    {
                        //���˵�.meta�ļ�
                        if (!folders[j].EndsWith(".meta"))
                        {
                            assetPathNames.Add(folders[j]);
                        }
                    }
                }
                else
                {
                    assetPathNames.Add(directoryPath);
                }
            }
        }

        items = new List<string>();

        for (int i = 0; i < assetPathNames.Count; i++)
        {
            var depends = AssetDatabase.GetDependencies(assetPathNames[i], true);
            for (int j = 0; j < depends.Length; j++)
            {
                AddFiles(withScript, depends[j]);
            }
        }


        items.Sort();
        selectionStates = new bool[items.Count];
        //Ĭ��ȫѡ 
        SelectAllItems();

    }


    private void OnEnable()
    {
        GetAllFiles(exportWithScript);
    }

    //��ӡ����ѡ����ļ�
    private void ShowFiles()
    {
        for (int i = 0; i < items.Count; i++)
        {
            Debug.Log($"all Files is {items[i]}");
        }
    }
    private void AddFiles(bool withScript, string filePath)
    {
        //�ض���Ŀ¼������
        if (filePath.StartsWith("Packages/"))
        {
            return;
        }
        if (withScript || !filePath.EndsWith(".cs"))
        {
            if (!items.Contains(filePath))
            {
                items.Add(filePath);
            }
        }
    }
    private void OnGUI()
    {
        EditorGUILayout.Space(10);

        // Scroll view
        using (var scrollView = new EditorGUILayout.ScrollViewScope(scrollPosition))
        {
            scrollPosition = scrollView.scrollPosition;

            for (int i = 0; i < items.Count; i++)
            {
                selectionStates[i] = EditorGUILayout.ToggleLeft(items[i], selectionStates[i]);
            }
        }

        GUILayout.Space(10);

        // Buttons
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("ȫѡ"))
        {
            SelectAllItems();
        }
        if (GUILayout.Button("ȫ��ѡ"))
        {
            DeselectAllItems();
        }
        GUILayout.EndHorizontal();

        GUILayout.Space(10);

        if (GUILayout.Button("����"))
        {
            OutputSelectedItems();

        }
    }
    #region ��ť�¼�
    private void SelectAllItems()
    {
        for (int i = 0; i < selectionStates.Length; i++)
        {
            selectionStates[i] = true;
        }
    }

    private void DeselectAllItems()
    {
        for (int i = 0; i < selectionStates.Length; i++)
        {
            selectionStates[i] = false;
        }
    }

    private void OutputSelectedItems()
    {
        List<string> exportItems = new List<string>();
        for (int i = 0; i < items.Count; i++)
        {
            if (selectionStates[i])
            {
                exportItems.Add(items[i]);
            }
        }
        if (exportItems.Count == 0)
        {
            EditorUtility.DisplayDialog("û���κ�ѡ�е���Դ", "��ѡ����Ҫ��������Դ", "OK");
            return;
        }
        var path = EditorUtility.SaveFilePanel("������Դ��", "", "", "unitypackage");
        if (path == "")
            return;

        var flag = ExportPackageOptions.Interactive | ExportPackageOptions.Recurse;
        //���ѡ������뵼������Դ��������Դ�������ExportPackageOptions.IncludeDependencies��Flag
        //if (exportWithScript)
        //{
        //    flag = flag | ExportPackageOptions.IncludeDependencies;
        //}
        AssetDatabase.ExportPackage(exportItems.ToArray(), path, flag);
        Close();

    }
    #endregion



}