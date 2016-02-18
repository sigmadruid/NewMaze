using System;
using System.Collections.Generic;

using StaticData;

namespace Battle
{
	public class HeroInfo : CharacterInfo
	{
		public new HeroData Data;

		public bool IsConverting;

		public float LastHitTime = -1000f;

		public HeroInfo (HeroData data) : base(data)
		{
			Data = data as HeroData;
		}

		public HeroInfo (HeroData data, HeroInfo info = null) : base(data)
		{
			Data = data;
			if (info != null)
			{
				hp = (int)(data.HP * info.HPRatio);
				IsConverting = info.IsConverting;
				LastHitTime = info.LastHitTime;
			}
		}
	}
}

