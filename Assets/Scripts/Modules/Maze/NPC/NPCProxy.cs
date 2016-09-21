using UnityEngine;

using System.Collections;
using System.Collections.Generic;
using System.Linq;

using Base;

namespace GameLogic
{
	public class NPCProxy : Proxy
	{
		private Dictionary<string, NPC> npcDic;

		public NPCProxy ()
		{
			npcDic = new Dictionary<string, NPC>();
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
		
		public void Dispose()
		{
			List<NPC> list = npcDic.Values.ToList();
			int count = list.Count;
			for (int i = 0; i < count; ++i)
			{
				NPC.Recycle(list[i]);
			}
			npcDic.Clear();
		}
	}
}

