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
            List<Exploration> toDeleteList = new List<Exploration>();
            explorationProxy.IterateInBlocks((Exploration expl) => 
                {
                    Vector2 pos = Maze.Instance.GetMazePosition(expl.WorldPosition);
                    if (block.Contains((int)pos.x, (int)pos.y))
                    {
                        toDeleteList.Add(expl);
                    }
                });

            int blockKey = Block.GetBlockKey(block.Col, block.Row);

            for (int i = 0; i < toDeleteList.Count; ++i)
            {
                Exploration expl = toDeleteList[i];
                explorationProxy.RemoveInBlock(expl.Uid);
                Exploration.Recycle(expl);
            }
		}
        private void CreateExploration(ExplorationType type, PositionScript birth, List<object> paramList = null)
        {
            if (birth != null)
            {
                Exploration expl = ExplorationFactory.Create(type, paramList);
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
                List<object> paramList = new List<object>();//TODO: How to change this hard code?
                paramList.Add(TransporterDirectionType.Back);
                CreateExploration(birth.Kid, birth, paramList);
            }
        }
        private void HandleHallDespawn(Hall hall)
        {
            explorationProxy.IterateInHall((Exploration expl) =>
                {
                    explorationProxy.RemoveInHall(expl.Uid);
                });
            explorationProxy.ClearInHall();
        }
        private void CreateExploration(int kid, PositionScript birth, List<object> paramList = null)
        {
            if (birth != null)
            {
                Exploration expl = ExplorationFactory.Create(kid, paramList);
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

