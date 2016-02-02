using UnityEngine;
using System;
using System.Collections.Generic;

namespace Base
{
    public class TextManager
    {
		private Dictionary<int, string> textDic;

		private static TextManager instance;
		public static TextManager Instance
		{
			get
			{
				if (instance == null)
				{
					instance = new TextManager();
				}
				return instance;
			}
		}

		public void Init()
        {
			textDic = new Dictionary<int, string>();

			TextAsset ta = Resources.Load<TextAsset>("GameText");
			string[] stringItems = ta.text.Split('\n');
			int length = stringItems.Length;
			for (int i = 0; i < length; ++i)
			{
				string item = stringItems[i];
				if (!string.IsNullOrEmpty(item))
				{
					string[] strs = item.Split('\t');
					int id = Convert.ToInt32(strs[0]);
					string text = strs[1];
					textDic.Add(id, text);
				}
			}
        }

		public string GetText(int id)
		{
			if (!textDic.ContainsKey(id))
			{
				Debug.Log("Text doesn't exist! id = " + id);
			}
			return textDic[id];
		}
    }
}

