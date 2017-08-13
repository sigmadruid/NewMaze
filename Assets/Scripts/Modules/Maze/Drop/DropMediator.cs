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
                NotificationEnum.HALL_SPAWN,
                NotificationEnum.HALL_DESPAWN,
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

		private void HandleBlockSpawn()
		{
            if(dropProxy.RecordDic.ContainsKey(0))
            {
                List<DropRecord> recordList = dropProxy.RecordDic[0];
                for (int i = 0; i < recordList.Count; ++i)
                {
                    DropRecord record = recordList[i];
                    Item item = Item.Create(record);
                    dropProxy.AddItem(item);
                }
            }
		}
		private void HandleBlockDespawn()
		{
            dropProxy.SaveRecord();
            dropProxy.Dispose();
		}
        private void HandleHallSpawn()
        {
            int hallKid = Hall.Instance.Data.Kid;
            if(dropProxy.RecordDic.ContainsKey(hallKid))
            {
                List<DropRecord> recordList = dropProxy.RecordDic[hallKid];
                for (int i = 0; i < recordList.Count; ++i)
                {
                    DropRecord record = recordList[i];
                    Item item = Item.Create(record);
                    dropProxy.AddItem(item);
                }
            }
        }
        private void HandleHallDespawn()
        {
            dropProxy.SaveRecord();
            dropProxy.Dispose();
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
                Item.Recycle(item);

			}

		}
    }
}

