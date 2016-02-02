using System;
using System.Collections.Generic;
using Base;

namespace StaticData
{
	public class MonsterData : CharacterData
	{
		public int AppearWeight;

		public List<int> DropKidList;

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

