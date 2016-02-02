using System;
using System.Collections.Generic;

using Base;

namespace StaticData
{
    public class HallDataManager : EntityManager
    {
		private Dictionary<int, HallData> kvDic;

        private static HallDataManager instance;
        public static HallDataManager Instance
        {
            get
            {
                if (instance == null) instance = new HallDataManager();
                return instance;
            }
        }

		public void Init ()
		{
			HallDataParser parser = new HallDataParser();
			parser.Parse("HallDataConfig.csv", out kvDic);
		}
		
		public override EntityData GetData(int kid)
		{
			if (!kvDic.ContainsKey(kid))
			{
				BaseLogger.LogFormat("No such hall kid: {0}", kid);
			}
			return kvDic[kid];
		}
    }
}

