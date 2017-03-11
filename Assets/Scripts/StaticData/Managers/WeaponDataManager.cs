using System;
using System.Collections.Generic;

using Base;

namespace StaticData
{
    public class WeaponDataManager : EntityManager
    {
        private Dictionary<int, WeaponData> kvDic;

        private static WeaponDataManager instance;
        public static WeaponDataManager Instance
        {
            get
            {
                if (instance == null) instance = new WeaponDataManager();
                return instance;
            }
        }

        public void Init ()
        {
            WeaponDataParser parser = new WeaponDataParser();
            parser.Parse("WeaponDataConfig.csv", out kvDic);
        }

        public override EntityData GetData(int kid)
        {
            if (!kvDic.ContainsKey(kid))
            {
                BaseLogger.LogFormat("No such weapon kid: {0}", kid);
            }
            return kvDic[kid];
        }

    }
}

