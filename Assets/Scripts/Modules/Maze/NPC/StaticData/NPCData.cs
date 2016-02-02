using System;
using Base;

namespace StaticData
{
    public enum NPCAppearScene
    {
        HomeTown,
        Maze,
    }

    public class NPCData : EntityData
    {
        public int Name;

        public NPCAppearScene AppearScene;

        public string Res3D;

		public override string GetResPath ()
		{
			if (resPath == null)
			{
				resPath = Utils.CombinePath("NPCs", Res3D);
			}
			return resPath;
		}
    }
}

