using UnityEngine;

using System;
using System.Collections.Generic;

using GameLogic;
using StaticData;

namespace Battle
{
    public class WeaponInfo
    {
        public WeaponData Data;
        
        public List<int> BuffKidList = new List<int>();

        public List<Color> ColorList = new List<Color>();
        
        public WeaponInfo(WeaponData data)
        {
            Data = data;
        }

        public WeaponInfo(WeaponRecord record)
        {
            Data = WeaponDataManager.Instance.GetData(record.Kid) as WeaponData;
            BuffKidList = record.BuffKidList;
            ColorList = new List<Color>();
            for(int i = 0; i < record.ColorList.Count; ++i)
            {
                ColorList.Add(record.ColorList[i].ToColor());
            }
        }
        
        public WeaponRecord ToRecord()
        {
            WeaponRecord record = new WeaponRecord();
            record.Kid = Data.Kid;
            record.BuffKidList = BuffKidList;
            record.ColorList = new List<ColorRecord>();
            for(int i = 0; i < ColorList.Count; ++i)
            {
                record.ColorList.Add(new ColorRecord(ColorList[i]));
            }
            return record;
        }

    }
}


