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
                CreateExploration(block.ExplorationType, birth);
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
            List<Exploration> toDeleteList = new List<Exploration>();
            explorationProxy.IterateInBlocks((Exploration expl) => 
                {
                    MazePosition pos = Maze.Instance.GetMazePosition(expl.WorldPosition);
                    if (block.Contains(pos.Col, pos.Row))
                    {
                        toDeleteList.Add(expl);
                    }
                });

            for (int i = 0; i < toDeleteList.Count; ++i)
            {
                Exploration expl = toDeleteList[i];
                explorationProxy.RemoveInBlock(expl.Uid);
                Exploration.Recycle(expl);
            }
		}
        private void CreateExploration(ExplorationType type, PositionScript birth)
        {
            if (birth != null)
            {
                Exploration expl = ExplorationFactory.Create(type);
                expl.SetPosition(birth.transform.position);
                expl.SetRotation(birth.transform.eulerAngles.y);
                explorationProxy.AddInBlock(expl);
            }
        }

        private void HandleHallSpawn(Hall hall)
        {
            PositionScript[] positionList = hall.Script.GetPositionList(PositionType.Exploration);
            for(int i = 0; i < positionList.Length; ++i)
            {
                PositionScript birth = positionList[i];
                CreateExploration(birth.Kid, birth);
            }
        }
        private void HandleHallDespawn(Hall hall)
        {
            explorationProxy.ClearInHall();
        }
        private void CreateExploration(int kid, PositionScript birth)
        {
            if (birth != null)
            {
                Exploration expl = ExplorationFactory.Create(kid);
                expl.SetPosition(birth.transform.position);
                expl.SetRotation(birth.transform.eulerAngles.y);
                explorationProxy.AddInHall(expl);
            }
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

