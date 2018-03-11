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

        ClearTags();

        FileLogger.Init("ab_size_log");

        MarkPrefabTags();
        MarkAssetsTags();

        FileLogger.Flush();

        Debug.Log((DateTime.Now - dt).Ticks / 10000000f + "s");

        AssetDatabase.Refresh();
    }

    private static void MarkPrefabTags()
    {
        string[] prefabFolderPathArray = Directory.GetDirectories(AssetBundleConst.PREFABS_PATH);
        List<string> prefabFolderPathList = new List<string>(prefabFolderPathArray);
        prefabFolderPathList.Sort();
        for (int i = 0; i < prefabFolderPathList.Count; ++i)
        {
            string prefabFolderPath = prefabFolderPathList[i];
            List<string> prefabPathList = new List<string>();
            ResourceUtils.GetAllFilePaths(prefabFolderPath, prefabPathList);
            prefabPathList.Sort();

            string abName = GetABName(prefabFolderPath);
            for (int j = 0; j < prefabPathList.Count; ++j)
            {
                string prefabPath = prefabPathList[j];
                if (ResourceUtils.IsFileIllegal(prefabPath))
                    continue;
                SetAssetBundleTag(AssetBundleConst.PREFAB_TAG, abName, prefabPath);
            }
        }
    }

    private static void MarkAssetsTags()
    {
        string[] assetFolderPathArray = Directory.GetDirectories(AssetBundleConst.ASSETS_PATH);
        List<string> assetFolderPathList = new List<string>(assetFolderPathArray);
        assetFolderPathList.Sort();
        for (int i = 0; i < assetFolderPathList.Count; ++i)
        {
            string assetFolderPath = assetFolderPathList[i];
            List<string> assetPathList = new List<string>();
            ResourceUtils.GetAllFilePaths(assetFolderPath, assetPathList);
            assetPathList.Sort();

            string abName = GetABName(assetFolderPath);
            for (int j = 0; j < assetPathList.Count; ++j)
            {
                string assetPath = assetPathList[j];
                if (ResourceUtils.IsFileIllegal(assetPath))
                    continue;
                SetAssetBundleTag(AssetBundleConst.ASSET_TAG, abName, assetPath);
            }
        }
    }

    private static string currentABName;
    private static long currentABSize = 0;
    private static int currentABIndex = 0;
    private static void SetAssetBundleTag(string type, string abName, string path)
    {
        if (abName != currentABName)
        {
            FileLogger.AddLog(string.Format("{0}_{1}_{2}, {3}KB", type, currentABName, currentABIndex, currentABSize / 1000));
            currentABName = abName;
            currentABSize = 0;
            currentABIndex = 0;
        }

        FileInfo fi = new FileInfo(path);
        AssetImporter importer = AssetImporter.GetAtPath(path);
        if (fi.Length >= AssetBundleConst.MAX_AB_SIZE || currentABSize >= AssetBundleConst.MAX_AB_SIZE)
        {
            FileLogger.AddLog(string.Format("{0}_{1}_{2}, {3}KB", type, currentABName, currentABIndex, currentABSize / 1000));
            currentABIndex++;
            currentABSize = 0;
        }
        string tag = string.Format("{0}_{1}_{2}", type, currentABName, currentABIndex);
        importer.SetAssetBundleNameAndVariant(tag, string.Empty);
        currentABSize += fi.Length;
        FileLogger.AddLog(path);
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
        ClearAssetBundles();

        BuildAssetBundleOptions options = BuildAssetBundleOptions.DeterministicAssetBundle |
                                        BuildAssetBundleOptions.UncompressedAssetBundle;
        BuildPipeline.BuildAssetBundles(AssetBundleConst.OUTPUT_PATH, options, BuildTarget.StandaloneOSXUniversal);

        Debug.LogError("build all asset bundles completed!");
    }

    [MenuItem("AssetBundle/Clear All")]
    public static void ClearAll()
    {
        ResourceUtils.ClearConsole();

        ClearTags();
        ClearAssetBundles();

        GC.Collect();

        Debug.LogError("clear all completed!");
    }

    private static void ClearTags()
    {
        string[] allAssets = AssetDatabase.GetAllAssetPaths();
        for(int i = 0; i < allAssets.Length; ++i)
        {
            string path = allAssets[i];
            if (ResourceUtils.IsFileIllegal(path))
                continue;
            AssetImporter.GetAtPath(path).SetAssetBundleNameAndVariant(string.Empty, string.Empty);
        }
    }
    private static void ClearAssetBundles()
    {
        string[] abFiles = Directory.GetFiles(AssetBundleConst.OUTPUT_PATH);
        foreach(string file in abFiles)
        {
            File.Delete(file);
        }
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

