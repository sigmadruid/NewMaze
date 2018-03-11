using UnityEditor;
using UnityEngine;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public static class AssetBundleTool
{
    private class Dependency
    {
        public string PrefabName;

        public string AssetPath;
    }


    [MenuItem("AssetBundle/Mark AssetBundle Tags")]
    public static void MarkABTags()
    {
        DateTime dt = DateTime.Now;

        ClearAll();

        MarkPrefabTags();
//        MarkAssetsTags();

        Debug.Log((DateTime.Now - dt).Ticks / 10000000f + "s");

        AssetDatabase.Refresh();
    }

    private static void MarkPrefabTags()
    {
        string[] prefabFolderPathList = Directory.GetDirectories(AssetBundleConst.PREFABS_PATH);
        foreach(string prefabFolderPath in prefabFolderPathList)
        {
            List<string> prefabPathList = new List<string>();
            ResourceUtils.GetAllFilePaths(prefabFolderPath, prefabPathList);
            string abName = GetABName(prefabFolderPath);
            foreach(string prefabPath in prefabPathList)
            {
                if (ResourceUtils.IsFileIllegal(prefabPath))
                    continue;
                SetAssetBundleTag(abName, prefabPath);
            }
        }
    }

//    private static void MarkAssetsTags()
//    {
//        FileLogger.Init("ab_dependencies");
//        List<Dependency> allDependencies = new List<Dependency>();
//        foreach(string fileName in depPathDic.Keys)
//        {
//            List<string> filePathList = depPathDic[fileName];
//            string[] dependencies = AssetDatabase.GetDependencies(filePathList.ToArray());
//            FileLogger.AddLog(fileName);
//            for(int i = 0; i < dependencies.Length; ++i)
//            {
//                string path = dependencies[i];
//                if(ResourceUtils.IsFileIllegal(path) || path.Contains(AssetBundleConst.PREFABS_PATH))
//                {
//                    continue;
//                }
//
//                Dependency dependency = new Dependency();
//                dependency.PrefabName = fileName;
//                dependency.AssetPath = path;
//                    
//                allDependencies.Add(dependency);
//                FileLogger.AddLog(string.Format("\t{0}", dependency.AssetPath));
//            }
//        }
//        FileLogger.Flush();
//
//        HashSet<string> tempHashSet = new HashSet<string>();
//        List<Dependency> uniqueDependencies = new List<Dependency>();
//        for(int i = 0; i < allDependencies.Count; ++i)
//        {
//            Dependency dependency = allDependencies[i];
//            if(tempHashSet.Contains(dependency.AssetPath))
//            {
//                continue;
//            }
//            tempHashSet.Add(dependency.AssetPath);
//            uniqueDependencies.Add(dependency);
//        }
//
//        FileLogger.Init("ab_division");
//        int tagIndex = 0;
//        long size = 0;
//        long logSize = 0;
//        for(int i = 0; i < uniqueDependencies.Count; ++i)
//        {
//            if(size == 0)
//            {
//                tagIndex++;
//            }
//            Dependency dependency = uniqueDependencies[i];
//            FileInfo fi = new FileInfo(dependency.AssetPath);
//            logSize = 0;
//            if(fi.Length >= AssetBundleConst.MAX_AB_SIZE)
//            {
//                FileLogger.AddLog(size / 1000 + "KB");
//                tagIndex++;
//                logSize = fi.Length;
//                size = 0;
//            }
//            else
//            {
//                size += fi.Length;
//                if(size >= AssetBundleConst.MAX_AB_SIZE)
//                {
//                    logSize = size;
//                    size = 0;
//                }
//            }
//
//            if(i == uniqueDependencies.Count - 1)
//                logSize = size;
//
//
//            string log = string.Format("{0}, {1}, {2}", dependency.PrefabName, dependency.AssetPath, tagIndex.ToString());
//            FileLogger.AddLog(log);
//            if (logSize > 0)
//                FileLogger.AddLog(logSize / 1000 + "KB");
//            var importer = AssetImporter.GetAtPath(dependency.AssetPath);
//            importer.assetBundleName = AssetBundleConst.ASSET_TAG + tagIndex.ToString();
//        }
//        FileLogger.Flush();
//
//        Debug.LogError("mark asset tags completed!");
//    }

    private static string currentABName;
    private static long currentABSize = 0;
    private static int currentABIndex = 0;
    private static void SetAssetBundleTag(string abName, string path)
    {
        if (abName != currentABName)
        {
            currentABName = abName;
            currentABSize = 0;
            currentABIndex = 0;
        }

        FileInfo fi = new FileInfo(path);
        AssetImporter importer = AssetImporter.GetAtPath(path);
        if (fi.Length >= AssetBundleConst.MAX_AB_SIZE)
        {
            currentABIndex++;
            importer.SetAssetBundleNameAndVariant(currentABName + "_" + currentABIndex.ToString(), string.Empty);
            currentABIndex++;
            currentABSize = 0;
            return;
        }
        importer.SetAssetBundleNameAndVariant(currentABName + "_" + currentABIndex.ToString(), string.Empty);
        currentABSize += fi.Length;
        if (currentABSize >= AssetBundleConst.MAX_AB_SIZE)
        {
            currentABIndex++;
            currentABSize = 0;
        }

    }

    private static string GetABName(string path)
    {
        int index = -1;
        int length = 0;
        index = path.IndexOf(AssetBundleConst.PREFABS_PATH);
        if (index != -1)
        {
            length = AssetBundleConst.PREFABS_PATH.Length;
        }
        else
        {
            index = path.IndexOf(AssetBundleConst.ASSETS_PATH);
            if (index != -1)
            {
                length = AssetBundleConst.ASSETS_PATH.Length;
            }
            else
            {
                return string.Empty;
            }
        }
        index = index + length + 1;
        int lastIndex = path.IndexOf(Path.DirectorySeparatorChar, index);
        if (lastIndex == -1)
            return path.Substring(index);
        else
            return path.Substring(index, lastIndex - index);
    }

    [MenuItem("AssetBundle/Build AssetBundles")]
    public static void BuildAllAB()
    {
        BuildAssetBundleOptions options = BuildAssetBundleOptions.DeterministicAssetBundle |
                                        BuildAssetBundleOptions.UncompressedAssetBundle;
        BuildPipeline.BuildAssetBundles(AssetBundleConst.OUTPUT_PATH, options, BuildTarget.StandaloneWindows64);

        Debug.LogError("build all asset bundles completed!");
    }

    [MenuItem("AssetBundle/Clear All")]
    public static void ClearAll()
    {
        string[] allAssets = AssetDatabase.GetAllAssetPaths();
        for(int i = 0; i < allAssets.Length; ++i)
        {
            string path = allAssets[i];
            if (ResourceUtils.IsFileIllegal(path))
                continue;
            AssetImporter.GetAtPath(path).SetAssetBundleNameAndVariant(string.Empty, string.Empty);
        }

        string[] abFiles = Directory.GetFiles(AssetBundleConst.OUTPUT_PATH);
        foreach(string file in abFiles)
        {
            File.Delete(file);
        }

        GC.Collect();

        Debug.LogError("clear all completed!");
    }

    #region Helper Functions

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

