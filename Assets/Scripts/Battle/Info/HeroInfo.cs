using System;
using System.Collections.Generic;

using Base;
using GameLogic;
using StaticData;


namespace Battle
{
    //For heroes, they have seperate level, skills, but they share buffs. In other word, they are one!
	public class HeroInfo : CharacterInfo
	{
        public new HeroData Data
        {
            get { return data as HeroData; }
            set { data = value; }
        }

        protected static Dictionary<int, Buff> heroBuffDic = new Dictionary<int, Buff>();
        protected override Dictionary<int, Buff> buffDic
        {
            get
            {
                return heroBuffDic;
            }
            set
            {
                heroBuffDic = value;
            }
        }

        public int Level;
        public int Exp;

		public float LastHitTime;

        private Dictionary<int, float> attrRaiseDic = new Dictionary<int, float>();

        public HeroInfo (HeroData data) : base(data)
        {
            Side = Side.Hero;
            Data = data;
            InitRaise();
            InitSkillList();

            Level = GlobalConfig.DemoConfig.InitLevel;
            Exp = 0;
            HP = (int)(GetBaseAttribute(BattleAttribute.HP));
        }
        public HeroInfo (HeroData data, HeroRecord record) : base(data)
        {
            Data = data;
            HP = record.HP;
            Level = record.Level;
            Exp = record.Exp;

            InitRaise();
            InitSkillList();
        }

        public HeroRecord ToRecord()
        {
            HeroRecord record = new HeroRecord();
            record.Kid = Data.Kid;
            record.HP = HP;
            record.Level = Level;
            record.Exp = Exp;
            return record;
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
                && !ApplicationFacade.Instance.RetrieveProxy<AdamProxy>().IsConverting 
                && (CurrentSkill == null || CurrentSkill.Data.CanMove);
        }

        #region Skill

        public override bool CanCastSkill(int index)
        {
            if (ApplicationFacade.Instance.RetrieveProxy<AdamProxy>().IsConverting)
                return false;
            return base.CanCastSkill(index);
        }

        #endregion
	}
}

