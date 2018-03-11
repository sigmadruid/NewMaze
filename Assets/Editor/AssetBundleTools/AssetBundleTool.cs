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

        FileLogger.Init("ab_size_log");
        MarkPrefabTags();
        MarkAssetsTags();
        FileLogger.Flush();

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
                SetAssetBundleTag(AssetBundleConst.PREFAB_TAG, abName, prefabPath);
            }
        }
    }

    private static void MarkAssetsTags()
    {
        string[] assetFolderPathList = Directory.GetDirectories(AssetBundleConst.ASSETS_PATH);
        foreach(string assetFolderPath in assetFolderPathList)
        {
            List<string> assetPathList = new List<string>();
            ResourceUtils.GetAllFilePaths(assetFolderPath, assetPathList);
            string abName = GetABName(assetFolderPath);
            foreach(string assetPath in assetPathList)
            {
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
        BuildAssetBundleOptions options = BuildAssetBundleOptions.DeterministicAssetBundle |
                                        BuildAssetBundleOptions.UncompressedAssetBundle;
        BuildPipeline.BuildAssetBundles(AssetBundleConst.OUTPUT_PATH, options, BuildTarget.StandaloneOSXUniversal);

        Debug.LogError("build all asset bundles completed!");
    }

    [MenuItem("AssetBundle/Clear All")]
    public static void ClearAll()
    {
        ResourceUtils.ClearConsole();

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

