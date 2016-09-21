using System;

namespace StaticData
{
	public enum AttackType
	{
		Melee = 1,
		Range,
	}

	public class CharacterData : EntityData
	{
        public string Name;

		public string Res3D;
		
		public int HP;
		public int Attack;
		public int Defense;
		public int Critical;
		public int Dodge;
		public float MoveSpeed;

		//A range within which the monster will start acting.
		public float DetectRange;
		//Melee or shoot
		public AttackType AttackType;
		//Attack Delay
		public float AttackDelay;
		//Attack range
		public float AttackRange;
		//If too close, wait a while and run away.
		public float DodgeDelay;
		//How near to the hero will the monster run away.
		public float DodgeRange;

		public int BulletKid;


	}
}

