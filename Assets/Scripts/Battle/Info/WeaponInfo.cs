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
        
        public WeaponRecord ToRecord()
        {
            WeaponRecord record = new WeaponRecord();
            record.Kid = Data.Kid;
            return record;
        }
    }
}


