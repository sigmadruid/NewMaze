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
        private Dictionary<int, HeroData> heroDataDic = new Dictionary<int, HeroData>();

        public void Init()
        {
            int kid;
            kid = IDManager.Instance.GetID(IDType.Hero, 1);
            heroDataDic.Add(kid, HeroDataManager.Instance.GetData(kid) as HeroData);
            kid = IDManager.Instance.GetID(IDType.Hero, 2);
            heroDataDic.Add(kid, HeroDataManager.Instance.GetData(kid) as HeroData);
        }

        public void Dispose()
        {
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

