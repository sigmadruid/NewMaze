using System;
using System.Collections.Generic;
using Base;

namespace StaticData
{
	public class MonsterData : CharacterData
	{
		public int AppearWeight;

		public int DropKid;

        //Attack Delay
        public float AttackDelay;
        //Attack range
        public float AttackRange;
        //A range within which the monster will start acting.
        public float DetectRange;
        //If too close, wait a while and run away.
        public float DodgeDelay;
        //How near to the hero will the monster run away.
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

