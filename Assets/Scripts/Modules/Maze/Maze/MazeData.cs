using System;
using System.Collections.Generic;

namespace StaticData
{
    [Serializable]
	public class MazeData
	{
		public int Kid;

		public int StartCol;
		public int StartRow;

		public float BlockSize;

		public int Cols;
		public int Rows;
		
		public float LinkRate;
		public float PassageRate;

		public int MonsterMaxCount;
		public int NPCMaxCount;
		public int ExplorationMaxCount;

        public Dictionary<ExplorationType, int> GlobalExplorationCountDic;

        public int LimitedLevel;
        public int MaxLevel;
	}
}

