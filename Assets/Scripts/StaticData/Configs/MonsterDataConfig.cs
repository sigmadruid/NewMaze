using System;
using System.Collections.Generic;

namespace StaticData
{
	public class MonsterDataConfig
	{
		public Dictionary<int, MonsterData> MonsterDataDic;

		public MonsterDataConfig ()
		{
			IDManager idManager = IDManager.Instance;
			MonsterDataDic = new Dictionary<int, MonsterData>();
			MonsterData data;

			data = new MonsterData();
			data.Kid = idManager.GetID(IDType.Monster, 1);
			data.Name = 0;
			data.Res3D = "Monster_1";
			data.HP = 300;
			data.Attack = 20;
			data.Defense = 5;
			data.Critical = 20;
			data.Dodge = 10;
			data.DetectRange = 8;
			data.AttackType = AttackType.Melee;
			data.AttackDelay = 2f;
			data.AttackRange = 1.5f;
			data.DodgeDelay = 0f;
			data.DodgeRange = 0f;
			data.AppearWeight = 500;
			MonsterDataDic.Add(data.Kid, data);

			data = new MonsterData();
			data.Kid = idManager.GetID(IDType.Monster, 2);
			data.Name = 0;
			data.Res3D = "Monster_2";
			data.HP = 200;
			data.Attack = 30;
			data.Defense = 0;
			data.Critical = 40;
			data.Dodge = 30;
			data.DetectRange = 8;
			data.AttackType = AttackType.Range;
			data.AttackDelay = 3f;
			data.AttackRange = 6f;
			data.DodgeDelay = 2f;
			data.DodgeRange = 2f;
			data.BulletKid = idManager.GetID(IDType.Bullet, 1);
			data.AppearWeight = 300;
			MonsterDataDic.Add(data.Kid, data);

			data = new MonsterData();
			data.Kid = idManager.GetID(IDType.Monster, 3);
			data.Name = 0;
			data.Res3D = "Monster_3";
			data.HP = 800;
			data.Attack = 50;
			data.Defense = 10;
			data.Critical = 60;
			data.Dodge = 0;
			data.DetectRange = 6;
			data.AttackType = AttackType.Melee;
			data.AttackDelay = 5f;
			data.AttackRange = 4f;
			data.DodgeDelay = 0;
			data.DodgeRange = 0;
			data.AppearWeight = 100;
			MonsterDataDic.Add(data.Kid, data);
		}
	}
}

