using UnityEngine;
using UnityEditor;

using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;

public static class ResourceTool
{
    [MenuItem("AssetBundle/Check Multi-Referenced Assets")]
    public static void CheckMultiReferencedAsset()
    {
        ResourceUtils.ClearConsole();

        string[] allFolderPaths = Directory.GetDirectories(AssetBundleConst.PREFABS_PATH);

        Dictionary<string, List<string>> dependencyDic = new Dictionary<string, List<string>>();
        for (int i = 0; i < allFolderPaths.Length; ++i)
        {
            string folderPath = allFolderPaths[i];
            List<string> filePaths = new List<string>();
            ResourceUtils.GetAllFilePaths(folderPath, filePaths);
            string[] dependencies = AssetDatabase.GetDependencies(filePaths.ToArray(), true);
            for (int j = 0; j < dependencies.Length; ++j)
            {
                string dependency = dependencies[j];
                if (dependency.Contains(AssetBundleConst.PREFABS_PATH))
                    continue;
                if (ResourceUtils.InDependentFolder(dependency))
                    continue;
                if (ResourceUtils.IsFileIllegal(dependency))
                    continue;

                List<string> assetPathList = null;
                if (dependencyDic.ContainsKey(dependency))
                {
                    assetPathList = dependencyDic[dependency];
                }
                else
                {
                    assetPathList = new List<string>();
                    dependencyDic[dependency] = assetPathList;
                }
                assetPathList.Add(folderPath);
            }
        }

        foreach(var pair in dependencyDic)
        {
            if (pair.Value.Count > 1)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(pair.Key);
                sb.Append("---");
                sb.Append(pair.Value.Count);
                sb.Append('\n');
                for (int i = 0; i < pair.Value.Count; ++i)
                {
                    sb.Append(pair.Value[i]);
                    sb.Append('\n');
                }
                Debug.LogError(sb.ToString());
            }
        }
    }
}

