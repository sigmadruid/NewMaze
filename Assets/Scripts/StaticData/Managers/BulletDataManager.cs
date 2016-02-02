using System;
using System.Collections.Generic;

using Base;

namespace StaticData
{
	public class BulletDataManager : EntityManager
	{
		private Dictionary<int, BulletData> kvDic;

		private static BulletDataManager instance;
		public static BulletDataManager Instance
		{
			get
			{
				if (instance == null) instance = new BulletDataManager();
				return instance;
			}
		}

		public void Init ()
		{
			BulletDataParser parser = new BulletDataParser();
			parser.Parse("BulletDataConfig.csv", out kvDic);
		}

		public override EntityData GetData(int kid)
		{
			if (!kvDic.ContainsKey(kid))
			{
				BaseLogger.LogFormat("No such bullet kid: {0}", kid);
			}
			return kvDic[kid];
		}
	}
}

