using UnityEngine;

using System;

using Base;
using StaticData;

namespace GameLogic
{
    public class NPC : Entity
    {
		private const int PRE_CACHE_COUNT = 10;
		
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
        public void OnFunction()
		{
			ApplicationFacade.Instance.DispatchNotification(NotificationEnum.NPC_DIALOG_SHOW, this);
		}

        public NPCRecord ToRecord()
        {
            NPCRecord record = new NPCRecord();
            record.Uid = Uid;
            record.Kid = Data.Kid;
            record.WorldPosition = new Vector3Record(WorldPosition);
            record.WorldAngle = WorldAngle;
            record.EventKid = EventData.Kid;
            return record;
        }

		public static NPC Create(int npcKid, int eventKid)
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
            npc.Script.Init(npc.Uid, npc.OnFunction, npc.OnNPCEnter, npc.OnNPCExit);
			return npc;
		}

        public static NPC Create(NPCRecord record)
        {
            return Create(record.Kid, record.EventKid);
        }

		public static void Recycle(NPC npc)
		{
			if (npc != null)
			{
                npc.Script.Dispose();
                ResourceManager.Instance.RecycleAsset(npc.Script.gameObject);
                npc.Script = null;
                npc.EventData = null;
                npc.Data = null;
                npc.Uid = null;
				
			}
			else
			{
				BaseLogger.Log("Recyle a null npc!");
			}
		}
    }
}

