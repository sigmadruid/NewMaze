using System;
using System.Collections.Generic;

using Base;

namespace StaticData
{
    public class BuffDataManager
    {
        private Dictionary<int, BuffData> kvDic;

        private static BuffDataManager instance;
        public static BuffDataManager Instance
        {
            get
            {
                if (instance == null) instance = new BuffDataManager();
                return instance;
            }
        }

        public void Init ()
        {
            BuffDataParser parser = new BuffDataParser();
            parser.Parse("BuffDataConfig.csv", out kvDic);
        }

        public BuffData GetData(int kid)
        {
            if (!kvDic.ContainsKey(kid))
            {
                BaseLogger.LogFormat("No such buff kid: {0}", kid);
            }
            return kvDic[kid];
        }
    }
}

