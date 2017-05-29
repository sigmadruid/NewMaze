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
        private Dictionary<string, NPC> npcDic = new Dictionary<string, NPC>();

        public void Init()
        {
        }
        public void Dispose()
        {
            foreach(NPC npc in npcDic.Values)
            {
                NPC.Recycle(npc);
            }
            npcDic.Clear();
        }

        public void IterateActives(Action<NPC> func)
        {
            if(func == null)
                return;
            var enumerator = npcDic.GetEnumerator();
            while(enumerator.MoveNext())
            {
                func(enumerator.Current.Value);
            }
        }
		public List<NPC> GetAllNPCs()
		{
			return npcDic.Values.ToList();
		}

		public void AddNPC(NPC npc)
		{
			if (!npcDic.ContainsKey(npc.Uid))
			{
				npcDic.Add(npc.Uid, npc);
			}
		}

		public void RemoveNPC(string uid)
		{
			if (npcDic.ContainsKey(uid))
			{
				npcDic.Remove(uid);
			}
		}
		
	}
}

