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
		public void Dispose()
		{
			adam = null;
            monsterProxy = null;
		}

        public void SetAdam(Adam adam)
        {
            this.adam = adam;
        }

        public void AttackMonster(SkillEffect effect)
		{
            if(adam.Data.AttackType == AttackType.Range)
            {
                //The monster can be just killed by your bullet when you are preparing for the next shot
//                if(Adam.Instance.TargetMonster == null)
//                    return;
                Bullet bullet = Bullet.Create(effect.Data.BulletKid);
                bullet.SkillEffect = effect;
                bullet.Start(adam.WorldPosition + adam.Script.EmitPosition, adam.TargetPosition);
            }
            else if (adam.Data.AttackType == AttackType.Melee)
            {
                monsterProxy.Foreach((Monster monster) =>
                    {
                        AreaData areaData = AreaDataManager.Instance.GetData(effect.Data.AreaKid) as AreaData;
                        float adamRadius = GlobalConfig.HeroConfig.AdamRadius;
                        float monsterRadius = MonsterProxy.GetMonsterRadius(monster.Data.Size);
                        bool inArea = JudgeInArea(adam.Script.transform, adamRadius, monster.Script.transform, monsterRadius, areaData);
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
                bullet.Start(monster.WorldPosition + monster.Script.EmitPosition, adam.WorldPosition + adam.Script.CenterPosition);
			}
			else if (monster.Data.AttackType == AttackType.Melee)
			{
                AreaData areaData = AreaDataManager.Instance.GetData(effect.Data.AreaKid) as AreaData;
                float adamRadius = GlobalConfig.HeroConfig.AdamRadius;
                float monsterRadius = MonsterProxy.GetMonsterRadius(monster.Data.Size);
                bool inArea = JudgeInArea(monster.Script.transform, monsterRadius, adam.Script.transform, adamRadius, areaData);
				if (inArea)
				{
                    DoAttackHero(effect);
				}
			}
		}

        public void DoAttackHero(SkillEffect attackContext)
		{
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
            NumberItem.Create(monster.WorldPosition + monster.Script.TopPosition, result);
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
//                if(monster == adam.TargetMonster)
//                {
//                    adam.ClearTarget();
//                }
                adam.Info.AddExp(monster.Data.Exp);
                Debug.LogErrorFormat("lv.{0}, exp:{1}", adam.Info.Level, adam.Info.Exp);
            }
        }

        public void DoHealHero(int hp)
        {
            AttackResult result = adam.Info.HealBy(hp);
            DispatchNotification(NotificationEnum.BATTLE_UI_UPDATE_HP, result);

        }

        private bool JudgeInArea(Transform attackerTrans, float attackerRadius, Transform defenderTrans, float defenderRadius, AreaData areaData)
		{
			bool inArea = false;
            AreaType areaType = areaData.AreaType;
            if(areaType == AreaType.Circle)
            {
                Vector3 basePosition = Vector3.zero;
                if(areaData.CenterBased)
                {
                    basePosition = MathUtils.XZDirection(attackerTrans.position);
                }
                else
                {
                    basePosition = MathUtils.XZDirection(attackerTrans.position) + MathUtils.XZDirection(attackerTrans.forward) * attackerRadius;
                }
                float radius = attackerRadius + areaData.Param1 + defenderRadius;
                Vector3 testPosition = MathUtils.XZDirection(defenderTrans.position);
                inArea = MathUtils.CircleContains(basePosition, radius, testPosition);
            }
			if (areaType == AreaType.Fan)
			{
                Vector3 basePosition = Vector3.zero;
                Vector3 direction = MathUtils.XZDirection(attackerTrans.forward);
                if(areaData.CenterBased)
                {
                    basePosition = MathUtils.XZDirection(attackerTrans.position);
                }
                else
                {
                    basePosition = MathUtils.XZDirection(attackerTrans.position) + direction * attackerRadius;
                }
                float range = attackerRadius + areaData.Param1 + defenderRadius;
                float angle = areaData.Param2;
                Vector3 testPosition = MathUtils.XZDirection(defenderTrans.position);
                inArea = MathUtils.FanContains(basePosition, direction, range, angle, testPosition);
			}
//			else if (areaType == AreaType.Rect)
//			{
//                float length = areaData.Param1;
//                float width = areaData.Param2;
//				inArea = MathUtils.RectContains(attackerTrans.position, attackerTrans.forward, width, length, defenderTrans.position);
//			}
			return inArea;
		}
	}
}

