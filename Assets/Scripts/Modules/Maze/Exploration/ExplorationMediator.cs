using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;

using Base;
using StaticData;

namespace GameLogic
{
    public class ExplorationMediator : Mediator
    {
        private BlockProxy blockProxy;
        private ExplorationProxy explorationProxy;
		
		public override void OnRegister ()
		{
            blockProxy = ApplicationFacade.Instance.RetrieveProxy<BlockProxy>();
			explorationProxy = ApplicationFacade.Instance.RetrieveProxy<ExplorationProxy>();
		}

		public override IList<Enum> ListNotificationInterests ()
		{
			return new Enum[]
			{
                NotificationEnum.BLOCK_SPAWN,
                NotificationEnum.BLOCK_DESPAWN,
                NotificationEnum.HALL_SPAWN,
                NotificationEnum.HALL_DESPAWN,
                NotificationEnum.EXPLORATION_FUNCTION,
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
                case NotificationEnum.EXPLORATION_FUNCTION:
                {
                    HandleFunction();
                    break;
                }
			}
		}

		private void HandleBlockSpawn()
		{
            if(explorationProxy.RecordDic.ContainsKey(0))
            {
                List<ExplorationRecord> recordList = explorationProxy.RecordDic[0];
                for (int i = 0; i < recordList.Count; ++i)
                {
                    ExplorationRecord record = recordList[i];
                    CreateExploration(record);
                }
            }
            else
            {
                blockProxy.ForeachBlocks((Block block) =>
                    {
                        RandomUtils.Seed = block.RandomID;
                        int explorationCount = RandomUtils.Range(0, MazeDataManager.Instance.CurrentMazeData.ExplorationMaxCount);
                        PositionScript birth = null;
                        if (block.ExplorationKid != 0)
                        {
                            birth = block.Script.GetGlobalPosition(PositionType.Exploration);
                            CreateExploration(block.ExplorationKid, birth);
                            explorationCount--;
                        }
                        
                        for (int i = 0; i < explorationCount; ++i)
                        {
                            birth = block.Script.GetRandomPosition(PositionType.Exploration);
                            CreateExploration(ExplorationType.Common, birth);
                        }
                    });
            }
		}
		private void HandleBlockDespawn()
		{
            explorationProxy.SaveRecord();
            explorationProxy.Dispose();
		}
        private void HandleHallSpawn()
        {
            int hallKid = Hall.Instance.Data.Kid;
            if(explorationProxy.RecordDic.ContainsKey(hallKid))
            {
                List<ExplorationRecord> recordList = explorationProxy.RecordDic[hallKid];
                for (int i = 0; i < recordList.Count; ++i)
                {
                    ExplorationRecord record = recordList[i];
                    CreateExploration(record);
                }
            }
            else
            {
                PositionScript[] positionList = Hall.Instance.Script.GetPositionList(PositionType.Exploration);
                for (int i = 0; i < positionList.Length; ++i)
                {
                    PositionScript birth = positionList[i];
                    CreateExploration(birth.Kid, birth);
                }
            }
        }
        private void HandleHallDespawn()
        {
            explorationProxy.SaveRecord();
            explorationProxy.Dispose();
        }
        private void CreateExploration(ExplorationType type, PositionScript birth)
        {
            if (birth != null)
            {
                Exploration expl = ExplorationFactory.Create(type);
                InitExploration(expl, birth.transform.position, birth.transform.localEulerAngles.y);
            }
        }
        private void CreateExploration(int kid, PositionScript birth)
        {
            if (birth != null)
            {
                Exploration expl = ExplorationFactory.Create(kid);
                InitExploration(expl, birth.transform.position, birth.transform.localEulerAngles.y);
            }
        }
        private void CreateExploration(ExplorationRecord record)
        {
            Exploration expl = ExplorationFactory.Create(record);
            InitExploration(expl, record.WorldPosition.ToVector3(), record.WorldAngle);
        }
        private void InitExploration(Exploration expl, Vector3 position, float angle)
        {
            expl.SetPosition(position);
            expl.SetRotation(angle);
            explorationProxy.AddExpl(expl);
        }

        private void HandleFunction()
        {
            Exploration enteredExpl = explorationProxy.FindNearbyExploration(Adam.Instance.WorldPosition);
            if (enteredExpl != null)
            {
                enteredExpl.OnFunction();
                explorationProxy.Claim(enteredExpl);
            }
        }
    }
}

