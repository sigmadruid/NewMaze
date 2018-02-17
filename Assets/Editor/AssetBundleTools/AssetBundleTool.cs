using UnityEditor;
using UnityEngine;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public static class AssetBundleTool
{
    private static Dictionary<string, List<string>> filePathDic = new Dictionary<string, List<string>>();

    [MenuItem("Tools/Mark AssetBundle Tags")]
    public static void MarkABTags()
    {
        ClearAll();

        MarkPrefabTags();
        MarkAssetsTags();

        AssetDatabase.Refresh();
    }

    private static void MarkPrefabTags()
    {
        string[] folderPathList = Directory.GetDirectories(AssetBundleConst.PREFABS_PATH);
        for (int i = 0; i < folderPathList.Length; ++i)
        {
            string folderPath = folderPathList[i];
            string folderName = Path.GetFileName(folderPath);
            List<string> filePathList = new List<string>();
            filePathDic.Add(folderName, filePathList);
            GetAllFilePaths(folderPath, filePathList);

            for(int j = 0; j < filePathList.Count; ++j)
            {
                string filePath = filePathList[j];
                if(filePath.Contains(".meta"))
                {
                    continue;
                }
                var importer = AssetImporter.GetAtPath(filePath);
                importer.SetAssetBundleNameAndVariant(folderName, string.Empty);
                Debug.LogErrorFormat("{0}, tag={1}", filePath, folderName);
            }
        }
    }

    private static void MarkAssetsTags()
    {
        List<string> allDependencies = new List<string>();
        foreach(string fileName in filePathDic.Keys)
        {
            List<string> filePathList = filePathDic[fileName];
//            List<string> fileNameList = new List<string>();
//            for(int j = 0; j < filePathList.Count; ++j)
//            {
//                string filePath = filePathList[j];
//                fileNameList.Add(filePath);
//            }
            string[] dependencies = AssetDatabase.GetDependencies(filePathList.ToArray());
            allDependencies.AddRange(dependencies);
        }

        for(int i = 0; i < allDependencies.Count; ++i)
        {
            string dependency = allDependencies[i];
            Debug.LogError(dependency);
        }
    }

    public static void BuildAllAB()
    {
    }

    public static void ClearAll()
    {
        foreach(var list in filePathDic.Values)
        {
            list.Clear();
        }
        filePathDic.Clear();
    }

    #region Helper Functions

    private static void GetAllFilePaths(string rootFolder, List<string> paths)
    {
        string[] files = Directory.GetFiles(rootFolder);
        for(int i = 0; i < files.Length; ++i)
        {
            string file = files[i];
            if(file.Contains(".prefab"))
            {
                paths.Add(file);
            }
        }

        string[] folders = Directory.GetDirectories(rootFolder);
        if(folders.Length > 0)
        {
            for(int i = 0; i < folders.Length; ++i)
            {
                string folder = folders[i];
                GetAllFilePaths(folder, paths);
            }
        }
    }

    #endregion
}

