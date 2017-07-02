using System;
using System.Collections.Generic;

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

        public string Res2D;
		public string Res3D;
		
		public int HP;
		public int Attack;
		public int Defense;
		public int Critical;
		public int Dodge;
        public float MoveSpeed;
        public float AttackSpeed;
		public float RollSpeed;

		//Melee or shoot
		public AttackType AttackType;

        public List<int> SkillList;

	}
}

