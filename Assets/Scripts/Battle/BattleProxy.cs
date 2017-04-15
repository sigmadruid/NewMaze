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

        public void AttackMonster(SkillEffect effect)
		{
            if(adam.Data.AttackType == AttackType.Range)
            {
                //The monster can be just killed by your bullet when you are preparing for the next shot
                if(Adam.Instance.TargetMonster == null)
                    return;
                Bullet bullet = Bullet.Create(effect.Data.BulletKid);
                bullet.SkillEffect = effect;
                bullet.Start(adam.Script.EmitPosition, Adam.Instance.TargetMonster.Script.CenterPosition);
            }
            else if (adam.Data.AttackType == AttackType.Melee)
            {
                monsterProxy.IterateActives((Monster monster) =>
                    {
                        AreaData areaData = AreaDataManager.Instance.GetData(effect.Data.AreaKid) as AreaData;
                        bool inArea = JudgeInArea(adam.Script.transform, monster.Script.transform, areaData);
                        if(inArea)
                        {
                            DoAttackMonster(monster, effect);
                        }
                    });
            }

		}

        public void AttackHero(Monster monster, SkillEffect effect)
		{
			if (monster.Data.AttackType == AttackType.Range)
			{
                Bullet bullet = Bullet.Create(effect.Data.BulletKid);
                bullet.SkillEffect = effect;
                bullet.Start(monster.Script.EmitPosition, adam.Script.CenterPosition);
			}
			else if (monster.Data.AttackType == AttackType.Melee)
			{
                AreaData areaData = AreaDataManager.Instance.GetData(effect.Data.AreaKid) as AreaData;
                bool inArea = JudgeInArea(adam.Script.transform, monster.Script.transform, areaData);
				if (inArea)
				{
                    DoAttackHero(effect);
				}
			}
		}

        public void DoAttackHero(SkillEffect attackContext)
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

        public void DoAttackMonster(Monster monster, SkillEffect attackContext)
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
                if(attackContext.Data != null)//Can be trap
                {
                    for(int i = 0; i < attackContext.Data.BuffKidList.Count; ++i)
                    {
                        monster.AddBuff(attackContext.Data.BuffKidList[i]);
                    }
                }
            }
            else
            {
                monster.Die();
                if(monster == adam.TargetMonster)
                {
                    adam.ClearTarget();
                }
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

