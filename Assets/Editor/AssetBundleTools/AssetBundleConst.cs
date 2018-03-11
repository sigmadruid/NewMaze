using UnityEngine;

using System;

public static class AssetBundleConst
{
    public static readonly string OUTPUT_PATH = Application.dataPath + "/AssetBundles/";

    public const string PREFABS_PATH = "Assets/PrefabResources";
    public const string ASSETS_PATH = "Assets/DependentResources";
    public const string COMMON_ASSETS_PATH = "Assets/DependentResources/Common";

    public const string PREFAB_EXTENSION = ".prefab";
    public const string ASSET_TAG = "asset_";
    public const int MAX_AB_SIZE = 10 * 1024 * 1024;

    public static string[] IgnoreExtension = new string[]
    {
            ".meta",
            ".cs",
            ".js",
    };
}

