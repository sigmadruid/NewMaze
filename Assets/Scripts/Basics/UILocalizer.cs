using UnityEngine;
using UnityEngine.UI;

using System;

using StaticData;

public class UILocalizer
{
    public static string KEY_MARKER = "&";

    public static void LocalizeByName(Transform root)
    {
        Text[] textList = root.GetComponentsInChildren<Text>(true);
        foreach(Text label in textList)
        {
            if (label.name.StartsWith(KEY_MARKER))
            {
                string key = label.name.Substring(1);
                label.text = TextDataManager.Instance.GetData(key);
            }
        }
    }
}

