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

        public void AttackMonster(Dictionary<AnimatorParamKey, string> paramDic)
		{
			Dictionary<string, Monster>.Enumerator enumerator = monsterDic.GetEnumerator();
			while (enumerator.MoveNext())
			{
				Monster monster = enumerator.Current.Value;
				bool inArea = JudgeInArea(adam.Script.transform, monster.Script.transform, paramDic);
				if (inArea)
				{
                    AttackContext context = new AttackContext();
                    context.Side = Side.Hero;
                    context.Attack = (int)adam.Info.GetAttribute(BattleAttribute.Attack);
                    context.Critical = (int)adam.Info.GetAttribute(BattleAttribute.Critical);

                    DoAttackMonster(monster, context);
				}
			}
		}

        public void AttackHero(Monster monster, Dictionary<AnimatorParamKey, string> paramDic)
		{
			AttackContext ac = new AttackContext();
			ac.Side = Side.Monster;
            ac.Attack = (int)monster.Info.GetAttribute(BattleAttribute.Attack);
            ac.Critical = (int)monster.Info.GetAttribute(BattleAttribute.Critical);

			if (monster.Data.AttackType == AttackType.Range)
			{
				Bullet bullet = Bullet.Create(monster.Data.BulletKid);
				bullet.AttackContext = ac;
				bullet.Start(monster.Script.EmitTransform);
			}
			else if (monster.Data.AttackType == AttackType.Melee)
			{
				bool inArea = JudgeInArea(adam.Script.transform, monster.Script.transform, paramDic);
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
            NumberItem.Create(monster.WorldPosition, result);
            if (monster.Info.HP > 0)
            {
                monster.Hit();
            }
            else
            {
                monster.Die();
            }
        }

        private bool JudgeInArea(Transform attackerTrans, Transform defenderTrans, Dictionary<AnimatorParamKey, string> paramDic)
		{
            if (paramDic == null || paramDic.Count == 0)
			{
				return false;
			}

			bool inArea = false;
            AreaType areaType = (AreaType)Enum.Parse(typeof(AreaType), paramDic[AnimatorParamKey.AreaType]);
			if (areaType == AreaType.Fan)
			{
                int range = int.Parse(paramDic[AnimatorParamKey.Range]);
                int angle = int.Parse(paramDic[AnimatorParamKey.Angle]);
				inArea = MathUtils.FanContains(attackerTrans.position, attackerTrans.forward, defenderTrans.position, range, angle);
			}
			else if (areaType == AreaType.Rectangle)
			{
                int length = int.Parse(paramDic[AnimatorParamKey.Range]);
                int width = int.Parse(paramDic[AnimatorParamKey.Width]);
				inArea = MathUtils.RectContains(attackerTrans.position, attackerTrans.forward, width, length, defenderTrans.position);
			}
			return inArea;
		}
	}
}

