using UnityEngine;

using System;
using System.Collections.Generic;

using Base;
using StaticData;

namespace GameLogic
{
	public class CharacterInfo
	{
		protected Dictionary<int, int> attributeDic;
		protected Dictionary<int, int> buffDic;
		
		public CharacterData Data;
		
		public CharacterInfo (CharacterData data)
		{
			Data = data;
			attributeDic = new Dictionary<int, int>();
			buffDic = new Dictionary<int, int>();
			
			Init();
		}
		
		public void Init()
		{
			attributeDic.Clear();

			hp = Data.HP;

			attributeDic.Add((int)BattleAttribute.HP, Data.HP);
			attributeDic.Add((int)BattleAttribute.Attack, Data.Attack);
			attributeDic.Add((int)BattleAttribute.Defense, Data.Defense);
			attributeDic.Add((int)BattleAttribute.Critical, Data.Critical);
			attributeDic.Add((int)BattleAttribute.Dodge, Data.Dodge);
			
			buffDic.Add((int)BattleAttribute.HP, 0);
			buffDic.Add((int)BattleAttribute.Attack, 0);
			buffDic.Add((int)BattleAttribute.Defense, 0);
			buffDic.Add((int)BattleAttribute.Critical, 0);
			buffDic.Add((int)BattleAttribute.Dodge, 0);
		}
		public void Dispose()
		{
			attributeDic.Clear();
			Data = null;
		}

		//TO OPTIMIZE: 288B garbage per frame. Do not use hash to retrive. Try to cache the values.
		public int GetAttribute(BattleAttribute attribute)
		{
			if (!attributeDic.ContainsKey((int)attribute))
			{
				BaseLogger.LogFormat("attributeDic has no attribute: {0}", attribute);
				return -1;
			}
			if (!buffDic.ContainsKey((int)attribute))
			{
				BaseLogger.LogFormat("buffDic has no attribute: {0}", attribute);
				return -1;
			}
			return attributeDic[(int)attribute] + buffDic[(int)attribute];
		}

		protected int hp;
		public int HP { get{ return hp; }}
		public float HPRatio { get { return hp * 1f / Data.HP; } }

		public bool IsAlive { get{ return hp > 0; } }

		public void AddHP(int value)
		{
			int maxHP = GetAttribute(BattleAttribute.HP);
			hp = Mathf.Clamp(hp + value, 0, maxHP);
		}

		public AttackResult HurtBy(AttackContext attackContext)
		{
			float randomValue = UnityEngine.Random.value;
			AttackResult result = new AttackResult();

			float dodge = GetAttribute(BattleAttribute.Dodge) / RandomUtils.RANDOM_BASE;
			if (randomValue <= dodge)
			{
				result.damage = 0;
				result.isCritical = false;
				result.isDodge = true;
//				Debug.Log(string.Format("{0} dodged...", Data.Kid));
				return result;
			}

			float critical = attackContext.Critical / RandomUtils.RANDOM_BASE;
			result.isCritical = randomValue <= critical;

			int attack = attackContext.Attack;
			int defense = GetAttribute(BattleAttribute.Defense);
			int criticalRatio = result.isCritical ? 2 : 1;
			result.damage = -(attack - defense) * criticalRatio;
			AddHP(result.damage);
//			Debug.Log(string.Format("{0} was attacked. Damage = {1}", Data.Kid, result.damage));
//			Debug.Log(string.Format("{0}'s HP: {1}", Data.Kid, hp));
			return result;
		}
	}

	public struct AttackContext
	{
		public Side Side;
		public int Attack;
		public int Critical;
	}

	public struct AttackResult
	{
		public int damage;
		public bool isCritical;
		public bool isDodge;
	}
}

