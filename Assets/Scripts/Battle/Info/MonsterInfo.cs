using System;
using System.Collections.Generic;

using GameLogic;
using StaticData;

namespace Battle
{
	public class MonsterInfo : CharacterInfo
	{
        public new MonsterData Data
        {
            get { return data as MonsterData; }
            set { data = value; }
        }
		
		public MonsterInfo (MonsterData data) : base(data)
		{
            Side = Side.Monster;
			Data = data;
            InitSkillList();
		}

		public MonsterInfo (MonsterData data, MonsterRecord record) : base(data)
		{
            Side = Side.Monster;
			Data = data;
			hp = record.HP;
            InitSkillList();
		}
	}
}

