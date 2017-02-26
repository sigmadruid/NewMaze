using System;
using System.Collections.Generic;

using Base;

namespace StaticData
{
	public class HeroData : CharacterData
	{
        public string Trigger;
        public List<int> SkillList;

		public override string GetResPath ()
		{
			if (resPath == null)
			{
				resPath = Utils.CombinePath("Heroes", Res3D);
			}
			return resPath;
		}
	}
}

