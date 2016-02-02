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
		private List<HeroData> heroDataList;
        private Dictionary<int, HeroData> heroDataDic;

        public HeroProxy()
        {
			heroDataDic = new Dictionary<int, HeroData>();

			int kid;
			kid = IDManager.Instance.GetID(IDType.Hero, 1);
			heroDataDic.Add(kid, HeroDataManager.Instance.GetData(kid) as HeroData);
			kid = IDManager.Instance.GetID(IDType.Hero, 2);
			heroDataDic.Add(kid, HeroDataManager.Instance.GetData(kid) as HeroData);
        }

		public List<HeroData> GetAllHeroDataList()
		{
			if (heroDataList == null)
			{
				heroDataList = heroDataDic.Values.ToList();
			}
			return heroDataList;
		}
    }
}

