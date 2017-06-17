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

        public List<int> GetUnlockedHeroKidList()
		{
            return GlobalConfig.DemoConfig.InitialHeroKids;
		}

    }
}

