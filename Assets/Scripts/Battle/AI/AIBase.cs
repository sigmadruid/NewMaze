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
            if(currentMonster.Info.IsStunned)
                return;
            destPosition = Adam.Instance.WorldPosition;
            currentMonster.Move(destPosition);
        }
        protected bool CheckCollision(bool toAdam)
        {
            Vector3 monsterPos = MathUtils.XZDirection(currentMonster.WorldPosition);
            Vector3 adamPos = MathUtils.XZDirection(Adam.Instance.WorldPosition);

            Vector3 origin =  monsterPos + Vector3.up * currentMonster.Script.EmitPosition.y;
            Vector3 direction = Vector3.zero;
            float distance = 0f;
            if(toAdam)
            {
                direction = adamPos - monsterPos;
                distance = direction.magnitude;
            }
            else
            {
                direction = currentMonster.Script.transform.forward;
                SkillData skillData = SkillDataManager.Instance.GetData(currentData.SkillList[0]) as SkillData;
                distance = skillData.Range;
            }

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

