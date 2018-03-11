using UnityEditor;
using UnityEngine;

using System.IO;
using System.Collections;

public class DependencyChecker
{
    [MenuItem("AssetBundle/Asset Dependencies")]
    public static void CheckAssetDependencies()
    {
        ResourceUtils.ClearConsole();

        UnityEngine.Object asset = Selection.activeObject;
        string path = AssetDatabase.GetAssetPath(asset);
        string extension = Path.GetExtension(path);
        if (extension != string.Empty)
        {
            string[] dependencies = AssetDatabase.GetDependencies(path, true);
            for (int i = 0; i < dependencies.Length; ++i)
            {
                string dependency = dependencies[i];
                if (dependency.EndsWith(".cs"))
                {
                    continue;
                }
                    
                Debug.LogError(dependency);
            }
        }
        else
        {
            string[] subPaths = Directory.GetFiles(path);
            string[] dependencies = AssetDatabase.GetDependencies(subPaths, true);
            for (int i = 0; i < dependencies.Length; ++i)
            {
                string dependency = dependencies[i];
                if (dependency.EndsWith(".cs"))
                {
                    continue;
                }

                Debug.LogError(dependency);
            }
        }
    }

}

