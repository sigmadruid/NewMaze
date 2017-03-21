using UnityEngine;

using System;
using System.Collections.Generic;

using Base;

namespace StaticData
{
    public class StaticDataException : Exception
    {
    }
    public static class StaticReader
    {
        public static bool ReadBool(string content)
        {
            return content.ToLower() == "true" || content == "1";
        }
        private static int ParseInt(string content)
        {
            if(GlobalConfig.StaticDataConfig.CheckType)
            {
                int value = 0;
                bool result = int.TryParse(content, out value);
                if(!result)
                    throw new Exception();
                return value;
            }
            return Convert.ToInt32(content);
        }
        public static int ReadInt(string content)
        {
            return ParseInt(content);
        }
        public static string ReadString(string content)
        {
            return content;
        }
        private static float ParseFloat(string content)
        {
            if(GlobalConfig.StaticDataConfig.CheckType)
            {
                float value = 0;
                bool result = float.TryParse(content, out value);
                if (!result)
                    throw new Exception();
                return value;
            }
            return Convert.ToSingle(content);
        }
        public static float ReadFloat(string content)
        {
            return ParseFloat(content);
        }
        public static T ReadEnum<T>(string content)
        {
            return ParseKey<T>(content);
        }
        private static Vector3 ParseVector3(string content)
        {
            string[] strList = content.Split('#');
            Vector3 vec = Vector3.zero;
            if(GlobalConfig.StaticDataConfig.CheckType)
            {
                bool result = strList != null && strList.Length == 3;
                if (!result)
                    throw new Exception();
            }
            vec.x = ParseFloat(strList[0]);
            vec.y = ParseFloat(strList[1]);
            vec.z = ParseFloat(strList[2]);
            return vec;
        }
        public static Vector3 ReadVector3(string content)
        {
            return ParseVector3(content);
        }
        public static List<string> ReadStringList(string content)
        {
            List<string> list = new List<string>();
            if (!string.IsNullOrEmpty(content))
            {
                string[] strList = content.Split('#');
                for (int i = 0; i < strList.Length; ++i)
                {
                    string str = strList[i];
                    list.Add(str);
                }
            }
            return list;
        }
        public static List<int> ReadIntList(string content)
        {
            List<int> list = new List<int>();
            if (!string.IsNullOrEmpty(content))
            {
                string[] strList = content.Split('#');
                for (int i = 0; i < strList.Length; ++i)
                {
                    string str = strList[i];
                    list.Add(ParseInt(str));
                }
            }
            return list;
        }
        public static List<T> ReadEnumList<T>(string content)
        {
            List<T> list = new List<T>();
            if (!string.IsNullOrEmpty(content))
            {
                string[] strList = content.Split('#');
                for (int i = 0; i < strList.Length; ++i)
                {
                    string str = strList[i];
                    list.Add(ParseKey<T>(str));
                }
            }
            return list;
        }
        public static Dictionary<K, V> ReadDictionary<K, V>(string content)
        {
            if (string.IsNullOrEmpty(content))
            {
                return new Dictionary<K, V>();
            }

            string[] valList = content.Split('#');
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


        private static T ParseKey<T>(string content)
        {
            object resultKey = null;
            if (typeof(T) == typeof(int))
            {
                resultKey = ParseInt(content);
            }
            else
            {
                if(!Enum.IsDefined(typeof(T), content))
                {
                    throw new Exception();
                }
                resultKey = Enum.Parse(typeof(T), content);
            }
            return (T)resultKey;
        }
        private static T ParseValue<T>(string content)
        {
            object resultVal = null;

            if(typeof(T) == typeof(int))
            {
                resultVal = ParseInt(content);
            }
            else if(typeof(T) == typeof(float))
            {
                resultVal = ParseFloat(content);
            }
            else
            {
                resultVal = content;
            }

            return (T)resultVal;
        }

        public static Color ReadColor(string content)
        {
            if (string.IsNullOrEmpty(content))
            {
                return Color.white;
            }

            string[] colorList = content.Split('#');
            Color color = new Color();
            color.r = ParseFloat(colorList[0]);
            color.g = ParseFloat(colorList[1]);
            color.b = ParseFloat(colorList[2]);
            if(colorList.Length == 4)
            {
                color.a = ParseFloat(colorList[3]);
            }
            else
            {
                color.a = 1f;
            }
            return color;
        }
    }
}

