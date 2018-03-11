using UnityEngine;

using System.Collections;
using System.Collections.Generic;
using System.IO;

public class ResourceUtils : MonoBehaviour
{
    public static bool IsFileIllegal(string filePath)
    {
        for(int i = 0; i < AssetBundleConst.IgnoreExtension.Length; ++i)
        {
            string extension = AssetBundleConst.IgnoreExtension[i];
            if(filePath.Contains(extension))
                return true;
        }
        return false;
    }

    public static bool InDependentFolder(string filePath)
    {
        for(int i = 0; i < AssetBundleConst.DependentFolders.Length; ++i)
        {
            string folderName = AssetBundleConst.DependentFolders[i];
            if(filePath.Contains(folderName))
                return true;
        }
        return false;
    }

    public static void GetAllFilePaths(string rootFolder, List<string> paths)
    {
        string[] files = Directory.GetFiles(rootFolder);
        for(int i = 0; i < files.Length; ++i)
        {
            string file = files[i];
            if(ResourceUtils.IsFileIllegal(file))
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

    public static void GetAllFolderPaths(string rootFolder, List<string> paths)
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

    public static void ClearConsole()  
    {  
        // This simply does "LogEntries.Clear()" the long way:  
        var logEntries = System.Type.GetType("UnityEditorInternal.LogEntries,UnityEditor.dll");  
        var clearMethod = logEntries.GetMethod("Clear",System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public);  
        clearMethod.Invoke(null, null);  
    }
}

