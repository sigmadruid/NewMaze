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
        public bool IsVisible;
        public float LastHitTime;

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
        private Dictionary<int, float> attrRaiseDic = new Dictionary<int, float>();

        public HeroInfo (HeroData data) : base(data)
        {
            Side = Side.Hero;
            Convert(data);
        }

        public override void Init()
        {
            base.Init();

            Level = GlobalConfig.DemoConfig.InitLevel;
            Exp = 0;
            HP = (int)(GetBaseAttribute(BattleAttribute.HP));
            IsConverting = false;
            IsInHall = false;
            IsVisible = true;
        }
        public void Init(HeroRecord record)
        {
            base.Init();

            HP = record.HP;
            Level = record.Level;
            Exp = record.Exp;
            IsConverting = false;
            IsInHall = record.IsInHall;
            IsVisible = record.IsVisible;
        }

        public HeroRecord ToRecord()
        {
            HeroRecord record = new HeroRecord();
            record.Kid = Data.Kid;
            record.HP = HP;
            record.IsInHall = IsInHall;
            record.IsVisible = IsVisible;
            return record;
        }

        public void Convert(HeroData data)
        {
            Data = data;

            InitRaise();
            InitSkillList();

            HP = (int)(GetAttribute(BattleAttribute.HP) * HPRatio);
        }

        private void InitRaise()
        {
            attrRaiseDic[(int)BattleAttribute.HP] = Data.HPRaise;
            attrRaiseDic[(int)BattleAttribute.Attack] = Data.AttackRaise;
            attrRaiseDic[(int)BattleAttribute.Defense] = Data.DefenseRaise;
            attrRaiseDic[(int)BattleAttribute.Critical] = Data.CriticalRaise;
            attrRaiseDic[(int)BattleAttribute.Dodge] = Data.DodgeRaise;
            attrRaiseDic[(int)BattleAttribute.MoveSpeed] = Data.MoveSpeedRaise;
            attrRaiseDic[(int)BattleAttribute.AttackSpeed] = Data.AttackSpeedRaise;
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



        #region Skill

        public override bool CanCastSkill(int index)
        {
            if (IsConverting)
                return false;
            return base.CanCastSkill(index);
        }

        #endregion

        public override bool CanMove()
        {
            return base.CanMove() && !IsConverting;
        }

        public void AddExp(int exp)
        {
            Adam adam = Adam.Instance;
            int newExp = adam.Info.Exp + exp;
            int deltaLevel = 0;
            while(true)
            {
                int maxExp = HeroLevelUpDataManager.Instance.GetExp(adam.Info.Level);
                if(newExp >= maxExp)
                {
                    deltaLevel++;
                    newExp -= maxExp;
                }
                else
                {
                    break;
                }
            }
            adam.Info.Level += deltaLevel;
            adam.Info.Exp = newExp;
        }
	}
}

