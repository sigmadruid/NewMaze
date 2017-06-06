using System;
using System.Collections.Generic;

namespace GameLogic
{
    [Serializable]
    public class AdamRecord
    {
        public Vector3Record WorldPosition;
        public float WorldAngle;

        public bool IsInHall;
        public bool IsVisible;
        public Dictionary<int, int> PackItems;
    }
}

