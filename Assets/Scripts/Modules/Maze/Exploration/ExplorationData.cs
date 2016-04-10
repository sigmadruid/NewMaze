using System;
using Base;

namespace StaticData
{
    public enum ExplorationType
    {
        Chest = 1,
        Corpse,
        Common,

		Transporter,
		Any,
    }

    public class ExplorationData : EntityData
    {
		public ExplorationType Type;

		public string Res3D;

		public int MazeKid;

		public bool IsGlobal;

        public string Param1;
        public string Param2;
        public string Param3;

		public override string GetResPath ()
		{
			if (resPath == null)
			{
				resPath = Utils.CombinePath("Exploration", Type, Res3D);
			}
			return resPath;
		}

    }
}

