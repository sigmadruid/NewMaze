using System;
using System.Collections.Generic;

using Base;

namespace StaticData
{
    public class HeroLevelUpDataManager
    {
        private Dictionary<int, HeroLevelUpData> kvDic;

        private static HeroLevelUpDataManager instance;
        public static HeroLevelUpDataManager Instance
        {
            get
            {
                if (instance == null) instance = new HeroLevelUpDataManager();
                return instance;
            }
        }

        public void Init ()
        {
            HeroLevelUpDataParser parser = new HeroLevelUpDataParser();
            parser.Parse("HeroLevelUpDataConfig.csv", out kvDic);
        }

        public int GetExp(int level)
        {
            if (!kvDic.ContainsKey(level))
            {
                BaseLogger.LogFormat("No such hero level up kid: {0}", level);
            }
            return kvDic[level].Exp;
        }
    }
}

