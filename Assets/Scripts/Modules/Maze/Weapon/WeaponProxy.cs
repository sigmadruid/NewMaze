using System;
using System.Collections.Generic;

using Base;
using Battle;
using StaticData;

namespace GameLogic
{
    public class WeaponProxy : Proxy
    {
        private Dictionary<int, WeaponInfo> infoDic = new Dictionary<int, WeaponInfo>();

        public void Init(Dictionary<int, WeaponRecord> recordDic)
        {
            if(recordDic != null)
            {
                foreach(WeaponRecord record in recordDic.Values)
                {
                    WeaponInfo info = new WeaponInfo(record);
                    infoDic.Add(info.Data.Kid, info);
                }
            }
            else
            {
                List<int> kidList = GlobalConfig.DemoConfig.InitialHeroKids;
                for(int i = 0; i < kidList.Count; ++i)
                {
                    int kid = kidList[i];
                    HeroData data = HeroDataManager.Instance.GetData(kid) as HeroData;
                    WeaponData weaponData = WeaponDataManager.Instance.GetData(data.WeaponKid) as WeaponData;
                    WeaponInfo info = new WeaponInfo(weaponData);
                    infoDic.Add(info.Data.Kid, info);
                }
            }
        }
        public void Dispose()
        {
        }
        public Dictionary<int, WeaponRecord> Save()
        {
            Dictionary<int, WeaponRecord> recordDic = new Dictionary<int, WeaponRecord>();
            foreach(WeaponInfo info in infoDic.Values)
            {
                WeaponRecord record = info.ToRecord();
                recordDic.Add(info.Data.Kid, record);
            }
            return recordDic;
        }

        public WeaponInfo GetInfoByKid(int kid)
        {
            if(!infoDic.ContainsKey(kid))
            {
                BaseLogger.LogFormat("Can't find weapon info {0}", kid);
            }
            return infoDic[kid];
        }
    }
}

