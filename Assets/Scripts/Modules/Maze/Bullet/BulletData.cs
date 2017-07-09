using System;

namespace StaticData
{
	public class BulletData : EntityData
	{
		public string Name;

		public string Res3D;

		public float Speed;

        public float Radius;

		public float EndDuration;

        public bool AddToTarget;

		public override string GetResPath ()
		{
			if (resPath == null)
			{
				resPath = Base.Utils.CombinePath("Bullets", Res3D);
			}
			return resPath;
		}
	}
}

