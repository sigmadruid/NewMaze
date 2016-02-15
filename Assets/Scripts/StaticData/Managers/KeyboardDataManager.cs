using UnityEngine;

using System;
using System.Collections.Generic;

using Base;

namespace StaticData
{
    public class KeyboardDataManager
    {
        private Dictionary<int, KeyboardData> kvDic;

        private static KeyboardDataManager instance;
        public static KeyboardDataManager Instance
        {
            get
            {
                if (instance == null) instance = new KeyboardDataManager();
                return instance;
            }
        }

        public void Init ()
        {
            KeyboardDataParser parser = new KeyboardDataParser();
            parser.Parse("KeyboardDataConfig.csv", out kvDic);
        }

        public KeyboardData GetData(KeyboardActionType type)
        {
            int typeID = (int)type;
            Debug.Log(typeID);
            if (!kvDic.ContainsKey(typeID))
            {
                BaseLogger.LogFormat("No such keyboard type: {0}", type);
            }
            return kvDic[typeID];
        }
    }
}

