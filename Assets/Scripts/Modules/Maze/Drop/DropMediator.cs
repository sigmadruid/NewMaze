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
//                NotificationEnum.BLOCK_SPAWN,
//                NotificationEnum.BLOCK_DESPAWN,
//                NotificationEnum.HALL_SPAWN,
//                NotificationEnum.HALL_DESPAWN,
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
                case NotificationEnum.HALL_SPAWN:
                    {
                        Hall hall = notification.Body as Hall;
                        HandleItemSpawn(hall);
                        break;
                    }
                case NotificationEnum.HALL_DESPAWN:
                    {
                        Hall hall = notification.Body as Hall;
                        HandleItemDespawn(hall);
                        break;
                    }
				case NotificationEnum.DROP_CREATED:
				    {
                        Tupple<int, Vector3> tupple = notification.Body as Tupple<int, Vector3>;
                        HandleItemSpawnSingle(tupple.Item1, tupple.Item2);
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
            block.ForeachNode((MazePosition mazePos) =>
                {
                    int location = Maze.GetCurrentLocation(mazePos.Col, mazePos.Row);
                    List<ItemRecord> recordList = dropProxy.GetRecordList(location);
                    if (recordList != null)
                    {
                        for (int i = 0; i < recordList.Count; ++i)
                        {
                            ItemRecord record = recordList[i];
                            Item item = Item.Create(record);
                            dropProxy.AddItem(item);
                        }
                    }
                });

		}
		private void HandleItemDespawn(Block block)
		{
            List<Item> toHideItemList = new List<Item>();
            dropProxy.IterateDrops((Item item) => 
            {
                MazePosition itemPos = Maze.Instance.GetMazePosition(item.WorldPosition);
                if (block.Contains(itemPos.Col, itemPos.Row))
				{
					toHideItemList.Add(item);
				}
			});

            dropProxy.InitRecordList(block);

			for (int i = 0; i < toHideItemList.Count; ++i)
			{
                Item drop = toHideItemList[i];
				dropProxy.HideItem(drop.Uid);
			}
		}
        private void HandleItemSpawn(Hall hall)
        {
            int location = Maze.GetCurrentLocation(hall.Data.Kid);
            List<ItemRecord> recordList = dropProxy.GetRecordList(location);
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
        private void HandleItemDespawn(Hall hall)
        {
            List<Item> toHideItemList = new List<Item>();
            dropProxy.IterateDrops((Item item) => 
                {
                    toHideItemList.Add(item);
                });

            dropProxy.InitRecordList(hall);

            for (int i = 0; i < toHideItemList.Count; ++i)
            {
                Item drop = toHideItemList[i];
                dropProxy.HideItem(drop.Uid);
            }
        }
        private void HandleItemSpawnSingle(int dropKid, Vector3 position)
		{
            DropData dropData = DropDataManager.Instance.GetData(dropKid) as DropData;
            int count = RandomUtils.Range(1, dropData.MaxNum);
            for(int i = 0; i < count; ++i)
            {
                Item item = Item.Create(dropData, position);
                dropProxy.AddItem(item);
                item.StartFlying(position);
            }
		}
        private void HandleItemDespawnSingle(ItemScript itemScript)
		{
            Item item = dropProxy.GetItemByUid(itemScript.Uid);
            if (item != null)
			{
                item.PickedUp();
                packProxy.ChangeCount(item.Data.Kid, item.Info.Count);
                dropProxy.RemoveItem(item.Uid);
			}
            string title = itemScript.name;
            TitlePanel.Show(title);
		}
    }
}

