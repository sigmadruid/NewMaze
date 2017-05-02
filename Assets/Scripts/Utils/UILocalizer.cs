using UnityEngine;
using UnityEngine.UI;

using System;

namespace GameLogic
{
    public static class UILocalizer
    {
        private const string TEXT_PREFIX = "text";
        public static void Replace(Transform transform)
        {
            Text[] textList = transform.GetComponentsInChildren<Text>();
            for(int i = 0; i < textList.Length; ++i)
            {
                Text text = textList[i];
                if(!text.name.StartsWith(TEXT_PREFIX))
                {
                    text.text = StaticData.TextDataManager.Instance.GetData(text.name);
                }
            }
        }
    }
}

