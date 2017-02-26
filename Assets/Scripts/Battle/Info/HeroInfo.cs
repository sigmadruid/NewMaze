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

        public List<Skill> SkillList = new List<Skill>();

        public bool CanMove = true;

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

        #region Skill

        private void InitSkillList()
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

        public bool CanCastSkill(int index)
        {
            if (!IsAlive || IsConverting)
                return false;

            Skill skill = SkillList[index];
            return skill.CD <= 0;
        }

        public void CastSkill(int index)
        {
            if(index < 0 || index >= SkillList.Count)
            {
                BaseLogger.LogFormat("skill index out of range: {0}", index);
            }
            Skill skill = SkillList[index];
            skill.Cast();
        }

        #endregion
	}
}

