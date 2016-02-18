using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;

using Base;
using GameLogic;
using StaticData;

namespace Battle
{
	public enum BattleAttribute
	{
		HP,
		Attack,
		Defense,
		Critical,
		Dodge,
	}

	public enum Side
	{
		Hero,
		Monster,
	}

	public class BattleProxy : Proxy
	{
		private Hero hero;
		private Dictionary<string, Monster> monsterDic;

		public BattleProxy () : base()
		{
			monsterDic = new Dictionary<string, Monster>();
		}

		public void SetHero(Hero hero)
		{
			this.hero = hero;
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
			hero = null;
			monsterDic.Clear();
		}

		public void AttackMonster(Dictionary<AnimatorParamKey, int> paramDic)
		{
			Dictionary<string, Monster>.Enumerator enumerator = monsterDic.GetEnumerator();
			while (enumerator.MoveNext())
			{
				Monster monster = enumerator.Current.Value;
				bool inArea = JudgeInArea(hero.Script.transform, monster.Script.transform, paramDic);
				if (inArea)
				{
					if (!monster.Info.IsAlive)
					{
						continue;
					}

					AttackContext context = new AttackContext();
					context.Side = Side.Hero;
					context.Attack = hero.Info.GetAttribute(BattleAttribute.Attack);
					context.Critical = hero.Info.GetAttribute(BattleAttribute.Critical);

					AttackResult result = monster.Info.HurtBy(context);
					NumberItem.CreateDamageNumber(monster.WorldPosition, result);
					if (monster.Info.HP > 0)
					{
						monster.Hit();
					}
					else
					{
						monster.Die();
					}
				}
			}
		}

		public void AttackHero(Monster monster, Dictionary<AnimatorParamKey, int> paramDic)
		{
			AttackContext ac = new AttackContext();
			ac.Side = Side.Monster;
			ac.Attack = monster.Info.GetAttribute(BattleAttribute.Attack);
			ac.Critical = monster.Info.GetAttribute(BattleAttribute.Critical);

			if (monster.Data.AttackType == AttackType.Range)
			{
				Bullet bullet = Bullet.Create(monster.Data.BulletKid);
				bullet.AttackContext = ac;
				bullet.Start(monster.Script.EmitTransform);
			}
			else if (monster.Data.AttackType == AttackType.Melee)
			{
				bool inArea = JudgeInArea(hero.Script.transform, monster.Script.transform, paramDic);
				if (inArea)
				{
					DoAttackHero(ac);
				}
			}
		}

		public void DoAttackHero(AttackContext attackContext)
		{
			if (hero.Info.IsConverting) return;

			AttackResult result = hero.Info.HurtBy(attackContext);
			if (hero.Info.HP > 0)
			{
				hero.Hit();
			}
			else
			{
				hero.Die();
			}

			DispatchNotification(NotificationEnum.BATTLE_UI_UPDATE_HP, result);
		}

		public Monster GetNearMonsters()
		{
			float sqrRange = hero.Data.AttackRange * hero.Data.AttackRange;
			Dictionary<string, Monster>.Enumerator enumerator = monsterDic.GetEnumerator();
			while (enumerator.MoveNext())
			{
				Monster monster = enumerator.Current.Value;
				if (monster.Info.IsAlive && MathUtils.XZSqrDistance(monster.WorldPosition, hero.WorldPosition) < sqrRange)
				{
					return monster;
				}
			}
			return null;
		}

		private bool JudgeInArea(Transform attackerTrans, Transform defenderTrans, Dictionary<AnimatorParamKey, int> paramDic)
		{
			if (paramDic == null)
			{
				return false;
			}

			bool inArea = false;
			AreaType areaType = (AreaType)paramDic[AnimatorParamKey.AreaType];
			if (areaType == AreaType.Fan)
			{
				int range = paramDic[AnimatorParamKey.Range];
				int angle = paramDic[AnimatorParamKey.Angle];
				inArea = MathUtils.FanContains(attackerTrans.position, attackerTrans.forward, defenderTrans.position, range, angle);
			}
			else if (areaType == AreaType.Rectangle)
			{
				int length = paramDic[AnimatorParamKey.Range];
				int width = paramDic[AnimatorParamKey.Width];
				inArea = MathUtils.RectContains(attackerTrans.position, attackerTrans.forward, width, length, defenderTrans.position);
			}
			return inArea;
		}
	}
}

