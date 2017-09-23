using UnityEngine;

using System;
using System.Collections.Generic;

namespace GameLogic
{
    [Serializable]
    public class WeaponRecord
    {
        public int Kid;

        public List<int> BuffKidList = new List<int>();

        public List<ColorRecord> ColorList = new List<ColorRecord>();
    }
}

