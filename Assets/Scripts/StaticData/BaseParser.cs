using UnityEngine;

using System;
using System.Collections.Generic;
using System.IO;

using Base;

namespace StaticData
{
    public enum DataFileType
    {
        Config,
        Plot,
    }
    public class BaseParser
    {
        public static bool CHECK_TYPE = true;
        public static string CONFIG_PATH = Application.streamingAssetsPath + "/Configs/";
		public static string PLOT_PATH = Application.streamingAssetsPath + "/Plots/";

		private int rowIndex;
        private List<string[]> dataStrList = new List<string[]>();

        protected void LoadFile(string path)
		{
			if (File.Exists(path))
			{
                StreamReader sr = new StreamReader(path, System.Text.UTF8Encoding.UTF8);
                bool isNameLine = true;
                bool isTypeLine = true;
				while (!sr.EndOfStream)
				{
					string line = sr.ReadLine();
					if (isNameLine)
					{
						isNameLine = false;
						continue;
					}
                    if (isTypeLine)
                    {
                        isTypeLine = false;
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
        private int ParseInt(string str, int col)
        {
            if(CHECK_TYPE)
            {
                int value = 0;
                bool result = int.TryParse(str, out value);
                if (!result)
                    BaseLogger.LogFormat("WRONG FORMAT IN CONFIG!! str={0},row={1},col={2},file={3}", str, rowIndex, col, this.ToString());
                return value;
            }
            return Convert.ToInt32(str);
        }
		protected int ReadInt(int col)
		{
			string[] strArray = dataStrList[rowIndex];
			string str = strArray[col];
            return ParseInt(str, col);
		}
		protected string ReadString(int col)
		{
			string[] strArray = dataStrList[rowIndex];
			string str = strArray[col];
			return str;
		}
        private float ParseFloat(string str, int col)
        {
            if(CHECK_TYPE)
            {
                float value = 0;
                bool result = float.TryParse(str, out value);
                if (!result)
                    BaseLogger.LogFormat("WRONG FORMAT IN CONFIG!! str={0},row={1},col={2},file={3}", str, rowIndex, col, this.ToString());
                return value;
            }
            return Convert.ToSingle(str);
        }
		protected float ReadFloat(int col)
		{
			string[] strArray = dataStrList[rowIndex];
			string str = strArray[col];
            return ParseFloat(str, col);
		}
		protected T ReadEnum<T>(int col)
		{
            string str = ReadString(col);
			return ParseKey<T>(str, col);
		}
        private Vector3 ParseVector3(string str, int col)
        {
            string[] strList = str.Split('#');
            Vector3 vec = Vector3.zero;
            if(CHECK_TYPE)
            {
                bool result = strList != null && strList.Length == 3;
                if (!result)
                    BaseLogger.LogFormat("WRONG FORMAT IN CONFIG!! str={0},row={1},col={2},file={3}", str, rowIndex, col, this.ToString());
            }
            vec.x = ParseFloat(strList[0], col);
            vec.y = ParseFloat(strList[1], col);
            vec.z = ParseFloat(strList[2], col);
            return vec;
        }
        protected Vector3 ReadVector3(int col)
        {
            string[] strArray = dataStrList[rowIndex];
            string str = strArray[col];
            return ParseVector3(str, col);
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
                    list.Add(ParseInt(str, col));
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
                    list.Add(ParseKey<T>(str, col));
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
                K k = ParseKey<K>(pairList[0].Trim(), col);
                V v = ParseValue<V>(pairList[1].Trim(), col);
                dic.Add(k, v);
            }
            return dic;
        }


        private T ParseKey<T>(string str, int col)
		{
            object resultKey = null;
            if (typeof(T) == typeof(int))
            {
                resultKey = ParseInt(str, col);
            }
            else
            {
                if(!Enum.IsDefined(typeof(T), str))
                {
                    BaseLogger.LogFormat("WRONG FORMAT IN CONFIG!! str={0},row={1},col={2},file={3}", str, rowIndex, col, this.ToString());
                }
                resultKey = Enum.Parse(typeof(T), str);
            }
            return (T)resultKey;
		}
        private T ParseValue<T>(string str, int col)
		{
			object resultVal = null;

            if(typeof(T) == typeof(int))
            {
                resultVal = ParseInt(str, col);
            }
            else if(typeof(T) == typeof(float))
            {
                resultVal = ParseFloat(str, col);
            }
            else
            {
                resultVal = str;
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
            color.r = ParseFloat(colorList[0], col);
            color.g = ParseFloat(colorList[1], col);
            color.b = ParseFloat(colorList[2], col);
            if(colorList.Length == 4)
            {
                color.a = ParseFloat(colorList[3], col);
            }
            else
            {
                color.a = 1f;
            }
            return color;
        }
    }
}

