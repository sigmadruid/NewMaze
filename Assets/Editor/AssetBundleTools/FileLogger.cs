using UnityEngine;

using System;
using System.Text;
using System.IO;

public static class FileLogger
{
    private static readonly string OUTPUT_PATH = Application.dataPath + "/Logs/AssetBundle/";

    private static string logName = string.Empty;
    private static StringBuilder sb = new StringBuilder();

    public static void Init(string name)
    {
        logName = name;
        sb = new StringBuilder();
    }

    public static void AddLog(string str)
    {
        sb.Append(str);
    }

    public static void Flush()
    {
        string path = OUTPUT_PATH + logName + ".txt";
        using(FileStream fs = new FileStream(path, FileMode.Create))
        {
            StreamWriter sw = new StreamWriter(fs);
            sw.Write(sb.ToString());
            sw.Flush();
            sw.Close();
        }
    }
        
}

