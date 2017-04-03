using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;

using Base;
using GameLogic;
using StaticData;

namespace Battle
{
	public class BattleProxy : Proxy
	{
        private Adam adam;
        private MonsterProxy monsterProxy;

        public void Init ()
		{
            monsterProxy = ApplicationFacade.Instance.RetrieveProxy<MonsterProxy>();
		}

        public void SetAdam(Adam adam)
		{
			this.adam = adam;
		}

		public void Dispose()
		{
			adam = null;
            monsterProxy = null;
		}

        public void AttackMonster(Skill skill)
		{
            AttackContext context = new AttackContext();
            context.CasterSide = Side.Hero;
            context.Attack = adam.Info.GetAttribute(BattleAttribute.Attack) * skill.Data.Ratio;
            context.Critical = (int)adam.Info.GetAttribute(BattleAttribute.Critical);

            if(adam.Data.AttackType == AttackType.Range)
            {
                Bullet bullet = Bullet.Create(skill.Data.BulletKid);
                bullet.AttackContext = context;
                bullet.Start(adam.Script.EmitPosition, monsterProxy.TargetMonster.Script.CenterPosition);
            }
            else if (adam.Data.AttackType == AttackType.Melee)
            {
                monsterProxy.IterateActives((Monster monster) =>
                    {
                        AreaData areaData = AreaDataManager.Instance.GetData(skill.Data.AreaKid) as AreaData;
                        bool inArea = JudgeInArea(adam.Script.transform, monster.Script.transform, areaData);
                        if(inArea)
                        {
                            DoAttackMonster(monster, context);
                        }
                    });
            }

		}

        public void AttackHero(Monster monster, Skill skill)
		{
			AttackContext ac = new AttackContext();
			ac.CasterSide = Side.Monster;
            ac.Attack = monster.Info.GetAttribute(BattleAttribute.Attack) * skill.Data.Ratio;
            ac.Critical = (int)monster.Info.GetAttribute(BattleAttribute.Critical);

			if (monster.Data.AttackType == AttackType.Range)
			{
                Bullet bullet = Bullet.Create(skill.Data.BulletKid);
				bullet.AttackContext = ac;
                bullet.Start(monster.Script.EmitPosition, adam.Script.CenterPosition);
			}
			else if (monster.Data.AttackType == AttackType.Melee)
			{
                AreaData areaData = AreaDataManager.Instance.GetData(skill.Data.AreaKid) as AreaData;
                bool inArea = JudgeInArea(adam.Script.transform, monster.Script.transform, areaData);
				if (inArea)
				{
					DoAttackHero(ac);
				}
			}
		}

		public void DoAttackHero(AttackContext attackContext)
		{
			if (adam.Info.IsConverting) return;

			AttackResult result = adam.Info.HurtBy(attackContext);
			if (adam.Info.HP > 0)
			{
				adam.Hit();
			}
			else
			{
				adam.Die();
			}

			DispatchNotification(NotificationEnum.BATTLE_UI_UPDATE_HP, result);
		}

        public void DoAttackMonster(Monster monster, AttackContext attackContext)
        {
            if (!monster.Info.IsAlive)
            {
                return;
            }
            AttackResult result = monster.Info.HurtBy(attackContext);
            NumberItem.Create(monster.Script.TopPosition, result);
            if (monster.Info.HP > 0)
            {
                monster.Hit();
            }
            else
            {
                monster.Die();
            }
        }

        private bool JudgeInArea(Transform attackerTrans, Transform defenderTrans, AreaData areaData)
		{
			bool inArea = false;
            AreaType areaType = areaData.AreaType;
			if (areaType == AreaType.Fan)
			{
                float range = areaData.Param1;
                float angle = areaData.Param2;
				inArea = MathUtils.FanContains(attackerTrans.position, attackerTrans.forward, defenderTrans.position, range, angle);
			}
			else if (areaType == AreaType.Rectangle)
			{
                float length = areaData.Param1;
                float width = areaData.Param2;
				inArea = MathUtils.RectContains(attackerTrans.position, attackerTrans.forward, width, length, defenderTrans.position);
			}
			return inArea;
		}
	}
}

