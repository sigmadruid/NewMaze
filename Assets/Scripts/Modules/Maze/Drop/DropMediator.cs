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
//					Block block = notification.Body as Block;
//					HandleDropSpawn(block);
                    break;
                }
                case NotificationEnum.BLOCK_DESPAWN:
				{
//					Block block = notification.Body as Block;
//					HandleDropDespawn(block);
					break;
				}
				case NotificationEnum.DROP_CREATED:
				{
//					Monster monster = notification.Body as Monster;
//					HandleDropSpawnSingle(monster);
					break;
				}
				case NotificationEnum.DROP_PICKED_UP:
                {
//					HandleDropDespawnSingle();
                    break;
                }
            }
        }

		private void HandleDropSpawn(Block block)
		{
			int blockKey = Block.GetBlockKey(block.Col, block.Row);
			List<DropRecord> recordList = proxy.GetRecordList(blockKey);
			if (recordList != null)
			{
				for (int i = 0; i < recordList.Count; ++i)
				{
					DropRecord record = recordList[i];
					Drop drop = Drop.Create(record);
					proxy.AddDrop(drop);
				}
			}
		}
		private void HandleDropDespawn(Block block)
		{
			List<Drop> toDeleteDropList = new List<Drop>();
			proxy.IterateDrops((Drop drop) => 
            {
				Vector2 dropPos = Maze.Instance.GetMazePosition(drop.WorldPosition);
				if (block.Contains((int)dropPos.x, (int)dropPos.y))
				{
					toDeleteDropList.Add(drop);
				}
			});

			int blockKey = Block.GetBlockKey(block.Col, block.Row);
			proxy.InitRecordList(blockKey);

			for (int i = 0; i < toDeleteDropList.Count; ++i)
			{
				Drop drop = toDeleteDropList[i];
				proxy.HideDrop(drop.Uid);
				Drop.Recycle(drop);
			}
		}
		private void HandleDropSpawnSingle(Monster monster)
		{
			Drop drop = Drop.Create(monster.Data.DropKidList[0], monster.WorldPosition);
			proxy.AddDrop(drop);
			drop.StartFlying(monster.WorldPosition);
		}
		private void HandleDropDespawnSingle()
		{
			Drop nearbyDrop = proxy.FindNearbyDrop(Hero.Instance.WorldPosition);
			if (nearbyDrop != null)
			{
				nearbyDrop.PickedUp();
				proxy.RemoveDrop(nearbyDrop.Uid);
				Drop.Recycle(nearbyDrop);
			}

		}
    }
}

