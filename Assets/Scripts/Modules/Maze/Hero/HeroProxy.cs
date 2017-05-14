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

        public void AddExp(int exp)
        {
            Adam adam = Adam.Instance;
            int newExp = adam.Info.Exp + exp;
            int deltaLevel = 0;
            while(true)
            {
                int maxExp = HeroLevelUpDataManager.Instance.GetExp(adam.Info.Level);
                if(newExp >= maxExp)
                {
                    deltaLevel++;
                    newExp -= maxExp;
                }
                else
                {
                    break;
                }
            }
            adam.Info.Level += deltaLevel;
            adam.Info.Exp = newExp;
        }
    }
}

