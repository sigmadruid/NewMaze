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

		private float delayTimer = 9999f;
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
        protected bool Guess(float possibility)
        {
            return UnityEngine.Random.value < possibility;
        }

        protected void Flee()
        {
            if(currentMonster.Info.CanMove())
            {
                Vector3 escapeDir = (currentMonster.WorldPosition - Adam.Instance.WorldPosition).normalized;
                currentMonster.MoveByDestination(currentMonster.WorldPosition + escapeDir);
            }
        }
        protected void SearchForHero()
        {
            if(currentMonster.Info.CanMove())
            {
                destPosition = Adam.Instance.WorldPosition;
                currentMonster.MoveByDestination(destPosition);
            }
        }
        protected bool CheckCollision(Vector3 direction, float distance)
        {
            //TODO: Make it more precise when the bullet is not a point.
            Vector3 origin = currentMonster.Script.EmitPosition;
            bool result = Physics.Raycast(origin, direction, distance, Layers.LayerBlock);
            return result;
        }

//        private void SearchForHeroByNode()
//		{
//            MazePosition pos = Maze.Instance.GetMazePosition(currentMonster.WorldPosition);
//            int monsterCol = pos.Col;
//            int monsterRow = pos.Row;
//			MazeNode currentNode = blockProxy.GetNode(monsterCol, monsterRow);
//            MazeData mazeData = MazeDataManager.Instance.CurrentMazeData;
//            Vector3 currentPosition = MazeUtil.GetWorldPosition(currentNode.Col, currentNode.Row, mazeData.BlockSize);
//
//            MazeNode nextNode = blockProxy.FindNextSearchNode(monsterCol, monsterRow);
//			if (nextNode != null)
//			{
//				if (nextNode == currentNode)
//				{
//                    SearchForHeroDirectly();
//				}
//				else
//				{
//                    Vector3 nextPosition = MazeUtil.GetWorldPosition(nextNode.Col, nextNode.Row, mazeData.BlockSize);
//                    if(CheckCollision(nextPosition))
//                    {
//                        currentMonster.Move(currentPosition - currentMonster.WorldPosition);
//                    }
//                    else
//                    {
//                        currentMonster.Move(nextPosition - currentMonster.WorldPosition);
//                    }
//				}
//			}
//			else
//			{
//				currentMonster.Move(Vector3.zero);
//			}
//		}
//
//        private void SearchForHeroDirectly()
//        {
//            currentMonster.Move(Adam.Instance.WorldPosition - currentMonster.WorldPosition);
//        }

	}
}

