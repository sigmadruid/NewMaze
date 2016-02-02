using UnityEngine;

using System;

using Base;
using StaticData;
using GameLogic;

namespace AI
{
	public class AIBase
	{
		protected Monster currentMonster;
		protected MonsterData currentData;
		
		public string Uid { get; private set; }

		private Transform destTransform;
		private Vector3 destPosition;

		protected BlockProxy blockProxy;

		public AIBase (Monster monster)
		{
			currentMonster = monster;
			currentData = monster.Data;

			Uid = monster.Uid;

			blockProxy = ApplicationFacade.Instance.RetrieveProxy<BlockProxy>();
		}

		public virtual void Start()
		{
		}

		public virtual void Update()
		{
		}

		public virtual void SlowUpdate()
		{
		}

		public virtual void End()
		{
		}

		private float delayTimer = 0f;
		protected bool Delay(float delay)
		{
			if (delayTimer < delay)
			{
				delayTimer += AICore.AI_UPDATE_INTERVAL;
				return false;
			}
			else
			{
				delayTimer = 0f;
				return true;
			}
		}

		protected void SearchForHero(Vector3 offset)
		{
			Vector2 pos = Maze.Instance.GetMazePosition(currentMonster.WorldPosition);
			int col = (int)pos.x;
			int row = (int)pos.y;
			MazeNode currentNode = blockProxy.GetNode(col, row);
			MazeNode nextNode = blockProxy.FindNextSearchNode(col, row);

			if (nextNode != null)
			{
				if (nextNode == currentNode)
				{
					currentMonster.Move(Hero.Instance.WorldPosition - currentMonster.WorldPosition);
				}
				else
				{
					MazeData mazeData = MazeDataManager.Instance.CurrentMazeData;
					Vector3 nextPosition = MazeUtil.GetWorldPosition(nextNode.Col, nextNode.Row, mazeData.BlockSize);
					currentMonster.Move(nextPosition - currentMonster.WorldPosition);
				}
			}
			else
			{
				currentMonster.Move(Vector3.zero);
			}
		}

	}
}

