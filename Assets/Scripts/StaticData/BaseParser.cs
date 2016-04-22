using UnityEngine;

using System;
using System.Collections.Generic;
using System.IO;

using Base;

namespace StaticData
{
    public class BaseParser
    {
		public static string CONFIG_PATH = Application.streamingAssetsPath + "/Configs/";

		private int rowIndex;
		private List<string[]> dataStrList;

        public BaseParser()
        {
			dataStrList = new List<string[]>();
        }

		protected void LoadFile(string name)
		{
			string path = CONFIG_PATH + name;

			if (File.Exists(path))
			{
				StreamReader sr = new StreamReader(path);
				bool isFirstLine = true;
				while (!sr.EndOfStream)
				{
					string line = sr.ReadLine();
					if (isFirstLine)
					{
						isFirstLine = false;
						continue;
					}
					char firstChar = line[0];
					if (firstChar == ',')
					{
						continue;
					}
					string[] strArray = line.Split(',');
					dataStrList.Add(strArray);
				}
				sr.Close();

				rowIndex = 0;
			}
			else
			{
				BaseLogger.LogFormat("Can't find config file: {0}", path);
			}

		}
		protected void UnloadFile()
		{
		}

		protected bool EndOfRow
		{
			get
			{
				return rowIndex == dataStrList.Count;
			}
		}
		protected void NextLine()
		{
			rowIndex++;
		}
		protected bool ReadBool(int col)
		{
			string value = ReadString(col);
			if(value.ToLower() == "true" || value == "1")
			{
				return true;
			}
			else
			{
				return false;
			}
		}
		protected int ReadInt(int col)
		{
			string[] strArray = dataStrList[rowIndex];
			string str = strArray[col];
			return Convert.ToInt32(str);
		}
		protected string ReadString(int col)
		{
			string[] strArray = dataStrList[rowIndex];
			string str = strArray[col];
			return str;
		}
		protected float ReadFloat(int col)
		{
			string[] strArray = dataStrList[rowIndex];
			string str = strArray[col];
			return Convert.ToSingle(str);
		}
		protected T ReadEnum<T>(int col)
		{
			string value = ReadString(col);
			if(!Enum.IsDefined(typeof(T), value))
			{
				BaseLogger.LogFormat("Parse error. CSV: {0}, Col:{1}, Row:{2}", this.ToString(), col, rowIndex);
			}
			return ParseKey<T>(value);
		}
		protected List<string> ReadStringList(int col)
		{
			string value = ReadString(col);
			List<string> list = new List<string>();
			if (!string.IsNullOrEmpty(value))
			{
				string[] strList = value.Split('#');
				for (int i = 0; i < strList.Length; ++i)
				{
					string str = strList[i];
					list.Add(str);
				}
			}
			return list;
		}
		protected List<int> ReadIntList(int col)
		{
			string value = ReadString(col);
			List<int> list = new List<int>();
			if (!string.IsNullOrEmpty(value))
			{
				string[] strList = value.Split('#');
				for (int i = 0; i < strList.Length; ++i)
				{
					string str = strList[i];
					list.Add(Convert.ToInt32(str));
				}
			}
			return list;
		}
		protected List<T> ReadEnumList<T>(int col)
		{
			string value = ReadString(col);
			List<T> list = new List<T>();
			if (!string.IsNullOrEmpty(value))
			{
				string[] strList = value.Split('#');
				for (int i = 0; i < strList.Length; ++i)
				{
					string str = strList[i];
					T enumVal = ParseKey<T>(str);
					list.Add(enumVal);
				}
			}
			return list;
		}
        protected Dictionary<K, V> ReadDictionary<K, V>(int col)
        {
            string value = ReadString(col);
            if (string.IsNullOrEmpty(value))
            {
                return new Dictionary<K, V>();
            }

            string[] valList = value.Split('#');
            Dictionary<K,V> dic = new Dictionary<K, V>();
            for(int i = 0; i < valList.Length; ++i)
            {
                string[] pairList = valList[i].Split(':');
                K k = ParseKey<K>(pairList[0].Trim());
                V v = ParseValue<V>(pairList[1].Trim());
                dic.Add(k, v);
            }
            return dic;
        }


		private T ParseKey<T>(string str)
		{
//            T enumKey = (T)Enum.Parse(typeof(T), str);
            object resultKey = null;
            if (typeof(T) == typeof(int))
            {
                resultKey = Convert.ToInt32(str);
            }
            else
            {
                resultKey = Enum.Parse(typeof(T), str);
            }
            return (T)resultKey;
		}
		private T ParseValue<T>(string str)
		{
			object resultVal = null;

            if(typeof(T) == typeof(int))
            {
                resultVal = Convert.ToInt32(str);
            }
            else if(typeof(T) == typeof(float))
            {
                resultVal = Convert.ToSingle(str);
            }
            else
            {
                resultVal = Enum.Parse(typeof(T), str);
            }

			return (T)resultVal;
		}

        protected Color ReadColor(int col)
        {
            string value = ReadString(col);
            if (string.IsNullOrEmpty(value))
            {
                return Color.white;
            }

            string[] colorList = value.Split('#');
            Color color = new Color();
            color.r = Convert.ToSingle(colorList[0]);
            color.g = Convert.ToSingle(colorList[1]);
            color.b = Convert.ToSingle(colorList[2]);
            if(colorList.Length == 4)
            {
                color.a = Convert.ToSingle(colorList[3]);
            }
            else
            {
                color.a = 1f;
            }
            return color;
        }
    }
}

