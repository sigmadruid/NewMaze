using System;
using System.Collections.Generic;

using StaticData;

namespace GameLogic
{
	public class HeroInfo : CharacterInfo
	{
		public new HeroData Data;

		public bool IsConverting;

		public bool IsTransporting;

		public bool InHall;

		public float LastHitTime = -1000;

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

