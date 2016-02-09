using UnityEngine;

using System;

using Base;
using StaticData;

namespace GameLogic
{
	public enum NPCState
	{
		Normal,
		Wait,
		End,
	}
    public class NPC : Entity
    {
		private const int PRE_CACHE_COUNT = 10;
		
		public NPCState State;

		public new NPCData Data
		{
			get { return data as NPCData; }
			protected set { data = value; }
		}
		
		public new NPCScript Script
		{
			get { return script as NPCScript; }
			protected set { script = value; }
		}

		public NPCEventData EventData { get; protected set; }

		private void OnNPCEnter()
		{
		}
		private void OnNPCExit()
		{
		}
		private void OnNPCClick()
		{
			ApplicationFacade.Instance.DispatchNotification(NotificationEnum.NPC_DIALOG_SHOW, this);
		}

		public static NPC Create(int npcKid, int eventKid, NPCState state = NPCState.Normal)
		{
			ResourceManager resManager = ResourceManager.Instance;
			
			NPC npc = null;
			if (!resManager.ContainsObjectKey(ObjectKey.NPC))
			{
				for (int i = 0; i < PRE_CACHE_COUNT; ++i)
				{
					npc = new NPC();
					resManager.AddObject(ObjectKey.NPC, npc);
				}
			}
			npc = new NPC();
			npc.Uid = Guid.NewGuid().ToString();
			npc.Data = NPCDataManager.Instance.GetData(npcKid) as NPCData;
			npc.EventData = NPCDataManager.Instance.GetEventDataByID(eventKid);
			npc.Script = ResourceManager.Instance.LoadAsset<NPCScript>(ObjectType.GameObject, npc.Data.GetResPath());
			npc.Script.transform.parent = RootTransform.Instance.NPCRoot;
			npc.Script.CallbackClick = npc.OnNPCClick;
			npc.State = state;
			return npc;
		}

		public static void Recycle(NPC npc)
		{
			if (npc != null)
			{
				npc.Data = null;
				npc.EventData = null;
				npc.Script.StopAllCoroutines();
				ResourceManager.Instance.RecycleAsset(npc.Script.gameObject);
				npc.Script = null;
				
			}
			else
			{
				BaseLogger.Log("Recyle a null npc!");
			}
		}
    }
}

