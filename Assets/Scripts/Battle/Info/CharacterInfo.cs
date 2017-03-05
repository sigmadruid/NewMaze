using UnityEngine;

using System;
using System.Collections.Generic;

using Base;
using GameLogic;
using StaticData;

namespace Battle
{
    public struct AttackContext
    {
        public Side CasterSide;
        public int Attack;
        public int Critical;
    }

    public struct AttackResult
    {
        public int Damage;
        public bool IsCritical;
        public bool IsDodge;
    }

    public class CharacterInfo : EntityInfo
	{
        protected Dictionary<int, float> attrDic = new Dictionary<int, float>();
        protected Dictionary<int, Buff> buffDic = new Dictionary<int, Buff>();

        public List<Skill> SkillList = new List<Skill>();
        public Skill CurrentSkill;
		
        public new CharacterData Data
        {
            get { return data as CharacterData; }
            set { data = value; }
        }
		
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
            attrDic.Add((int)BattleAttribute.MoveSpeed, Data.MoveSpeed);
            attrDic.Add((int)BattleAttribute.AttackSpeed, Data.AttackSpeed);
		}
		public void Dispose()
		{
			attrDic.Clear();
			Data = null;
		}

        #region Attribute

		protected int hp;
		public int HP { get{ return hp; }}
		public float HPRatio { get { return hp * 1f / Data.HP; } }

		public bool IsAlive { get{ return hp > 0; } }

        public float GetAttribute(BattleAttribute attribute)
        {
            int attrID = (int)attribute;
            if (!attrDic.ContainsKey(attrID))
            {
                BaseLogger.LogFormat("attributeDic has no attribute: {0}", attribute);
                return -1;
            }
            float ratio = GetBuffRatioAttribute(attribute);
            int raise = GetBuffRaiseAttribute(attribute);
            return attrDic[attrID] * ratio + raise;
        }

		public void AddHP(int value)
		{
            int maxHP = (int)GetAttribute(BattleAttribute.HP);
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
            int defense = (int)GetAttribute(BattleAttribute.Defense);
			int criticalRatio = result.IsCritical ? 2 : 1;
			result.Damage = -(attack - defense) * criticalRatio;
			AddHP(result.Damage);
//			Debug.Log(string.Format("{0} was attacked. Damage = {1}", Data.Kid, result.damage));
//			Debug.Log(string.Format("{0}'s HP: {1}", Data.Kid, hp));
			return result;
		}

        private float GetBuffRatioAttribute(BattleAttribute attribute)
        {
            Dictionary<int, Buff>.Enumerator attrEnum = buffDic.GetEnumerator();
            float resultVal = 1f;
            while(attrEnum.MoveNext())
            {
                float attrVal = attrEnum.Current.Value.GetAttributeRatio(attribute);
                resultVal *= attrVal;
            }
            return resultVal;
        }
        private int GetBuffRaiseAttribute(BattleAttribute attribute)
        {
            Dictionary<int, Buff>.Enumerator attrEnum = buffDic.GetEnumerator();
            int resultVal = 0;
            while(attrEnum.MoveNext())
            {
                int attrVal = attrEnum.Current.Value.GetAttributeRaise(attribute);
                resultVal += attrVal;
            }
            return resultVal;
        }

        #endregion

        #region Buff

        public Buff GetBuff(int kid)
        {
            if(buffDic.ContainsKey(kid))
            {
                return buffDic[kid];
            }
            return null;
        }
        public void UpdateBuff(float deltaTime)
        {
            Dictionary<int, Buff>.Enumerator enumerator = buffDic.GetEnumerator();
            while (enumerator.MoveNext())
            {
                Buff buff = enumerator.Current.Value;
                buff.Update(deltaTime);
            }
        }
        public void AddBuff(Buff buff)
        {
            if(!buffDic.ContainsKey(buff.Data.Kid))
            {
                buffDic.Add(buff.Data.Kid, buff);
            }
        }
        public void RemoveBuff(int kid)
        {
            if(buffDic.ContainsKey(kid))
            {
                buffDic.Remove(kid);
            }
        }

        public Dictionary<int, float> RecordBuff()
        {
            Dictionary<int, float> recordDic = new Dictionary<int, float>();
            Dictionary<int, Buff>.Enumerator enumerator = buffDic.GetEnumerator();
            while (enumerator.MoveNext())
            {
                Buff buff = enumerator.Current.Value;
                recordDic.Add(buff.Data.Kid, buff.RemainTime);
            }
            return recordDic;
        }

        #endregion

        #region Skill

        protected void InitSkillList()
        {
            for(int i = 0; i < Data.SkillList.Count; ++i)
            {
                int skillKid = Data.SkillList[i];
                Skill skill = Skill.Create(skillKid, 0);
                SkillList.Add(skill);
            }
        }

        public void UpdateSkill(float deltaTime)
        {
            for(int i = 0; i < SkillList.Count; ++i)
            {
                Skill skill = SkillList[i];
                if(skill.CD > 0)
                {
                    skill.CD -= deltaTime;
                }
            }
        }

        public Skill GetSkill(int index)
        {
            if(index < 0 || index >= SkillList.Count)
            {
                BaseLogger.LogFormat("skill index out of range: {0}", index);
            }
            Skill skill = SkillList[index];
            return skill;
        }

        public virtual bool CanCastSkill(int index)
        {
            if (!IsAlive)
                return false;

            Skill skill = SkillList[index];
            return skill.CD <= 0;
        }

        #endregion
	}

}

