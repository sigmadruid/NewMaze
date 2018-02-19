using UnityEngine;

using System;
using System.Text;
using System.IO;

public static class FileLogger
{
    private static readonly string OUTPUT_PATH = Application.dataPath + "/Logs/AssetBundle/mark_tags.txt";

    private static StringBuilder sb = new StringBuilder();

    public static void Init()
    {
        sb = new StringBuilder();
    }

    public static void AddLog(string str)
    {
        sb.Append(str);
    }

    public static void LogToFile()
    {
        using(FileStream fs = new FileStream(OUTPUT_PATH, FileMode.Create))
        {
            StreamWriter sw = new StreamWriter(fs);
            sw.Write(sb.ToString());
            sw.Flush();
            sw.Close();
        }
    }
        
}

