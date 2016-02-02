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
		private ExplorationProxy exploreProxy;
		
		public override void OnRegister ()
		{
			exploreProxy = ApplicationFacade.Instance.RetrieveProxy<ExplorationProxy>();
		}

		public override IList<Enum> ListNotificationInterests ()
		{
			return new Enum[]
			{
				NotificationEnum.EXPLORATION_SPAWN,
				NotificationEnum.EXPLORATION_DESPAWN,
			};
		}

		public override void HandleNotification (INotification notification)
		{
            switch((NotificationEnum)notification.NotifyEnum)
			{
				case NotificationEnum.EXPLORATION_SPAWN:
				{
					Block block = notification.Body as Block;
					HandleSpawn(block);
					break;
				}
				case NotificationEnum.EXPLORATION_DESPAWN:
				{
					Block block = notification.Body as Block;
					HandleDespawn(block);
					break;
				}
			}
		}

		private void HandleSpawn(Block block)
		{
			int explorationCount = RandomUtils.Range(0, MazeDataManager.Instance.CurrentMazeData.ExplorationMaxCount);
            PositionScript birth = null;

            if (block.ExplorationType != ExplorationType.Common)
            {
                birth = block.Script.GetGlobalPosition(BlockScript.PositionListType.ExplorationPositions);
                CreateExploration(block.ExplorationType, birth);
                explorationCount--;
            }

			for (int i = 0; i < explorationCount; ++i)
			{
				birth = block.Script.GetRandomPosition(BlockScript.PositionListType.ExplorationPositions);
                CreateExploration(ExplorationType.Common, birth);
			}
		}
		private void HandleDespawn(Block block)
		{
			List<Exploration> explorationList = exploreProxy.GetAll();
			float blockSize = MazeDataManager.Instance.CurrentMazeData.BlockSize;
			int count = explorationList.Count;
			for (int i = 0; i < count; ++i)
			{
				Exploration exploratioin = explorationList[i];
				int col, row;
				MazeUtil.GetMazePosition(exploratioin.WorldPosition, blockSize, out col, out row);
				if (block.Contains(col, row))
				{
					exploreProxy.Remove(exploratioin.Uid);
					Exploration.Recycle(exploratioin);
				}
			}
		}

        private void CreateExploration(ExplorationType type, PositionScript birth)
        {
            if (birth != null)
            {
                Exploration exploration = Exploration.Create(type);
                exploration.SetPosition(birth.transform.position);
                exploration.SetRotation(birth.transform.eulerAngles.y);
                exploreProxy.Add(exploration);
            }
        }
    }
}

