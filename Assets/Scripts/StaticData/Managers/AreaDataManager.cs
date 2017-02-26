using System;
using System.Collections.Generic;

using Base;

namespace StaticData
{
    public class AreaDataManager
    {
        private Dictionary<int, AreaData> kvDic;

        private static AreaDataManager instance;
        public static AreaDataManager Instance
        {
            get
            {
                if (instance == null) instance = new AreaDataManager();
                return instance;
            }
        }

        public void Init ()
        {
            AreaDataParser parser = new AreaDataParser();
            parser.Parse("AreaDataConfig.csv", out kvDic);
        }

        public AreaData GetData(int kid)
        {
            if (!kvDic.ContainsKey(kid))
            {
                BaseLogger.LogFormat("No such area kid: {0}", kid);
            }
            return kvDic[kid];
        }

    }
}

