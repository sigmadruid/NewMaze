using UnityEngine;

using System;

using Base;
using GameLogic;

namespace Battle
{
	public class ShooterAI : AIBase
	{
		private float sqrDodgeDistance;
		private float sqrAttackDistance;
		private float sqrDetectDistance;

		public ShooterAI (Monster monster) : base(monster)
		{
			sqrAttackDistance = currentData.AttackRange * currentData.AttackRange;
            sqrDodgeDistance = currentData.DodgeRange * currentData.DodgeRange;
			sqrDetectDistance = currentData.DetectRange * currentData.DetectRange;
		}

		public override void SlowUpdate ()
		{
            base.SlowUpdate();

            if(!currentMonster.IsActive)
                return;
            if (!currentMonster.Info.IsAlive)
                return;
            if(currentMonster.Info.IsStunned)
                return;
            if(currentMonster.Info.CurrentSkill != null)
                return;

            Adam adam = Adam.Instance;
			float heroSqrDistance = MathUtils.XZSqrDistance(adam.WorldPosition, currentMonster.WorldPosition);
            Vector3 heroDir = adam.WorldPosition - currentMonster.WorldPosition;

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
            if(heroSqrDistance < sqrDodgeDistance && Delay(currentData.DodgeDelay))
            {
                Flee();
            }
			//To far, run to the hero
            else if(heroSqrDistance > sqrAttackDistance || CheckCollision(heroDir, heroDir.magnitude))
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

