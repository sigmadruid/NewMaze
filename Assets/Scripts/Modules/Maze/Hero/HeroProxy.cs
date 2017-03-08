using UnityEngine;

using System.Collections;
using System.Collections.Generic;
using System.Linq;

using Base;
using StaticData;

namespace GameLogic
{
    public class HeroProxy : Proxy
    {
        public HeroRecord Record;

		private List<HeroData> heroDataList;

        public void Init()
        {
        }

        public void Dispose()
        {
        }

		public List<HeroData> GetUnlockHeroList()
		{
			if (heroDataList == null)
			{
                heroDataList = HeroDataManager.Instance.GetAllData();
			}
			return heroDataList;
		}
    }
}

