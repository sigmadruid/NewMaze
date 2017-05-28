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
        public Dictionary<int, HeroRecord> RecordDic = new Dictionary<int, HeroRecord>();

        private Dictionary<int, HeroInfo> infoDic = new Dictionary<int, HeroInfo>();

        public void Init()
        {
            if(RecordDic.Count == 0)
            {
                List<int> kids = GlobalConfig.DemoConfig.InitialHeroKids;
                for(int i = 0; i < kids.Count; ++i)
                {
                    int kid = kids[i];
                    HeroData data = HeroDataManager.Instance.GetData(kid) as HeroData;
                    HeroInfo info = new HeroInfo(data);
                    infoDic.Add(kid, info);
                }
            }
            else
            {
                foreach(int kid in RecordDic.Keys)
                {
                    HeroRecord record = RecordDic[kid];
                    HeroData data = HeroDataManager.Instance.GetData(kid) as HeroData;
                    HeroInfo info = new HeroInfo(data, record);
                    infoDic.Add(kid, info);
                }
            }
        }

        public void Dispose()
        {
            RecordDic.Clear();
            infoDic.Clear();
        }

        public List<HeroInfo> GetUnlockedHeroInfoList()
		{
            return infoDic.Values.ToList();
		}

        public HeroInfo GetHeroInfo(int kid)
        {
            if (!infoDic.ContainsKey(kid))
            {
                BaseLogger.LogFormat("Can't load hero info: {0}", kid);
            }
            return infoDic[kid];
        }

        public void DoRecord()
        {
            RecordDic.Clear();
            foreach(int kid in infoDic.Keys)
            {
                HeroInfo info = infoDic[kid];
                HeroRecord record = info.ToRecord();
                RecordDic.Add(kid, record);
            }
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

