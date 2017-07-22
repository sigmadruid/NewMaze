using UnityEngine;

using System;
using System.Collections.Generic;

using Base;

namespace StaticData
{
    public class UIDataManager
    {
        private Dictionary<string, UIData> kvDic;

        private static UIDataManager instance;
        public static UIDataManager Instance
        {
            get
            {
                if (instance == null) instance = new UIDataManager();
                return instance;
            }
        }

        public void Init ()
        {
            UIDataParser parser = new UIDataParser();
            parser.Parse("UIDataConfig.csv", out kvDic);
        }

        public UIData GetData(string name)
        {
            if (!kvDic.ContainsKey(name))
            {
                BaseLogger.LogFormat("No such ui data: {0}", name);
            }
            return kvDic[name];
        }
    }
}

