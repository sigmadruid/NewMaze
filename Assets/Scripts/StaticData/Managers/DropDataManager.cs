using System;
using System.Collections.Generic;

using Base;

namespace StaticData
{
    public class DropDataManager
    {
		private Dictionary<int, DropData> kvDic;

		private static DropDataManager instance;
		public static DropDataManager Instance
		{
			get
			{
				if (instance == null) instance = new DropDataManager();
				return instance;
			}
		}

		public void Init ()
		{
			DropDataParser parser = new DropDataParser();
			parser.Parse("DropDataConfig.csv", out kvDic);
		}

		public DropData GetData(int kid)
		{
			if (!kvDic.ContainsKey(kid))
			{
				BaseLogger.LogFormat("No such drop kid: {0}", kid);
			}
			return kvDic[kid];
		}

    }
}

