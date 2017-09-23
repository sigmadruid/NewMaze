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
        private Dictionary<int, HeroInfo> infoDic = new Dictionary<int, HeroInfo>();

        public void Init(Dictionary<int, HeroRecord> recordDic)
        {
            if(recordDic != null)
            {
                var enumerator = recordDic.GetEnumerator();
                while(enumerator.MoveNext())
                {
                    int kid = enumerator.Current.Key;
                    HeroRecord record = enumerator.Current.Value;
                    HeroData data = HeroDataManager.Instance.GetData(kid) as HeroData;
                    HeroInfo info = new HeroInfo(data, record);
                    infoDic.Add(kid, info);
                }
            }
            else
            {
                List<int> kidList = GlobalConfig.DemoConfig.InitialHeroKids;
                for(int i = 0; i < kidList.Count; ++i)
                {
                    int kid = kidList[i];
                    HeroData data = HeroDataManager.Instance.GetData(kid) as HeroData;
                    HeroInfo info = new HeroInfo(data);
                    infoDic.Add(kid, info);
                }
            }
        }
        public void Dispose()
        {
        }
        public Dictionary<int, HeroRecord> Save()
        {
            Dictionary<int, HeroRecord> recordDic = new Dictionary<int, HeroRecord>();
            foreach(HeroInfo info in infoDic.Values)
            {
                HeroRecord record = info.ToRecord();
                recordDic[info.Data.Kid] = record;
            }
            return recordDic;
        }

        public List<HeroInfo> GetProfileHeroInfoList()
		{
            return infoDic.Values.ToList();
		}

        public HeroInfo GetInfoByKid(int kid)
        {
            if(!infoDic.ContainsKey(kid))
            {
                BaseLogger.LogFormat("Can't find hero info {0}", kid);
            }
            return infoDic[kid];
        }

    }
}

