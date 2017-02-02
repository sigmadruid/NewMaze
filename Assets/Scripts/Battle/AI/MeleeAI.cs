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
			if (!currentMonster.Info.IsAlive)
			{
				return;
			}

			base.SlowUpdate();

			Hero hero = Hero.Instance;
			float heroSqrDistance = MathUtils.XZSqrDistance(hero.WorldPosition, currentMonster.WorldPosition);

            if(!hero.CanBeAttacked)
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
				if (Delay(currentData.AttackDelay) && hero.Info.IsAlive)
				{
                    currentMonster.Idle();
                    currentMonster.LookAt(hero.WorldPosition);
					currentMonster.Attack();
				}
				else
				{
					currentMonster.Idle();
				}
				
			}
			else
			{
                if (hero.Info.IsAlive)
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

