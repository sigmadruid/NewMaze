using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;

using Base;
using StaticData;

namespace GameLogic
{
	public class MonsterMediator : Mediator
	{
		private MonsterProxy monsterProxy;
		private BattleProxy battleProxy;

		public MonsterMediator () : base()
		{
			monsterProxy = ApplicationFacade.Instance.RetrieveProxy<MonsterProxy>();
			battleProxy = ApplicationFacade.Instance.RetrieveProxy<BattleProxy>();
		}

		public override IList<Enum> ListNotificationInterests ()
		{
			return new Enum[]
			{
                NotificationEnum.BLOCK_SPAWN,
                NotificationEnum.BLOCK_DESPAWN,
//                NotificationEnum.HALL_SPAWN,
//                NotificationEnum.HALL_DESPAWN,
				NotificationEnum.BATTLE_PAUSE,
			};
		}
		
		public override void HandleNotification (INotification notification)
		{
            switch((NotificationEnum)notification.NotifyEnum)
			{
                case NotificationEnum.BLOCK_SPAWN:
				{
					Block block = notification.Body as Block;
					HandleBlockSpawn(block);
					break;
				}
                case NotificationEnum.BLOCK_DESPAWN:
				{
					Block block = notification.Body as Block;
					HandleBlockDespawn(block);
					break;
				}
                case NotificationEnum.HALL_SPAWN:
                {
                    Hall hall = notification.Body as Hall;
                    HandleHallSpawn(hall);
                    break;
                }
                case NotificationEnum.HALL_DESPAWN:
                {
                    Hall hall = notification.Body as Hall;
                    HandleHallDespawn(hall);
                    break;
                }
				case NotificationEnum.BATTLE_PAUSE:
				{
					bool isPause = (bool)notification.Body;
					HandleBattlePause(isPause);
					break;
				}
			}
		}

		//When spawn a block, check if any monster already stores on this pos.
		private void HandleBlockSpawn(Block block)
		{
			//Room??
			int blockKey = Block.GetBlockKey(block.Col, block.Row);
			List<MonsterRecord> recordList = monsterProxy.GetRecordBlockList(blockKey);
			if (recordList != null)
			{
				for (int i = 0; i < recordList.Count; ++i)
				{
					MonsterRecord record = recordList[i];
					Monster monster = Monster.Create(record);
                    InitMonster(monster, record.WorldPosition);
				}
			}
			else
			{
				int monsterCount = RandomUtils.Range(0, MazeDataManager.Instance.CurrentMazeData.MonsterMaxCount);
				for (int i = 0; i < monsterCount; ++i)
				{
                    PositionScript birth = block.Script.GetRandomPosition(PositionType.Monster);
					if (birth != null)
					{
						Monster monster = Monster.Create(null);
                        InitMonster(monster, birth.transform.position);
					}
				}
			}

		}

		//When a block despawns, remove all monsters on it from the monsterDic, and store them in the recordDic
		private void HandleBlockDespawn(Block block)
		{
			List<Monster> toDeleteMonsterList = new List<Monster>();
			monsterProxy.IterateMonstersInBlocks((Monster monster) => 
	         {
				Vector2 monsterPos = Maze.Instance.GetMazePosition(monster.WorldPosition);
				if (block.Contains((int)monsterPos.x, (int)monsterPos.y))
				{
					toDeleteMonsterList.Add(monster);
				}
			});

			int blockKey = Block.GetBlockKey(block.Col, block.Row);
			monsterProxy.InitRecordBlockList(blockKey);

			for (int i = 0; i < toDeleteMonsterList.Count; ++i)
			{
				Monster monster = toDeleteMonsterList[i];
				battleProxy.RemoveMonster(monster.Uid);

				if (monster.Info.IsAlive)
				{
					monsterProxy.HideMonsterInBlock(monster.Uid);
				}
				else
				{
					monsterProxy.RemoveMonsterInBlock(monster.Uid);
				}
				Monster.Recycle(monster);
			}
		}

        private void HandleHallSpawn(Hall hall)
        {
            List<MonsterRecord> recordList = monsterProxy.GetRecordHallList(hall.Data.Kid);
            if(recordList != null)
            {
                for(int i = 0; i < recordList.Count; ++i)
                {
                    MonsterRecord record = recordList[i];
                    Monster monster = Monster.Create(record);
                    InitMonster(monster, record.WorldPosition);
                }
            }
            else
            {
                PositionScript[] positionList = hall.Script.GetPositionList(PositionType.Monster);
                for (int i = 0; i < positionList.Length; ++i)
                {
                    PositionScript birth = positionList[i];
                    Monster monster = Monster.Create(birth.Kid);
                    InitMonster(monster, birth.transform.position);
                }
            }
        }
        private void HandleHallDespawn(Hall hall)
        {
            monsterProxy.IterateMonstersInBlocks((Monster monster) => 
                {
                    battleProxy.RemoveMonster(monster.Uid);

                    if (monster.Info.IsAlive)
                    {
                        monsterProxy.HideMonsterInBlock(monster.Uid);
                    }
                    else
                    {
                        monsterProxy.RemoveMonsterInBlock(monster.Uid);
                    }
                    Monster.Recycle(monster);
                });

            monsterProxy.InitRecordHallList(hall.Data.Kid);
        }

        private void InitMonster(Monster monster, Vector3 position)
        {
            monster.SetPosition(position);
            monsterProxy.AddMonsterInBlock(monster);
            battleProxy.AddMonster(monster);
        }

		private void HandleBattlePause(bool isPause)
		{
			monsterProxy.IterateMonstersInBlocks((Monster monster) => 
			{
				monster.Pause(isPause);
			});
		}

	}
}

