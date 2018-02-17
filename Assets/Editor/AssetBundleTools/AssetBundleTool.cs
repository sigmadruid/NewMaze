using UnityEditor;
using UnityEngine;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public static class AssetBundleTool
{
    private static Dictionary<string, List<string>> depPathDic = new Dictionary<string, List<string>>();


    [MenuItem("Tools/Mark AssetBundle Tags")]
    public static void MarkABTags()
    {
        DateTime dt = DateTime.Now;

        ClearAll();

        MarkPrefabTags();
        MarkAssetsTags();

        Debug.Log((DateTime.Now - dt).Ticks / 10000000f + "s");

        AssetDatabase.Refresh();
    }

    private static void MarkPrefabTags()
    {
        List<string> folderPathList = new List<string>();
        GetAllFolderPaths(AssetBundleConst.PREFABS_PATH, folderPathList);

        for (int i = 0; i < folderPathList.Count; ++i)
        {
            string folderPath = folderPathList[i];
            string folderTag = GetFolderTag(folderPath);
            List<string> filePathList = new List<string>();
            depPathDic.Add(folderTag, filePathList);
            Debug.LogError(folderTag);

            string[] allFilePathList = Directory.GetFiles(folderPath);
            for(int j = 0; j < allFilePathList.Length; ++j)
            {
                string filePath = allFilePathList[j];
                if(IsFileIllegal(filePath))
                {
                    continue;
                }

                filePathList.Add(filePath);
                var importer = AssetImporter.GetAtPath(filePath);
                importer.SetAssetBundleNameAndVariant(folderTag, string.Empty);
            }

//            for(int j = 0; j < filePathList.Count; ++j)
//            {
//                string filePath = filePathList[j];
//                Debug.Log(filePath);
//            }
        }
    }

    private static void MarkAssetsTags()
    {
        List<string> allDependencies = new List<string>();
        foreach(string fileName in depPathDic.Keys)
        {
            List<string> filePathList = depPathDic[fileName];
            string[] dependencies = AssetDatabase.GetDependencies(filePathList.ToArray());
            for(int i = 0; i < dependencies.Length; ++i)
            {
                string dependency = dependencies[i];
                if(IsFileIllegal(dependency))
                {
                    continue;
                }
                allDependencies.Add(dependency);
            }
        }

        HashSet<string> tempHashSet = new HashSet<string>();
        List<string> uniqueDependencies = new List<string>();
        for(int i = 0; i < allDependencies.Count; ++i)
        {
            string dependency = allDependencies[i];
            if(tempHashSet.Contains(dependency))
            {
                continue;
            }
            tempHashSet.Add(dependency);
            uniqueDependencies.Add(dependency);
        }

        int tagIndex = 1;
        long size = 0;
        for(int i = 0; i < uniqueDependencies.Count; ++i)
        {
            string dependency = uniqueDependencies[i];
            Debug.Log(dependency);

//            var importer = AssetImporter.GetAtPath(dependency);
//            importer.assetBundleName = AssetBundleConst.ASSET_TAG + tagIndex.ToString();

            FileInfo fi = new FileInfo(dependency);
            size += fi.Length;
            if(size >= AssetBundleConst.MAX_AB_SIZE)
            {
                Debug.LogErrorFormat("{0}, {1}KB", tagIndex, size / 1000);
                tagIndex++;
                size = 0;
            }
        }
    }

    public static void BuildAllAB()
    {
    }

    public static void ClearAll()
    {
        foreach(var list in depPathDic.Values)
        {
            list.Clear();
        }
        depPathDic.Clear();

        GC.Collect();
    }

    #region Helper Functions

    private static bool IsFileIllegal(string filePath)
    {
        for(int i = 0; i < AssetBundleConst.IgnoreExtension.Length; ++i)
        {
            string extension = AssetBundleConst.IgnoreExtension[i];
            if(filePath.Contains(extension))
                return true;
        }
        return false;
    }

    private static void GetAllFilePaths(string rootFolder, List<string> paths)
    {
        string[] files = Directory.GetFiles(rootFolder);
        for(int i = 0; i < files.Length; ++i)
        {
            string file = files[i];
            if(IsFileIllegal(file))
            {
                continue;
            }
            paths.Add(file);
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

    private static void GetAllFolderPaths(string rootFolder, List<string> paths)
    {
        string[] folders = Directory.GetDirectories(rootFolder);
        if(folders.Length > 0)
        {
            for(int i = 0; i < folders.Length; ++i)
            {
                string folder = folders[i];
                paths.Add(folder);
                GetAllFolderPaths(folder, paths);
            }
        }
    }

    private static string GetFolderTag(string path)
    {
        int index = path.IndexOf(AssetBundleConst.PREFABS_PATH) + AssetBundleConst.PREFABS_PATH.Length + 1;
        string subPath = path.Substring(index);
        subPath = subPath.Replace(Path.DirectorySeparatorChar, '+');
        subPath = subPath.Replace(Path.AltDirectorySeparatorChar, '+');
        return subPath.ToLower();
    }

    #endregion
}

