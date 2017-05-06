using System;
using System.Collections.Generic;
using System.Linq;

using Base;

namespace StaticData
{
	public class MonsterDataManager : EntityManager
	{
		private Dictionary<int, MonsterData> kvDic;
		private List<MonsterData> dataList;

		private int weightSum;
		private List<int> weightList;
		
		private static MonsterDataManager instance;
		public static MonsterDataManager Instance
		{
			get
			{
				if (instance == null) instance = new MonsterDataManager();
				return instance;
			}
		}
		
		public void Init ()
		{
			MonsterDataParser parser = new MonsterDataParser();
			parser.Parse("MonsterDataConfig.csv", out kvDic, out dataList);
		}

		public override EntityData GetData(int kid)
		{
			if (!kvDic.ContainsKey(kid))
			{
				BaseLogger.LogFormat("No such monster kid: {0}", kid);
				return null;
			}
			return kvDic[kid];
		}
		public List<MonsterData> GetAllMonsterData()
		{
			return dataList;
		}
		public MonsterData GetRandomMonsterData()
		{
			if (weightList == null)
			{
				weightList = new List<int>();
				for (int i = 0; i < dataList.Count; ++i)
				{
					int weight = dataList[i].AppearWeight;
					weightList.Add(weight);
				}
			}
			int index = RandomUtils.Weight(weightList);
			return dataList[index];
		}
		
	}
}

