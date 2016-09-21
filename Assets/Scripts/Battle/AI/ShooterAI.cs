using UnityEngine;

using System;

using Base;
using GameLogic;

namespace Battle
{
	public class ShooterAI : AIBase
	{
		private float sqrAvoidDistance;
		private float sqrAttackDistance;
		private float sqrDetectDistance;

		public ShooterAI (Monster monster) : base(monster)
		{
			sqrAttackDistance = currentData.AttackRange * currentData.AttackRange;
			sqrAvoidDistance = currentData.DodgeRange * currentData.DodgeRange;
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

			//To near, run away
			if (heroSqrDistance < sqrAvoidDistance)
			{
				if (Delay(currentData.DodgeDelay))
				{
					currentMonster.Move(currentMonster.WorldPosition - hero.WorldPosition);
				}
			}
			//To far, run to the hero
			else if (heroSqrDistance > sqrAttackDistance)
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
			else
			{
				if (Delay(currentData.AttackDelay) && hero.Info.IsAlive)
				{
					currentMonster.Move(Vector3.zero);
					currentMonster.LookAt(hero.WorldPosition);
					currentMonster.Attack();
				}
				else
				{
					currentMonster.LookAt(hero.WorldPosition);
					currentMonster.Idle();
				}
			}
		}
	}
}

