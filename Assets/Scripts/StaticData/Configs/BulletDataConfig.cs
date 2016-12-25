using System;
using System.Collections.Generic;

namespace StaticData
{

	public class BulletDataConfig
	{
		public Dictionary<int, BulletData> BulletDataDic;

		public BulletDataConfig ()
		{
			IDManager idManager = IDManager.Instance;
			BulletDataDic = new Dictionary<int, BulletData>();
			BulletData data;

			data = new BulletData();
			data.Kid = idManager.GetKid(IDType.Bullet, 1);
			data.Name = "FireBall";
			data.Res3D = "Bullet_1";
			data.Speed = 3f;
			data.Radius = 0.2f;
			BulletDataDic.Add(data.Kid, data);
		}
	}
}

