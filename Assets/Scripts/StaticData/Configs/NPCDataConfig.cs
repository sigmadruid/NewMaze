using System;
using System.Collections.Generic;

namespace StaticData
{
    public class NPCDataConfig
    {
        public Dictionary<int, NPCData> NPCDataDic;
		public Dictionary<int, NPCEventData> NPCEventDataDic;

        public NPCDataConfig()
        {
			InitNPCData();
			InitNPCEventData();
        }

		private void InitNPCData()
		{
			IDManager idManager = IDManager.Instance;
			NPCDataDic = new Dictionary<int, NPCData>();
			NPCData data;
			
			data = new NPCData();
			data.Kid = idManager.GetID(IDType.NPC, 1);
			data.Name = 10001;
			data.Res3D = "NPC_1";
			data.AppearScene = NPCAppearScene.HomeTown;
			NPCDataDic.Add(data.Kid, data);

			data = new NPCData();
			data.Kid = idManager.GetID(IDType.NPC, 2);
			data.Name = 10004;
			data.Res3D = "NPC_1";
			data.AppearScene = NPCAppearScene.Maze;
			NPCDataDic.Add(data.Kid, data);

			data = new NPCData();
			data.Kid = idManager.GetID(IDType.NPC, 3);
			data.Name = 10006;
			data.Res3D = "NPC_2";
			data.AppearScene = NPCAppearScene.Maze;
			NPCDataDic.Add(data.Kid, data);

			data = new NPCData();
			data.Kid = idManager.GetID(IDType.NPC, 4);
			data.Name = 10008;
			data.Res3D = "NPC_2";
			data.AppearScene = NPCAppearScene.Maze;
			NPCDataDic.Add(data.Kid, data);
		}

		private void InitNPCEventData()
		{
			IDManager idManager = IDManager.Instance;
			NPCEventDataDic = new Dictionary<int, NPCEventData>();
			NPCEventData data;

			data = new NPCEventData();
			data.Kid = idManager.GetID(IDType.NPCEvent, 1);
			data.AppearScene = NPCAppearScene.HomeTown;
			data.Type = NPCEventType.Normal;
			data.FirstTalkList = new List<int>();
			data.FirstTalkList.Add(10002);
			data.FirstTalkList.Add(10003);
			NPCEventDataDic.Add(data.Kid, data);

			data = new NPCEventData();
			data.Kid = idManager.GetID(IDType.NPCEvent, 2);
			data.AppearScene = NPCAppearScene.Maze;
			data.Type = NPCEventType.Normal;
			data.FirstTalkList = new List<int>();
			data.FirstTalkList.Add(10005);
			NPCEventDataDic.Add(data.Kid, data);

			data = new NPCEventData();
			data.Kid = idManager.GetID(IDType.NPCEvent, 3);
			data.AppearScene = NPCAppearScene.Maze;
			data.Type = NPCEventType.Normal;
			data.FirstTalkList = new List<int>();
			data.FirstTalkList.Add(10007);
			NPCEventDataDic.Add(data.Kid, data);

			data = new NPCEventData();
			data.Kid = idManager.GetID(IDType.NPCEvent, 4);
			data.AppearScene = NPCAppearScene.Maze;
			data.Type = NPCEventType.Normal;
			data.FirstTalkList = new List<int>();
			data.FirstTalkList.Add(10009);
			data.FirstTalkList.Add(10010);
			NPCEventDataDic.Add(data.Kid, data);
		}
    }
}

