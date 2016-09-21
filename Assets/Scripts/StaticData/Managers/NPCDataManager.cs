using System;
using System.Collections.Generic;
using System.Linq;

using Base;

namespace StaticData
{
	public class NPCDataManager : EntityManager
    {
		private Dictionary<int, NPCData> npcKVDic;
		private Dictionary<int, NPCEventData> eventKVDic;
		private Dictionary<NPCAppearScene, List<NPCData>> npcTypeDic;
		private Dictionary<NPCAppearScene, List<NPCEventData>> eventTypeDic;
		

		private static NPCDataManager instance;
		public static NPCDataManager Instance
		{
			get
			{
				if (instance == null) instance = new NPCDataManager();
                return instance;
            }
        }

		public void Init()
		{
			NPCDataParser npcParser = new NPCDataParser();
			npcParser.Parse("NPCDataConfig.csv", out npcTypeDic, out npcKVDic);
			NPCEventDataParser eventParser = new NPCEventDataParser();
			eventParser.Parse("NPCEventDataConfig.csv", out eventTypeDic, out eventKVDic);
		}

		public override EntityData GetData(int kid)
		{
			if (!npcKVDic.ContainsKey(kid))
			{
				BaseLogger.LogFormat("No such npc kid: {0}", kid);
				return null;
			}
			return npcKVDic[kid];
        }
		public NPCEventData GetEventDataByID(int kid)
		{
			if (!eventKVDic.ContainsKey(kid))
			{
				BaseLogger.LogFormat("No such npc event kid: {0}" + kid);
				return null;
			}
			return eventKVDic[kid];
		}

		public NPCData GetRandomNPCData(NPCAppearScene appearScene)
		{
			List<NPCData> dataList = npcTypeDic[appearScene];
			return dataList[RandomUtils.Range(0, dataList.Count)];
		}
		public NPCEventData GetRandomEventData(NPCAppearScene appearScene)
		{
			List<NPCEventData> dataList = eventTypeDic[appearScene];
			return dataList[RandomUtils.Range(0, dataList.Count)];
		}

    }
}

