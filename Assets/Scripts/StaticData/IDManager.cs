using System;

namespace StaticData
{
	public enum IDType
	{
		Currency = 0,
		Maze,
		Block,
		Hero,
		Monster,
		NPC,
		NPCEvent,
		Exploration,
		Bullet,
		Animator,
		Drop,
		Hall,
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

		public int GetID(IDType type, int subID)
		{
			return ((int)type) * PRE_MULTIPLIER + subID;
		}

		public IDType GetIDType(int id)
		{
			IDType type = (IDType)(id / PRE_MULTIPLIER);
			return type;
		}
    }
}

