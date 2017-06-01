using System;
using System.Text;

namespace Base
{
    public static class BaseLogger
    {
		public static void Log(string str)
		{
			UnityEngine.Debug.Log(str);
		}
        public static void LogWarning(string str)
        {
            UnityEngine.Debug.LogWarning(str);
        }
        public static void LogError(string str)
        {
            UnityEngine.Debug.LogError(str);
        }

		public static void LogFormat(string str, params object[] objList)
		{
			str = string.Format(str, objList);
			UnityEngine.Debug.Log(str);
		}

		public static void LogList(params object[] objList)
		{
			StringBuilder sb = new StringBuilder();
			for (int i = 0; i < objList.Length; ++i)
			{
				object obj = objList[i];
				if (obj != null )
				{
					sb.Append(obj.ToString());
				}
				else
				{
					sb.Append("Null");
				}
				sb.Append(", ");
			}
			UnityEngine.Debug.Log(sb.ToString());
		}
    }
}

