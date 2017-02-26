using System;
using System.Collections.Generic;

using Base;

namespace StaticData
{
    public class SkillDataManager
    {
        private Dictionary<int, SkillData> kvDic;

        private static SkillDataManager instance;
        public static SkillDataManager Instance
        {
            get
            {
                if (instance == null) instance = new SkillDataManager();
                return instance;
            }
        }

        public void Init ()
        {
            SkillDataParser parser = new SkillDataParser();
            parser.Parse("SkillDataConfig.csv", out kvDic);
        }

        public SkillData GetData(int kid)
        {
            if (!kvDic.ContainsKey(kid))
            {
                BaseLogger.LogFormat("No such skill kid: {0}", kid);
            }
            return kvDic[kid];
        }

    }
}

