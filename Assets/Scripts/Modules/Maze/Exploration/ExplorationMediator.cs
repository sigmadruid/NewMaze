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
        private ExplorationProxy explorationProxy;
		
		public override void OnRegister ()
		{
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
                case NotificationEnum.EXPLORATION_FUNCTION:
                {
                    HandleFunction();
                    break;
                }
			}
		}

		private void HandleBlockSpawn(Block block)
		{
			int explorationCount = RandomUtils.Range(0, MazeDataManager.Instance.CurrentMazeData.ExplorationMaxCount);
            PositionScript birth = null;

            if (block.ExplorationType != ExplorationType.Common)
            {
                birth = block.Script.GetGlobalPosition(PositionType.Exploration);
                List<object> paramList = new List<object>();
                paramList.Add(TransporterDirectionType.Forward);
                CreateExploration(block.ExplorationType, birth, paramList);
                explorationCount--;
            }

			for (int i = 0; i < explorationCount; ++i)
			{
				birth = block.Script.GetRandomPosition(PositionType.Exploration);
                CreateExploration(ExplorationType.Common, birth);
			}
		}
		private void HandleBlockDespawn(Block block)
		{
			List<Exploration> explorationList = explorationProxy.GetAll();
			float blockSize = MazeDataManager.Instance.CurrentMazeData.BlockSize;
			int count = explorationList.Count;
			for (int i = 0; i < count; ++i)
			{
				Exploration exploratioin = explorationList[i];
				int col, row;
				MazeUtil.GetMazePosition(exploratioin.WorldPosition, blockSize, out col, out row);
				if (block.Contains(col, row))
				{
					explorationProxy.Remove(exploratioin.Uid);
					Exploration.Recycle(exploratioin);
				}
			}
		}
        private void CreateExploration(ExplorationType type, PositionScript birth, List<object> paramList = null)
        {
            if (birth != null)
            {
                Exploration expl = explorationProxy.CreateExploration(type, paramList);
                expl.SetPosition(birth.transform.position);
                expl.SetRotation(birth.transform.eulerAngles.y);
                explorationProxy.Add(expl);
            }
        }

        private void HandleHallSpawn(Hall hall)
        {
        }
        private void HandleHallDespawn(Hall hall)
        {
        }

        private void HandleFunction()
        {
            Exploration enteredExpl = explorationProxy.FindNearbyExploration(Hero.Instance.WorldPosition);
            if (enteredExpl != null)
            {
                enteredExpl.OnFunction();
            }
        }
    }
}

