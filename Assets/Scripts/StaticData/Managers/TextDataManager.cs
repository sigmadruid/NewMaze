using System;
using System.Collections.Generic;

using Base;

namespace StaticData
{
    public class TextDataManager
    {
        private Dictionary<string, string> kvDic;

		private static TextDataManager instance;
		public static TextDataManager Instance
		{
			get
			{
				if (instance == null) instance = new TextDataManager();
				return instance;
			}
		}

		public void Init()
		{
			TextDataParser parser = new TextDataParser();
			parser.Parse("TextDataConfig.csv", out kvDic);
		}

        public string GetData(string id)
		{
			if (!kvDic.ContainsKey(id))
			{
				BaseLogger.LogFormat("No such id in TextDataManager: {0}", id);
                return id;
			}
			return kvDic[id];
		}
    }
}

