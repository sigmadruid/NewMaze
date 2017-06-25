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

        public void Init()
        {
        }
        public void Dispose()
        {
        }
        public void SaveRecord()
        {
            Record = Adam.Instance.ToRecord();
        }

        public List<HeroInfo> GetProfileHeroInfoList()
		{
            List<int> kidList = GlobalConfig.DemoConfig.InitialHeroKids;
            HeroInfo adamInfo = Adam.Instance.Info;

            List<HeroInfo> infoList = new List<HeroInfo>();
            for(int i = 0; i < kidList.Count; ++i)
            {
                int kid = kidList[i];
                HeroData data = HeroDataManager.Instance.GetData(kid) as HeroData;
                HeroInfo info = new HeroInfo(data);
                info.Level = adamInfo.Level;
                info.Exp = adamInfo.Exp;
                infoList.Add(info);
            }
            return infoList;
		}

    }
}

