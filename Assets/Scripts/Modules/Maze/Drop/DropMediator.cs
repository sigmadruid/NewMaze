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
		private DropProxy proxy;

        public override void OnRegister ()
        {
			proxy = ApplicationFacade.Instance.RetrieveProxy<DropProxy>();
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
                    HandleItemDespawnSingle();
                    break;
                }
            }
        }

		private void HandleItemSpawn(Block block)
		{
			int blockKey = Block.GetBlockKey(block.Col, block.Row);
			List<ItemRecord> recordList = proxy.GetRecordList(blockKey);
			if (recordList != null)
			{
				for (int i = 0; i < recordList.Count; ++i)
				{
					ItemRecord record = recordList[i];
                    Item item = Item.Create(record);
					proxy.AddItem(item);
				}
			}
		}
		private void HandleItemDespawn(Block block)
		{
            List<Item> toDeleteDropList = new List<Item>();
            proxy.IterateDrops((Item item) => 
            {
                Vector2 itemPos = Maze.Instance.GetMazePosition(item.WorldPosition);
				if (block.Contains((int)itemPos.x, (int)itemPos.y))
				{
					toDeleteDropList.Add(item);
				}
			});

			int blockKey = Block.GetBlockKey(block.Col, block.Row);
			proxy.InitRecordList(blockKey);

			for (int i = 0; i < toDeleteDropList.Count; ++i)
			{
                Item drop = toDeleteDropList[i];
				proxy.HideItem(drop.Uid);
                Item.Recycle(drop);
			}
		}
		private void HandleItemSpawnSingle(Monster monster)
		{
            Item item = Item.Create(monster.Data.DropKidList[0], monster.WorldPosition);
			proxy.AddItem(item);
			item.StartFlying(monster.WorldPosition);
		}
		private void HandleItemDespawnSingle()
		{
            Item nearbyItem = proxy.FindNearbyItem(Hero.Instance.WorldPosition);
			if (nearbyItem != null)
			{
				nearbyItem.PickedUp();
				proxy.RemoveItem(nearbyItem.Uid);
				Item.Recycle(nearbyItem);
			}

		}
    }
}

