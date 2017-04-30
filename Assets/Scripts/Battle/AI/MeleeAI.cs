using System;
using UnityEngine;

using Base;
using GameLogic;

namespace Battle
{
	public class MeleeAI : AIBase
	{
		private float sqrAttackDistance;
		private float sqrDetectDistance;
		
		public MeleeAI (Monster monster) : base(monster)
		{
			sqrAttackDistance = currentData.AttackRange * currentData.AttackRange;
			sqrDetectDistance = currentData.DetectRange * currentData.DetectRange;
		}

		public override void SlowUpdate ()
		{
			base.SlowUpdate();

            if(!currentMonster.Info.IsAlive)
                return;
            if(currentMonster.Info.IsStunned)
                return;

            Adam adam = Adam.Instance;
			float heroSqrDistance = MathUtils.XZSqrDistance(adam.WorldPosition, currentMonster.WorldPosition);

            if(!adam.CanBeAttacked)
            {
                currentMonster.Idle();
                return;
            }

            if (heroSqrDistance > sqrDetectDistance)
			{
				currentMonster.Idle();
				return;
			}

			if (heroSqrDistance < sqrAttackDistance)
			{
				if (Delay(currentData.AttackDelay) && adam.Info.IsAlive)
				{
                    currentMonster.Idle();
                    currentMonster.LookAt(adam.WorldPosition);
					currentMonster.Skill(0);
				}
				else
				{
					currentMonster.Idle();
				}
				
			}
			else
			{
                if (adam.Info.IsAlive)
				{
                    SearchForHero();
				}
				else
				{
					currentMonster.Idle();
				}
			}
		}
	}
}

