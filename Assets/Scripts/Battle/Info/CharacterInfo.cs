using UnityEngine;

using System;
using System.Collections.Generic;

using Base;
using StaticData;

namespace Battle
{
    public struct AttackContext
    {
        public Side Side;
        public int Attack;
        public int Critical;
    }

    public struct AttackResult
    {
        public int Damage;
        public bool IsCritical;
        public bool IsDodge;
    }

	public class CharacterInfo
	{
        protected Dictionary<int, int> attrDic = new Dictionary<int, int>();
        protected Dictionary<int, int> buffAttrDic = new Dictionary<int, int>();
		
		public CharacterData Data;
		
		public CharacterInfo (CharacterData data)
		{
			Data = data;
			
			Init();
		}
		
		public void Init()
		{
			attrDic.Clear();

			hp = Data.HP;

			attrDic.Add((int)BattleAttribute.HP, Data.HP);
			attrDic.Add((int)BattleAttribute.Attack, Data.Attack);
			attrDic.Add((int)BattleAttribute.Defense, Data.Defense);
			attrDic.Add((int)BattleAttribute.Critical, Data.Critical);
			attrDic.Add((int)BattleAttribute.Dodge, Data.Dodge);
			
			buffAttrDic.Add((int)BattleAttribute.HP, 0);
			buffAttrDic.Add((int)BattleAttribute.Attack, 0);
			buffAttrDic.Add((int)BattleAttribute.Defense, 0);
			buffAttrDic.Add((int)BattleAttribute.Critical, 0);
			buffAttrDic.Add((int)BattleAttribute.Dodge, 0);
		}
		public void Dispose()
		{
			attrDic.Clear();
			Data = null;
		}

		public int GetAttribute(BattleAttribute attribute)
		{
            int attrID = (int)attribute;
            if (!attrDic.ContainsKey(attrID))
			{
				BaseLogger.LogFormat("attributeDic has no attribute: {0}", attribute);
				return -1;
			}
            if (!buffAttrDic.ContainsKey(attrID))
			{
				BaseLogger.LogFormat("buffDic has no attribute: {0}", attribute);
				return -1;
			}
            return attrDic[attrID] + buffAttrDic[attrID];
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
				result.Damage = 0;
				result.IsCritical = false;
				result.IsDodge = true;
//				Debug.Log(string.Format("{0} dodged...", Data.Kid));
				return result;
			}

			float critical = attackContext.Critical / RandomUtils.RANDOM_BASE;
			result.IsCritical = randomValue <= critical;

			int attack = attackContext.Attack;
			int defense = GetAttribute(BattleAttribute.Defense);
			int criticalRatio = result.IsCritical ? 2 : 1;
			result.Damage = -(attack - defense) * criticalRatio;
			AddHP(result.Damage);
//			Debug.Log(string.Format("{0} was attacked. Damage = {1}", Data.Kid, result.damage));
//			Debug.Log(string.Format("{0}'s HP: {1}", Data.Kid, hp));
			return result;
		}
	}

}

