using UnityEngine;

using System;

namespace GameLogic
{
    [Serializable]
    public class EntityRecord
    {
		public string Uid;

		public int Kid;

		public Vector3Record WorldPosition;

        public float WorldAngle;
    }
}

