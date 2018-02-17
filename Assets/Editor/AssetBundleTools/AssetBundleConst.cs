using System;

public static class AssetBundleConst
{
    public const string PREFABS_PATH = "Assets/PrefabResources";
    public const string ASSETS_PATH = "Assets/RawResources";

    public const string PREFAB_EXTENSION = ".prefab";
    public const string ASSET_TAG = "asset_";
    public const int MAX_AB_SIZE = 10 * 1024 * 1024;

    public static string[] IgnoreExtension = new string[]
    {
            ".meta",
            ".cs",
    };
}

