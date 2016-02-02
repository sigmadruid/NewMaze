using System;
using System.Collections.Generic;

using Base;

namespace StaticData
{
	public class CurrencyDataManager : EntityManager
    {
		private Dictionary<int, CurrencyData> kvDic;

		private static CurrencyDataManager instance;
		public static CurrencyDataManager Instance
		{
			get
			{
				if (instance == null) instance = new CurrencyDataManager();
				return instance;
			}
		}

		public void Init ()
		{
			CurrencyDataParser parser = new CurrencyDataParser();
			parser.Parse("CurrencyDataConfig.csv", out kvDic);
		}
		
		public override EntityData GetData(int kid)
		{
			if (!kvDic.ContainsKey(kid))
			{
				BaseLogger.LogFormat("No such currency kid: {0}", kid);
			}
			return kvDic[kid];
		}
    }
}

