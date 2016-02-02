using System;
using System.Collections.Generic;

namespace StaticData
{
	public class HeroDataConfig
	{
		public Dictionary<int, HeroData> HeroDataDic;

		public HeroDataConfig ()
		{
			IDManager idManager = IDManager.Instance;
			int index = 1;
			HeroDataDic = new Dictionary<int, HeroData>();
			HeroData data;

			data = new HeroData();
			data.Kid = idManager.GetID(IDType.Hero, index++);
			data.Name = 10011;
			data.Res3D = "Hero_1";
			data.HP = 1000;
			data.Attack = 100;
			data.Defense = 10;
			data.Critical = 20;
			data.Dodge = 10;
			data.AttackRange = 2f;
			data.DodgeRange = 0f;
			HeroDataDic.Add(data.Kid, data);

			data = new HeroData();
			data.Kid = idManager.GetID(IDType.Hero, index++);
			data.Name = 10012;
			data.Res3D = "Hero_2";
			data.HP = 5000;
			data.Attack = 200;
			data.Defense = 20;
			data.Critical = 50;
			data.Dodge = 0;
			data.AttackRange = 3f;
			data.DodgeRange = 0f;
			HeroDataDic.Add(data.Kid, data);
		}
	}
}

