using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Base
{
	public enum DirectionEnum
	{
		Up,
		Right,
		Down,
		Left,
	}

	public class Utils
	{
		public delegate void CallbackVoid();
		public delegate void CallbackBoolean(bool state);
		public delegate void CallbackVector3(Vector3 vec3);
		public delegate void CallbackParam(object obj);

		private static Shader rimShader;
		public static Shader RimShader
		{
			get
			{
				if (rimShader == null)
				{
					rimShader = Shader.Find("Custom/RimShader");
				}
				return rimShader;
			}
		}
		private static Shader diffuseShader;
		public static Shader DiffuseShader
		{
			get
			{
				if (diffuseShader == null)
				{
					diffuseShader = Shader.Find("Mobile/Diffuse");
				}
				return diffuseShader;
			}
		}
		private static Shader transparentShader;
		public static Shader TransparentShader
		{
			get
			{
				if (transparentShader == null)
				{
					transparentShader = Shader.Find("Transparent/Diffuse");
				}
				return transparentShader;
			}
		}

        public static void Shift<T>(IList<T> list, int start = 0, int count = 0)
		{
            if (count == 0)
                count = list.Count;
            
			for (int i = start; i < count; ++i)
			{
                int index = RandomUtils.Range(start, count);
                T node = list[i];
				list[i] = list[index];
                list[index] = node;
			}
		}

        public static List<int> GetRandomIndexList(int count, int length)
        {
            const int MAX_CONFLICT_TIMES = 10;
            int conflictTimes = 0;
            HashSet<int> resultSet = new HashSet<int>();
            for (int i = 0; i < count; ++i)
            {
                int index = RandomUtils.Range(0, length);
                if (!resultSet.Contains(index))
                {
                    resultSet.Add(index);
                }
                else
                {
                    i--;
                    conflictTimes++;
                    if (conflictTimes >= MAX_CONFLICT_TIMES)
                    {
                        BaseLogger.LogFormat("Too many conflicts when get random index list. Length={0}, Count={1}", length, count);
                        break;
                    }
                }
            }
            return resultSet.ToList();
        }

		public static string CombineString(params object[] objlist)
		{
			StringBuilder sb = new StringBuilder();
			for (int i = 0; i < objlist.Length; ++i)
			{
				sb.Append(objlist[i].ToString());
			}
			return sb.ToString();
		}
		public static string CombinePath(params object[] objlist)
		{
			StringBuilder sb = new StringBuilder();
			for (int i = 0; i < objlist.Length; ++i)
			{
				sb.Append(objlist[i].ToString());
				if (i != objlist.Length - 1)
				{
					sb.Append("/");
				}
			}
			return sb.ToString();
		}

		private static long t;
		public static void GetStartTime()
		{
			t = DateTime.Now.Ticks;
		}
		public static void GetEndTime()
		{
			DateTime dt = new DateTime(DateTime.Now.Ticks - t);
			Debug.Log(dt.Millisecond.ToString() + "ms");
		}
	}
}

