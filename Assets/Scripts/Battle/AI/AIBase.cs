using UnityEngine;

using System;

using Base;
using StaticData;
using GameLogic;

namespace Battle
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

        public virtual void Start() {}

        public virtual void Update() {}

        public virtual void SlowUpdate() {}

        public virtual void End() {}

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

        protected void SearchForHero()
        {
            bool monsterInMaze = blockProxy.CheckInRange(currentMonster.WorldPosition);
            bool heroInMaze = blockProxy.CheckInRange(Hero.Instance.WorldPosition);
            if (heroInMaze && monsterInMaze)
            {
                SearchForHeroByNode();
            }
            else if (!heroInMaze && !monsterInMaze)
            {
                SearchForHeroDirectly();
            }
        }

        private void SearchForHeroByNode()
		{
            Vector2 pos = Maze.Instance.GetMazePosition(currentMonster.WorldPosition);
            int monsterCol = (int)pos.x;
            int monsterRow = (int)pos.y;
			MazeNode currentNode = blockProxy.GetNode(monsterCol, monsterRow);
            MazeData mazeData = MazeDataManager.Instance.CurrentMazeData;
            Vector3 currentPosition = MazeUtil.GetWorldPosition(currentNode.Col, currentNode.Row, mazeData.BlockSize);

            MazeNode nextNode = blockProxy.FindNextSearchNode(monsterCol, monsterRow);
			if (nextNode != null)
			{
				if (nextNode == currentNode)
				{
                    SearchForHeroDirectly();
				}
				else
				{
                    Vector3 nextPosition = MazeUtil.GetWorldPosition(nextNode.Col, nextNode.Row, mazeData.BlockSize);
                    if(CheckCollision(nextPosition))
                    {
                        currentMonster.Move(currentPosition - currentMonster.WorldPosition);
                    }
                    else
                    {
                        currentMonster.Move(nextPosition - currentMonster.WorldPosition);
                    }
				}
			}
			else
			{
				currentMonster.Move(Vector3.zero);
			}
		}

        private void SearchForHeroDirectly()
        {
            currentMonster.Move(Hero.Instance.WorldPosition - currentMonster.WorldPosition);
        }

        private bool CheckCollision(Vector3 nextPosition)
        {
            const float h = 0.3f;
            Vector3 origin = currentMonster.WorldPosition + Vector3.up * h;
            Vector3 direction = nextPosition - currentMonster.WorldPosition;
            bool result = Physics.Raycast(origin, direction, 2f, Layers.LayerBlock);
            return result;
        }
	}
}

