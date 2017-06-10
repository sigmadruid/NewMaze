using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using Base;

namespace GameLogic
{
	public class NPCProxy : Proxy
	{
        private Dictionary<string, NPC> npcBlockDic = new Dictionary<string, NPC>();
        private Dictionary<string, NPC> npcHallDic = new Dictionary<string, NPC>();

        public Dictionary<int, List<NPCRecord>> RecordDic = new Dictionary<int, List<NPCRecord>>();

        public void Init()
        {
        }
        public void Dispose()
        {
            Dictionary<string, NPC> npcDic = GetCurrentDic();
            foreach(NPC npc in npcDic.Values)
            {
                NPC.Recycle(npc);
            }
            npcDic.Clear();
        }
        public void SaveRecord()
        {
            int hallKid = Hall.IsActive ? Hall.Instance.Data.Kid : 0;
            Dictionary<string, NPC> npcDic = GetCurrentDic();

            List<NPCRecord> recordList = null;
            if(RecordDic.ContainsKey(hallKid))
            {
                recordList = RecordDic[hallKid];
                recordList.Clear();
            }
            else
            {
                recordList = new List<NPCRecord>();
                RecordDic[hallKid] = recordList;
            }
            foreach(NPC npc in npcDic.Values)
            {
                recordList.Add(npc.ToRecord());
            }
        }

        public void Foreach(Action<NPC> action)
        {
            if(action == null) return;

            Dictionary<string, NPC> npcDic = GetCurrentDic();
            var enumerator = npcDic.GetEnumerator();
            while(enumerator.MoveNext())
            {
                action(enumerator.Current.Value);
            }
        }

		public void AddNPC(NPC npc)
		{
            Dictionary<string, NPC> npcDic = GetCurrentDic();
			if (!npcDic.ContainsKey(npc.Uid))
			{
				npcDic.Add(npc.Uid, npc);
			}
		}

		public void RemoveNPC(string uid)
		{
            Dictionary<string, NPC> npcDic = GetCurrentDic();
			if (npcDic.ContainsKey(uid))
			{
				npcDic.Remove(uid);
			}
		}
		
        private Dictionary<string, NPC> GetCurrentDic()
        {
            return Hall.IsActive ? npcHallDic : npcBlockDic;
        }
	}
}

