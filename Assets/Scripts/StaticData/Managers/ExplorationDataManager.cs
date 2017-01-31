using System;
using System.Collections.Generic;
using System.Linq;

using Base;

namespace StaticData
{
	public class ExplorationDataManager : EntityManager
    {
		private static ExplorationDataManager instance;
		public static ExplorationDataManager Instance
        {
            get
            {
				if (instance == null) instance = new ExplorationDataManager();
                return instance;
            }
        }
        
		private Dictionary<int, ExplorationData> kvDic;
		private Dictionary<ExplorationType, List<ExplorationData>> typeDic;
        
		public void Init()
		{
			ExplorationDataParser parser = new ExplorationDataParser();
			parser.Parse("ExplorationDataConfig.csv", out kvDic, out typeDic);
		}

		public override EntityData GetData(int kid)
		{
			if (!kvDic.ContainsKey(kid))
			{
				BaseLogger.LogFormat("No such kid in Exploration kvDic: {0}", kid);
			}
			return kvDic[kid];
		}
        
		public ExplorationData GetRandomData()
        {
            int maxEnumValue = (int)ExplorationType.Common;
			int randomValue = RandomUtils.Range(1, maxEnumValue);
			ExplorationType type = (ExplorationType)randomValue;

			List<ExplorationData> dataList = typeDic[type];
			int index = RandomUtils.Range(0, dataList.Count);
			return dataList[index];
        }
        public ExplorationData GetRandomData(ExplorationType type)
        {
            if (type == ExplorationType.Common)
			{
				return GetRandomData();
			}
			List<ExplorationData> dataList = typeDic[type];
			int index = RandomUtils.Range(0, dataList.Count);
			return dataList[index];
        }
        public List<ExplorationData> GetDataList(ExplorationType type)
        {
            if(!typeDic.ContainsKey(type))
            {
                BaseLogger.LogFormat("No such type in Exploration typeDic: {0}", type);
            }
            return typeDic[type];
        }
    }
}

