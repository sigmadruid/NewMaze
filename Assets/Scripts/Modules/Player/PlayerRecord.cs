using System;
using System.Collections.Generic;

namespace GameLogic
{
    [Serializable]
    public class PlayerRecord
    {
		public string Uid;

		public string Name;
        public int HeroKid;
        public Vector3Record StartPosition;
        public float StartAngle;

        public bool IsConverting;
        public bool IsInHall;
        public bool IsVisible;


        public int HallKid;
        public Vector3Record LeavePosition;
    }
}

