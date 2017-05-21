using UnityEngine;

using System.Collections;
using System.Collections.Generic;
using System.Linq;

using Base;
using Battle;
using StaticData;

namespace GameLogic
{
    public class HeroProxy : Proxy
    {
        public HeroRecord Record;

        private List<HeroInfo> heroInfoList;

        public void Init()
        {
        }

        public void Dispose()
        {
        }

        public List<HeroInfo> GetInitialHeroInfoList()
        {
            HeroData data;
            HeroInfo info;

            List<int> kids = GlobalConfig.DemoConfig.InitialHeroKids;
            List<HeroInfo> infoList = new List<HeroInfo>();
            for(int i = 0; i < kids.Count; ++i)
            {
                int kid = kids[i];
                data = HeroDataManager.Instance.GetData(kid) as HeroData;
                info = new HeroInfo(data);
                infoList.Add(info);
            }
            return infoList;
        }

        public List<HeroInfo> GetUnlockedHeroInfoList()
		{
            if (heroInfoList == null)
			{
                heroInfoList = GetInitialHeroInfoList();
			}
			return heroInfoList;
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

