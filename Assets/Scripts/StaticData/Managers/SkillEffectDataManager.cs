using System;
using System.Collections.Generic;

using Base;

namespace StaticData
{
    public class SkillEffectDataManager
    {
        private Dictionary<int, SkillEffectData> kvDic;

        private static SkillEffectDataManager instance;
        public static SkillEffectDataManager Instance
        {
            get
            {
                if (instance == null) instance = new SkillEffectDataManager();
                return instance;
            }
        }

        public void Init ()
        {
            SkillEffectDataParser parser = new SkillEffectDataParser();
            parser.Parse("SkillEffectDataConfig.csv", out kvDic);
        }

        public SkillEffectData GetData(int kid)
        {
            if (!kvDic.ContainsKey(kid))
            {
                BaseLogger.LogFormat("No such skill effect kid: {0}", kid);
            }
            return kvDic[kid];
        }

    }
}

