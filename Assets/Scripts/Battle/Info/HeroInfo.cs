using System;
using System.Collections.Generic;

using Base;
using GameLogic;
using StaticData;


namespace Battle
{
	public class HeroInfo : CharacterInfo
	{
        public new HeroData Data
        {
            get { return data as HeroData; }
            set { data = value; }
        }

        public int Level;
        public int Exp;

		public bool IsConverting;
        public bool IsInHall;

		public float LastHitTime = -1000f;

        private Dictionary<int, float> attrRaiseDic = new Dictionary<int, float>();

		public HeroInfo (HeroData data, HeroInfo info) : base(data)
		{
            Side = Side.Hero;
			Data = data;
			if (info != null)
			{
                HP = (int)(data.HP * info.HPRatio);
                Level = info.Level;
                Exp = info.Exp;
				IsConverting = info.IsConverting;
                IsInHall = info.IsInHall;
				LastHitTime = info.LastHitTime;
			}

            InitRaise();
            InitSkillList();
		}
        public HeroInfo (HeroData data, HeroRecord record) : base(data)
        {
            Data = data;
            HP = record.HP;
            Level = record.Level;
            Exp = record.Exp;

            IsInHall = record.IsInHall;

            InitRaise();
            InitSkillList();
        }

        public void InitRaise()
        {
            attrRaiseDic.Add((int)BattleAttribute.HP, Data.HPRaise);
            attrRaiseDic.Add((int)BattleAttribute.Attack, Data.AttackRaise);
            attrRaiseDic.Add((int)BattleAttribute.Defense, Data.DefenseRaise);
            attrRaiseDic.Add((int)BattleAttribute.Critical, Data.CriticalRaise);
            attrRaiseDic.Add((int)BattleAttribute.Dodge, Data.DodgeRaise);
            attrRaiseDic.Add((int)BattleAttribute.MoveSpeed, Data.MoveSpeedRaise);
            attrRaiseDic.Add((int)BattleAttribute.AttackSpeed, Data.AttackSpeedRaise);
        }

        public override float GetBaseAttribute(BattleAttribute attribute)
        {
            int attrID = (int)attribute;
            if (!attrDic.ContainsKey(attrID))
            {
                BaseLogger.LogFormat("attributeDic has no attribute: {0}", attribute);
                return -1;
            }
            return attrDic[attrID] + attrRaiseDic[attrID] * (Level - 1);
        }

        public bool CanMove()
        {
            return IsAlive 
                && !IsConverting 
                && (CurrentSkill == null || CurrentSkill.Data.CanMove);
        }

        #region Skill

        public override bool CanCastSkill(int index)
        {
            if (IsConverting)
                return false;
            return base.CanCastSkill(index);
        }

        #endregion
	}
}

