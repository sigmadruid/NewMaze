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

            attrDic[(int)BattleAttribute.HP] = Data.HP;
            attrDic[(int)BattleAttribute.SP] = Data.SP;
            attrDic[(int)BattleAttribute.Attack] = Data.Attack;
            attrDic[(int)BattleAttribute.Defense] = Data.Defense;
            attrDic[(int)BattleAttribute.Critical] = Data.Critical;
            attrDic[(int)BattleAttribute.Dodge] = Data.Dodge;
            attrDic[(int)BattleAttribute.MoveSpeed] = Data.MoveSpeed;
            attrDic[(int)BattleAttribute.AttackSpeed] = Data.AttackSpeed;

            HP = Data.HP;
            SP = Data.SP;
		}
		
        public virtual void Init()
		{
		}
        public virtual void Dispose()
		{
            attrDic.Clear();
            buffDic.Clear();
			Data = null;
		}

        #region State

        public bool IsStunned;
        public bool IsRolling;

        #endregion

        #region Attribute

        public int HP { get; protected set; }
        public int SP { get; protected set; }

        public float HPRatio { get { return HP * 1f / GetAttribute(BattleAttribute.HP); } }

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
        public void AddSP(int value)
        {
            int maxSP = (int)GetAttribute(BattleAttribute.SP);
            SP = Mathf.Clamp(SP + value, 0, maxSP);
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

        public AttackResult HealBy(int hp)
        {
            AddHP(hp);

            AttackResult result = new AttackResult();
            result.Damage = hp;
            result.IsCritical = false;
            result.IsDodge = false;
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

        public void UpdateSP(float deltaTime)
        {
            int maxSP = (int)GetAttribute(BattleAttribute.SP);
            if(SP < maxSP)
            {
                int deltaSP = (int)(maxSP * GlobalConfig.BattleConfig.SPRecoverRatioPerSecond * deltaTime);
                AddSP(deltaSP);
            }
        }

        #endregion

        #region Buff

        private List<Buff> toRemoveBuffList = new List<Buff>();

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
            if(toRemoveBuffList.Count > 0)
                toRemoveBuffList.Clear();
            
            var enumerator = buffDic.GetEnumerator();
            while (enumerator.MoveNext())
            {
                Buff buff = enumerator.Current.Value;
                if(buff.RemainTime > 0)
                {
                    buff.Update(deltaTime);
                    if(buff.RemainTime <= 0)
                    {
                        toRemoveBuffList.Add(buff);
                        buff.End();
                    }
                }
            }

            for(int i = 0; i < toRemoveBuffList.Count; ++i)
            {
                buffDic.Remove(toRemoveBuffList[i].Data.Kid);
            }
        }
        public void AddBuff(Buff buff)
        {
            if(buff != null)
            {
                buffDic[buff.Data.Kid] = buff;
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
            SkillList.Clear();
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
                return null;
            }
            Skill skill = SkillList[index - 1];
            return skill;
        }

        public virtual bool CanCastSkill(int index)
        {
            if (!IsAlive)
                return false;

            Skill skill = SkillList[index - 1];
            return !IsStunned && skill.CD <= 0 && CurrentSkill == null;
        }

        #endregion

        public virtual bool CanMove()
        {
            return IsAlive
                && !IsStunned
                && (CurrentSkill == null || CurrentSkill.Data.CanMove);
        }

	}

}

