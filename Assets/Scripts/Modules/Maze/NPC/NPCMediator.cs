using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;

using Base;
using GameUI;
using StaticData;

namespace GameLogic
{
	public class NPCMediator : Mediator
	{
		private DialogPanel panel;

        private BlockProxy blockProxy;
		private NPCProxy npcProxy;

		private NPC currentNPC;

		public NPCMediator () : base()
		{
            blockProxy = ApplicationFacade.Instance.RetrieveProxy<BlockProxy>();
			npcProxy = ApplicationFacade.Instance.RetrieveProxy<NPCProxy>();
		}

		public override IList<Enum> ListNotificationInterests ()
		{
			return new Enum[]
			{
				NotificationEnum.NPC_INIT,
				NotificationEnum.NPC_DISPOSE,
				NotificationEnum.TOWN_NPC_SPAWN,
                NotificationEnum.BLOCK_SPAWN,
                NotificationEnum.BLOCK_DESPAWN,
                NotificationEnum.HALL_SPAWN,
                NotificationEnum.HALL_DESPAWN,
				NotificationEnum.NPC_DIALOG_SHOW,
			};
		}

		public override void HandleNotification (INotification notification)
		{
            switch((NotificationEnum)notification.NotifyEnum)
			{
				case NotificationEnum.NPC_INIT:
					HandleNPCInit();
					break;
				case NotificationEnum.NPC_DISPOSE:
					HandleNPCDispose();
					break;
				case NotificationEnum.TOWN_NPC_SPAWN:
					HandleTownNPCSpawn();
					break;
                case NotificationEnum.BLOCK_SPAWN:
				{
					HandleBlockSpawn();
					break;
				}
                case NotificationEnum.BLOCK_DESPAWN:
				{
					HandleBlockDespawn();
					break;
				}
                case NotificationEnum.HALL_SPAWN:
                {
                    HandleHallSpawn();
                    break;
                }
                case NotificationEnum.HALL_DESPAWN:
                {
                    HandleHallDespawn();
                    break;
                }
				case NotificationEnum.NPC_DIALOG_SHOW:
					NPC npc = notification.Body as NPC;
					HandleDialogShow(npc);
					break;
			}
		}

		private void HandleNPCInit()
		{

		}
		private void HandleNPCDispose()
		{
			npcProxy.Dispose();
		}
		private void HandleTownNPCSpawn()
		{
			int npcID = IDManager.Instance.GetKid(IDType.NPC, 1);
			int npcEventID = IDManager.Instance.GetKid(IDType.NPCEvent, 1);
			NPC npc = NPC.Create(npcID, npcEventID);
			npc.SetPosition(new Vector3(-1.45f, -0.1f, 17));
			npc.SetRotation(180f);
		}
		private void HandleBlockSpawn()
		{
            if(npcProxy.RecordDic.ContainsKey(0))
            {
                List<NPCRecord> recordList = npcProxy.RecordDic[0];
                for (int i = 0; i < recordList.Count; ++i)
                {
                    NPCRecord record = recordList[i];
                    NPC npc = NPC.Create(record);
                    InitNPC(npc, record.WorldPosition.ToVector3(), record.WorldAngle);
                }
            }
            else
            {
                blockProxy.ForeachBlocks((Block block) =>
                    {
                        RandomUtils.Seed = block.RandomID;
                        int npcCount = RandomUtils.Range(0, MazeDataManager.Instance.CurrentMazeData.NPCMaxCount);

                        for (int i = 0; i < npcCount; ++i)
                        {
                            PositionScript birth = block.Script.GetRandomPosition(PositionType.NPC);
                            if (birth != null)
                            {
                                NPCData npcData = NPCDataManager.Instance.GetRandomNPCData(NPCAppearScene.Maze);
                                NPCEventData eventData = NPCDataManager.Instance.GetRandomEventData(NPCAppearScene.Maze);
                                NPC npc = NPC.Create(npcData.Kid, eventData.Kid);
                                InitNPC(npc, birth.transform.position, birth.transform.eulerAngles.y);
                            }
                        }
                    });
            }
		}
		private void HandleBlockDespawn()
		{
            npcProxy.SaveRecord();
            npcProxy.Dispose();
		}
        private void HandleHallSpawn()
        {
        }
        private void HandleHallDespawn()
        {
        }
        private void InitNPC(NPC npc, Vector3 position, float angle)
        {
            npc.SetPosition(position);
            npc.SetRotation(angle);
            npcProxy.AddNPC(npc);
        }
		private void HandleDialogShow(NPC npc)
		{
			currentNPC = npc;

			panel = PopupManager.Instance.CreateAndAddPopup<DialogPanel>();
			panel.CallbackDialogFinish = OnDialogClick;

			string npcName = TextDataManager.Instance.GetData(npc.Data.Name);
			panel.LabelTitle.text = npcName;

			NPCData data = npc.Data;
			NPCEventData eventData = npc.EventData;
            panel.Init(data.Name, eventData.TalkList);
		}

		private void OnDialogClick()
		{
			PopupManager.Instance.RemovePopup(panel);
			if (currentNPC.Data.AppearScene == NPCAppearScene.HomeTown)
			{
				Game.Instance.SwitchStage(StageEnum.Maze);
			}
			currentNPC = null;
		}
	}
}

