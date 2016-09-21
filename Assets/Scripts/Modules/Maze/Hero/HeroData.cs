using System;

using Base;

namespace StaticData
{
	public class HeroData : CharacterData
	{
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

