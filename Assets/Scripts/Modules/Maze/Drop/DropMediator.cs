using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;

using Base;
using StaticData;

namespace GameLogic
{
    public class DropMediator : Mediator
    {
        private DropProxy dropProxy;
        private PackProxy packProxy;

        public override void OnRegister ()
        {
            dropProxy = ApplicationFacade.Instance.RetrieveProxy<DropProxy>();
            packProxy = ApplicationFacade.Instance.RetrieveProxy<PackProxy>();
        }
        
        public override IList<Enum> ListNotificationInterests ()
        {
            return new Enum[]
            {
                NotificationEnum.BLOCK_SPAWN,
                NotificationEnum.BLOCK_DESPAWN,
				NotificationEnum.DROP_CREATED,
                NotificationEnum.DROP_PICKED_UP,
            };
        }
        
        public override void HandleNotification (INotification notification)
        {
            switch((NotificationEnum)notification.NotifyEnum)
            {
                case NotificationEnum.BLOCK_SPAWN:
                {
					Block block = notification.Body as Block;
                    HandleItemSpawn(block);
                    break;
                }
                case NotificationEnum.BLOCK_DESPAWN:
				{
					Block block = notification.Body as Block;
                    HandleItemDespawn(block);
					break;
				}
				case NotificationEnum.DROP_CREATED:
				{
					Monster monster = notification.Body as Monster;
                    HandleItemSpawnSingle(monster);
					break;
				}
				case NotificationEnum.DROP_PICKED_UP:
                {
                    ItemScript itemScript = notification.Body as ItemScript;
                    HandleItemDespawnSingle(itemScript);
                    break;
                }
            }
        }

		private void HandleItemSpawn(Block block)
		{
			int blockKey = Block.GetBlockKey(block.Col, block.Row);
			List<ItemRecord> recordList = dropProxy.GetRecordList(blockKey);
			if (recordList != null)
			{
				for (int i = 0; i < recordList.Count; ++i)
				{
					ItemRecord record = recordList[i];
                    Item item = Item.Create(record);
					dropProxy.AddItem(item);
				}
			}
		}
		private void HandleItemDespawn(Block block)
		{
            List<Item> toDeleteDropList = new List<Item>();
            dropProxy.IterateDrops((Item item) => 
            {
                Vector2 itemPos = Maze.Instance.GetMazePosition(item.WorldPosition);
				if (block.Contains((int)itemPos.x, (int)itemPos.y))
				{
					toDeleteDropList.Add(item);
				}
			});

			int blockKey = Block.GetBlockKey(block.Col, block.Row);
			dropProxy.InitRecordList(blockKey);

			for (int i = 0; i < toDeleteDropList.Count; ++i)
			{
                Item drop = toDeleteDropList[i];
				dropProxy.HideItem(drop.Uid);
                Item.Recycle(drop);
			}
		}
		private void HandleItemSpawnSingle(Monster monster)
		{
            DropData dropData = DropDataManager.Instance.GetData(monster.Data.DropKid) as DropData;
            Item item = Item.Create(dropData, monster.WorldPosition);
			dropProxy.AddItem(item);
			item.StartFlying(monster.WorldPosition);
		}
        private void HandleItemDespawnSingle(ItemScript itemScript)
		{
            Item item = dropProxy.GetItemByUid(itemScript.Uid);
            if (item != null)
			{
                item.PickedUp();
                packProxy.ChangeCount(item.Data.Kid, item.Info.Count);
                dropProxy.RemoveItem(item.Uid);
                Item.Recycle(item);
			}

		}
    }
}

