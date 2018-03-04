using UnityEditor;
using UnityEngine;

using System.IO;
using System.Collections;

public class DependencyChecker
{
    [MenuItem("AssetBundle/Asset Dependencies")]
    public static void CheckAssetDependencies()
    {
        ClearConsole();

        UnityEngine.Object asset = Selection.activeObject;
        string path = AssetDatabase.GetAssetPath(asset);
        string extension = Path.GetExtension(path);
        if (extension != string.Empty)
        {
            string[] dependencies = AssetDatabase.GetDependencies(path, true);
            for (int i = 0; i < dependencies.Length; ++i)
            {
                Debug.LogError(dependencies[i]);
            }
        }
        else
        {
            string[] subPaths = Directory.GetFiles(path);
            string[] dependencies = AssetDatabase.GetDependencies(subPaths, true);
            for (int i = 0; i < dependencies.Length; ++i)
            {
                Debug.LogError(dependencies[i]);
            }
        }
    }

    public static void ClearConsole()  
    {  
        // This simply does "LogEntries.Clear()" the long way:  
        var logEntries = System.Type.GetType("UnityEditorInternal.LogEntries,UnityEditor.dll");  
        var clearMethod = logEntries.GetMethod("Clear",System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public);  
        clearMethod.Invoke(null, null);  
    }

}

