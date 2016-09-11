using System;
using System;
using System.Collections.Generic;
using System.Linq;

using Base;

namespace StaticData
{
    public class ItemDataManager : EntityManager
    {
        private Dictionary<int, ItemData> kvDic;

        private static ItemDataManager instance;
        public static ItemDataManager Instance
        {
            get
            {
                if (instance == null) instance = new ItemDataManager();
                return instance;
            }
        }

        public void Init ()
        {
            ItemDataParser parser = new ItemDataParser();
            parser.Parse("ItemDataConfig.csv", out kvDic);
        }

        public override EntityData GetData(int kid)
        {
            if (!kvDic.ContainsKey(kid))
            {
                BaseLogger.LogFormat("No such item kid: {0}", kid);
                return null;
            }
            return kvDic[kid];
        }
    }
}

