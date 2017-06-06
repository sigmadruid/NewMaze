using UnityEngine;

using System;
using System.Collections.Generic;

namespace GameLogic
{
    [Serializable]
    public class MonsterRecord : EntityRecord
    {
		public int HP;

        public bool IsInHall;

        public Dictionary<int, float> buffRemainTimeDic = new Dictionary<int, float>();
    }
}

