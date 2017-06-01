using System;
using System.Collections.Generic;

namespace GameLogic
{
    [Serializable]
    public class AdamRecord
    {
        public Vector3Record WorldPosition;

        public float WorldAngle;

        public Dictionary<int, int> PackItems;
    }
}

