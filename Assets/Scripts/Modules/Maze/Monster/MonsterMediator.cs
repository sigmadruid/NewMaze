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
				NotificationEnum.MONSTER_SPAWN,
				NotificationEnum.MONSTER_DESPAWN,
				NotificationEnum.BATTLE_PAUSE,
			};
		}
		
		public override void HandleNotification (INotification notification)
		{
            switch((NotificationEnum)notification.NotifyEnum)
			{
				case NotificationEnum.MONSTER_SPAWN:
				{
					Block block = notification.Body as Block;
					HandleSpawnMonster(block);
					break;
				}
				case NotificationEnum.MONSTER_DESPAWN:
				{
					Block block = notification.Body as Block;
					HandleDespawnMonster(block);
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
		private void HandleSpawnMonster(Block block)
		{
			//Room??
			int blockKey = Block.GetBlockKey(block.Col, block.Row);
			List<MonsterRecord> recordList = monsterProxy.GetRecordList(blockKey);
			if (recordList != null)
			{
				for (int i = 0; i < recordList.Count; ++i)
				{
					MonsterRecord record = recordList[i];
					Monster monster = Monster.Create(record);
					monster.SetPosition(record.WorldPosition);
					monsterProxy.AddMonster(monster);
					battleProxy.AddMonster(monster);
				}
			}
			else
			{
				int monsterCount = RandomUtils.Range(0, MazeDataManager.Instance.CurrentMazeData.MonsterMaxCount);
				
				for (int i = 0; i < monsterCount; ++i)
				{
                    PositionScript birth = block.Script.GetRandomPosition(BlockScript.PositionType.MonsterPositions);
					
					if (birth != null)
					{
						Monster monster = Monster.Create(null);
						monster.SetPosition(birth.transform.position);
						monsterProxy.AddMonster(monster);
						battleProxy.AddMonster(monster);
					}
				}
			}

		}

		//When a block despawns, remove all monsters on it from the monsterDic, and store them in the recordDic
		private void HandleDespawnMonster(Block block)
		{
			List<Monster> toDeleteMonsterList = new List<Monster>();
			monsterProxy.IterateMonsters((Monster monster) => 
	         {
				Vector2 monsterPos = Maze.Instance.GetMazePosition(monster.WorldPosition);
				if (block.Contains((int)monsterPos.x, (int)monsterPos.y))
				{
					toDeleteMonsterList.Add(monster);
				}
			});

			int blockKey = Block.GetBlockKey(block.Col, block.Row);
			monsterProxy.InitRecordList(blockKey);

			for (int i = 0; i < toDeleteMonsterList.Count; ++i)
			{
				Monster monster = toDeleteMonsterList[i];
				battleProxy.RemoveMonster(monster.Uid);

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
		private void HandleBattlePause(bool isPause)
		{
			monsterProxy.IterateMonsters((Monster monster) => 
			{
				monster.Pause(isPause);
			});
		}

	}
}

