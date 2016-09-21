using System;
using System.Collections.Generic;

using Base;

namespace StaticData
{
    public class TrapDataManager : EntityManager
    {
        private Dictionary<int, TrapData> kvDic;

        private static TrapDataManager instance;
        public static TrapDataManager Instance
        {
            get
            {
                if (instance == null) instance = new TrapDataManager();
                return instance;
            }
        }

        public void Init ()
        {
            TrapDataParser parser = new TrapDataParser();
            parser.Parse("TrapDataConfig.csv", out kvDic);
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

