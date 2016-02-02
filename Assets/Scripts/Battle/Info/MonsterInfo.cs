using System;
using System.Collections.Generic;

using StaticData;

namespace GameLogic
{
	public class MonsterInfo : CharacterInfo
	{
		public new MonsterData Data;
		
		public MonsterInfo (MonsterData data) : base(data)
		{
			Data = data;
		}

		public MonsterInfo (MonsterData data, MonsterRecord record) : base(data)
		{
			Data = data;

			hp = record.HP;
		}
	}
}

