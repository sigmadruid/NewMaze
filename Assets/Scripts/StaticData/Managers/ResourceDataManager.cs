using System;
using System.Collections.Generic;

using Base;

namespace StaticData
{
    public class ResourceDataManager
    {
		private Dictionary<int, List<ResourceData>> typeDic;
		
		private static ResourceDataManager instance;
		public static ResourceDataManager Instance
		{
			get
			{
				if (instance == null) instance = new ResourceDataManager();
				return instance;
			}
		}

		public void Init()
		{
			ResourceDataParser parser = new ResourceDataParser();
			parser.Parse("ResourceDataConfig.csv", out typeDic);
		}
		public List<ResourceData> GetResourceDataList(int mazeKid)
		{
			if (!typeDic.ContainsKey(mazeKid))
			{
				BaseLogger.LogFormat("No such maze kid in resource data dic: {0}", mazeKid);
			}
			return typeDic[mazeKid];
		}

    }
}

