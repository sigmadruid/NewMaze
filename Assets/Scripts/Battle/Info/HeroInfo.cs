using System;
using System.Collections.Generic;

using GameLogic;
using StaticData;

namespace Battle
{
	public class HeroInfo : CharacterInfo
	{
        public new HeroData Data
        {
            get { return data as HeroData; }
            set { data = value; }
        }

		public bool IsConverting;
        public bool IsInHall;

		public float LastHitTime = -1000f;

		public HeroInfo (HeroData data) : base(data)
		{
			Data = data;
		}

		public HeroInfo (HeroData data, HeroInfo info) : base(data)
		{
			Data = data;
			if (info != null)
			{
				hp = (int)(data.HP * info.HPRatio);
				IsConverting = info.IsConverting;
                IsInHall = info.IsInHall;
				LastHitTime = info.LastHitTime;
			}
		}

        public HeroInfo (HeroData data, HeroRecord record) : base(data)
        {
            Data = data;
            hp = record.HP;
            IsInHall = record.IsInHall;
        }
	}
}

