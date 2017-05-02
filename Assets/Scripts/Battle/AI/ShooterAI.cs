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
            base.SlowUpdate();

            if (!currentMonster.Info.IsAlive)
                return;
            if(currentMonster.Info.IsStunned)
                return;
            if(currentMonster.Info.CurrentSkill != null)
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

			//To near, run away
            if(heroSqrDistance < sqrAvoidDistance)
            {
//                if(Delay(currentData.DodgeDelay))
                {
                    Vector3 escapeDir = (currentMonster.WorldPosition - adam.WorldPosition).normalized;
                    currentMonster.Move(currentMonster.WorldPosition + escapeDir);
                }
            }
			//To far, run to the hero
            else if(heroSqrDistance > sqrAttackDistance || CheckCollision(true))
            {
                if(adam.Script != null)
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
                if (Delay(currentData.AttackDelay) && adam.Info.IsAlive)
				{
                    currentMonster.Idle();
					currentMonster.LookAt(adam.WorldPosition);
					currentMonster.Skill(1);
				}
				else
				{
                    currentMonster.Idle();
                    currentMonster.LookAt(adam.WorldPosition);
				}
			}
		}
	}
}

