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

        public WeaponInfo WeaponInfo;

        public int Level;
        public int Exp;

        private Dictionary<int, float> attrRaiseDic = new Dictionary<int, float>();

        private PlayerProxy playerProxy;
        private WeaponProxy weaponProxy;

        public HeroInfo (HeroData data) : base(data)
        {
            Init();

            Level = GlobalConfig.DemoConfig.InitLevel;
            Exp = 0;
            HP = (int)(GetBaseAttribute(BattleAttribute.HP));
            WeaponInfo = weaponProxy.GetInfoByKid(Data.WeaponKid);
        }

        public HeroInfo(HeroData data, HeroRecord record) : base(data)
        {
            Init();

            Level = record.Level;
            Exp = record.Exp;
            HP = record.HP;
            WeaponInfo = weaponProxy.GetInfoByKid(Data.WeaponKid);
        }
        private void Init()
        {
            playerProxy = ApplicationFacade.Instance.RetrieveProxy<PlayerProxy>();
            weaponProxy = ApplicationFacade.Instance.RetrieveProxy<WeaponProxy>();

            InitRaise();
            InitSkillList();

            Side = Side.Hero;
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

        private void InitRaise()
        {
            attrRaiseDic[(int)BattleAttribute.HP] = Data.HPRaise;
            attrRaiseDic[(int)BattleAttribute.SP] = 0;
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
            if (playerProxy.CurrentInfo.IsConverting)
                return false;
            Skill skill = SkillList[index - 1];
            if(SP < skill.Data.CostSP)
                return false;
            return base.CanCastSkill(index);
        }

        #endregion

        public override bool CanMove()
        {
            return base.CanMove() && !playerProxy.CurrentInfo.IsConverting;
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

