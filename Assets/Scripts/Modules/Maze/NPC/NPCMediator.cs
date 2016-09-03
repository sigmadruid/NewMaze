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

		private NPCProxy npcProxy;

		private NPC currentNPC;

		public NPCMediator () : base()
		{
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
					Block block = notification.Body as Block;
					HandleNPCSpawn(block);
					break;
				}
                case NotificationEnum.BLOCK_DESPAWN:
				{
					Block block = notification.Body as Block;
					HandleNPCDespawn(block);
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
			int npcID = IDManager.Instance.GetID(IDType.NPC, 1);
			int npcEventID = IDManager.Instance.GetID(IDType.NPCEvent, 1);
			NPC npc = NPC.Create(npcID, npcEventID);
			npc.SetPosition(new Vector3(-1.45f, -0.1f, 17));
			npc.SetRotation(180f);
		}
		private void HandleNPCSpawn(Block block)
		{
			int npcCount = RandomUtils.Range(0, MazeDataManager.Instance.CurrentMazeData.MonsterMaxCount);
			
			for (int i = 0; i < npcCount; ++i)
			{
				PositionScript birth = block.Script.GetRandomPosition(PositionType.NPC);

				if (birth != null)
				{
					NPCData npcData = NPCDataManager.Instance.GetRandomNPCData(NPCAppearScene.Maze);
					NPCEventData eventData = NPCDataManager.Instance.GetRandomEventData(NPCAppearScene.Maze);
					NPC npc = NPC.Create(npcData.Kid, eventData.Kid);
					npc.SetPosition(birth.transform.position);
					npc.SetRotation(birth.transform.eulerAngles.y);
					npcProxy.AddNPC(npc);
				}
			}
		}
		private void HandleNPCDespawn(Block block)
		{
			List<NPC> npcList = npcProxy.GetAllNPCs();
			float blockSize = MazeDataManager.Instance.CurrentMazeData.BlockSize;
			int count = npcList.Count;
			for (int i = 0; i < count; ++i)
			{
				NPC npc = npcList[i];
				int col, row;
				MazeUtil.GetMazePosition(npc.WorldPosition, blockSize, out col, out row);
				if (block.Contains(col, row))
				{
					npcProxy.RemoveNPC(npc.Uid);
					
					NPC.Recycle(npc);
				}
			}
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
			if (eventData.Type == NPCEventType.Normal)
			{
				panel.Init(data.Name, eventData.FirstTalkList);
			}
			else if (eventData.Type == NPCEventType.Result)
			{
                List<string> talkList = npc.State == NPCState.Normal ? eventData.FirstTalkList : eventData.EndTalkList;
				panel.Init(data.Name, talkList);
			}
			else if (eventData.Type == NPCEventType.Task)
			{

			}

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

