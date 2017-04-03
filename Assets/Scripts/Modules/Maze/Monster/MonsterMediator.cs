using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;

using Base;
using Battle;
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
                NotificationEnum.HALL_SPAWN,
                NotificationEnum.HALL_DESPAWN,
                NotificationEnum.ENVIRONMENT_DAYNIGHT_CHANGE,
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
					HandleSpawn(block);
					break;
				}
                case NotificationEnum.BLOCK_DESPAWN:
				{
					Block block = notification.Body as Block;
					HandleDespawn(block);
					break;
				}
                case NotificationEnum.HALL_SPAWN:
                {
                    Hall hall = notification.Body as Hall;
                    HandleSpawn(hall);
                    break;
                }
                case NotificationEnum.HALL_DESPAWN:
                {
                    Hall hall = notification.Body as Hall;
                    HandleDespawn(hall);
                    break;
                }
                case NotificationEnum.ENVIRONMENT_DAYNIGHT_CHANGE:
                {
                    bool isNight = (bool)notification.Body;
                    HandleEnvironmentChange(isNight);
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
		private void HandleSpawn(Block block)
		{
            RandomUtils.Seed = block.RandomID;
            block.ForeachNode((MazePosition mazePos) =>
                {
                    int location = Maze.GetCurrentLocation(mazePos.Col, mazePos.Row);
                    List<MonsterRecord> recordList = monsterProxy.GetRecordList(location);
                    if (recordList != null)
                    {
                        for (int i = 0; i < recordList.Count; ++i)
                        {
                            MonsterRecord record = recordList[i];
                            Monster monster = Monster.Create(record);
                            InitMonster(monster, record.WorldPosition.ToVector3());
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
                });
			
		}
		//When a block despawns, remove all monsters on it from the monsterDic, and store them in the recordDic
		private void HandleDespawn(Block block)
		{
			List<Monster> toDeleteMonsterList = new List<Monster>();
			monsterProxy.IterateActives((Monster monster) => 
	         {
                MazePosition monsterPos = Maze.Instance.GetMazePosition(monster.WorldPosition);
                if (block.Contains(monsterPos.Col, monsterPos.Row))
				{
					toDeleteMonsterList.Add(monster);
				}
			});

            monsterProxy.InitRecordList(block);

			for (int i = 0; i < toDeleteMonsterList.Count; ++i)
			{
				Monster monster = toDeleteMonsterList[i];

				if (monster.Info.IsAlive)
				{
					monsterProxy.HideMonster(monster.Uid);
				}
				else
				{
					monsterProxy.RemoveMonster(monster.Uid);
				}
				Monster.Recycle(monster);
			}
		}
        private void HandleSpawn(Hall hall)
        {
            int location = Maze.GetCurrentLocation(hall.Data.Kid);
            List<MonsterRecord> recordList = monsterProxy.GetRecordList(location);
            if(recordList != null)
            {
                for(int i = 0; i < recordList.Count; ++i)
                {
                    MonsterRecord record = recordList[i];
                    Monster monster = Monster.Create(record);
                    InitMonster(monster, record.WorldPosition.ToVector3());
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
        private void HandleDespawn(Hall hall)
        {
            List<Monster> toDeleteMonsterList = new List<Monster>();
            monsterProxy.IterateActives((Monster monster) => 
                {
                    toDeleteMonsterList.Add(monster);
                });

            monsterProxy.InitRecordList(hall);

            for (int i = 0; i < toDeleteMonsterList.Count; ++i)
            {
                Monster monster = toDeleteMonsterList[i];

                if (monster.Info.IsAlive)
                {
                    monsterProxy.HideMonster(monster.Uid);
                }
                else
                {
                    monsterProxy.RemoveMonster(monster.Uid);
                }
                Monster.Recycle(monster);
            }
        }
        private void InitMonster(Monster monster, Vector3 position)
        {
            bool isNight = ApplicationFacade.Instance.RetrieveProxy<EnvironmentProxy>().IsNight;
            monster.SetAtNight(isNight);
            monster.SetPosition(position);
            monsterProxy.AddMonster(monster);
        }

        private void HandleEnvironmentChange(bool isNight)
        {
            monsterProxy.IterateActives((Monster monster) => 
                {
                    monster.SetAtNight(isNight);
                });
        }

		private void HandleBattlePause(bool isPause)
		{
			monsterProxy.IterateActives((Monster monster) => 
			{
				monster.Pause(isPause);
			});
		}

	}
}

