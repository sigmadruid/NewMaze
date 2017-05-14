using System;
using System.Collections.Generic;
using Base;

namespace StaticData
{
    public enum MonsterSize
    {
        Small,
        Medium,
        Big,
    }

	public class MonsterData : CharacterData
	{
        public int Exp;

        public MonsterSize Size;

		public int AppearWeight;

		public int DropKid;

        public float AttackDelay;

        public float AttackRange;

        public float DetectRange;

        public float DodgeDelay;

        public float DodgeRange;

		public override string GetResPath ()
		{
			if (resPath == null)
			{
				resPath = Utils.CombinePath("Monsters", Res3D);
			}
			return resPath;
		}
	}
}

