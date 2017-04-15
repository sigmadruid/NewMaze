using System;

namespace StaticData
{
	public enum IDType
	{
		Currency = 0,
		Maze = 1,
		Block = 2,
		Hero = 3,
		Monster = 4,
		NPC = 5,
		NPCEvent = 6,
		Exploration = 7,
		Bullet = 8,
		Animator = 9,
		Drop = 10,
		Hall = 11,
        Buff = 12,
        Trap = 13,
        Item = 14,
        Skill = 15,
        Area = 16,
        Weapon = 17,
        SkillEffect = 18,
	}

    public class IDManager
    {
		public const int PRE_MULTIPLIER = 10000;

        private static IDManager instance;
        public static IDManager Instance
        {
            get
            {
                if (instance == null) instance = new IDManager();
                return instance;
            }
        }

		public int GetKid(IDType type, int subID)
		{
			return ((int)type) * PRE_MULTIPLIER + subID;
		}

        public int GetID(int kid)
        {
            return kid % PRE_MULTIPLIER;
        }

		public IDType GetIDType(int id)
		{
			IDType type = (IDType)(id / PRE_MULTIPLIER);
			return type;
		}
    }
}

