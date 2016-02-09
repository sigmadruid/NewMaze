using System;
using UnityEngine;

using Base;
using GameLogic;

namespace AI
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

			if (heroSqrDistance > sqrDetectDistance)
			{
				currentMonster.Idle();
				return;
			}

			if (heroSqrDistance < sqrAttackDistance)
			{
				if (Delay(currentData.AttackDelay) && hero.Info.IsAlive)
				{
					currentMonster.Move(Vector3.zero);
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
				if (hero.Script != null)
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

