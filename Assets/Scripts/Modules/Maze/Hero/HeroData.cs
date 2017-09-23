using System;
using System.Collections.Generic;

using Base;

namespace StaticData
{
	public class HeroData : CharacterData
	{
        public float HPRaise;
        public float AttackRaise;
        public float DefenseRaise;
        public float CriticalRaise;
        public float DodgeRaise;
        public float MoveSpeedRaise;
        public float AttackSpeedRaise;

        public int WeaponKid;

		public override string GetResPath ()
		{
			if (resPath == null)
			{
				resPath = Utils.CombinePath("Heroes", Res3D);
			}
			return resPath;
		}
	}
}

