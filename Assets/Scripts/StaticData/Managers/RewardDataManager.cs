using System;
using System.Collections.Generic;

using Base;

namespace StaticData
{
    public class RewardDataManager
    {
		private Dictionary<int, RewardData> kvDic;

		private static RewardDataManager instance;
		public static RewardDataManager Instance
		{
			get
			{
				if (instance == null) instance = new RewardDataManager();
				return instance;
			}
		}

		public void Init ()
		{
			RewardDataParser parser = new RewardDataParser();
			parser.Parse("RewardDataConfig.csv", out kvDic);
		}

		public RewardData GetData(int kid)
		{
			if (!kvDic.ContainsKey(kid))
			{
				BaseLogger.LogFormat("No such reward kid: {0}", kid);
			}
			return kvDic[kid];
		}

		public RewardData GetData(List<int> kidList)
		{
			List<int> weightList = new List<int>();
			for(int i = 0; i < kidList.Count; ++i)
			{
				int rewardKid = kidList[i];
				RewardData rd = RewardDataManager.Instance.GetData(rewardKid);
				weightList.Add(rd.Weight);
			}
			int index = RandomUtils.Weight(weightList);
			RewardData rewardData = GetData(kidList[index]);
			return rewardData;
		}
    }
}

