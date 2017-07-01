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
        private BlockProxy blockProxy;
		private MonsterProxy monsterProxy;

		public MonsterMediator () : base()
		{
            blockProxy = ApplicationFacade.Instance.RetrieveProxy<BlockProxy>();
			monsterProxy = ApplicationFacade.Instance.RetrieveProxy<MonsterProxy>();
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
    					HandleBlockInit();
    					break;
    				}
                case NotificationEnum.BLOCK_DESPAWN:
    				{
    					HandleBlockDispose();
    					break;
    				}
                case NotificationEnum.HALL_SPAWN:
                    {
                        HandleHallInit();
                        break;
                    }
                case NotificationEnum.HALL_DESPAWN:
                    {
                        HandleHallDispose();
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

		private void HandleBlockInit()
		{
            if(monsterProxy.RecordDic.ContainsKey(0))
            {
                List<MonsterRecord> recordList = monsterProxy.RecordDic[0];
                for (int i = 0; i < recordList.Count; ++i)
                {
                    MonsterRecord record = recordList[i];
                    Monster monster = Monster.Create(record);
                    InitMonster(monster, record.WorldPosition.ToVector3());
                }
            }
            else
            {
                blockProxy.ForeachBlocks((Block block) =>
                    {
                        RandomUtils.Seed = block.RandomID;
                        int monsterCount = RandomUtils.Range(0, MazeDataManager.Instance.CurrentMazeData.MonsterMaxCount);
                        for(int i = 0; i < monsterCount; ++i)
                        {
                            PositionScript birth = block.Script.GetRandomPosition(PositionType.Monster);
                            if(birth != null)
                            {
                                Monster monster = Monster.Create(null);
                                InitMonster(monster, birth.transform.position);
                            }
                        }
                    });
            }
		}
		private void HandleBlockDispose()
		{
            monsterProxy.SaveRecord();
            monsterProxy.Dispose();
		}
        private void HandleHallInit()
        {
            int hallKid = Hall.Instance.Data.Kid;
            if(monsterProxy.RecordDic.ContainsKey(hallKid))
            {
                List<MonsterRecord> recordList = monsterProxy.RecordDic[hallKid];
                for (int i = 0; i < recordList.Count; ++i)
                {
                    MonsterRecord record = recordList[i];
                    Monster monster = Monster.Create(record);
                    InitMonster(monster, record.WorldPosition.ToVector3());
                }
            }
            else
            {
                PositionScript[] positionList = Hall.Instance.Script.GetPositionList(PositionType.Monster);
                for (int i = 0; i < positionList.Length; ++i)
                {
                    PositionScript birth = positionList[i];
                    Monster monster = Monster.Create(birth.Kid);
                    InitMonster(monster, birth.transform.position);
                }
            }
        }
        private void HandleHallDispose()
        {
            monsterProxy.SaveRecord();
            monsterProxy.Dispose();
        }
        private void InitMonster(Monster monster, Vector3 position)
        {
            bool isNight = ApplicationFacade.Instance.RetrieveProxy<EnvironmentProxy>().IsNight;
            monster.SetAtNight(isNight);
            monster.SetPosition(position);
        }

        private void HandleEnvironmentChange(bool isNight)
        {
            monsterProxy.Foreach((Monster monster) => 
                {
                    monster.SetAtNight(isNight);
                });
        }

		private void HandleBattlePause(bool isPause)
		{
            monsterProxy.Foreach((Monster monster) =>
                {
                    monster.Pause(isPause);
                });

		}

	}
}

