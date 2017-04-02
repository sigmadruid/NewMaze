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
		private Dictionary<string, Monster> monsterDic;

		public BattleProxy () : base()
		{
			monsterDic = new Dictionary<string, Monster>();
		}

        public void SetAdam(Adam adam)
		{
			this.adam = adam;
		}

		public void AddMonster(Monster monster)
		{
			if (!monsterDic.ContainsKey(monster.Uid))
			{
				monsterDic.Add(monster.Uid, monster);
			}
		}
		public void RemoveMonster(string uid)
		{
			if (monsterDic.ContainsKey(uid))
			{
				monsterDic.Remove(uid);
			}
		}
		public void Dispose()
		{
			adam = null;
			monsterDic.Clear();
		}

        //TODO: set skill instead of paramDic
        public void AttackMonster(Skill skill)
		{
            AttackContext context = new AttackContext();
            context.CasterSide = Side.Hero;
            context.Attack = adam.Info.GetAttribute(BattleAttribute.Attack) * skill.Data.Ratio;
            context.Critical = (int)adam.Info.GetAttribute(BattleAttribute.Critical);

			Dictionary<string, Monster>.Enumerator enumerator = monsterDic.GetEnumerator();
            if(adam.Data.AttackType == AttackType.Range)
            {
                Bullet bullet = Bullet.Create(skill.Data.BulletKid);
                bullet.AttackContext = context;
                bullet.Start(adam.Script.EmitTransform);

            }
            else if (adam.Data.AttackType == AttackType.Melee)
            {
                while(enumerator.MoveNext())
                {
                    Monster monster = enumerator.Current.Value;
                    AreaData areaData = AreaDataManager.Instance.GetData(skill.Data.AreaKid) as AreaData;
                    bool inArea = JudgeInArea(adam.Script.transform, monster.Script.transform, areaData);
                    if (inArea)
                    {
                        DoAttackMonster(monster, context);
                    }
                }
            }

		}

        //TODO: set skill instead of paramDic
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
				bullet.Start(monster.Script.EmitTransform);
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
            NumberItem.Create(monster.Script.TopPosTransform.position, result);
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

