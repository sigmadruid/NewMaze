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

		public bool IsConverting;
        public bool IsInHall;

		public float LastHitTime = -1000f;

		public HeroInfo (HeroData data) : base(data)
		{
			Data = data;
            InitSkillList();
		}

		public HeroInfo (HeroData data, HeroInfo info) : base(data)
		{
            Side = Side.Hero;
			Data = data;
			if (info != null)
			{
				hp = (int)(data.HP * info.HPRatio);
				IsConverting = info.IsConverting;
                IsInHall = info.IsInHall;
				LastHitTime = info.LastHitTime;
			}
            InitSkillList();
		}

        public HeroInfo (HeroData data, HeroRecord record) : base(data)
        {
            Data = data;
            hp = record.HP;
            IsInHall = record.IsInHall;
            InitSkillList();
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

