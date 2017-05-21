using UnityEngine;

using System;
using System.Collections.Generic;

using Base;
using GameLogic;
using StaticData;

namespace Battle
{
    public struct AttackResult
    {
        public int Damage;
        public bool IsCritical;
        public bool IsDodge;
    }

    public class CharacterInfo : EntityInfo, ICharacterInfoAgent
	{
        protected Dictionary<int, float> attrDic = new Dictionary<int, float>();
        protected Dictionary<int, Buff> selfBuffDic = new Dictionary<int, Buff>();
        protected virtual Dictionary<int, Buff> buffDic
        {
            get { return selfBuffDic; }
            set { selfBuffDic = value; }
        }

        public Side Side;
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

            HP = Data.HP;

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
            buffDic.Clear();
			Data = null;
		}

        #region State

        public bool IsStunned;

        #endregion

        #region Attribute

        public int HP { get; protected set; }
        public float HPRatio { get { return HP * 1f / Data.HP; } }

        public bool IsAlive { get{ return HP > 0; } }

        public Side GetSide()
        {
            return Side;
        }

        public virtual float GetBaseAttribute(BattleAttribute attribute)
        {
            int attrID = (int)attribute;
            if (!attrDic.ContainsKey(attrID))
            {
                BaseLogger.LogFormat("attributeDic has no attribute: {0}", attribute);
                return -1;
            }
            return attrDic[attrID];
        }

        public float GetAttribute(BattleAttribute attribute)
        {
            int attrID = (int)attribute;
            if (!attrDic.ContainsKey(attrID))
            {
                BaseLogger.LogFormat("attributeDic has no attribute: {0}", attribute);
                return -1;
            }
            float baseVal = GetBaseAttribute(attribute);
            float ratio = GetBuffRatioAttribute(attribute);
            int raise = GetBuffRaiseAttribute(attribute);
            return baseVal * ratio + raise;
        }

		public void AddHP(int value)
		{
            int maxHP = (int)GetAttribute(BattleAttribute.HP);
            HP = Mathf.Clamp(HP + value, 0, maxHP);
		}

        public AttackResult HurtBy(SkillEffect skillEffect)
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

			float critical = skillEffect.Critical / RandomUtils.RANDOM_BASE;
			result.IsCritical = randomValue <= critical;

            int attack = (int)skillEffect.Attack;
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
            var attrEnum = buffDic.GetEnumerator();
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
            var attrEnum = buffDic.GetEnumerator();
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
            var enumerator = buffDic.GetEnumerator();
            while (enumerator.MoveNext())
            {
                Buff buff = enumerator.Current.Value;
                if(buff.RemainTime > 0)
                {
                    buff.Update(deltaTime);
                    if(buff.RemainTime <= 0)
                    {
                        buff.End();
                    }
                }
            }
        }
        public void AddBuff(Buff buff)
        {
            buffDic[buff.Data.Kid] = buff;
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
            var enumerator = buffDic.GetEnumerator();
            while (enumerator.MoveNext())
            {
                Buff buff = enumerator.Current.Value;
                if (buff.RemainTime > 0)
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
            if(index < 1 || index > SkillList.Count)
            {
                BaseLogger.LogFormat("skill index out of range: {0}", index);
            }
            Skill skill = SkillList[index - 1];
            return skill;
        }

        public virtual bool CanCastSkill(int index)
        {
            if (!IsAlive)
                return false;

            Skill skill = SkillList[index - 1];
            return skill.CD <= 0 && CurrentSkill == null;
        }

        #endregion
	}

}

