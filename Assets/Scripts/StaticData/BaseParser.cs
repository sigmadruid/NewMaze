using UnityEngine;

using System;
using System.Collections.Generic;
using System.IO;

using Base;

namespace StaticData
{
    public class BaseParser
    {
		public const int BUFFER_MAX_SIZE = 1024 * 1024;

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
					if (firstChar == ',' || char.IsLetter(firstChar))
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
			return ParseEnum<T>(value);
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
					T enumVal = ParseEnum<T>(str);
					list.Add(enumVal);
				}
			}
			return list;
		}
		protected Dictionary<K, V> ReadDictionary<K, V>(int keyCol, int valCol)
		{
			string keyStr = ReadString(keyCol);
			string valStr = ReadString(valCol);

			if (string.IsNullOrEmpty(keyStr) || string.IsNullOrEmpty(valStr))
			{
				return null;
			}

			string[] keyStrList = keyStr.Split('#');
			string[] valStrList = valStr.Split('#');

			Dictionary<K,V> dic = new Dictionary<K, V>();
			for (int i = 0; i < keyStrList.Length; ++i)
			{
				string key = keyStrList[i];
				string val = valStrList[i];
				K k = ParseEnum<K>(key);
				V v = ParseValue<V>(val);
				dic.Add(k, v);
			}
			return dic;
		}

		private T ParseEnum<T>(string str)
		{
//            T enumKey = (T)Enum.Parse(typeof(T), str);
            object resultKey = null;
            if (typeof(T) == typeof(int))
            {
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

			if (typeof(T) == typeof(int))
			{
				resultVal = Convert.ToInt32(str);
			}
			return (T)resultVal;
		}
    }
}

